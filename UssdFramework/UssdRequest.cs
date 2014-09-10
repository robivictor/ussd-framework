using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    /// <summary>
    /// Incoming USSD request model.
    /// </summary>
    public class UssdRequest
    {
        /// <summary>
        /// Mobile number from which USSD request originated.
        /// </summary>
        [Required]
        public string Mobile { get; set; }
        /// <summary>
        /// USSD session ID.
        /// </summary>
        [Required]
        public string SessionId { get; set; }
        /// <summary>
        /// Service Code user dialed to access the client.
        /// </summary>
        [Required]
        public string ServiceCode { get; set; }
        /// <summary>
        /// Type of USSD request. Can be "Initiation", "Response", "Release" or "Timeout".
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// Message attached to USSD request.
        /// </summary>
        [Required]
        public string Message { get; set; }
        /// <summary>
        /// Mobile network or operator the request is from.
        /// </summary>
        [Required]
        public string Operator { get; set; }
        /// <summary>
        /// Sequence number of the USSD request.
        /// </summary>
        [Required]
        public int Sequence { get; set; }
        public string ClientState { get; set; }
    }
}
