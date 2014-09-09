using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models
{
    public class ScreenResponses
    {
        public static async Task<UssdResponse> MainMenu(Session session)
        {
            return UssdResponse.Generate(UssdResponseTypes.Response
                , "Welcome to the Demo App." + Environment.NewLine
                  + "1. Just greet me" + Environment.NewLine
                  + "2. Greet me with my name" + Environment.NewLine
                  + "3. Another menu");
        }

        public static async Task<UssdResponse> SimpleGreeting(Session session)
        {
            return UssdResponse.Generate(UssdResponseTypes.Release
                , "Hello Boss!");
        }

        public static async Task<UssdResponse> CustomGreeting(Session session)
        {
            return UssdResponse.Generate(UssdResponseTypes.Response
                , "Enter your name" + Environment.NewLine
                  + "First Name" + Environment.NewLine);
        }

        public static async Task<UssdResponse> AnotherMenu(Session session)
        {
            return UssdResponse.Generate(UssdResponseTypes.Response
                , "Another menu with dummy stuff. Only back works!" + Environment.NewLine
                  + "1. Nowhere" + Environment.NewLine
                  + "2. Nowhere" + Environment.NewLine
                  + "0. Go back");
        } 
    }
}