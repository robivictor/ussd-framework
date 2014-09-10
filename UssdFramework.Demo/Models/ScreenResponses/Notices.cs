using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UssdFramework.Demo.Models.ScreenResponses
{
    public class Notices
    {
        public static async Task<UssdResponse> SimpleGreeting(Session session)
        {
            return UssdResponse.Notice("Hello Boss!");
        }
    }
}