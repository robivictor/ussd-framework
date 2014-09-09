using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    public class UssdRequest
    {
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string ServiceCode { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Operator { get; set; }
        [Required]
        public int Sequence { get; set; }
        public string ClientState { get; set; }
    }
}
