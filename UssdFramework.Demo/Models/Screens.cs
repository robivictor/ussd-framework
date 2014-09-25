using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace UssdFramework.Demo.Models
{
    public class Screens
    {
        public static Dictionary<string, UssdScreen> All = new Dictionary<string, UssdScreen>();

        static Screens()
        {
            All.Add("1", UssdScreen.Menu("Main menu", ScreenResponses.Menus.MainMenu));

            All.Add("1.1", UssdScreen.Notice("Simple Greeting", ScreenResponses.Notices.SimpleGreeting));

            All.Add("1.2", UssdScreen.Input("Your details."
                , new List<UssdInput>()
                {
                    new UssdInput("FirstName", "First Name"),
                    new UssdInput("LastName", "Last Name", true),
                    new UssdInput("Company", new List<UssdInputOption>()
                    {
                        new UssdInputOption("SMSGH"),
                        new UssdInputOption("MPower"),
                        new UssdInputOption("JumpFon"),
                    })
                }, InputProcessors.CustomGreeting));

            All.Add("1.3", UssdScreen.Menu("Another menu", ScreenResponses.Menus.AnotherMenu));
        }
    }
}