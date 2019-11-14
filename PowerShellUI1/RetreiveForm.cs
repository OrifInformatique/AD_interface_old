﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class RetreiveForm : Form
    {
        readonly string sriptSubfolder = "\\Scripts\\";
        readonly string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private readonly Dictionary<string, string> userProperties = new Dictionary<string, string>() { };
        private readonly List<string> groupsList = new List<string>();

        public RetreiveForm()
        {
            InitializeComponent();
        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search(sender, e);
            }
        }

        private void Search(object sender, EventArgs e)
        {
            try
            {
                groups_list.Text = "";
                output.Text = "";
                application_statue.Text = "";
                Collection<PSObject> users, groups;
                string scriptUser = StoreScritpt("getUser.ps1").Replace("{identifiant}", username.Text),
                scriptGroups = StoreScritpt("getGroups.ps1").Replace("{identifiant}", username.Text);

                userProperties.Clear();
                groupsList.Clear();

                using (PowerShell ps = PowerShell.Create())
                {
                    users = ps.AddScript(scriptUser).Invoke();
                }
                using (PowerShell ps = PowerShell.Create())
                {
                    groups = ps.AddScript(scriptGroups).Invoke();
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
                            userProperties.Add(split[0], split[1]);
                            output.Text += split[0] + " : " + split[1] + Environment.NewLine;
                        }
                    }
                }

                if (output.Text.Length == 0)
                {
                    output.Text = "Utilisateur introuvable";
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
                                groups_list.Text += split[1] + Environment.NewLine;
                            }
                        }
                    }
                }
            }
            catch
            {
                groups_list.Text = "";
                output.Text = "Erreur rencontrée";
                userProperties.Clear();
                groupsList.Clear();
            }
        }

        private string StoreScritpt(string filename)
        {
            try
            {
                using (StreamReader strReader = new StreamReader(currentPath + sriptSubfolder + filename))
                {
                    Console.WriteLine(currentPath + sriptSubfolder + filename);
                    return strReader.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }

        private void Check_app_Click(object sender, EventArgs e)
        {
            application_statue.Text = "";
            switch (applications_list.Text)
            {
                case "SAI":
                    bool enabled = userProperties.TryGetValue("Enabled", out string enabledStr);
                    enabled &= enabledStr == "True";
                    Check_condition(enabled, "Utilisateur actif");
                    Check_condition(groupsList.IndexOf("MSP") != -1, "Groupe MSP");
                    break;
                default:
                    break;
            }
        }

        private void Check_condition(bool condition, string text)
        {
            application_statue.Text += text + "\t" + (condition ? "OK" : "Echec") + Environment.NewLine;
        }
    }
}
