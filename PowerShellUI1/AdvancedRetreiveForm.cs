using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;

namespace PowerShellUI1
{
    /// <summary>
    /// AdvancedRetreiveForm is like RetreiveForm but for more items.
    /// It is also not translated back in french
    /// </summary>
    public partial class AdvancedRetreiveForm : Form
    {
        #region Variables

        #region Readonly

        // Name of the folder containing the scripts
        private readonly string scriptSubfolder = ChoiceForm.ScriptSubfolder,
            // Path to scripts
            path = ChoiceForm.Path;

        #endregion Readonly

        #region Static

        /// <summary>
        /// List of properties in an user
        /// </summary>
        private static string[] Props;

        /// <summary>
        /// List of items that are booleans
        /// </summary>
        private static string[] BooleanList => new string[]{
            "AccountNotDelegated", "AllowReversiblePasswordEncryption", "CannotChangePassword",
            "ChangePasswordAtLogon", "Deleted", "DoesNotRequirePreAuth",
            "Enabled", "HomedirRequired", "LockedOut",
            "MNSLogonAccount", "PasswordExpired", "PasswordNeverExpires",
            "PasswordNotRequired", "ProtectedFromAccidentalDeletion", "SmartcardLogonRequired",
            "TrustedForDelegation", "TrustedToAuthForDelegation", "UseDESKeyOnly"
        };

        /// <summary>
        /// Name of the file that contains the script to launch
        /// </summary>
        private static string ScriptName => "AdvancedGetUser.ps1";

        #endregion Static

        private Dictionary<string, string>[] lastitems = null;

        #endregion Variables

        #region Contructors

        /// <summary>
        /// Creates a new AdvancedRetreiveForm.
        /// </summary>
        public AdvancedRetreiveForm()
        {
            InitializeComponent();
            CenterToScreen();

            if (path == null)
            {
                path = Path.GetDirectoryName(Application.ExecutablePath);
                // Go upward until in AD_interface
                while (!Directory.Exists(path + ChoiceForm.ScriptSubfolder))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
            }

            if (Props == null)
            {
                StringBuilder strBui = new StringBuilder();
                using (Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Verb = "runas",
                        Arguments = "(Get-ADUser -Filter 'samAccountName -like \"*\"' -Properties *)[0].propertynames",
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                })
                {
                    _ = process.Start();
                    while (!process.HasExited)
                    {
                        _ = strBui.Append(process.StandardOutput.ReadToEnd());
                    }
                }
                string result = strBui.ToString();
                Props = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            SetBoxesValues();
        }

        /// <summary>
        /// Creates a new Advanced RetreiveForm
        /// </summary>
        /// <param name="path">The path to the base file</param>
        public AdvancedRetreiveForm(string path)
        {
            InitializeComponent();
            CenterToScreen();

            if (Props == null)
            {
                string result = Utilities.GetScriptResults("(Get-ADUser -Filter 'samAccountName -like \"*\"' -Properties *)[0].propertynames | Out-String");
                Props = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            SetBoxesValues();

            this.path = path;
        }

        #endregion Contructors

        #region Init methods

        /// <summary>
        /// Updates the values of the listbox and checkedlistbox
        /// </summary>
        private void SetBoxesValues()
        {
            if (Props == null)
            {
                return;
            }

            CriteriaListBox.Items.Clear();
            DisplayCheckBox.Items.Clear();
            foreach (string s in Props)
            {
                _ = CriteriaListBox.Items.Add(s);
                _ = DisplayCheckBox.Items.Add(s);
            }
            CriteriaListBox.SelectedItem = "SamAccountName";
        }

        #endregion Init methods

        #region Main methods

        /// <summary>
        /// Displays the result in a window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayResultWindow(object sender = null, EventArgs e = null)
        {
            string script = GetBaseScript().Replace("Out-String", "Out-Gridview -Title 'Informations sur les utilisateurs'");
            try
            {
                // Display window
                _ = Utilities.GetScriptResults(script);
            }
            catch
            {
                // Problem in script execution, tell the user
                WarningLabel.Visible = true;
                WarningLabel.Text = "Erreur dans l'execution du script";
                return;
            }
        }

        /// <summary>
        /// Obtains the powershell script, modified according to the user inputs
        /// </summary>
        /// <returns>The powershell script</returns>
        private string GetBaseScript()
        {
            // Storage for the script
            string scriptContent = Utilities.GetFileContents(path + scriptSubfolder + ScriptName),
                // User input
                userPart = SearchTextBox.Text
                    // Prevent user from using exit chars and variables
                    .Replace("`", "``")
                    .Replace("$", "`$"),
                // Selected criteria
                criteriaSelection = CriteriaListBox.SelectedItem as string;

            if (scriptContent.Length == 0)
            {
                WarningLabel.Text = "Erreur: Le fichier '" + ScriptName + "' n'est pas dans le dossier Scripts!";
                WarningLabel.Visible = true;
                return null;
            }

            if (Array.IndexOf(BooleanList, criteriaSelection) > 0)
            {
                scriptContent = scriptContent.Replace("-like", "-eq");
                userPart = userPart.ToLower().Replace("oui", "True").Replace("non", "False");
            }

            return scriptContent.Replace("{Part}", userPart).Replace("{FilterSelection}", criteriaSelection);
        }

        /// <summary>
        /// Turns an array of items into an array of dictionaries representing an item
        /// </summary>
        /// <param name="items">The list of items</param>
        /// <returns>An array of dictionaries per item</returns>
        private Dictionary<string, string>[] SliceItems(string[] items)
        {
            Dictionary<string, string>[] slicedItems = new Dictionary<string, string>[items.Length];

            int index = 0;
            foreach (string item in items)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                string[] entries = item.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string entry in entries)
                {
                    string[] split = entry.Split(':');
                    string key = split[0].Trim(),
                        value = "";
                    if (split.Length > 1)
                    {
                        value = split[1]
                            .Replace(",", "\n\t")
                            .Replace("=", " = ");
                    }
                    dict.Add(key, value);
                }
                slicedItems[index] = dict;
                index++;
            }

