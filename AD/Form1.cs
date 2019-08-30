using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Windows.Forms;

namespace AD
{
    public partial class Form1 : Form
    {
        readonly string sriptSubfolder = "\\Scripts\\";
        readonly string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        private Dictionary<string, string> userProperties = new Dictionary<string, string>() { };
        private List<string> groupsList = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Search(sender, e);
            }
        }

        private void Search(object sender, EventArgs e)
        {
            groups_list.Text = "";
            output.Text = "";
            application_statue.Text = "";

            userProperties.Clear();
            groupsList.Clear();

            PowerShell ps = PowerShell.Create();

            string scriptUser = StoreScritpt("getUser.ps1");
            scriptUser = scriptUser.Replace("{identifiant}", username.Text);
            Collection<PSObject> users = ps.AddScript(scriptUser).Invoke();

            string scriptGroups = StoreScritpt("getGroups.ps1");
            scriptGroups = scriptGroups.Replace("{identifiant}", username.Text);
            Collection<PSObject> groups = ps.AddScript(scriptGroups).Invoke();

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
                        output.Text += split[0] + " :\t" + split[1] + Environment.NewLine;
                    }
                }
            }

            if (output.Text == "")
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
            switch(applications_list.Text)
            {
                case "SAI":
                    string enabledStr = "";
                    bool enabled = userProperties.TryGetValue("Enabled", out enabledStr);
                    enabled &= enabledStr == "True";
                    Check_condition(enabled, "Utilisateur actif");
                    Check_condition(groupsList.IndexOf("MSP") != -1, "Groupe MSP");
                    break;
            }
        }

        private void Check_condition(bool condition, string text)
        {
            application_statue.Text += text + "\t" + (condition ? "OK" : "Echec") + Environment.NewLine;
        }
    }
}
