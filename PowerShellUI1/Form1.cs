using System;
using System.Management.Automation; // Enables PowerShell
using System.IO;  // Enables Stream Reader
using System.Windows.Forms;
using System.Drawing; // For colors

namespace PowerShellUI1 {

    public partial class Form1 : Form {
        const string scriptPath = "/Scripts/";
        string scriptName;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void Button1_Click(object sender, EventArgs e) {
            statusLabel.BackColor = Color.FromKnownColor(KnownColor.Salmon);
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
            } else if(!scriptName.EndsWith(".ps1")) {
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
            } catch (FileNotFoundException d) {
                Console.Write(d.Message);
                // Problem with opening the script, tell the user
                statusLabel.Text = "Erreur: Fichier '" + scriptName + "' n'est pas dans /Scripts!\n" +
                    "Utilisez '/' pour les sous-dossiers";
                return;
            }
            // Launch script
            ps.AddScript(scriptContent);
            try {
                IAsyncResult psAsyncResult = ps.BeginInvoke();
            } catch {
                // Problem in reading the script, tell the user
                statusLabel.Text = "Erreur: problème dans l'execution du script '" + scriptName + "'.";
                return;
            }
            // Everything went well
            statusLabel.BackColor = Color.FromKnownColor(KnownColor.GreenYellow);
            statusLabel.Text = "Le script '" + scriptName + "' à été lancé.";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.Enter:
                    Button1_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void ScriptTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    Button1_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }
    }
}
