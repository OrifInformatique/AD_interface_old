using System;
using System.IO;
using System.Management.Automation;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class ChoiceForm : Form
    {
        private const string scriptSubfolder = "\\Scripts\\",
            installScript = "Install-ADModule.ps1";
        private readonly string path;

        readonly ToolTip toolTip1 = new ToolTip();
        RetreiveForm retreiveData;
        ChangePasswordForm changePassword;

        public static string ScriptSubfolder => scriptSubfolder;

        public ChoiceForm()
        {
            InitializeComponent();
            toolTip1.SetToolTip(openRetrieveFrom, "Ouvrir un formulaire pour voir les informations d'un utilisateur / ordinateur");

            // Get the current path
            path = Path.GetDirectoryName(Application.ExecutablePath);
            // Go upward until in AD_interface  
            while (!path.EndsWith("AD_interface"))
            {
                int index = path.LastIndexOf("\\");
                path = path.Substring(0, index);
            }
        }

        private void OpenARetreiveForm(object sender, EventArgs e)
        {
            if (retreiveData == null || retreiveData.IsDisposed)
                retreiveData = new RetreiveForm(path);
            retreiveData.Show();
        }

        private void OpenPwdForm_Click(object sender, EventArgs e)
        {
            if (changePassword == null || changePassword.IsDisposed)
                changePassword = new ChangePasswordForm(path);
            changePassword.Show();
        }

        private void InstallADModulePowershell(object sender, EventArgs e)
        {
            // Use path to powershell.exe for full script
            var newProcessInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell.exe",
                Verb = "runas",
                Arguments = path + scriptSubfolder + installScript,
                CreateNoWindow = false
            };
            System.Diagnostics.Process.Start(newProcessInfo);
        }
    }
}