﻿namespace PowerShellUI1
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
            this.OpenInstallADForm = new System.Windows.Forms.Button();
            this.openPwdForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openRetrieveFrom
            // 
            this.openRetrieveFrom.Location = new System.Drawing.Point(177, 12);
            this.openRetrieveFrom.Name = "openRetrieveFrom";
            this.openRetrieveFrom.Size = new System.Drawing.Size(158, 40);
            this.openRetrieveFrom.TabIndex = 1;
            this.openRetrieveFrom.Text = "Informations";
            this.openRetrieveFrom.UseVisualStyleBackColor = true;
            this.openRetrieveFrom.Click += new System.EventHandler(this.OpenRetreiveForm);
            // 
            // OpenInstallADForm
            // 
            this.OpenInstallADForm.Location = new System.Drawing.Point(12, 12);
            this.OpenInstallADForm.Name = "OpenInstallADForm";
            this.OpenInstallADForm.Size = new System.Drawing.Size(159, 40);
            this.OpenInstallADForm.TabIndex = 0;
            this.OpenInstallADForm.Text = "Installation";
            this.OpenInstallADForm.UseVisualStyleBackColor = true;
            this.OpenInstallADForm.Click += new System.EventHandler(this.OpenInstallADForm_Click);
            // 
            // openPwdForm
            // 
            this.openPwdForm.Location = new System.Drawing.Point(341, 12);
            this.openPwdForm.Name = "openPwdForm";
            this.openPwdForm.Size = new System.Drawing.Size(158, 40);
            this.openPwdForm.TabIndex = 2;
            this.openPwdForm.Text = "Mot de passe";
            this.openPwdForm.UseVisualStyleBackColor = true;
            this.openPwdForm.Click += new System.EventHandler(this.OpenPwdForm_Click);
            // 
            // ChoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OpenInstallADForm);
            this.Controls.Add(this.openPwdForm);
            this.Controls.Add(this.openRetrieveFrom);
            this.MinimumSize = new System.Drawing.Size(550, 125);
            this.Name = "ChoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choisir une option";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openRetrieveFrom;
        private System.Windows.Forms.Button OpenInstallADForm;
        private System.Windows.Forms.Button openPwdForm;
    }
}