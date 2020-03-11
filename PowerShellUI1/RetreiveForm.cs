﻿using System;
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
        private static string ScriptSubfolder => Utilities.ScriptSubfolder;

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
        private readonly List<Application> applications = new List<Application>(new Application[] {
            new Application("AD", "Active Directory"),
            new Application("AC", "AIRS Capture"),
            new Application("AS", "AIRS Dossier"),
            new Application("EV", "EasyVista"),
            new Application("EX", "Exchange"),
            new Application("FS", "Files System"),
            new Application("GE", "Générique"),
            new Application("JX", "Jedox (Gestion des budgets)"),
            new Application("MO", "MonOrif"),
            new Application("RH", "Dossier RH sur Sharepoint"),
            new Application("SP", "SharePoint (OrifIntra)"),
            new Application("SG", "Sigem"),
            new Application("VPN", "VPN"),
            new Application("WIFI", "Wi-Fi")
        });

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
                while (!Directory.Exists(path + Utilities.ScriptSubfolder))
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
        /// Checks that a group can access an application
        /// </summary>
        /// <param name="groupName">The name of the group</param>
        /// <param name="application">The name of the application</param>
        /// <returns>True if the group can access</returns>
        private static bool CheckGroupApplication(string groupName, string application)
        {
            string[] nameParts = groupName.Split('-');
            if (nameParts.Length >= 2)
            {
                return nameParts[0] == "GS" && nameParts[1] == application;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies that an application can be accessed by any group
        /// </summary>
        /// <param name="application">The application to be checked</param>
        /// <returns>True if the the application can be accessed, false otherwise</returns>
        private bool CheckApplication(string application)
        {
            foreach (string group in groupsList)
            {
                if (CheckGroupApplication(group, application))
                {
                    return true;
                }
            }
            return false;
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
            if (e.KeyCode == Keys.Enter)
            {
                Search(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
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

            Lb_applications.DataSource = new BindingList<Application>(applications.FindAll(item => CheckApplication(item.Abreviation)));
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
                if (CheckGroupApplication(group, ((Application) Lb_applications.SelectedItem).Abreviation))
                {
                    Application_status.Text += group + Environment.NewLine;
                }
            }
        }

        #endregion Events
    }
}