using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
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

        public async Task PrepareInputDataAsync(Session session)
        {
            foreach (var input in Inputs)
            {
                var value = await session.Redis.HashGetAsync(session.InputDataHash, input);
                InputData.Add(input, value.ToString());
            }
        }

        public async Task ReceiveInputAsync(Session session, int position)
        {
            await session.Redis.HashSetAsync(session.InputDataHash, Inputs[position]
                , session.UssdRequest.Message);
            await session.Redis.HashSetAsync(session.InputMetaHash, "Position", ++position);
        } 

        public async Task<UssdResponse> ReceiveInputAndRespondAsync(Session session, int position)
        {
            await ReceiveInputAsync(session, position);
            return UssdResponse.Generate(UssdResponseTypes.Response
                , Title + Environment.NewLine + Inputs[++position]);
        }
    }
}
