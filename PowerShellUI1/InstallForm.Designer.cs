namespace PowerShellUI1
{
    partial class InstallForm
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
            this.installBtn = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // installBtn
            // 
            this.installBtn.Location = new System.Drawing.Point(13, 195);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(277, 62);
            this.installBtn.TabIndex = 0;
            this.installBtn.Text = "Installer les choses requises pour l\'application";
            this.installBtn.UseVisualStyleBackColor = true;
            this.installBtn.Click += new System.EventHandler(this.InstallADModulePowershell);
            this.installBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InstallOnEnter);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Salmon;
            this.statusLabel.Location = new System.Drawing.Point(10, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(276, 172);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Visible = false;
            // 
            // resultLabel
            // 
            this.resultLabel.AllowDrop = true;
            this.resultLabel.BackColor = System.Drawing.Color.PaleGreen;
            this.resultLabel.Location = new System.Drawing.Point(13, 269);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(276, 172);
            this.resultLabel.TabIndex = 2;
            this.resultLabel.Visible = false;
            // 
            // InstallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 450);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.installBtn);
            this.Name = "InstallForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installeur de l\'AD";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InstallOnEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label resultLabel;
    }
}