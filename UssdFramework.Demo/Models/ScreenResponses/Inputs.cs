using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models.ScreenResponses
{
    public class Inputs
    {
        public static async Task<UssdResponse> CustomGreeting(Session session)
        {
            return UssdResponse.Response("Enter your name" + Environment.NewLine
                  + "First Name" + Environment.NewLine);
        }
    }
}