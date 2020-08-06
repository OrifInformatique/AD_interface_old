using System;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace PowerShellUI1
{
    /// <summary>
    /// Class containing multiple useful methods/values
    /// that are used through the application
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// The subfolder containing all scripts.
        /// </summary>
        public const string SCRIPT_SUBFOLDER = "\\Scripts\\";

        /// <summary>
        /// String that separates the multiple entries.
        /// </summary>
        public const string RESULT_ENTRY_SEPARATOR = "――――――";

        /// <summary>
        /// Obtains the output from a powershell script.
        /// </summary>
        /// <param name="script">Powershell script to execute</param>
        /// <returns>Output of the script</returns>
        public static string GetScriptResults(string script)
        {
            StringBuilder res = new StringBuilder();
            // Create a new PowerShell, load the script, launch it and obtain the output
            using (PowerShell ps = PowerShell.Create().AddScript(script))
            {
                // Get results from the PowerShell
                System.Collections.ObjectModel.Collection<PSObject> results = ps.Invoke();
                foreach (PSObject result in results)
                {
                    // Add the results to the return value
                    _ = res.Append(result);
                }
            }
            return res.ToString();
        }

        /// <summary>
        /// Returns the contents of a file
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <returns>The contents of the file</returns>
        public static string GetFileContents(string filepath)
        {
            string content = "";
            try
            {
                using (StreamReader strReader = new StreamReader(filepath))
                {
                    content = strReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                if (e is IOException ||
                    e is OutOfMemoryException)
                {
                    return "";
                }
            }
            return content;
        }
    }
}