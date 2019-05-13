using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerShellUI1
{
    public partial class InstallForm : Form
    {
        readonly string path,
            scriptSubfolder = ChoiceForm.ScriptSubfolder,
            installScript = "Install-ADModule.ps1";

        /// <summary>
        /// Whether or not the ActiveDirectory module is installed.
        /// </summary>
        public static bool IsADInstalled { get; private set; }

        /// <summary>
        /// Creates a new instance of InstallForm.
        /// </summary>
        public InstallForm()
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

        /// <summary>
        /// Creates a new instance of InstallForm.
        /// </summary>
        /// <param name="path">Path to base file.</param>
        public InstallForm(string path)
        {
            InitializeComponent();
            this.path = path;
        }

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
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = "Get-ExecutionPolicy",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
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
            // Checks that the policy is correct
            bool resetRequired = false;
            if (!(new string[] { "unrestricted", "remotesigned", "bypass" }).Contains(currentExecPoli))
            {
                // Otherwise, change it
                if(!ChangeExecPoli())
                {
                    // Could not change the execution policy, cancel everything
                    statusLabel.Visible = true;
                    statusLabel.Text = "Erreur inconnue: Annulation de l'autorisation des scripts";
                    return;
                }
                resetRequired = true;
            }

            UpdateIsADInstalled();
            if (!IsADInstalled)
            {
                // Command not found, install AD
                if (!InstallAD())
                {
                    // AD could not be installed
                    ChangeExecPoli(currentExecPoli);
                    statusLabel.Text = "Erreur inconnue: Annulation de l'installation du mode ActiveDirectory";
                    statusLabel.Visible = true;
                    return;
                }
            }

            // Reset execution policy to what it was before
            if (!ChangeExecPoli(currentExecPoli) && resetRequired)
            {
                // Could not reset the execution policy, cancel everything
                statusLabel.Visible = true;
                statusLabel.Text = "Erreur inconnue: Impossible de remettre l'execution de scripts comme précédent";
                return;
            }

            UpdateIsADInstalled();
        }

        /// <summary>
        /// Changes the execution policy.
        /// </summary>
        /// <param name="newExecPoli">The new execution policy. Defaults to null.</param>
        /// <returns>True if the policy was changed, false if it was not.</returns>
        private bool ChangeExecPoli(string newExecPoli = null)
        {
            string arg = "set-executionpolicy remotesigned -force";
            if (newExecPoli != null)
            {
                arg = arg.Replace("remotesigned", newExecPoli);
            }
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = arg,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            try
            {
                process.Start();
            }
            catch
            {
                return false;
            }
            return true;
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
            var newProcessInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell.exe",
                Verb = "runas",
                Arguments = path + scriptSubfolder + installScript,
                CreateNoWindow = false
            };
            try
            {
                System.Diagnostics.Process.Start(newProcessInfo);
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

        /// <summary>
        /// Updates <code>IsADInstalled</code> to true if AD was installed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static public void UpdateIsADInstalled(object sender = null, FormClosedEventArgs e = null)
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
    }
}
