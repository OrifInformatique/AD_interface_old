using System;
using System.IO;
using System.Windows.Forms;

namespace PowerShellUI1
{
    /// <summary>
    /// <code>ChoiceForm</code> serves as the hub of the app.
    /// </summary>
    public partial class ChoiceForm : Form
    {
        #region Variables
        private RetreiveForm retreiveData;
        private ChangePasswordForm changePassword;
        private InstallForm installAD;
        private AdvancedRetreiveForm advancedRetreiveData;

        #region Static
        /// <summary>
        /// The subfolder containing all scripts.
        /// </summary>
        public static string ScriptSubfolder => "\\Scripts\\";
        /// <summary>
        /// Path to AD_interface folder.
        /// </summary>
        public static string Path { get; private set; }
        #endregion
        #endregion

        /// <summary>
        /// Creates a new ChoiceForm.
        /// </summary>
        public ChoiceForm()
        {
            InitializeComponent();

            InstallForm.UpdateIsADInstalled();
            if (!InstallForm.IsADInstalled)
            {
                _ = MessageBox.Show("Le module AD n'est pas installé. L'application ne peut pas fonctionner sans.");
            }

            // Get the current path
            Path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface
            while (!Directory.Exists(Path + ScriptSubfolder))
            {
                int index = Path.LastIndexOf("\\");
                Path = Path.Substring(0, index);
                if (Path.EndsWith(":"))
                { // Could not find \Scripts, exit application
                    _ = MessageBox.Show("Le dossier \\Scripts n'a pas été trouvé dans le dossier du l'application ou un dossier parent.");
                    if (Application.MessageLoop)
                    {
                        // WinForms app
                        Application.Exit();
                    }
                    else
                    {
                        // Console app
                        Environment.Exit(1);
                    }
                }
            }

            UpdateEnabledButtons(this, null);
        }

        #region Forms opening
        /// <summary>
        /// Opens a <code>RetreiveForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenRetreiveForm(object sender, EventArgs e)
        {
            if (retreiveData == null || retreiveData.IsDisposed)
            {
                retreiveData = new RetreiveForm(Path);
            }

            retreiveData.Show();
        }

        /// <summary>
        /// Opens a <code>ChangePasswordForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPwdForm(object sender, EventArgs e)
        {
            if (changePassword == null || changePassword.IsDisposed)
            {
                changePassword = new ChangePasswordForm(Path);
            }

            changePassword.Show();
        }

        /// <summary>
        /// Opens an <code>InstallForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenInstallADForm(object sender, EventArgs e)
        {
            if (installAD == null || installAD.IsDisposed)
            {
                installAD = new InstallForm(Path);
                installAD.FormClosed += new FormClosedEventHandler(UpdateEnabledButtons);
            }
            if (!InstallForm.IsADInstalled)
            {
                InstallForm.UpdateIsADInstalled();
                UpdateEnabledButtons(sender, null);
            }
            installAD.Show();
        }

        /// <summary>
        /// Opens an AdvancedRetreiveForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAdvancedRetreiveForm(object sender, EventArgs e)
        {
            if(advancedRetreiveData == null || advancedRetreiveData.IsDisposed)
            {
                advancedRetreiveData = new AdvancedRetreiveForm(Path);
            }

            advancedRetreiveData.Show();
        }
        #endregion

        /// <summary>
        /// Enables/disables the different buttons depending on the state of the AD module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEnabledButtons(object sender, FormClosedEventArgs e)
        {
            bool b = InstallForm.IsADInstalled;
            openRetrieveFrom.Enabled = b;
            openPwdForm.Enabled = b;
            openAdvancedRetreiveForm.Enabled = b;
        }
    }
}