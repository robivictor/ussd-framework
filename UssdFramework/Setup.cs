using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UssdFramework
{
    /// <summary>
    /// Setup a session.
    /// </summary>
    public class Setup
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// StackExchange.Redis instance.
        /// </summary>
        public IDatabase Redis { get; set; }
        /// <summary>
        /// Dictionary containing USSD screens.
        /// </summary>
        public Dictionary<string, UssdScreen> UssdScreens { get; set; }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address of Redis store. Uses StackExchange.Redis underneath.</param>
        /// <param name="screens">A dictionary containing <param name="screens"></param></param>
        public Setup(string name, string redisAddress, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase();
            UssdScreens = screens;
        }
    }
}
