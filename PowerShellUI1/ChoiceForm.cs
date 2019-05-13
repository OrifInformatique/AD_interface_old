using System;
using System.IO;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class ChoiceForm : Form
    {
        private const string scriptSubfolder = "\\Scripts\\";

        private readonly string path;
        private readonly ToolTip toolTip = new ToolTip();

        private RetreiveForm retreiveData;
        private ChangePasswordForm changePassword;
        private InstallForm installAD;

        /// <summary>
        /// The subfolder containing all scripts.
        /// </summary>
        public static string ScriptSubfolder => scriptSubfolder;

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
            path = Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!path.EndsWith("AD_interface"))
            {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }

            UpdateEnabledButtons();
        }

        /// <summary>
        /// Opens a <code>RetreiveForm</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenRetreiveForm(object sender, EventArgs e)
        {
            if (retreiveData == null || retreiveData.IsDisposed)
                retreiveData = new RetreiveForm(path);
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
                changePassword = new ChangePasswordForm(path);
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
                installAD = new InstallForm(path);
            installAD.Show();
            installAD.FormClosed += new FormClosedEventHandler(UpdateEnabledButtons);
        }

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