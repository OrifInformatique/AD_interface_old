using System;
using System.Management.Automation; // Enables PowerShell
using System.IO;  // Enables Stream Reader
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerShellUI1
{

    public partial class RetreiveForm : Form
    {
        #region variables
        // Name of the folder containing the scripts
        readonly string scriptSubfolder = ChoiceForm.ScriptSubfolder,
            // String that separates the multiple entries
            entrySeparator = "――――――",
            // Script that contains the logic behind the loading
            userScript = "getUser.ps1",
            // Path to scripts
            path;
        // Strings of text for the list of different items
        readonly string[] chooseItemText = { "Il y a ", "s à choix." };
        // Translates between powershell filters and french
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
            // Reverse of convert
            iconvert;
        // Options shared by all groups
        readonly Collection<string> sharedOptions = new Collection<string>() {
            "Nom Technique", "Actif", "Nom", "Identifiant", "SID"
        },
            // Options that only the users have
            userOptions = new Collection<string>() {
                "Nom Complet", "Prénom", "Addresse E-mail"
            },
            // Options that only the computers have
            computerOptions = new Collection<string>() { "Nom DNS" },
            // Current list of options for the user
            currentList = new Collection<string>();

        // Text retrieved from powershell
        string psText;
        // psText but cut where there are 2 new linews
        string[] psTexts;
        // Whether it's set on users or computers
        bool isUser = true;
        // options for the user
        Dictionary<string, bool> options;
        #endregion

        /// <summary>
        /// Creates a new <code>RetreiveForm</code>.
        /// </summary>
        public RetreiveForm()
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

        /// <summary>
        /// Creates a new <code>RetreiveForm</code>.
        /// </summary>
        /// <param name="path">The path to the base file.</param>
        public RetreiveForm(string path)
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
            this.path = path;

            searchTextBox.Select();
            filterList.SelectedItem = "Identifiant";
        }

        /// <summary>
        /// The main function of the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            psText = "";
            string userPart = searchTextBox.Text, selected, scriptName, scriptContent;
            if (userPart.Equals(""))
            {
                if (Array.IndexOf(new object[] { multipleCheckBox, userRButton, computerRButton }, sender) != -1)
                {
                    return;
                }
                // A user should be entered
                statusLabel.Text = "Erreur: Aucune donnée entrée.";
                statusLabel.Visible = true;
                return;
            }

            // Create a new powershell
            PowerShell ps = PowerShell.Create();
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
            catch
            {
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
            finally
            {
                ps.Dispose();
            }

            // In case nothing was returned from the script
            if (psText.Equals(""))
            {
                if (sender == ownWindowButton)
                {
                    return;
                }
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
                resultTextBox.Text = "";
                return;
            }
            // Load only the text needed
            psText = psText.Replace("=", " = ").Trim();
            // Isolate the users
            if (psText.IndexOf("\r\n\r\n") != -1)
            {
                // Ask the user to choose which item to show
                ifMultipleLabel.Enabled = true;
                whichNumberUD.Enabled = !multipleCheckBox.Checked;
                multipleCheckBox.Enabled = true;
                // Split the entries into an array of entries
                psTexts = psText.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var selectedUser = whichNumberUD.Value;
                ifMultipleLabel.Text = chooseItemText[0] + psTexts.Length + " " + selected + chooseItemText[1];
                // The user selected something in the list
                if (psTexts.Length >= selectedUser)
                {
                    psText = psTexts[(int)selectedUser - 1];
                }
                else
                {
                    // The user selected an item that doesn't exist
                    statusLabel.Text = "Danger: Il y a moins que " + selectedUser.ToString() + " " + selected + ".\n" +
                        "Le premier " + selected + " a été sélectionné.";
                    statusLabel.Visible = true;
                    psText = psTexts[0];
                }
            }
            else
            {
                // Disable choosing a specific item, there is only 1
                ifMultipleLabel.Enabled = false;
                whichNumberUD.Enabled = false;
                multipleCheckBox.Enabled = false;
                if (userRButton.Checked)
                {
                    ifMultipleLabel.Text = "S'il y a plusieurs utilisateurs, choisir le quel montrer";
                }
                else if (computerRButton.Checked)
                {
                    ifMultipleLabel.Text = "S'il y a plusieurs ordinateurs, choisir le quel montrer";
                }
            }

            if (!multipleCheckBox.Checked || psTexts.Length == 1)
            {
                // Only 1 item selected, display it to the user
                resultTextBox.Text = GetItem();
            }
            else
            {
                // The user wants to see all items
                resultTextBox.Text = GetItems();
            }
        }

        #region Item-related functions
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
        #endregion

        #region Other functions
        /// <summary>
        /// Using psText, it makes a dictionary of all the content from "key : value"
        /// </summary>
        /// <returns>A ready to use dictionary for output text</returns>
        private Dictionary<string, string> GetResultText()
        {
            // A dictionary for all results
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (string s in psText.Split('\n'))
            {
                string[] split = s.Split(':');
                string key = split[0].Trim();
                string value = "";
                // If there is something after the :
                if(split.Length > 1 && iconvert.ContainsKey(key))
                {
                    string k = iconvert[key] + " : ";
                    value += char.ToUpper(k[0]) + k.Substring(1)
                        + split[1]
                        .Trim()
                        .Replace(",", "\n\t")
                        .Replace("True", "Oui")
                        .Replace("False", "Non") + "\n";
                }
                res.Add(key, value);
            }
            return res;
        }

        /// <summary>
        /// Using psTexts, it makes an array of dictionaries that have all the content from "key : value"
        /// </summary>
        /// <returns>A ready to use array of dictionaries for output text</returns>
        private Dictionary<string, string>[] GetResultTexts()
        {
            // Array of dictionaries for all results
            Dictionary<string, string>[] res = new Dictionary<string, string>[psTexts.Length];
            // Load dictionary array with a dictionary
            foreach (string text in psTexts)
            {
                // A dictionary for current result
                Dictionary<string, string> result = new Dictionary<string, string>();
                // Current index in psTexts
                int index = Array.FindIndex(psTexts, row => row.Equals(text));
                foreach (string s in text.Split('\n'))
                {
                    string[] split = s.Split(':');
                    string key = split[0].Trim();
                    string value = "";
                    if (split.Length > 1 && iconvert.ContainsKey(key))
                    {
                        string k = iconvert[key] + " : ";
                        value += char.ToUpper(k[0]) + k.Substring(1);
                        value += split[1]
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
            // If there is at least 1 true
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
            // Checks that the current list of options exists
            if (currentList != null)
            {
                // Clears all options
                optionsListBox.Items.Clear();
                filterList.Items.Clear();
                // Adds the new options
                foreach (var item in currentList)
                {
                    optionsListBox.Items.Add(item);
                    filterList.Items.Add(item);
                }
                filterList.SelectedItem = "Identifiant";
            }
        }

        /// <summary>
        /// Updates the collection currentlist
        /// </summary>
        /// <param name="otherlist">The additional list to add in the currentlist</param>
        private void UpdateCurrentList(Collection<string> otherlist)
        {
            // Empties the current list of options
            currentList.Clear();
            // Adds all entries in sharedOptions
            foreach (string s in sharedOptions)
            {
                currentList.Add(s);
            }
            // Adds all entries in the secondary list
            foreach(string s in otherlist)
            {
                currentList.Add(s);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Executes <code>Button1_Click</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultipleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Reloads the data shown
            Button1_Click(sender, null);
            // Removes an ugly scrollbar that isn't even working
            resultTextBox.Refresh();
        }

        /// <summary>
        /// Updates <code>isUser</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserRButton_CheckedChanged(object sender, EventArgs e)
        {
            // Checks that button was checked
            if (userRButton.Checked)
            {
                // It's now an user
                isUser = true;
                getItemButton.Text = "Obtenir les informations de l'utilisateur";
                // Unchecks other button
                computerRButton.Checked = false;
                UpdateCurrentList(userOptions);
                UpdateOptionBoxes();
                Button1_Click(sender, null);
            }
        }

        /// <summary>
        /// Updates <code>isUser</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComputerRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (computerRButton.Checked)
            {
                // It's not an user
                isUser = false;
                getItemButton.Text = "Obtenir les informations de l'ordinateur";
                // Unchecks other button
                userRButton.Checked = false;
                UpdateCurrentList(computerOptions);
                UpdateOptionBoxes();
                Button1_Click(sender, null);
            }
        }

        /// <summary>
        /// As the name suggests, makes it so you submit when you press enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitOnEnter(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode) {
                // Press enter for input
                case Keys.Enter:
                    Button1_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }
        #endregion
    }
}
