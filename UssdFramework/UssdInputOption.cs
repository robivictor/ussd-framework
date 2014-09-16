using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    /// <summary>
    /// Input parameter option.
    /// </summary>
    public class UssdInputOption
    {
        /// <summary>
        /// Input option's name.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Input option's displayed name.
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Initialize with input value.   
        /// </summary>
        /// <param name="value">Input value.</param>
        public UssdInputOption(string value)
        {
            Value = value;
            DisplayValue = value;
        }

        /// <summary>
        /// Initialize with input's value and displayed value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <param name="displayValue">Displayed input value.</param>
        public UssdInputOption(string value, string displayValue) : this(value)
        {
            DisplayValue = displayValue;
        }
    }
}
