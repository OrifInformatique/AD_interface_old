using System;
using System.Management.Automation; // enables PowerShell
using System.IO;  // enables Stream Reader
using System.Windows.Forms;
using System.Drawing;

namespace PowerShellUI1 {

    public partial class Form1 : Form {
        const string scriptPath = "\\Scripts\\";
        string scriptName;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void Button1_Click(object sender, EventArgs e) {
            statusLabel.Visible = false;
            // Create a new powershell
            PowerShell ps = PowerShell.Create();

            // Get the current path
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface
            while (!path.EndsWith("AD_interface")) {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
            // Set the script
            scriptName = scriptTextBox.Text;
            if(scriptName.Equals("")) {
                scriptName = "foo.ps1";
            }
            if(!scriptName.EndsWith(".ps1")) {
                scriptName += ".ps1";
            }
            // Set path on the current script
            path += scriptPath + scriptName;

            // Read script
            string scriptContent = "";
            try {
                using (StreamReader strReader = new StreamReader(path)) {
                    scriptContent = strReader.ReadToEnd();
                }
            } catch (FileNotFoundException) {
                statusLabel.BackColor = Color.FromKnownColor(KnownColor.Salmon);
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur: Fichier '" + scriptName + "' n'est pas dans /Scripts!";
                return;
            }
            // Launch script
            ps.AddScript(scriptContent);
            try {
                IAsyncResult psAsyncResult = ps.BeginInvoke();
            } catch {
                statusLabel.BackColor = Color.FromKnownColor(KnownColor.Salmon);
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur: problème dans l'execution du script '" + scriptName + "'.";
                return;
            }
            statusLabel.BackColor = Color.FromKnownColor(KnownColor.GreenYellow);
            statusLabel.Visible = true;
            statusLabel.Text = "Fichier '" + scriptName + "' à été lancé.";
        }
    }
}
