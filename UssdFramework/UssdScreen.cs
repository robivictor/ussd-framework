using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    /// <summary>
    /// USSD screen model.
    /// </summary>
    public class UssdScreen
    {
        /// <summary>
        /// Title of screen. Screens of Type "Input" will have this displayed  before
        /// the input parameter's name.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Screen type. "Menu", "Input" or "Notice".
        /// </summary>
        public UssdScreenTypes Type { get; set; }
        /// <summary>
        /// List of input parameter names for this screen.
        /// </summary>
        public List<UssdInput> Inputs { get; set; }
        /// <summary>
        /// Populated when all inputs have been collected and sent to InputProcessorAsync delegate method.
        /// </summary>
        public Dictionary<string,string> InputData = new Dictionary<string, string>();
        /// <summary>
        /// Screen response delegate method.
        /// </summary>
        public ResponseAsyncDelegate ResponseAsync { get; set; }
        /// <summary>
        /// Input processor delegate method.
        /// </summary>
        public InputProcessorAsyncDelegate InputProcessorAsync { get; set; }

        public delegate Task<UssdResponse> ResponseAsyncDelegate(Session session);

        public delegate Task<UssdResponse> InputProcessorAsyncDelegate(Session session
            , Dictionary<string, string> data);

        /// <summary>
        /// Prepare input data to be passed to <see cref="InputProcessorAsync"/>.
        /// </summary>
        /// <param name="session">Session instance.</param>
        public async Task PrepareInputDataAsync(Session session)
        {
            InputData.Clear();
            foreach (var input in Inputs)
            {
                var value = await session.Redis.HashGetAsync(session.InputDataHash, input.Name);
                InputData.Add(input.Name, value.ToString());
            }
        }

        /// <summary>
        /// Receive user input.
        /// </summary>
        /// <param name="session">Session instance.</param>
        /// <param name="position">Input's position.</param>
        /// <returns></returns>
        public async Task ReceiveInputAsync(Session session, int position)
        {
            var input = Inputs[position];
            var receivedMessage = session.UssdRequest.Message;
            String value;
            if (input.HasOptions)
            {
                var optionNumber = Convert.ToInt32(receivedMessage);
                value = optionNumber < 0 || optionNumber > input.Options.Count
                    ? receivedMessage
                    : input.Options[optionNumber - 1].Value;
            }
            else
            {
                value = receivedMessage;
            }
            await session.Redis.HashSetAsync(session.InputDataHash, input.Name
                , value);
            await session.Redis.HashSetAsync(session.InputMetaHash, "Position", ++position);
        } 

        /// <summary>
        /// Receive user input and send a <see cref="UssdResponse"/>.
        /// </summary>
        /// <param name="session">Session instance</param>
        /// <param name="position">Input's position.</param>
        /// <returns></returns>
        public async Task<UssdResponse> ReceiveInputAndRespondAsync(Session session, int position)
        {
            await ReceiveInputAsync(session, position);
            return InputResponse(++position);
        }

        /// <summary>
        /// Return appropriate response for input.
        /// </summary>
        /// <param name="position">Input's position.</param>
        /// <returns></returns>
        public UssdResponse InputResponse(int position)
        {
            var input = Inputs[position];
            var message = new StringBuilder();
            message.Append(Title + Environment.NewLine);
            if (input.HasOptions)
            {
                message.Append("Choose " + input.DisplayName + Environment.NewLine);
                var options = input.Options;
                for (var i = 0; i < options.Count; i++)
                {
                    message.AppendFormat("{0}. {1}" + Environment.NewLine
                        , i + 1, options[i].DisplayValue);
                }
            }
            else
            {
                message.Append("Enter " + input.DisplayName + Environment.NewLine);
            }
            return UssdResponse.Response(message.ToString());
        }
    }
}
