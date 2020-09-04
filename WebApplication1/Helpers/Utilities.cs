using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace WebApplication1.Helpers
{
    public static class Utilities
    {
        public static string GetScriptResults(string script)
        {
            PowerShell powershell = PowerShell.Create();
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            powershell.Runspace = runspace;
            powershell.AddScript(script);
            StringBuilder res = new StringBuilder();
            var results = powershell.Invoke();
            foreach (PSObject result in results)
            {
                res.Append(result);
            }
            return res.ToString();
        }
    }
}