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
        public List<string> Inputs { get; set; }
        /// <summary>
        /// Populated when all inputs have been collected and sent to InputProcessorAsync delegate method.
        /// </summary>
        public Dictionary<string,string> InputData = new Dictionary<string, string>();
        /// <summary>
        /// Screen response delegate method.
        /// </summary>
        public RespondAsyncDelegate RespondAsync { get; set; }
        /// <summary>
        /// Input processor delegate method.
        /// </summary>
        public InputProcessorAsyncDelegate InputProcessorAsync { get; set; }

        public delegate Task<UssdResponse> RespondAsyncDelegate(Session session);

        public delegate Task<UssdResponse> InputProcessorAsyncDelegate(Session session, Dictionary<string, string> data);

        /// <summary>
        /// Prepare input data to be passed to <see cref="InputProcessorAsync"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task PrepareInputDataAsync(Session session)
        {
            InputData.Clear();
            foreach (var input in Inputs)
            {
                var value = await session.Redis.HashGetAsync(session.InputDataHash, input);
                InputData.Add(input, value.ToString());
            }
        }

        /// <summary>
        /// Receive user input.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public async Task ReceiveInputAsync(Session session, int position)
        {
            await session.Redis.HashSetAsync(session.InputDataHash, Inputs[position]
                , session.UssdRequest.Message);
            await session.Redis.HashSetAsync(session.InputMetaHash, "Position", ++position);
        } 

        /// <summary>
        /// Receive user input and send a <see cref="UssdResponse"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public async Task<UssdResponse> ReceiveInputAndRespondAsync(Session session, int position)
        {
            await ReceiveInputAsync(session, position);
            return UssdResponse.Response(Title + Environment.NewLine
                + Inputs[++position]);
        }
    }
}
