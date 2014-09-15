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
            All.Add("1", new UssdScreen()
            {
                Title = "Main menu",
                Type = UssdScreenTypes.Menu,
                RespondAsync = ScreenResponses.Menus.MainMenu,
            });

            All.Add("1.1",  new UssdScreen()
            {
                Title = "Simple Greeting",
                Type = UssdScreenTypes.Notice,
                RespondAsync = ScreenResponses.Notices.SimpleGreeting,
            });

            All.Add("1.2", new UssdScreen()
            {
                Title = "Enter your name",
                Type = UssdScreenTypes.Input,
                Inputs = new List<string>()
                {
                    "First Name",
                    "Last Name",
                },
                InputProcessorAsync = InputProcessors.CustomGreeting
            });

            All.Add("1.3", new UssdScreen()
            {
                Title = "Another menu",
                Type = UssdScreenTypes.Menu,
                RespondAsync = ScreenResponses.Menus.AnotherMenu
            });
        }
    }
}