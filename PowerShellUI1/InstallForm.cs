using System;
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
        #region Variables

        private readonly string path = ChoiceForm.Path,
            scriptSubfolder = ChoiceForm.ScriptSubfolder;

        /// <summary>
        /// Whether or not the ActiveDirectory module is installed.
        /// </summary>
        public static bool IsADInstalled { get; private set; }

        /// <summary>
        /// Whether or not the module can be installed
        /// </summary>
        public static bool CanInstallAD { get; private set; }

        /// <summary>
        /// The name of the installation script.
        /// </summary>
        private static string InstallScript => "Install-ADModule.ps1";

        /// <summary>
        /// Whether or not the AD module is being installed
        /// </summary>
        private static bool IsAdInstalling = false;

        #endregion Variables

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
                while (!Directory.Exists(path + ChoiceForm.ScriptSubfolder))
                {
                    int index = path.LastIndexOf("\\");
                    path = path.Substring(0, index);
                }
            }
            installBtn.Enabled = !(IsADInstalled || IsAdInstalling);
            if (IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "Le module AD est déjà installé";
                return;
            }
            else if (IsAdInstalling)
            {
                statusLabel.Visible = true;
                statusLabel.Text = "Le module AD est en cours d'installation";
                return;
            }

            UpdateCanInstallAD();
        }

        /// <summary>
        /// Creates a new instance of InstallForm.
        /// </summary>
        /// <param name="path">Path to base file.</param>
        public InstallForm(string path)
        {
            InitializeComponent();
            this.path = path;
            installBtn.Enabled = !(IsADInstalled || IsAdInstalling);
            if (IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "Le module AD est déjà installé";
                return;
            }
            else if (IsAdInstalling)
            {
                statusLabel.Visible = true;
                statusLabel.Text = "Le module AD est en cours d'installation";
                return;
            }

            UpdateCanInstallAD();
        }

        #endregion Constructors

        #region AD installation

        /// <summary>
        /// Installs the ActiveDirectory module on powershell.
        /// Does nothing if it is already installed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallADModulePowershell(object sender, EventArgs e)
        {
            if (!CanInstallAD)
            {
                statusLabel.Visible = true;
                statusLabel.Text = "Votre version de PowerShell ne peux pas installer le module AD.\nVeuillez installer PowerShell en version 4 ou plus";
                return;
            }
            else if (IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "Le module AD est déjà installé";
                return;
            }
            else if (IsAdInstalling)
            {
                statusLabel.Visible = true;
                statusLabel.Text = "Le module AD est en cours d'installation";
                return;
            }
            else
            {
                resultLabel.Visible = false;
                statusLabel.Visible = false;

                // AD not installed yet, install it
                InstallAD();
            }

            UpdateIsADInstalled();
        }

        /// <summary>
        /// Installs the ActiveDirectory module on powershell.
        /// </summary>
        /// <returns>True if the module was installed, false otherwise.</returns>
        private void InstallAD()
        {
            try
            {
                string targetfolder = path + scriptSubfolder;

                // Tell user that installation is in progress
                IsAdInstalling = true;
                installBtn.Enabled = false;
                statusLabel.Text = "Installation du module AD, veuillez patienter.\nÇa va prendre autour de 5 minutes.";
                statusLabel.Visible = true;
                // Load a new powershell
                using (Process proc = new Process
                {
                    EnableRaisingEvents = true,
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Verb = "runas",
                        Arguments = $"set-executionpolicy unrestricted -force;\r\n{targetfolder + InstallScript}"
                    }
                })
                {
                    _ = proc.Start();
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                    {
                        ADInstallFinished();
                    }
                    else
                    {
                        IsAdInstalling = false;
                        statusLabel.Text = "L'installation du module AD a été annulée";
                        statusLabel.Visible = true;
                        installBtn.Enabled = true;
                    }
                }
            }
            catch
            {
                IsAdInstalling = false;
                statusLabel.Text = "L'installation du module AD n'a pas pus être commencée";
                statusLabel.Visible = true;
                installBtn.Enabled = true;
                return;
            }
        }

        #endregion AD installation

        #region Events

        /// <summary>
        /// Updates CanInstallAD
        /// </summary>
        public static void UpdateCanInstallAD()
        {
            StringBuilder strBui = new StringBuilder();
            using (Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "$PSVersionTable.PSVersion.Major | Out-String",
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
                        _ = strBui.Append(process.StandardOutput.ReadToEnd());
                    }
                }
                catch
                {
                    _ = strBui.Clear();
                }
            }
            int psversion = int.TryParse(strBui.ToString(), out int i) ? i : 1;
            CanInstallAD = !(psversion < 4);
        }

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
                        _ = strBui.Append(process.StandardOutput.ReadToEnd());
                    }
                }
                catch
                {
                    _ = strBui.Clear();
                }
            }
            IsADInstalled = strBui.ToString().Length != 0;
            if (IsADInstalled)
            {
                IsAdInstalling = false;
            }
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

        /// <summary>
        /// Tells the user that the module has been installed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ADInstallFinished(object sender = null, EventArgs e = null)
        {
            IsAdInstalling = false;
            installBtn.Enabled = true;
            statusLabel.Visible = false;
            resultLabel.Visible = true;
            resultLabel.Text = "Le module AD a été installé.";
            _ = MessageBox.Show("Le module AD a été installé.");

            {
                OperatingSystem os = Environment.OSVersion;
                Version vs = os.Version;
                // Special message for windows 7 users,
                // because I can't manage that part yet
                if (os.Platform == PlatformID.Win32NT &&
                    vs.Major == 6 && vs.Minor == 1)
                {
                    _ = MessageBox.Show("Le module AD a été installé mais n'a pas été activé.\nVeuillez l'activer manuellement.");
                }
            }

            UpdateIsADInstalled();
        }

        #endregion Events
    }
}