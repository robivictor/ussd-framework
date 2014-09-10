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
        public string Title { get; set; }
        public UssdScreenTypes Type { get; set; }
        public List<string> Inputs { get; set; }
        public Dictionary<string,string> InputData = new Dictionary<string, string>();
        public RespondAsyncDelegate RespondAsync { get; set; }
        public InputProcessorAsyncDelegate InputProcessorAsync { get; set; }

        public delegate Task<UssdResponse> RespondAsyncDelegate(Session session);

        public delegate Task<UssdResponse> InputProcessorAsyncDelegate(Dictionary<string, string> data);

        /// <summary>
        /// Prepare input data to be passed to <see cref="InputProcessorAsync"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task PrepareInputDataAsync(Session session)
        {
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
            return UssdResponse.Generate(UssdResponseTypes.Response
                , Title + Environment.NewLine + Inputs[++position]);
        }
    }
}
