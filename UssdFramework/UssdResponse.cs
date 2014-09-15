using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    /// <summary>
    /// USSD response model.
    /// </summary>
    public class UssdResponse
    {
        /// <summary>
        /// Type of USSD response. Can be "Response" or "Release".
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// Message to send back in response.
        /// </summary>
        [Required]
        public string Message { get; set; }
        /// <summary>
        /// A value to be sent back to client on subsequent request.
        /// </summary>
        public string ClientState { get; set; }

        /// <summary>
        /// Generate an appropriate USSD response based on <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static UssdResponse Generate(UssdResponseTypes type, string message)
        {
            return new UssdResponse()
            {
                Type = type.ToString(),
                Message = message
            };
        }

        /// <summary>
        /// Generate a "Response" USSD response.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static UssdResponse Response(string message)
        {
            return Generate(UssdResponseTypes.Response, message);
        }

        /// <summary>
        /// Generate a "Release" USSD response.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static UssdResponse Release(string message)
        {
            return Generate(UssdResponseTypes.Release, message);
        }
    }
}
