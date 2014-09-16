using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models
{
    public class InputProcessors
    {
        public static async Task<UssdResponse> CustomGreeting(Session session, Dictionary<string, string> data)
        {
            return UssdResponse.Input(String.Format("Hello {0} of {1}!"
                , data["Name"], data["Company"]));
        }
    }
}