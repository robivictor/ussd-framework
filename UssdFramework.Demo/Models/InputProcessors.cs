using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models
{
    public class InputProcessors
    {
        public static async Task<UssdResponse> CustomGreeting(Dictionary<string, string> data)
        {
            return UssdResponse.Generate(UssdResponseTypes.Release
                , String.Format("Hello {0} {1}!", data["First Name"], data["Last Name"]));
        }
    }
}