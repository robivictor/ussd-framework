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
        public string Name { get; private set; }
        /// <summary>
        /// StackExchange.Redis instance.
        /// </summary>
        public IDatabase Redis { get; private set; }
        /// <summary>
        /// Dictionary containing USSD screens.
        /// </summary>
        public Dictionary<string, UssdScreen> UssdScreens { get; set; }
        /// <summary>
        /// Salt used for encrypting inputs.
        /// </summary>
        public string EncryptionSalt { get; private set; }

        private const string DefaultSalt = "6f4a68e0-44be-11e4-916c-0800200c9a66";

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address of Redis store. Uses StackExchange.Redis underneath.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD screens</param></param>
        public Setup(string name, string redisAddress, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase();
            UssdScreens = screens;
            EncryptionSalt = DefaultSalt;
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address of Redis store. Uses StackExchange.Redis underneath.</param>
        /// <param name="encryptionSalt">Salt used for input encryption.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD screens</param></param>
        public Setup(string name, string redisAddress, string encryptionSalt, Dictionary<string, UssdScreen> screens)
            : this(name, redisAddress, screens)
        {
            EncryptionSalt = encryptionSalt;
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        /// <param name="redisDatabaseNumber">Redis database number.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        public Setup(string name, string redisAddress, int redisDatabaseNumber, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase(redisDatabaseNumber);
            UssdScreens = screens;
            EncryptionSalt = DefaultSalt;
        }

        /// <summary>
        /// Setup initializer.
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        /// <param name="redisDatabaseNumber">Redis database number.</param>
        /// <param name="encryptionSalt">Salt used for input encryption.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        public Setup(string name, string redisAddress, int redisDatabaseNumber, string encryptionSalt, Dictionary<string, UssdScreen> screens)
        {
            Name = name;
            Redis = ConnectionMultiplexer.Connect(redisAddress).GetDatabase(redisDatabaseNumber);
            UssdScreens = screens;
            EncryptionSalt = encryptionSalt;
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        public Setup(string name, Dictionary<string, UssdScreen> screens, string redisAddress) 
            : this(name, redisAddress, screens)
        {
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        /// <param name="encryptionSalt">Salt used for input encryption.</param>
        public Setup(string name, Dictionary<string, UssdScreen> screens, string redisAddress, string encryptionSalt)
            : this(name, redisAddress, encryptionSalt, screens)
        {
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        /// <param name="redisDatabaseNumber">Redis database number.</param>
        public Setup(string name, Dictionary<string, UssdScreen> screens, string redisAddress, int redisDatabaseNumber)
            : this(name, redisAddress, redisDatabaseNumber, screens)
        {
        }

        /// <summary>
        /// Setup initializer
        /// </summary>
        /// <param name="name">App name.</param>
        /// <param name="screens">A dictionary containing <param name="screens">USSD Screens</param></param>
        /// <param name="redisAddress">Address/location of Redis instance. Uses StackExchange.Redis underneath.</param>
        /// <param name="redisDatabaseNumber">Redis database number.</param>
        /// <param name="encryptionSalt">Salt used for input encryption.</param>
        public Setup(string name, Dictionary<string, UssdScreen> screens, string redisAddress, int redisDatabaseNumber, string encryptionSalt)
            : this(name, redisAddress, redisDatabaseNumber, encryptionSalt, screens)
        {
        }
    }
}
