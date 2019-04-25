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
        // Name of the folder containing the scripts
        const string scriptSubfolder = "/Scripts/",
            // String that separates the multiple entries
            entrySeparator = "――――――",
            userScript = "getUser.ps1";

        readonly string path;
        readonly string[] chooseItemText = { "Il y a ", "s à choix." };
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
                "Nom Complet", "Prénom", "Addresse E-mail"
            },
            computerOptions = new Collection<string>() { "Nom DNS" },
            currentList = new Collection<string>();

        string psText;
        string[] psTexts;
        bool isUser = true;
        Dictionary<string, bool> options;
        PowerShell ps;

        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
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
            searchTextBox.Select();
            filterList.SelectedItem = "Identifiant";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            psText = "";
            string userPart = searchTextBox.Text, selected, scriptName, scriptContent;
            if (userPart.Equals(""))
            {
                // A user should be entered
                statusLabel.Text = "Erreur: Aucune donnée entrée.";
                statusLabel.Visible = true;
                return;
            }
            // Create a new powershell
            ps = PowerShell.Create();
            scriptName = userScript;
            string currentPath = path + scriptSubfolder + scriptName;

            // Read script
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
            if (isUser)
            {
                selected = "utilisateur";
            }
            else
            {
                selected = "ordinateur";
                scriptContent = scriptContent.Replace("Get-ADUser", "Get-ADComputer");
            }
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
            if (sender == ownWindowButton)
            {
                // Show the results in their own window
                scriptContent = scriptContent.Replace("Out-String", "Out-Gridview -Title 'Informations sur les " + selected + "s'");
                // TODO: Find out how to show only some part of the items
            }
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
            if (psText.Equals("") && sender != ownWindowButton)
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
                ifMultipleLabel.Enabled = true;
                whichNumberUD.Enabled = true;
                multipleCheckBox.Enabled = true;
                psTexts = psText.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var selectedUser = whichNumberUD.Value;
                ifMultipleLabel.Text = chooseItemText[0] + psTexts.Length + " " + selected + chooseItemText[1];
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
            else
            {
                ifMultipleLabel.Enabled = false;
                whichNumberUD.Enabled = false;
                multipleCheckBox.Enabled = false;
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
        /// <summary>
        /// Converts psText into a string.
        /// </summary>
        /// <returns>A formatted string of the item.</returns>
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

        /// <summary>
        /// Loads psTexts and converts every entry into a chunk of string.
        /// Uses separator for making sure that you're not seeing everything glued
        /// </summary>
        /// <returns>A formatted string with all the items</returns>
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
                    resultText += "\r\n" + entrySeparator + "\r\n\r\n";
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
                        resultText += "\r\n" + entrySeparator + "\r\n\r\n";
                    }
                }
            }
            // Remove last separator and trim
            resultText = resultText.Substring(0, resultText.LastIndexOf(entrySeparator)).Trim();
            // Return the result text
            return resultText;
        }

        // Other functions
        /// <summary>
        /// Using psText, it makes a dictionary of all the content from "key : value"
        /// </summary>
        /// <returns>A ready to use dictionary for output text</returns>
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

        private void MultipleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Button1_Click(sender, null);
        }

        /// <summary>
        /// Using psTexts, it makes an array of dictionaries that have all the content from "key : value"
        /// </summary>
        /// <returns>A ready to use array of dictionaries for output text</returns>
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

        /// <summary>
        /// Checks all the options and updates the option dictionary
        /// </summary>
        /// <returns>Whether there is at least one option enabled or not</returns>
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

        /// <summary>
        /// Updates the content boxes that have all user options.
        /// </summary>
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
                filterList.SelectedItem = "Identifiant";
            }
        }

        /// <summary>
        /// Updates the collection currentlist
        /// </summary>
        /// <param name="otherlist">The additional list to add in the currentlist</param>
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
