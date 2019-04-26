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
            this.currentUserTextBox = new System.Windows.Forms.TextBox();
            this.currentUserPasswordTextBox = new System.Windows.Forms.TextBox();
            this.currentUserLabel = new System.Windows.Forms.Label();
            this.currentUserPasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(34, 167);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(185, 20);
            this.usernameTextBox.TabIndex = 2;
            this.usernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // changePasswordButton
            // 
            this.changePasswordButton.Location = new System.Drawing.Point(34, 338);
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
            this.newPasswordTextBox.Location = new System.Drawing.Point(34, 227);
            this.newPasswordTextBox.Name = "newPasswordTextBox";
            this.newPasswordTextBox.PasswordChar = '•';
            this.newPasswordTextBox.Size = new System.Drawing.Size(185, 20);
            this.newPasswordTextBox.TabIndex = 3;
            this.newPasswordTextBox.Text = "OrifInfo2009";
            this.newPasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // usernameLabel
            // 
            this.usernameLabel.Location = new System.Drawing.Point(34, 144);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(185, 20);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Nom de l\'utilisateur à changer";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // passwordLabel
            // 
            this.passwordLabel.Location = new System.Drawing.Point(34, 204);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(185, 20);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Nouveau mot de passe";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // passwordAgainLabel
            // 
            this.passwordAgainLabel.Location = new System.Drawing.Point(34, 268);
            this.passwordAgainLabel.Name = "passwordAgainLabel";
            this.passwordAgainLabel.Size = new System.Drawing.Size(185, 20);
            this.passwordAgainLabel.TabIndex = 6;
            this.passwordAgainLabel.Text = "Répéter le nouveau mot de passe";
            this.passwordAgainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newPasswordAgainTextBox
            // 
            this.newPasswordAgainTextBox.Location = new System.Drawing.Point(34, 291);
            this.newPasswordAgainTextBox.Name = "newPasswordAgainTextBox";
            this.newPasswordAgainTextBox.PasswordChar = '•';
            this.newPasswordAgainTextBox.Size = new System.Drawing.Size(185, 20);
            this.newPasswordAgainTextBox.TabIndex = 4;
            this.newPasswordAgainTextBox.Text = "OrifInfo2009";
            this.newPasswordAgainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // errorLabel
            // 
            this.errorLabel.BackColor = System.Drawing.Color.Salmon;
            this.errorLabel.Location = new System.Drawing.Point(34, 50);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(448, 80);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.errorLabel.Visible = false;
            // 
            // currentUserTextBox
            // 
            this.currentUserTextBox.Location = new System.Drawing.Point(288, 227);
            this.currentUserTextBox.Name = "currentUserTextBox";
            this.currentUserTextBox.Size = new System.Drawing.Size(194, 20);
            this.currentUserTextBox.TabIndex = 0;
            this.currentUserTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // currentUserPasswordTextBox
            // 
            this.currentUserPasswordTextBox.Location = new System.Drawing.Point(288, 287);
            this.currentUserPasswordTextBox.Name = "currentUserPasswordTextBox";
            this.currentUserPasswordTextBox.PasswordChar = '•';
            this.currentUserPasswordTextBox.Size = new System.Drawing.Size(194, 20);
            this.currentUserPasswordTextBox.TabIndex = 1;
            this.currentUserPasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitOnEnter);
            // 
            // currentUserLabel
            // 
            this.currentUserLabel.Location = new System.Drawing.Point(288, 204);
            this.currentUserLabel.Name = "currentUserLabel";
            this.currentUserLabel.Size = new System.Drawing.Size(194, 20);
            this.currentUserLabel.TabIndex = 8;
            this.currentUserLabel.Text = "Votre nom d\'utilisateur";
            this.currentUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentUserPasswordLabel
            // 
            this.currentUserPasswordLabel.Location = new System.Drawing.Point(288, 264);
            this.currentUserPasswordLabel.Name = "currentUserPasswordLabel";
            this.currentUserPasswordLabel.Size = new System.Drawing.Size(194, 20);
            this.currentUserPasswordLabel.TabIndex = 9;
            this.currentUserPasswordLabel.Text = "Votre mot de passe";
            this.currentUserPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 450);
            this.Controls.Add(this.currentUserPasswordLabel);
            this.Controls.Add(this.currentUserLabel);
            this.Controls.Add(this.currentUserPasswordTextBox);
            this.Controls.Add(this.currentUserTextBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.passwordAgainLabel);
            this.Controls.Add(this.newPasswordAgainTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.newPasswordTextBox);
            this.Controls.Add(this.changePasswordButton);
            this.Controls.Add(this.usernameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChangePasswordForm";
            this.ShowIcon = false;
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
        private System.Windows.Forms.TextBox currentUserTextBox;
        private System.Windows.Forms.TextBox currentUserPasswordTextBox;
        private System.Windows.Forms.Label currentUserLabel;
        private System.Windows.Forms.Label currentUserPasswordLabel;
    }
}