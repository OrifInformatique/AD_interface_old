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
        private readonly string scriptSubfolder = Utilities.ScriptSubfolder, path = ChoiceForm.Path;

        private readonly Dictionary<string, string> userProperties = new Dictionary<string, string>() { };
        private readonly List<string> groupsList = new List<string>();

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

            lb_applications.ValueMember = "abreviation";
            lb_applications.DisplayMember = "name";
        }

        public RetreiveForm(string path)
        {
            InitializeComponent();
            this.path = path;

            lb_applications.ValueMember = "abreviation";
            lb_applications.DisplayMember = "name";
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
                string scriptUser = StoreScript("getUser.ps1").Replace("{identifiant}", username.Text),
                scriptGroups = StoreScript("getGroups.ps1").Replace("{identifiant}", username.Text);

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
                lb_applications.DataSource = new BindingList<Application>(applications.FindAll(item => CheckApplication(item.abreviation)));
            }
            catch
            {
                groups_list.Text = "";
                output.Text = "Erreur rencontrée";
                userProperties.Clear();
                groupsList.Clear();
                lb_applications.DataSource = new BindingList<Application>();
            }
        }

        private string StoreScript(string filename)
        {
            try
            {
                using (StreamReader strReader = new StreamReader(path + scriptSubfolder + filename))
                {
                    Console.WriteLine(path + scriptSubfolder + filename);
                    return strReader.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }

        private void Check_condition(bool condition, string text)
        {
            application_statue.Text += text + "\t" + (condition ? "OK" : "Echec") + Environment.NewLine;
        }

        private bool CheckGroupApplication(string groupName, string application)
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

        private void lb_applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            application_statue.Text = "";
            foreach (string group in groupsList)
            {
                if(CheckGroupApplication(group, ((Application)lb_applications.SelectedItem).abreviation))
                {
                    application_statue.Text += group + Environment.NewLine;
                }
            }
        }
    }
}
