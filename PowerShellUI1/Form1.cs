using System;
using System.Management.Automation; // Enables PowerShell
using System.IO;  // Enables Stream Reader
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerShellUI1
{

    public partial class Form1 : Form
    {
        const string scriptPath = "/Scripts/",
            separator = "――――――",
            computerScript = "getComputer.ps1",
            userScript = "getUser.ps1";

        readonly string path;
        readonly Dictionary<string, string> convert = new Dictionary<string, string>() {
            {"nom", "Surname"},
            {"prénom", "GivenName"},
            {"identifiant", "SamAccountName"},
            {"adresse e-mail", "UserPrincipalName"},
            {"nom technique", "DistinguishedName"},
            {"actif", "Enabled"},
            {"nom complet", "Name"},
            {"sid", "SID"},
            {"nom dns", "DNSHostName"}
        },
            iconvert;
        readonly Collection<string> sharedOptions = new Collection<string>() {
            "Nom Technique", "Actif", "Nom", "Identifiant", "SID"
        },
            userOptions = new Collection<string>() {
                "Nom Complet", "Prénom", "addresse E-mail"
            },
            computerOptions = new Collection<string>() { "Nom DNS" },
            currentList = new Collection<string>();

        string scriptName, userPart, psText;
        string[] psTexts;
        Dictionary<string, bool> options;
        bool isUser = true;
        PowerShell ps;

        public Form1()
        {
            InitializeComponent();
            iconvert = new Dictionary<string, string>();
            // Reverse convert dictionary
            foreach (KeyValuePair<string, string> entry in convert)
            {
                iconvert[entry.Value] = entry.Key;
            }
            UpdateCurrentList(userOptions);
            UpdateOptionBoxes();

            // Get the current path
            path = Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!path.EndsWith("AD_interface"))
            {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            psText = "";
            userPart = searchTextBox.Text;
            if (userPart.Equals(""))
            {
                // A user should be entered
                statusLabel.Text = "Erreur: Aucune donnée entrée.";
                statusLabel.Visible = true;
                return;
            }
            // Create a new powershell
            ps = PowerShell.Create();
            string selected;
            if (isUser)
            {
                scriptName = userScript;
                selected = "utilisateur";
            }
            else
            {
                scriptName = computerScript;
                selected = "ordinateur";
            }
            string currentPath = path + scriptPath + scriptName;

            // Read script
            string scriptContent = "";
            try
            {
                using (StreamReader strReader = new StreamReader(currentPath))
                {
                    scriptContent = strReader.ReadToEnd();
                }
            }
            catch (FileNotFoundException d)
            {
                Console.Write(d.Message);
                // Problem with the script, tell the user
                statusLabel.Text = "Erreur: Fichier '" + scriptName + "' n'est pas dans /Scripts!";
                statusLabel.Visible = true;
                return;
            }
            // Set the filter
            string filterSelection = "SamAccountName";
            if (filterList.SelectedItem != null)
            {
                if (convert.ContainsKey((filterList.SelectedItem as string).ToLower()))
                {
                    filterSelection = convert[(filterList.SelectedItem as string).ToLower()];
                }
                if ((filterList.SelectedItem as string).ToLower().Equals("actif"))
                {
                    scriptContent = scriptContent.Replace("-like", "-eq");
                    if (userPart.ToLower().Contains("oui") || userPart.ToLower().Contains("non"))
                    {
                        userPart = userPart.ToLower().Replace("oui", "True").Replace("non", "False");
                    }
                }
            }
            scriptContent = scriptContent.Replace("{part}", userPart).Replace("{FilterSelection}", filterSelection);
            // Launch script
            ps.AddScript(scriptContent);
            try
            {
                var results = ps.Invoke();
                foreach (var result in results)
                {
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
            ps.Dispose();
            // Nothing was found
            if (psText.Equals(""))
            {
                statusLabel.Text = "Erreur: ";
                if (filterList.SelectedItem != null)
                {
                    statusLabel.Text += filterList.SelectedItem as string;
                }
                else
                {
                    statusLabel.Text += selected;
                }
                statusLabel.Text += " \"" + userPart + "\" n'existe pas.";
                statusLabel.Visible = true;
                return;
            }

            // Load only the text needed
            psText = psText.Replace("=", " = ").Trim();
            // Isolate the users
            if (psText.IndexOf("\r\n\r\n") != -1)
            {
                psTexts = psText.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var selectedUser = whichNumberUD.Value;
                if (psTexts.Length >= selectedUser)
                {
                    psText = psTexts[(int)selectedUser - 1];
                }
                else
                {
                    // The user selected a user that doesn't exist
                    statusLabel.Text = "Danger: Il y a moins que " + selectedUser.ToString() + " " + selected + ".\n" +
                        "Le premier " + selected + " a été sélectionné.";
                    statusLabel.Visible = true;
                    psText = psTexts[0];
                }
            }
            if (!multipleCheckBox.Checked || psTexts.Length == 1)
            {
                resultTextBox.Text = GetItem();
            }
            else
            {
                resultTextBox.Text = GetItems();
            }
        }

        // User-related functions
        private string GetItem()
        {
            // Load the prepared text
            var resultTexts = GetResultText();
            string resultText = "";
            // Verify which options are checked
            if (CheckOptions())
            {
                foreach (KeyValuePair<string, bool> entry in options)
                {
                    if (entry.Value)
                    {
                        resultText += resultTexts[convert[entry.Key]];
                    }
                }
            }
            else
            {
                // If nothing was checked, just give all the data
                if (resultText.Equals(""))
                {
                    foreach (KeyValuePair<string, string> entry in convert)
                    {
                        if (resultTexts.ContainsKey(entry.Value))
                            resultText += resultTexts[entry.Value];
                    }
                }
            }
            // Return the result text
            return resultText;
        }

        private string GetItems()
        {
            // Load the prepared texts
            var resultTexts = GetResultTexts();
            string resultText = "";
            // Verify which options are checked
            if (CheckOptions())
            {
                foreach (var texts in resultTexts)
                {
                    foreach (KeyValuePair<string, bool> entry in options)
                    {
                        if (entry.Value)
                        {
                            resultText += texts[convert[entry.Key]];
                        }
                    }
                    resultText += "\r\n" + separator + "\r\n\r\n";
                }
            }
            else
            {
                // If nothing was checked, just give all the data
                if (resultText.Equals(""))
                {
                    foreach (var texts in resultTexts)
                    {
                        foreach (KeyValuePair<string, string> entry in convert)
                        {
                            if (texts.ContainsKey(entry.Value))
                                resultText += texts[entry.Value];
                        }
                        resultText += "\r\n" + separator + "\r\n\r\n";
                    }
                }
            }
            // Remove last separator and trim
            resultText = resultText.Substring(0, resultText.LastIndexOf(separator)).Trim();
            // Return the result text
            return resultText;
        }

        // Other functions
        private Dictionary<string, string> GetResultText()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (string s in psText.Split('\n'))
            {
                string key = s.Split(':')[0].Trim();
                string value = "";
                if(s.Split(':').Length > 1 && iconvert.ContainsKey(key))
                {
                    string k = iconvert[key] + " : ";
                    value += char.ToUpper(k[0]) + k.Substring(1)
                        + s.Split(':')[1]
                        .Trim()
                        .Replace(",", "\n\t")
                        .Replace("True", "Oui")
                        .Replace("False", "Non") + "\n";
                }
                res.Add(key, value);
            }
            return res;
        }

        private Dictionary<string, string>[] GetResultTexts()
        {
            Dictionary<string, string>[] res = new Dictionary<string, string>[psTexts.Length];
            // Load dictionary array with a dictionary
            foreach (string text in psTexts)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                int index = Array.FindIndex(psTexts, row => row.Equals(text));
                foreach (string s in text.Split('\n'))
                {
                    string key = s.Split(':')[0].Trim();
                    string value = "";
                    if (s.Split(':').Length > 1 && iconvert.ContainsKey(key))
                    {
                        string k = iconvert[key] + " : ";
                        value += char.ToUpper(k[0]) + k.Substring(1);
                        value += s.Split(':')[1]
                            .Trim()
                            .Replace(",", "\n\t")
                            .Replace("True", "Oui")
                            .Replace("False", "Non") + "\n";
                    }
                    result.Add(key, value);
                }
                res[index] = result;
            }
            return res;
        }

        private bool CheckOptions()
        {
            bool anyTrue = false;
            // Display according to checkboxes
            options = new Dictionary<string, bool>() {
                {"nom", false},
                {"prénom", false},
                {"identifiant", false},
                {"adresse e-mail", false},
                {"nom technique", false},
                {"actif", false},
                {"nom complet", false},
                {"sid", false},
                {"nom dns", false}
            };
            foreach (object itemchecked in optionsListBox.CheckedItems)
            {
                string s = (itemchecked as string).ToLower();
                options[s] = true;
                anyTrue = true;
            }
            return anyTrue;
        }

        private void UpdateOptionBoxes()
        {
            if (currentList != null)
            {
                optionsListBox.Items.Clear();
                filterList.Items.Clear();
                foreach (var item in currentList)
                {
                    optionsListBox.Items.Add(item);
                    filterList.Items.Add(item);
                }
                optionsListBox.Refresh();
                filterList.Refresh();
            }
        }

        private void UpdateCurrentList(Collection<string> otherlist)
        {
            currentList.Clear();

            foreach (string s in sharedOptions)
            {
                currentList.Add(s);
            }
            foreach(string s in otherlist)
            {
                currentList.Add(s);
            }
        }

        private void UserRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (userRButton.Checked)
            {
                isUser = true;
                getItemButton.Text = "Obtenir les informations de l'utilisateur";
                computerRButton.Checked = false;
                ifMultipleLabel.Text = "S'il y a plusieurs utilisateurs, choisir le quel montrer";
                UpdateCurrentList(userOptions);
                UpdateOptionBoxes();
            }
        }

        private void ComputerRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (computerRButton.Checked)
            {
                isUser = false;
                getItemButton.Text = "Obtenir les informations de l'ordinateur";
                userRButton.Checked = false;
                ifMultipleLabel.Text = "S'il y a plusieurs ordinateurs, choisir le quel montrer";
                UpdateCurrentList(computerOptions);
                UpdateOptionBoxes();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode) {
                case Keys.Enter:
                    Button1_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void ScriptTextBox_KeyDown(object sender, KeyEventArgs e)
        {
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
