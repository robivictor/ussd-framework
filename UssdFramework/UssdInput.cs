using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
    /// <summary>
    /// Input parameter.
    /// </summary>
    public class UssdInput
    {
        /// <summary>
        /// Input's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Input's displayed name.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// List of input's options.
        /// </summary>
        public List<UssdInputOption> Options { get; set; }
        /// <summary>
        /// Check if input has options.
        /// </summary>
        public bool HasOptions { get { return !(Options == null || Options.Count == 0); } }
        /// <summary>
        /// Encrypt input flag.
        /// </summary>
        public bool Encrypt { get; private set; }
     

        /// <summary>
        /// Initialize with only input's name.
        /// </summary>
        /// <param name="name">Input's name.</param>
        public UssdInput(string name)
            : this(name, name, false)
        {
        }

        /// <summary>
        /// Initialize with input's name and displayed name.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="displayName">Input's displayed name.</param>
        public UssdInput(string name, string displayName) 
            : this(name, displayName, false)
        {
        }

        /// <summary>
        /// Initialize encrypted input.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="encrypt"></param>
        public UssdInput(string name, bool encrypt)
            : this(name, name, encrypt)
        {
        }

        /// <summary>
        /// Initialize with input's name and list of options.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="options">List of input options.</param>
        public UssdInput(string name, List<UssdInputOption> options)
            : this(name)
        {
            Options = options;
        }

        /// <summary>
        /// Initialize encrypted input.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="displayName">Input's displayed name.</param>
        /// <param name="encrypt">Encryption flag.</param>
        public UssdInput(string name, string displayName, bool encrypt)
        {
            Name = name;
            DisplayName = displayName;
            Encrypt = encrypt;
        }

        /// <summary>
        /// Initalize with input's name, displayed name and list of options.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="displayName">Input's displayed name.</param>
        /// <param name="options">List of input options.</param>
        public UssdInput(string name, string displayName, List<UssdInputOption> options) 
            : this(name, displayName)
        {
            Options = options;
        }
    }
}
