using System;
using System.Management.Automation; // Enables PowerShell
using System.IO;  // Enables Stream Reader
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerShellUI1
{
    /// <summary>
    /// <code>RetreiveForm</code> is for obtaining informations about an user or computer.
    /// </summary>
    public partial class RetreiveForm : Form
    {
        #region Variables
        #region readonly
        // Name of the folder containing the scripts
        readonly string scriptSubfolder = ChoiceForm.ScriptSubfolder,
            // Path to scripts
            path = ChoiceForm.Path;
        // Strings of text for the list of different items
        private readonly string[] chooseItemText = { "Il y a ", "s à choix." };
        // Translates between powershell filters and french
        private readonly Dictionary<string, string> convert = new Dictionary<string, string>() {
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
        private readonly Collection<string> sharedOptions = new Collection<string>() {
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
        #endregion

        #region static
        /// <summary>
        /// String that separates the multiple entries.
        /// </summary>
        private static string EntrySeparator => "――――――";
        /// <summary>
        /// Script that contains the logic behind the loading.
        /// </summary>
        private static string UserScript => "getUser.ps1";
        #endregion

        // The item selected
        private string selected = "utilisateur";
        // options for the user
        private Dictionary<string, bool> options;
        #endregion

        #region Constructors
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

            // Get the current path
            if (path == null)
            {
                path = Path.GetDirectoryName(Application.ExecutablePath);
                // Go upward until in AD_interface  
                while (!path.EndsWith("AD_interface"))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
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

            // Get the current path
            this.path = path;

            searchTextBox.Select();
            filterList.SelectedItem = "Identifiant";
        }
        #endregion

        #region Displays
        /// <summary>
        /// Updates the resultTextBox's Text with the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateResultTextBox(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            // Get script
            string script = GetBaseScript(),
                // Run script and get results
                item;
            try
            {
                item = GetScriptResults(script);
            }
            catch
            {
                // Problem in script execution, tell the user
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur dans l'execution du script";
                return;
            }
            // Split script results into separate items
            string[] items = item.Trim().Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            // Obtain ordered items
            var dicts = GetResultTextsFromItems(items);
            // Display items
            resultTextBox.Text = GetItemsFromDict(dicts);
        }

        /// <summary>
        /// Displays the informations in another window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayWindowResult(object sender, EventArgs e)
        {
            statusLabel.Visible = false;
            // Obtain script and modify it
            string script = GetBaseScript().Replace("Out-String", "Out-Gridview -Title 'Informations sur les " + selected + "s'");
            try
            {
                // Display window
                _ = GetScriptResults(script);
            }
            catch
            {
                // Problem in script execution, tell the user
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur dans l'execution du script";
                return;
            }
        }
        #endregion

        #region Item-related functions
        /// <summary>
        /// Obtains the powershell script.
        /// </summary>
        /// <returns>The powershell script.</returns>
        private string GetBaseScript()
        {
            string scriptContent = "",
                // Load the user input
                userPart = searchTextBox.Text
                    // User can't use the escape character
                    .Replace("`", "``")
                    // User also can't call a variable
                    .Replace("$", "`$"),
                filterSelection = convert[(filterList.SelectedItem as string).ToLower()];

            try
            {
                // Read script file
                string currentPath = path + scriptSubfolder + UserScript;
                using (StreamReader strReader = new StreamReader(currentPath))
                {
                    scriptContent = strReader.ReadToEnd();
                }
            }
            catch
            {
                // Problem with the script, tell the user
                statusLabel.Text = "Erreur: Le fichier '" + UserScript + "' n'est pas dans le dossier Scripts!";
                statusLabel.Visible = true;
                return null;
            }

            // Enabled is a special case, the script must be edited
            if ((filterList.SelectedItem as string).ToLower().Equals("actif"))
            {
                scriptContent = scriptContent.Replace("-like", "-eq");
                userPart = userPart.ToLower().Replace("oui", "True").Replace("non", "False");
            }
            if (selected.Equals("ordinateur"))
            {
                // The user is looking for a computer, edit script
                scriptContent = scriptContent.Replace("Get-ADUser", "Get-ADComputer");
            }

            // Replace the placeholders with their values
            return scriptContent.Replace("{part}", userPart).Replace("{FilterSelection}", filterSelection);
        }

        /// <summary>
        /// Obtains the output from a powershell script.
        /// </summary>
        /// <param name="script">Powershell script to execute</param>
        /// <returns>Output of the script</returns>
        private string GetScriptResults(string script)
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
        /// Prepares the text to write from an array of items.
        /// </summary>
        /// <param name="items">The items to prepare</param>
        /// <returns>The prepared text</returns>
        private string GetItemsFromDict(Dictionary<string, string>[] items)
        {
            switch (items.Length)
            {
                // There is no item, return nothing
                case 0:
                    return "";
                // Disable the components only if there is a single item
                case 1:
                    ifMultipleLabel.Enabled = false;
                    whichNumberUD.Enabled = false;
                    multipleCheckBox.Enabled = false;
                    break;
                default:
                    ifMultipleLabel.Enabled = true;
                    whichNumberUD.Enabled = !multipleCheckBox.Checked;
                    multipleCheckBox.Enabled = true;
                    break;
            }
            // Check which item has been selected
            if (!multipleCheckBox.Checked)
            {
                int i = Convert.ToInt32(whichNumberUD.Value) - 1,
                    j = (i > -1 && i < items.Length) ? i : 0;
                items = new Dictionary<string, string>[] { items[j] };
            }

            string res = "";
            // Whether all parts should be displayed
            bool all = !CheckOptions();
            foreach (Dictionary<string, string> item in items)
            {
                foreach (KeyValuePair<string, string> entry in convert)
                {
                    // Check if the dictionnary contains the entry's value
                    if ((all || options[entry.Key]) && item.ContainsKey(entry.Value))
                    {
                        // Add the value to the returned value
                        res += item[entry.Value];
                    }
                }
                // Add line separators, because having it all glued it ugly
                res += "\r\n" + EntrySeparator + "\r\n\r\n";
            }

            // Remove last separator before returning the value
            return res.Substring(0, res.LastIndexOf(EntrySeparator)).Trim();
        }

        /// <summary>
        /// Converts an array of items into a prepared array of items.
        /// Mostly for processing purposes.
        /// </summary>
        /// <param name="items">The items to prepare</param>
        /// <returns>The prepared items</returns>
        private Dictionary<string, string>[] GetResultTextsFromItems(string[] items)
        {
            // Prepare an array of dictionaries
            Dictionary<string, string>[] res = new Dictionary<string, string>[items.Length];

            // Get which item we are at in the array
            int index = 0;
            foreach (string item in items)
            {
                // Prepare a dictionary
                Dictionary<string, string> itemResult = new Dictionary<string, string>();

                // Split string at the new line
                foreach (string s in item.Split('\n'))
                {
                    // Split string at :
                    string[] split = s.Split(':');
                    // Get key (which item is gonna be grabbed at which point)
                    string key = split[0].Trim(),
                        // Get value (the item)
                        value = "";
                    // Check if there is a value
                    if (split.Length > 1 && iconvert.ContainsKey(key))
                    {
                        // Add the translated key and :
                        string k = iconvert[key] + " : ";
                        // Make k start with an uppercase letter
                        value += char.ToUpper(k[0]) + k.Substring(1) +
                            // Get the other half of the pair and modify it for readability
                            split[1]
                            .Trim()
                            .Replace(",", "\n\t")
                            .Replace("True", "Oui")
                            .Replace("False", "Non")
                            .Replace("=", " = ") + "\n";
                    }
                    // Add key and value to dictionary
                    itemResult.Add(key, value);
                }
                // Dictionary is complete, add it to the array and go to the next one
                res[index] = itemResult;
                index++;
            }

            return res;
        }
        #endregion

        #region Other functions
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
                foreach (string item in currentList)
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
            UpdateOptionBoxes();
        }

        /// <summary>
        /// Updates the form's componenents' text depending on what is selected.
        /// </summary>
        /// <returns>Which item is selected.</returns>
        private void UpdateSelected()
        {
            ifMultipleLabel.Text = "S'il y a plusieurs " + selected + "s, choisir le quel montrer";
            getItemButton.Text = "Obtenir les informations de l'" + selected;
        }
        #endregion

        #region Events
        /// <summary>
        /// Updates <code>isUser</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchSelected(object sender, EventArgs e)
        {
            // Checks which button was checked
            if (sender.Equals(computerRButton))
            {
                // It's now an user
                selected = "utilisateur";
                // Unchecks other button
                computerRButton.Checked = false;
                UpdateSelected();
                UpdateCurrentList(userOptions);
                UpdateResultTextBox(sender, null);
            }
            else if (sender.Equals(userRButton))
            {
                // It's not an user
                selected = "ordinateur";
                // Unchecks other button
                userRButton.Checked = false;
                UpdateSelected();
                UpdateCurrentList(computerOptions);
                UpdateResultTextBox(sender, null);
            }
        }

        /// <summary>
        /// Calls <code>RetreiveData</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadItems(object sender, EventArgs e)
        {
            // Reloads the data shown
            UpdateResultTextBox(sender, null);
            // Removes an ugly scrollbar that isn't even working
            resultTextBox.Refresh();
        }

        /// <summary>
        /// Calls <code>RetreiveData</code> when the user presses enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitOnEnter(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    UpdateResultTextBox(sender, null);
                    // One of these prevents the error sound when you press enter
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }
        #endregion
    }
}