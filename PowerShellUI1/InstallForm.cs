using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
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
            scriptSubfolder = ChoiceForm.ScriptSubfolder,
            imagesSubfolder = ChoiceForm.ImagesSubFolder;

        /// <summary>
        /// Whether or not the ActiveDirectory module is installed.
        /// </summary>
        public static bool IsADInstalled { get; private set; }
        /// <summary>
        /// The name of the installation script.
        /// </summary>
        private static string InstallScript => "Install-ADModule.ps1";

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        private readonly int SW_HIDE = 0;
        #endregion

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
        private void InstallADModulePowershell(object sender, EventArgs e)
        {
            if (IsADInstalled)
            {
                resultLabel.Visible = true;
                resultLabel.Text = "Le module AD est déjà installé";
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
                installBtn.Enabled = false;
                statusLabel.Text = "Installation du module AD, veuillez patienter.\nÇa va prendre un moment.";
                Image i = Image.FromFile(path + imagesSubfolder + "Loading.gif");
                statusLabel.BackgroundImage = i;
                statusLabel.Visible = true;
                // Load a new powershell
                Process proc = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Verb = "runas",
                    Arguments = $"set-executionpolicy unrestricted -force;\r\n{path + scriptSubfolder + InstallScript}",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                _ = ShowWindow(proc.MainWindowHandle, SW_HIDE);
                // Tell user that installation is in progress
                proc.Exited += ADInstallFinished;
            }
            catch
            {
                statusLabel.Text = "L'installation du module AD n'a pas pus être installé";
                statusLabel.Visible = true;
                installBtn.Enabled = true;
            }
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

        /// <summary>
        /// Tells the user that the module has been installed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ADInstallFinished(object sender, object e)
        {
            statusLabel.BackgroundImage = null;
            statusLabel.Visible = false;
            resultLabel.Visible = true;
            resultLabel.Text = "Le module AD a été installé.";
            _ = MessageBox.Show("Le module AD a été installé.");
            UpdateIsADInstalled();
        }
        #endregion
    }
}