namespace PowerShellUI1
{
    partial class ChangePasswordForm
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
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.changePasswordButton = new System.Windows.Forms.Button();
            this.newPasswordTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordAgainLabel = new System.Windows.Forms.Label();
            this.newPasswordAgainTextBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(302, 151);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(185, 20);
            this.usernameTextBox.TabIndex = 0;
            this.usernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // changePasswordButton
            // 
            this.changePasswordButton.Location = new System.Drawing.Point(302, 314);
            this.changePasswordButton.Name = "changePasswordButton";
            this.changePasswordButton.Size = new System.Drawing.Size(185, 38);
            this.changePasswordButton.TabIndex = 5;
            this.changePasswordButton.Text = "Changer le mot de passe de l\'utilisateur";
            this.changePasswordButton.UseVisualStyleBackColor = true;
            this.changePasswordButton.Click += new System.EventHandler(this.ChangePasswordButton_Click);
            this.changePasswordButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // newPasswordTextBox
            // 
            this.newPasswordTextBox.Location = new System.Drawing.Point(302, 211);
            this.newPasswordTextBox.Name = "newPasswordTextBox";
            this.newPasswordTextBox.PasswordChar = '•';
            this.newPasswordTextBox.Size = new System.Drawing.Size(185, 20);
            this.newPasswordTextBox.TabIndex = 1;
            this.newPasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // usernameLabel
            // 
            this.usernameLabel.Location = new System.Drawing.Point(302, 128);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(185, 20);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Nom d\'utilisateur";
            // 
            // passwordLabel
            // 
            this.passwordLabel.Location = new System.Drawing.Point(302, 188);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(185, 20);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Nouveau mot de passe";
            // 
            // passwordAgainLabel
            // 
            this.passwordAgainLabel.Location = new System.Drawing.Point(302, 252);
            this.passwordAgainLabel.Name = "passwordAgainLabel";
            this.passwordAgainLabel.Size = new System.Drawing.Size(185, 20);
            this.passwordAgainLabel.TabIndex = 6;
            this.passwordAgainLabel.Text = "Répéter le nouveau mot de passe";
            // 
            // newPasswordAgainTextBox
            // 
            this.newPasswordAgainTextBox.Location = new System.Drawing.Point(302, 275);
            this.newPasswordAgainTextBox.Name = "newPasswordAgainTextBox";
            this.newPasswordAgainTextBox.PasswordChar = '•';
            this.newPasswordAgainTextBox.Size = new System.Drawing.Size(185, 20);
            this.newPasswordAgainTextBox.TabIndex = 2;
            this.newPasswordAgainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // errorLabel
            // 
            this.errorLabel.BackColor = System.Drawing.Color.Salmon;
            this.errorLabel.Location = new System.Drawing.Point(302, 34);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(185, 80);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.errorLabel.Visible = false;
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.passwordAgainLabel);
            this.Controls.Add(this.newPasswordAgainTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.newPasswordTextBox);
            this.Controls.Add(this.changePasswordButton);
            this.Controls.Add(this.usernameTextBox);
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Changer le mot de passe d\'un utilisateur";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Button changePasswordButton;
        private System.Windows.Forms.TextBox newPasswordTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label passwordAgainLabel;
        private System.Windows.Forms.TextBox newPasswordAgainTextBox;
        private System.Windows.Forms.Label errorLabel;
    }
}