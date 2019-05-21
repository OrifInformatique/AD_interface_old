﻿using System;
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
            while (!Path.EndsWith("AD_interface"))
            {
                int index = Path.LastIndexOf("\\");
                Path = Path.Substring(0, index);
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
        private void OpenPwdForm_Click(object sender, EventArgs e)
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
        private void OpenInstallADForm_Click(object sender, EventArgs e)
        {
            UpdateEnabledButtons(sender, null);
            if (installAD == null || installAD.IsDisposed)
            {
                installAD = new InstallForm(Path);
                installAD.FormClosed += new FormClosedEventHandler(UpdateEnabledButtons);
            }

            installAD.Show();
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
        }
    }
}