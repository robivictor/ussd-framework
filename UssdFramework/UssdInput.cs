using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UssdFramework
{
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
        /// Initialize with only input's name.
        /// </summary>
        /// <param name="name">Input's name.</param>
        public UssdInput(string name)
        {
            Name = name;
            DisplayName = name;
        }

        /// <summary>
        /// Initialize with input's name and displayed name.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="displayName">Input's displayed name.</param>
        public UssdInput(string name, string displayName) : this(name)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Initalize with input's name, displayed name and list of options.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="displayName">Input's displayed name.</param>
        /// <param name="options">List of input options.</param>
        public UssdInput(string name, string displayName, List<UssdInputOption> options) : this(name, displayName)
        {
            Options = options;
        }

        /// <summary>
        /// Initialize with input's name and list of options.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="options">List of input options.</param>
        public UssdInput(string name, List<UssdInputOption> options) : this(name)
        {
            Options = options;
        }
    }
}
