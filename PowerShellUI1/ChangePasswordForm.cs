using System;
using System.IO;
using System.Management.Automation;
using System.Security;
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
            // Hide error label until we get an error
            errorLabel.Visible = false;
            // Declare all future variables
            string username = usernameTextBox.Text,
                password = newPasswordTextBox.Text,
                passwordAgain = newPasswordAgainTextBox.Text,
                scriptContent,
                currentPath;

            // Is the password empty
            if (password.Equals("") || newPasswordTextBox.Equals("") || currentUserPasswordTextBox.Text.Equals(""))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Entrez un mot de passe!";
                return;
            }
            // Is there a username provided
            else if (username.Equals("") || currentUserTextBox.Text.Equals(""))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Fournissez un nom d'utilisateur";
                return;
            }
            // Is the new password repeated twice
            else if (!password.Equals(passwordAgain))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Le mot de passe doit être le même!";
                return;
            }

            // Create a new powershell
            PowerShell ps = PowerShell.Create();
            int minPasswordLength = 6;
            ps.AddScript("(Get-ADDefaultDomainPasswordPolicy).MinPasswordLength");
            try
            {
                var results = ps.Invoke();
                foreach (var result in results)
                {
                    // Get the minimum length of password
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

            // Check that password is longer than minimum length
            if (!(password.Length >= minPasswordLength))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Le mot de passe doit faire au moins " + minPasswordLength.ToString() + " caractères de long!";
                return;
            }

            // Prepare script path
            currentPath = path + scriptSubfolder + passwordScript;
            try
            {
                // Read script
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

            // Prepare the script for usage
            scriptContent = scriptContent.Replace("{part}", username).Replace("{newPassword}", password);
            ps = PowerShell.Create()
                // May need AddCommand instead of AddScript
                .AddScript(scriptContent);
            try
            {
                // Launch script and check for results
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
