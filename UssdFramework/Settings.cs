using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UssdFramework
{
    public class Settings
    {
        public string Name { get; set; }
        public IDatabase Redis { get; set; }
        public Dictionary<string, UssdScreen> UssdScreens { get; set; }

        public Settings(string name, string redisAddress, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase();
            UssdScreens = screens;
        }
    }
}
