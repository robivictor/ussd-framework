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
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
        public string ClientState { get; set; }

        /// <summary>
        /// Generate an appropriate USSD response.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static UssdResponse Generate(UssdResponseTypes type, string message)
        {
            return new UssdResponse()
            {
                Type = type.ToString(),
                Message = message
            };
        }
    }
}
