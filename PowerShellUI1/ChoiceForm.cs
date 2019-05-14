using System;
using System.IO;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class ChoiceForm : Form
    {
        #region Variables
        private const string scriptSubfolder = "\\Scripts\\";
        private static string pathInner;

        private readonly ToolTip toolTip = new ToolTip();

        private RetreiveForm retreiveData;
        private ChangePasswordForm changePassword;
        private InstallForm installAD;

        /// <summary>
        /// The subfolder containing all scripts.
        /// </summary>
        public static string ScriptSubfolder => scriptSubfolder;

        /// <summary>
        /// Path to AD_interface folder.
        /// </summary>
        public static string Path { get => pathInner; private set => pathInner = value; }
        #endregion

        /// <summary>
        /// <code>ChoiceForm</code>s are the hub of the app.
        /// </summary>
        public ChoiceForm()
        {
            InitializeComponent();
            toolTip.SetToolTip(openRetrieveFrom, "Ouvrir un formulaire pour voir les informations d'un utilisateur / ordinateur");

            InstallForm.UpdateIsADInstalled();
            if (!InstallForm.IsADInstalled)
                MessageBox.Show("Le module AD n'est pas installé. L'application ne peut pas fonctionner sans.");

            // Get the current path
            pathInner = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!pathInner.EndsWith("AD_interface"))
            {
                int index = pathInner.LastIndexOf("\\");
                pathInner = pathInner.Substring(0, index);
            }

            UpdateEnabledButtons();
        }

        #region Forms
        /// <summary>
        /// Opens a <code>RetreiveForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenRetreiveForm(object sender, EventArgs e)
        {
            if (retreiveData == null || retreiveData.IsDisposed)
                retreiveData = new RetreiveForm(pathInner);
            retreiveData.Show();
        }

        /// <summary>
        /// Opens a <code>ChangePasswordForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPwdForm_Click(object sender, EventArgs e)
        {
            if (changePassword == null || changePassword.IsDisposed)
                changePassword = new ChangePasswordForm(pathInner);
            changePassword.Show();
        }

        /// <summary>
        /// Opens an <code>InstallForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenInstallADForm_Click(object sender, EventArgs e)
        {
            if (installAD == null || installAD.IsDisposed)
                installAD = new InstallForm(pathInner);
            installAD.Show();
            installAD.FormClosed += new FormClosedEventHandler(UpdateEnabledButtons);
        }
        #endregion

        /// <summary>
        /// Enables/disables the different buttons depending on the state of the AD module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEnabledButtons(object sender = null, FormClosedEventArgs e = null)
        {
            bool b = InstallForm.IsADInstalled;
            openRetrieveFrom.Enabled = b;
            openPwdForm.Enabled = b;
        }
    }
}