using System;
using System.Management.Automation; // enables PowerShell
using System.IO;  // enables Stream Reader
using System.Windows.Forms;

namespace PowerShellUI1 {

    public partial class Form1 : Form {
        const string scriptPath = "\\Scripts\\";
        string scriptName = "foo.ps1";

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void Button1_Click(object sender, EventArgs e) {
            // Create a new powershell
            PowerShell ps = PowerShell.Create();

            // Get the current path
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface
            while (!path.EndsWith("AD_interface")) {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
            // Set path on the current script
            path += scriptPath + scriptName;

            // Read script
            string scriptContent = "";
            using (StreamReader strReader = new StreamReader(path)) {
                scriptContent = strReader.ReadToEnd();
            }
            // Launch script
            ps.AddScript(scriptContent);
            try {
                IAsyncResult psAsyncResult = ps.BeginInvoke();
            } catch {
                Exception error = new Exception();
            }
        }
    }
}
