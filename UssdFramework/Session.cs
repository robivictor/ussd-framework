using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UssdFramework
{
    /// <summary>
    /// Manages screen states and processing for a USSD session.
    /// Uses Redis for session store.
    /// </summary>
    public class Session
    {
        public string AppName { get; set; }
        public string Mobile { get; set; }
        public string Screen { get; set; }
        public UssdRequestTypes State { get; set; }
        public UssdRequest UssdRequest { get; set; }
        public Dictionary<string, UssdScreen> UssdScreens { get; set; }
        public string InputMetaHash { get { return Mobile + PostfixInputMeta; } }
        public string InputDataHash { get { return Mobile + PostfixInputData; } }

        private const string PostfixInputData = "_InputData";
        private const string PostfixInputMeta = "_InputMeta";

        public readonly IDatabase Redis;

        /// <summary>
        /// Initializes a new session.
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="request"></param>
        public Session(Setup setup, UssdRequest request)
        {
            AppName = setup.Name;
            Redis = setup.Redis;
            UssdScreens = setup.UssdScreens;
            Mobile = request.Mobile;
            UssdRequest = request;
        }

        /// <summary>
        /// Automatically setup a session based on 
        /// <param name="initialScreenAddress">initial screen address</param>.
        /// </summary>
        /// <param name="initialScreenAddress"></param>
        /// <returns></returns>
        public async Task<UssdResponse> AutoSetupAsync(string initialScreenAddress)
        {
            switch (UssdRequest.Type)
            {
                case "Initiation":
                    var exists = await ExistsAndTimedOutAsync();
                    if (exists) await ResumeAsync();
                    else await StartAsync(initialScreenAddress);
                    break;
                case "Response":
                    await ContinueAsync();
                    break;
                case "Release":
                    await EndAsync();
                    break;
                case "Timeout":
                    await TimeoutAsync();
                    break;
                default:
                    return UssdResponse.Generate(UssdResponseTypes.Release
                        , "Failed to setup session. Check the Type parameter of USSD request.");
            }
            return await RespondAsync();
        }

        /// <summary>
        /// Automatically setup a session by setting initial screen address to "1".
        /// </summary>
        /// <returns></returns>
        public async Task<UssdResponse> AutoSetupAsync()
        {
            return await AutoSetupAsync("1");
        } 

        /// <summary>
        /// Check if current request has an existing session.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExistsAsync()
        {
            return await Redis.KeyExistsAsync(UssdRequest.Mobile);
        }

        /// <summary>
        /// Check if current request has an existing session that timed out previously.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExistsAndTimedOutAsync()
        {
            var exists = await this.ExistsAsync();
            if (!exists) return false;
            var status = await Redis.HashGetAsync(Mobile, "Status");
            return status == "Timeout";
        }

        #region Session State Modifiers

        /// <summary>
        /// Start a new USSD session.
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public async Task StartAsync(string screen)
        {
            await Redis.KeyDeleteAsync(Mobile);
            await Redis.KeyDeleteAsync(InputDataHash);
            await Redis.KeyDeleteAsync(InputMetaHash);
            await Redis.HashSetAsync(Mobile, "Mobile", Mobile);
            await Redis.HashSetAsync(Mobile, "Status", "Active");
            await Redis.HashSetAsync(Mobile, "Screen", screen);
            await ResetExpiryAsync();
            Screen = screen;
            State = UssdRequestTypes.Initiation;
        }

        /// <summary>
        /// Resume a previously timed out session.
        /// </summary>
        /// <returns></returns>
        public async Task ResumeAsync()
        {
            await Redis.KeyDeleteAsync(InputDataHash);
            await Redis.KeyDeleteAsync(InputMetaHash);
            await ResetExpiryAsync();
            await Redis.HashSetAsync(Mobile, "Status", "Active");
            Screen = await Redis.HashGetAsync(Mobile, "Screen");
            State = UssdRequestTypes.Initiation;
        }        

        /// <summary>
        /// Continue an existing session.
        /// </summary>
        /// <returns></returns>
        public async Task ContinueAsync()
        {
            await ResetExpiryAsync();
            Screen = await Redis.HashGetAsync(Mobile, "Screen");
            State = UssdRequestTypes.Response;
        }

        /// <summary>
        /// End a session.
        /// </summary>
        /// <returns></returns>
        public async Task EndAsync()
        {
            await Redis.KeyDeleteAsync(Mobile);
            await Redis.KeyDeleteAsync(InputDataHash);
            await Redis.KeyDeleteAsync(InputMetaHash);
            State = UssdRequestTypes.Release;
        }

        /// <summary>
        /// Timeout the session.
        /// </summary>
        /// <returns></returns>
        public async Task TimeoutAsync()
        {
            await Redis.KeyDeleteAsync(InputMetaHash);
            await Redis.KeyDeleteAsync(InputDataHash);
            await Redis.HashSetAsync(Mobile, "Status", "Timeout");
            await ResetExpiryAsync();
            State = UssdRequestTypes.Timeout;
        }

        #endregion Session State Modifiers

        /// <summary>
        /// Send appropriate response for USSD session.
        /// </summary>
        /// <returns></returns>
        public async Task<UssdResponse> RespondAsync()
        {
            UssdScreen screen;
            const string noScreenMessage = "Failed to get appropriate response. Please try again.";
            switch (State)
            {
                case UssdRequestTypes.Release:
                    return UssdResponse.Generate(UssdResponseTypes.Release
                        , String.Format("Thank you for using {0}.", AppName));
                case UssdRequestTypes.Timeout:
                    return UssdResponse.Generate(UssdResponseTypes.Release
                        , "Session timed out. Try again.");
                case UssdRequestTypes.Initiation:
                    if (!UssdScreens.ContainsKey(Screen))
                        return UssdResponse.Generate(UssdResponseTypes.Release
                            , noScreenMessage);
                    screen = UssdScreens[Screen];
                    return await screen.RespondAsync(this);
                case UssdRequestTypes.Response:
                    if (!UssdScreens.ContainsKey(Screen))
                        return UssdResponse.Generate(UssdResponseTypes.Release
                            , noScreenMessage);
                    screen = UssdScreens[Screen];
                    switch (screen.Type)
                    {
                        case UssdScreenTypes.Menu:
                            var screenList = Screen.Split(new[] { "." }
                                , StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (screenList.Count > 1 && UssdRequest.Message == "0")
                                screenList.RemoveAt(screenList.Count - 1);
                            else
                                screenList.Add(UssdRequest.Message);
                            var screenAddress = string.Join(".", screenList);
                            if (!UssdScreens.ContainsKey(screenAddress))
                                return UssdResponse.Generate(UssdResponseTypes.Release
                                    , noScreenMessage);
                            screen = UssdScreens[screenAddress];
                            await Redis.HashSetAsync(Mobile, "Screen", screenAddress);
                            Screen = screenAddress;
                            break;
                        case UssdScreenTypes.Input:
                            var inputMetaExists = await Redis.KeyExistsAsync(InputMetaHash);
                            if (!inputMetaExists
                                || Screen != await Redis.HashGetAsync(InputMetaHash, "Screen"))
                                await ResetInputAsync(screen);
                            var length = screen.Inputs.Count;
                            var position = Convert.ToInt32(await Redis.HashGetAsync(InputMetaHash
                                , "Position"));
                            if (position < length - 1)
                            {
                                await ResetInputExpiryAsync();
                                return await screen.ReceiveInputAndRespondAsync(this, position);
                            }
                            await screen.ReceiveInputAsync(this, position);
                            await screen.PrepareInputDataAsync(this);
                            return await screen.InputProcessorAsync(screen.InputData);
                    }
                    return await screen.RespondAsync(this);
                default:
                    return UssdResponse.Generate(UssdResponseTypes.Release
                        , noScreenMessage);
            }
        }

        /// <summary>
        /// Reset the session's expiry.
        /// </summary>
        /// <returns></returns>
        private async Task ResetExpiryAsync()
        {
            await Redis.KeyExpireAsync(Mobile, TimeSpan.FromSeconds(120));
        }

        /// <summary>
        /// Reset an input process' expiry.
        /// </summary>
        /// <returns></returns>
        private async Task ResetInputExpiryAsync()
        {
            await Redis.KeyExpireAsync(InputMetaHash, TimeSpan.FromSeconds(90));
            await Redis.KeyExpireAsync(InputDataHash, TimeSpan.FromSeconds(90)); 
        }

        /// <summary>
        /// Reset input process.
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        private async Task ResetInputAsync(UssdScreen screen)
        {
            await Redis.HashSetAsync(InputMetaHash, "Screen"
                                    , Screen);
            await Redis.HashSetAsync(InputMetaHash, "Length"
                , screen.Inputs.Count);
            await Redis.HashSetAsync(InputMetaHash, "Position"
                , 0);
            await ResetInputExpiryAsync();
        }
    }
}