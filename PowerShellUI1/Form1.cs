using System;
using System.Management.Automation; // Enables PowerShell
using System.IO;  // Enables Stream Reader
using System.Windows.Forms;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;

namespace PowerShellUI1
{

    public partial class Form1 : Form
    {
        const string scriptPath = "/Scripts/";
        readonly Dictionary<string, string> convert = new Dictionary<string, string>() {
            {"nom", "Surname"},
            {"prénom", "GivenName"},
            {"identifiant", "SamAccountName"},
            {"adresse e-mail", "UserPrincipalName"},
            {"nom technique", "DistinguishedName"},
            {"actif", "Enabled"},
            {"nom complet", "Name"},
            {"sid", "SID"}
        };

        string scriptName, userName, psText;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            psText = "";
            // Create a new powershell
            PowerShell ps = PowerShell.Create();

            // Get the current path
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!path.EndsWith("AD_interface"))
            {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
            // Set the script
            userName = usernameTextBox.Text;
            if (userName.Equals(""))
            {
                // A user should be entered
                statusLabel.Text = "Erreur: Aucune donnée d'utilisateur entré.\n";
                statusLabel.Visible = true;
                return;
            }
            scriptName = "getUser.ps1";
            // Set path on the current script
            path += scriptPath + scriptName;

            // Read script
            string scriptContent = "";
            try
            {
                using (StreamReader strReader = new StreamReader(path)) {
                    scriptContent = strReader.ReadToEnd();
                }
            }
            catch (FileNotFoundException d)
            {
                Console.Write(d.Message);
                // Problem with opening the script, tell the user
                statusLabel.Text = "Erreur: Fichier '" + scriptName + "' n'est pas dans /Scripts!";
                statusLabel.Visible = true;
                return;
            }
            string filter = "SamAccountName";
            if (filterList.SelectedItem != null && convert.ContainsKey((filterList.SelectedItem as string).ToLower()))
            {
                filter = convert[(filterList.SelectedItem as string).ToLower()];
            }
            scriptContent = scriptContent.Replace("{username}", userName).Replace("{FilterSelection}", filter);
            // Launch script
            ps.AddScript(scriptContent);
            try
            {
                var results = ps.Invoke();
                foreach(var result in results) {
                    psText += result;
                }
            }
            catch
            {
                // Problem in reading the script, tell the user
                statusLabel.Text = "Erreur: problème dans l'execution du script \"" + scriptName + "\".";
                statusLabel.Visible = true;
                return;
            }
            if (psText.Equals(""))
            {
                statusLabel.Text = "Erreur: ";
                if (filterList.SelectedItem != null) {
                    statusLabel.Text += filterList.SelectedItem as string;
                } else {
                    statusLabel.Text += "utilisateur";
                }
                statusLabel.Text += " \"" + userName + "\" n'existe pas.";
                statusLabel.Visible = true;
                return;
            }

            // Load only the text needed
            psText = psText.Replace("=", " = ").Trim();
            // Isolate the first user
            if (psText.IndexOf("\r\n\r\n") != -1)
            {
                var psTexts = psText.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var selectedUser = whichNumberUD.Value;
                if(psTexts.Length >= selectedUser)
                {
                    psText = psTexts[(int) selectedUser-1];
                }
                else
                {
                    statusLabel.Text = "Danger: Il y a moins que " + selectedUser.ToString() + " utilisateurs.\n"+
                        "Le premier utilisateur a été sélectionné.";
                    statusLabel.Visible = true;
                    psText = psTexts[0];
                }
            }
            var resultTexts = GetResultTexts();
            Dictionary<string, bool> options = new Dictionary<string, bool>() {
                {"nom", false},
                {"prénom", false},
                {"identifiant", false},
                {"adresse e-mail", false},
                {"nom technique", false},
                {"actif", false},
                {"nom complet", false},
                {"sid", false}
            };
            foreach (object itemchecked in optionsListBox.CheckedItems)
            {
                string s = (itemchecked as string).ToLower();
                options[s] = true;
            }
            string resultText = "";
            foreach(KeyValuePair<string, bool> entry in options)
            {
                if(!entry.Value) {
                    continue;
                }
                resultText += resultTexts[convert[entry.Key]];
            }
            if(resultText.Equals(""))
            {
                foreach(KeyValuePair<string, string> entry in convert)
                {
                    if (resultTexts.ContainsKey(entry.Value))
                        resultText += resultTexts[entry.Value];
                }
            }
            resultTextBox.Text = resultText;
        }

        private Dictionary<string, string> GetResultTexts() {
            Dictionary<string, string> res = new Dictionary<string, string>();
            Dictionary<string, string> iconvert = new Dictionary<string, string>();
            foreach(KeyValuePair<string, string> entry in convert)
            {
                iconvert[entry.Value] = entry.Key;
            }
            foreach (string s in psText.Split('\n'))
            {
                string key = s.Split(':')[0].Trim();
                string value = "";
                if(s.Split(':').Length > 1 && iconvert.ContainsKey(key))
                {
                    string k = iconvert[key] + " : ";
                    value += char.ToUpper(k[0]) + k.Substring(1);
                    value += s.Split(':')[1]
                        .Trim()
                        .Replace(",", "\n\t")
                        .Replace("True", "Oui")
                        .Replace("False", "Non") + "\n";
                }
                res.Add(key, value);
            }
            return res;
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
