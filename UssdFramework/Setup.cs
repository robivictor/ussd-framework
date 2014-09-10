using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UssdFramework
{
    /// <summary>
    /// Settings used to initialize a session appropriately.
    /// </summary>
    public class Setup
    {
        public string Name { get; set; }
        public IDatabase Redis { get; set; }
        public Dictionary<string, UssdScreen> UssdScreens { get; set; }

        /// <summary>
        /// Settings initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address of Redis store. Uses StackExchange.Redis.</param>
        /// <param name="screens">A dictionary of <see cref="UssdScreens"/></param>
        public Setup(string name, string redisAddress, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase();
            UssdScreens = screens;
        }
    }
}