            return slicedItems;
        }

        /// <summary>
        /// Returns formatted text for all items
        /// </summary>
        /// <param name="items">The items to format</param>
        /// <returns>The formatted text for all items</returns>
        private string TextFromItems(Dictionary<string, string>[] items)
        {
            string result = "";
            bool all = CheckAllOptions();

            switch (items.Length)
            {
                case 0:
                    if (SearchTextBox.Text.Length > 0)
                    {
                        WarningLabel.Visible = true;
                        WarningLabel.Text = "Aucun utilisateur n'est appelé " + SearchTextBox.Text;
                    }
                    return "";

                case 1:
                    WhichOne.Enabled = false;
                    MultipleCheckBox.Enabled = false;
                    break;

                default:
                    WhichOne.Enabled = true;
                    MultipleCheckBox.Enabled = true;
                    break;
            }

            if (!MultipleCheckBox.Checked)
            {
                int i = Convert.ToInt32(WhichOne.Value) - 1,
                    j = (i >= 0 && i < items.Length) ? i : 0;
                items = new Dictionary<string, string>[] { items[j] };
            }

            foreach (Dictionary<string, string> item in items)
            {
                foreach (KeyValuePair<string, string> pair in item)
                {
                    string key = pair.Key,
                        value = pair.Value;
                    if (all || DisplayCheckBox.CheckedItems.Contains(key))
                    {
                        result += (key + " : " + value).Trim() + "\n";
                    }
                }

                result += Environment.NewLine + Utilities.ResultEntrySeparator + Environment.NewLine + Environment.NewLine;
            }

            return result.Substring(0, result.LastIndexOf(Utilities.ResultEntrySeparator)).Trim();
        }

        /// <summary>
        /// Updates the result textbox with the found user(s)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateResultTextBox(object sender = null, EventArgs e = null)
        {
            string script = GetBaseScript(),
                item = Utilities.GetScriptResults(script),
                result;
            string[] items = item.Trim().Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            lastitems = SliceItems(items);
            result = TextFromItems(lastitems);
            ResultTextBox.Text = result;
        }

        #endregion Main methods

        #region Misc methods

        /// <summary>
        /// Checks all options and returns whether all must be displayed
        /// </summary>
        /// <returns>True if all/none options are checked</returns>
        private bool CheckAllOptions()
        {
            CheckedListBox.CheckedItemCollection checkedItems = DisplayCheckBox.CheckedItems;
            return checkedItems.Count == 0 || checkedItems.Count == DisplayCheckBox.Items.Count;
        }

        #endregion Misc methods

        #region Events

        /// <summary>
        /// Changes displayed items when what to display is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeDisplay(object sender, EventArgs e)
        {
            if (lastitems == null)
            {
                return;
            }
            _ = BeginInvoke((MethodInvoker) (
                () =>
                {
                    string result = TextFromItems(lastitems);
                    ResultTextBox.Text = result;
                }
            ));
        }

        /// <summary>
        /// Submit on enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
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

        #endregion Events
    }
}