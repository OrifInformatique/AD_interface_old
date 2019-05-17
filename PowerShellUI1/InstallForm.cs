using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace PowerShellUI1
{
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
            if(IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "AD déjà installée";
                return;
            }
            resultLabel.Visible = false;
            statusLabel.Visible = false;
            // Obtain current execution policy.
            // Required for installation.
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = "Get-ExecutionPolicy",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };
            // Obtain output data
            StringBuilder strBui = new StringBuilder();
            try
            {
                process.Start();
                while(!process.HasExited)
                {
                    strBui.Append(process.StandardOutput.ReadToEnd());
                }
            }
            catch
            {
                // Error in data, cancel fetching
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur inconnue: Annulation de l'autorisation des scripts";
                return;
            }
            // Current execution policy
            string currentExecPoli = strBui.ToString().ToLower();
            // Clears variables
            strBui.Clear();

            if (!IsADInstalled)
            {
                // Command not found, install AD
                InstallAD();
            }

            UpdateIsADInstalled();
        }

        /// <summary>
        /// Installs the ActiveDirectory module on powershell.
        /// </summary>
        /// <returns>True if the module was installed, false otherwise.</returns>
        private bool InstallAD()
        {
            // Tell user that installation is in progress
            statusLabel.Text = "Installation du module AD, veuillez patienter.\nÇa va prendre un moment.";
            statusLabel.Visible = true;
            // Load a new powershell
            ProcessStartInfo newProcessInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Verb = "runas",
                Arguments = $"set-executionpolicy unrestricted -force;\r\n{path + scriptSubfolder + installScript}",
                CreateNoWindow = false
            };
            try
            {
                Process proc = Process.Start(newProcessInfo);
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
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = "Get-Command get-aduser -errorAction SilentlyContinue",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            try
            {
                process.Start();
                while (!process.HasExited)
                {
                    strBui.Append(process.StandardOutput.ReadToEnd());
                }
            }
            catch { }
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
