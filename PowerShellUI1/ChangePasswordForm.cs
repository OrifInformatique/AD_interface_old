using System;
using System.IO;
using System.Windows.Forms;

namespace PowerShellUI1
{
    /// <summary>
    /// <code>ChangePasswordForm</code> is for changing an user's password.
    /// NOTE: untested.
    /// </summary>
    public partial class ChangePasswordForm : Form
    {
        #region Variables

        private readonly string path = ChoiceForm.Path,
            scriptSubfolder = Utilities.ScriptSubfolder;

        // Tooltip object
        private readonly ToolTip tooltip = new ToolTip();

        /// <summary>
        /// The name of the script.
        /// </summary>
        private static string PasswordScript => "ChangeUserPassword.ps1";

        #endregion Variables

        #region Constructors

        /// <summary>
        /// Creates a new <code>ChangePasswordForm</code>.
        /// </summary>
        public ChangePasswordForm()
        {
            InitializeComponent();
            if (path == null)
            {
                path = Path.GetDirectoryName(Application.ExecutablePath);
                // Go upward until \scripts is found
                while (!Directory.Exists(path + Utilities.ScriptSubfolder))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
            }
            SetToolTips();
        }

        /// <summary>
        /// Creates a new <code>ChangePasswordForm</code>.
        /// </summary>
        /// <param name="path">Path to the base file.</param>
        public ChangePasswordForm(string path)
        {
            InitializeComponent();
            this.path = path;
            SetToolTips();
        }

        #endregion Constructors

        /// <summary>
        /// Changes the password of the user specified.
        /// Theorically.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePassword(object sender, EventArgs e)
        {
            // Hide error label until we get an error
            errorLabel.Visible = false;
            // Declare all future variables
            string username = usernameTextBox.Text,
                password = newPasswordTextBox.Text,
                passwordAgain = newPasswordAgainTextBox.Text,
                scriptContent, currentPath;

            // Is the password empty
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordAgain) || string.IsNullOrEmpty(currentUserPasswordTextBox.Text))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Entrez un mot de passe!";
                return;
            }
            // Is there an username provided
            else if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentUserTextBox.Text))
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
            string result = Utilities.GetScriptResults("(Get-ADDefaultDomainPasswordPolicy).MinPasswordLength");
            int minPasswordLength = 6;
            if (int.TryParse(result, out int i))
            {
                minPasswordLength = i;
            }

            // Check that password is longer than minimum length
            if (!(password.Length >= minPasswordLength))
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Le mot de passe doit faire au moins " + minPasswordLength.ToString() + " caractères de long!";
                return;
            }

            try
            {
                // Prepare script path
                currentPath = path + scriptSubfolder + PasswordScript;
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
                errorLabel.Text = "Erreur: Le fichier '" + PasswordScript + "' n'est pas dans /Scripts!";
                return;
            }

            // TODO: Find someone who can test that
            // Prepare the script for usage
            scriptContent = scriptContent.Replace("{part}", username).Replace("{newPassword}", password);
            result = Utilities.GetScriptResults(scriptContent);
            errorLabel.Visible = true;
            errorLabel.Text = result;
        }

        /// <summary>
        /// Creates tooltips for different textboxes
        /// </summary>
        private void SetToolTips()
        {
            tooltip.SetToolTip(usernameLabel, "Le nom de l'utilisateur qui a besoin d'un nouveau mot de passe");
            tooltip.SetToolTip(passwordLabel, "Le nouveau mot de passe de l'utilisateur");
            tooltip.SetToolTip(passwordAgainLabel, "Le nouveau mot de passe de l'utilisateur répété");
            tooltip.SetToolTip(currentUserLabel, "Un nom d'utilisateur qui a accès à l'AD");
            tooltip.SetToolTip(currentUserPasswordLabel, "Le mot de passe de l'utilisateur");
        }

        #region Events

        /// <summary>
        /// Calls <code>ChangePassword</code> when the user presses enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitOnEnter(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Press enter for input
                case Keys.Enter:
                    ChangePassword(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        #endregion Events
    }
}