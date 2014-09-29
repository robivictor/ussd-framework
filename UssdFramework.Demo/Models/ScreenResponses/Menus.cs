using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models.ScreenResponses
{
    public class Menus
    {
        public static async Task<UssdResponse> MainMenu(Session session)
        {
            return await Task.FromResult(UssdResponse.Menu(
                "Welcome to the Demo App." + Environment.NewLine
                  + "1. Just greet me" + Environment.NewLine
                  + "2. Custom greeting" + Environment.NewLine
                  + "3. Another menu"));
        }

        public static async Task<UssdResponse> AnotherMenu(Session session)
        {
            return await Task.FromResult(UssdResponse.Menu(
                "Another menu with dummy stuff. Only back works!" + Environment.NewLine
                + "1. Nowhere" + Environment.NewLine
                + "2. Nowhere" + Environment.NewLine
                + "0. Go back"));
        }
    }
}