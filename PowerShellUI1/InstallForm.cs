using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PowerShellUI1
{
    /// <summary>
    /// <code>InstallForm</code> is for installing the ActiveDirectory PowerShell module.
    /// </summary>
    public partial class InstallForm : Form
    {
        readonly string path = ChoiceForm.Path,
            scriptSubfolder = ChoiceForm.ScriptSubfolder,
            installScript = "Install-ADModule.ps1";

        /// <summary>
        /// Whether or not the ActiveDirectory module is installed.
        /// </summary>
        public static bool IsADInstalled { get; private set; }

        #region Constructors
        /// <summary>
        /// Creates a new instance of InstallForm.
        /// </summary>
        public InstallForm()
        {
            InitializeComponent();
            // Don't recalculate path if it already exists
            if (path == null)
            {
                path = Path.GetDirectoryName(Application.ExecutablePath);
                // Go upward until in AD_interface  
                while (!path.EndsWith("AD_interface"))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of InstallForm.
        /// </summary>
        /// <param name="path">Path to base file.</param>
        public InstallForm(string path)
        {
            InitializeComponent();
            this.path = path;
        }
        #endregion

        #region AD installation
        /// <summary>
        /// Installs the ActiveDirectory module on powershell.
        /// Does nothing if it is already installed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallADModulePowershell(object sender, System.EventArgs e)
        {
            if (IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "AD déjà installée";
                return;
            }
            else
            {
                resultLabel.Visible = false;
                statusLabel.Visible = false;

                // AD not installed yet, install it
                _ = InstallAD();
            }

            UpdateIsADInstalled();
        }

        /// <summary>
        /// Installs the ActiveDirectory module on powershell.
        /// </summary>
        /// <returns>True if the module was installed, false otherwise.</returns>
        private bool InstallAD()
        {
            // Load a new powershell
            try
            {
                Process proc = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = $"set-executionpolicy unrestricted -force;\r\n{path + scriptSubfolder + installScript}",
                    CreateNoWindow = false
                });
                // Tell user that installation is in progress
                statusLabel.Text = "Installation du module AD, veuillez patienter.\nÇa va prendre un moment.";
                statusLabel.Visible = true;
                while (!proc.HasExited) { };
                statusLabel.Visible = false;
            }
            catch
            {
                return false;
            }
            // Installation successful
            statusLabel.Visible = false;
            resultLabel.Visible = true;
            resultLabel.Text = "Le module AD a été installé.";

            return true;
        }
        #endregion

        #region Events
        /// <summary>
        /// Updates <code>IsADInstalled</code> to true if AD was installed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UpdateIsADInstalled(object sender = null, FormClosedEventArgs e = null)
        {
            StringBuilder strBui = new StringBuilder();
            using (Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = "Get-Command get-aduser -errorAction SilentlyContinue",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            })
            {
                try
                {
                    _ = process.Start();
                    while (!process.HasExited)
                    {
                        strBui.Append(process.StandardOutput.ReadToEnd());
                    }
                }
                catch { }
            }
            IsADInstalled = strBui.ToString().Length != 0;
        }

        /// <summary>
        /// Calls <code>InstallADModulePowershell</code> when the user presses enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallOnEnter(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Press enter for input
                case Keys.Enter:
                    InstallADModulePowershell(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }
        #endregion
    }
}
