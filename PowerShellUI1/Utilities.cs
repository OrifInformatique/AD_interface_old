using System;
using System.IO;
using System.Management.Automation;

namespace PowerShellUI1
{
    /// <summary>
    /// Class containing multiple useful methods/values
    /// that are used through the application
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// String that separates the multiple entries.
        /// </summary>
        public static string ResultEntrySeparator => "――――――";

        /// <summary>
        /// Obtains the output from a powershell script.
        /// </summary>
        /// <param name="script">Powershell script to execute</param>
        /// <returns>Output of the script</returns>
        public static string GetScriptResults(string script)
        {
            string res = "";
            // Create a new PowerShell, load the script, launch it and obtain the output
            using (PowerShell ps = PowerShell.Create().AddScript(script))
            {
                // Get results from the PowerShell
                var results = ps.Invoke();
                foreach (PSObject result in results)
                {
                    // Add the results to the return value
                    res += result;
                }
            }
            return res;
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