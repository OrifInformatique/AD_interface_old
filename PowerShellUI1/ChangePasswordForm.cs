using System;
using System.IO;
using System.Management.Automation;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class ChangePasswordForm : Form
    {
        readonly string path,
            scriptSubfolder = ChoiceForm.ScriptSubfolder,
            passwordScript = "ChangeUserPassword.ps1";

        public ChangePasswordForm()
        {
            InitializeComponent();
            path = Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!path.EndsWith("AD_interface"))
            {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
        }

        public ChangePasswordForm(string path)
        {
            InitializeComponent();
            this.path = path;
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            errorLabel.Visible = false;
            string username = usernameTextBox.Text,
                password = newPasswordTextBox.Text,
                passwordAgain = newPasswordAgainTextBox.Text,
                scriptContent,
                currentPath;

            if (password.Equals(""))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Entrez un mot de passe!";
                return;
            }
            else if (!password.Equals(passwordAgain))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Le mot de passe doit être le même!";
                return;
            }
            else if (username.Equals(""))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Fournissez un nom d'utilisateur";
                return;
            }

            PowerShell ps = PowerShell.Create();
            int minPasswordLength = 6;
            ps.AddScript("(Get-ADDefaultDomainPasswordPolicy).MinPasswordLength");
            try
            {
                var results = ps.Invoke();
                foreach (var result in results)
                {
                    if (int.TryParse(result.ToString(), out int i))
                    {
                        minPasswordLength = i;
                    }
                }
            }
            catch
            {
                minPasswordLength = 6;
            }
            finally
            {
                ps.Dispose();
            }

            if (!(password.Length >= minPasswordLength))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Le mot de passe doit faire au moins " + minPasswordLength.ToString() + " caractères de long!";
                return;
            }

            currentPath = path + scriptSubfolder + passwordScript;
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
                errorLabel.Visible = true;
                errorLabel.Text = "Erreur: Fichier '" + passwordScript + "' n'est pas dans /Scripts!";
                return;
            }

            scriptContent = scriptContent.Replace("{part}", username).Replace("{newPassword}", password);
            SecureString userPassword = new SecureString();
            foreach (char c in currentUserPasswordTextBox.Text.ToCharArray())
            {
                userPassword.AppendChar(c);
            }
            username = currentUserTextBox.Text;
            PSCredential credential = new PSCredential(username, userPassword);
            ps = PowerShell.Create()
                .AddCommand("Set-Variable")
                .AddParameter("Name", "credential")
                .AddParameter("Value", credential)
                .AddScript(scriptContent);
            try
            {
                var results = ps.Invoke();
                foreach (PSObject result in results)
                {
                    // TODO: Find someone who can test that
                    errorLabel.Visible = true;
                    errorLabel.Text += result;
                }
            }
            catch
            {
                // Problem with the script, tell the user
                errorLabel.Visible = true;
                errorLabel.Text = "Erreur: problème dans l'execution du script.";
                return;
            }
            finally
            {
                ps.Dispose();
            }
        }

        private void SubmitOnEnter(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Press enter for input
                case Keys.Enter:
                    ChangePasswordButton_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }
    }
}
