namespace PowerShellUI1
{
    partial class ChoiceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openRetrieveFrom = new System.Windows.Forms.Button();
            this.openInstallADForm = new System.Windows.Forms.Button();
            this.openPwdForm = new System.Windows.Forms.Button();
            this.ADInstallInfoLabel = new System.Windows.Forms.Label();
            this.RetreiveInfoLabel = new System.Windows.Forms.Label();
            this.PwdInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openRetrieveFrom
            // 
            this.openRetrieveFrom.Location = new System.Drawing.Point(176, 55);
            this.openRetrieveFrom.Name = "openRetrieveFrom";
            this.openRetrieveFrom.Size = new System.Drawing.Size(158, 40);
            this.openRetrieveFrom.TabIndex = 1;
            this.openRetrieveFrom.Text = "Informations";
            this.openRetrieveFrom.UseVisualStyleBackColor = true;
            this.openRetrieveFrom.Click += new System.EventHandler(this.OpenRetreiveForm);
            // 
            // openInstallADForm
            // 
            this.openInstallADForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openInstallADForm.Location = new System.Drawing.Point(12, 55);
            this.openInstallADForm.Name = "openInstallADForm";
            this.openInstallADForm.Size = new System.Drawing.Size(158, 40);
            this.openInstallADForm.TabIndex = 0;
            this.openInstallADForm.Text = "Installation";
            this.openInstallADForm.UseVisualStyleBackColor = true;
            this.openInstallADForm.Click += new System.EventHandler(this.OpenInstallADForm_Click);
            // 
            // openPwdForm
            // 
            this.openPwdForm.Location = new System.Drawing.Point(341, 55);
            this.openPwdForm.Name = "openPwdForm";
            this.openPwdForm.Size = new System.Drawing.Size(158, 40);
            this.openPwdForm.TabIndex = 2;
            this.openPwdForm.Text = "Mot de passe";
            this.openPwdForm.UseVisualStyleBackColor = true;
            this.openPwdForm.Click += new System.EventHandler(this.OpenPwdForm_Click);
            // 
            // ADInstallInfoLabel
            // 
            this.ADInstallInfoLabel.Location = new System.Drawing.Point(12, 12);
            this.ADInstallInfoLabel.Name = "ADInstallInfoLabel";
            this.ADInstallInfoLabel.Size = new System.Drawing.Size(158, 40);
            this.ADInstallInfoLabel.TabIndex = 3;
            this.ADInstallInfoLabel.Text = "Installe les fonctionnalités requises pour l\'application.";
            // 
            // RetreiveInfoLabel
            // 
            this.RetreiveInfoLabel.Location = new System.Drawing.Point(176, 12);
            this.RetreiveInfoLabel.Name = "RetreiveInfoLabel";
            this.RetreiveInfoLabel.Size = new System.Drawing.Size(158, 40);
            this.RetreiveInfoLabel.TabIndex = 4;
            this.RetreiveInfoLabel.Text = "Ouvre une fenêtre pour obtenir les informations sur un utilisateur/ordinateur.";
            // 
            // PwdInfoLabel
            // 
            this.PwdInfoLabel.Location = new System.Drawing.Point(341, 12);
            this.PwdInfoLabel.Name = "PwdInfoLabel";
            this.PwdInfoLabel.Size = new System.Drawing.Size(158, 40);
            this.PwdInfoLabel.TabIndex = 5;
            this.PwdInfoLabel.Text = "Ouvre une fenêtre pour changer le mot de passe d\'un utilisateur.";
            // 
            // ChoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 450);
            this.Controls.Add(this.PwdInfoLabel);
            this.Controls.Add(this.RetreiveInfoLabel);
            this.Controls.Add(this.ADInstallInfoLabel);
            this.Controls.Add(this.openInstallADForm);
            this.Controls.Add(this.openPwdForm);
            this.Controls.Add(this.openRetrieveFrom);
            this.MinimumSize = new System.Drawing.Size(530, 125);
            this.Name = "ChoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choisir une option";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openRetrieveFrom;
        private System.Windows.Forms.Button openInstallADForm;
        private System.Windows.Forms.Button openPwdForm;
        private System.Windows.Forms.Label ADInstallInfoLabel;
        private System.Windows.Forms.Label RetreiveInfoLabel;
        private System.Windows.Forms.Label PwdInfoLabel;
    }
}