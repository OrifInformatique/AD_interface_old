using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class RetreiveForm : Form
    {
        #region Variables

        /// <summary>
        /// The subfolder containing all the scripts
        /// </summary>
        private static string ScriptSubfolder => Utilities.SCRIPT_SUBFOLDER;

        /// <summary>
        /// Path to AD_Interface folder
        /// </summary>
        private readonly string path = ChoiceForm.Path;

        /// <summary>
        /// List of ther user's groups
        /// </summary>
        private readonly List<string> groupsList = new List<string>();

        /// <summary>
        /// Converts allowed applications from acronym to full text
        /// </summary>
        private readonly Dictionary<string, string> apps = new Dictionary<string, string>
        {
            { "AD" , "Active Directory" },
            { "AC" , "AIRS Capture" },
            { "AS" , "AIRS Dossier" },
            { "EV" , "EasyVista" },
            { "EX" , "Exchange" },
            { "FS" , "Files System" },
            { "GE" , "Générique" },
            { "JX" , "Jedox (Gestion des budgets)" },
            { "MO" , "MonOrif" },
            { "RH" , "Dossier RH sur Sharepoint" },
            { "SP" , "SharePoint (OrifIntra)" },
            { "SG" , "Sigem" },
            { "VPN" , "VPN" },
            { "WIFI" , "Wi-Fi" }
        };

        #endregion Variables

        #region Constructors

        /// <summary>
        /// Creates a new RetreiveForm
        /// </summary>
        public RetreiveForm()
        {
            InitializeComponent();
            if (path == null)
            {
                path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                // Go upward until in AD_interface
                while (!Directory.Exists(path + Utilities.SCRIPT_SUBFOLDER))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
            }

            Lb_applications.ValueMember = "abreviation";
            Lb_applications.DisplayMember = "name";
        }

        /// <summary>
        /// Creates a new RetreiveForm
        /// </summary>
        /// <param name="path">Path to base file</param>
        public RetreiveForm(string path)
        {
            InitializeComponent();
            this.path = path;

            Lb_applications.ValueMember = "abreviation";
            Lb_applications.DisplayMember = "name";
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Returns the contents of a file in the script folder
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <returns>The file's contents</returns>
        private string StoreScript(string filename)
        {
            return Utilities.GetFileContents(path + ScriptSubfolder + filename);
        }

        /// <summary>
        /// Verifies that an application can be accessed by any group
        /// </summary>
        /// <param name="application">The application to be checked</param>
        /// <returns>True if the the application can be accessed, false otherwise</returns>
        private bool CheckApplication(string application)
        {
            string name = "GS-" + application;
            return groupsList.FindAll(group => group.StartsWith(name)).Count > 0;
        }

        #endregion Methods

        #region Events

        /// <summary>
        /// Calls Search when the user presses enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchEnter(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                default:
                    return;
                case Keys.Enter:
                    Search(sender, e);
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Displays all informations about the entered user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search(object sender, EventArgs e)
        {
            Groups_list.Text = "";
            Output.Text = "";
            Application_status.Text = "";
            Collection<PSObject> users, groups;
            string scriptUser = StoreScript("getUser.ps1").Replace("{identifiant}", Username.Text),
            scriptGroups = StoreScript("getGroups.ps1").Replace("{identifiant}", Username.Text);

            groupsList.Clear();

            using (PowerShell ps = PowerShell.Create())
            {
                users = ps.AddScript(scriptUser).Invoke();
            }
            foreach (PSObject user in users)
            {
                string[] lines = (user + "").Split('\n');
                foreach (string line in lines)
                {
                    string[] split = line.Split(':');
                    if (split.Length > 1)
                    {
                        split[0] = split[0].Trim();
                        split[1] = split[1].Trim();
                        Output.Text += split[0] + " : " + split[1] + Environment.NewLine;
                    }
                }
            }
            if (Output.Text.Length == 0)
            {
                Output.Text = "Utilisateur introuvable";
            }

            using (PowerShell ps = PowerShell.Create())
            {
                groups = ps.AddScript(scriptGroups).Invoke();
            }
            foreach (PSObject group in groups)
            {
                string[] lines = (group + "").Split('\n');
                foreach (string line in lines)
                {
                    string[] split = line.Split(':');
                    if (split.Length > 1)
                    {
                        split[0] = split[0].Trim();
                        split[1] = split[1].Trim();
                        if (split[0] == "name")
                        {
                            groupsList.Add(split[1]);
                            Groups_list.Text += split[1] + Environment.NewLine;
                        }
                    }
                }
            }

            // This part doesn't work and never did
            // even though we think it did
            BindingList<string> b = new BindingList<string>();
            foreach (string v in apps.Keys)
            {
                if (CheckApplication(v))
                {
                    b.Add(apps[v]);
                }
            }
            Lb_applications.DataSource = b;
        }

        /// <summary>
        /// Adds all groups the user is part of in Application_status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListAllGroups(object sender, EventArgs e)
        {
            Application_status.Text = "";
            foreach (string group in groupsList)
            {
                if (group.StartsWith("GS-" + (Lb_applications.SelectedItem as Application).Abreviation))
                {
                    Application_status.Text += group + Environment.NewLine;
                }
            }
        }

        #endregion Events
    }
}