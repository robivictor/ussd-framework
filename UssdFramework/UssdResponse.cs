using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    public class UssdResponse
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
        public string ClientState { get; set; }

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
