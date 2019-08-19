using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

namespace PowerShellUI1
{
    /// <summary>
    /// Class containing multiple useful methods/values
    /// that are used through the application
    /// </summary>
    internal class Utilities
    {

        /// <summary>
        /// String that separates the multiple entries.
        /// </summary>
        public static string ResultEntrySeparator => "――――――";

        /// <summary>
        /// Creates a new instance of Utilities.
        /// Illegal.
        /// </summary>
        /// <throws>An error because you should not have to use an instance</throws>
        public Utilities()
        {
            throw new System.Exception("Cannot create a new instance of Utilities");
        }

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
                Collection<PSObject> results = ps.Invoke();
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
        /// <param name="file">The path to the file</param>
        /// <returns>The contents of the file</returns>
        public static string GetFileContents(string file)
        {
            string content = "";
            try
            {
                using(StreamReader strReader = new StreamReader(file))
                {
                    content = strReader.ReadToEnd();
                }
            }
            catch { }
            return content;
        }
    }
}
