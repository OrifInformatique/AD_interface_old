namespace PowerShellUI1
{
    partial class Form1
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
            this.getUserButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.optionsListBox = new System.Windows.Forms.CheckedListBox();
            this.filterList = new System.Windows.Forms.ListBox();
            this.ifMultipleLabel = new System.Windows.Forms.Label();
            this.whichNumberUD = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.whichNumberUD)).BeginInit();
            this.SuspendLayout();
            // 
            // getUserButton
            // 
            this.getUserButton.Location = new System.Drawing.Point(100, 120);
            this.getUserButton.Name = "getUserButton";
            this.getUserButton.Size = new System.Drawing.Size(215, 23);
            this.getUserButton.TabIndex = 0;
            this.getUserButton.Text = "Obtenir les informations de l\'utilisateur";
            this.getUserButton.UseVisualStyleBackColor = true;
            this.getUserButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(100, 83);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(215, 20);
            this.usernameTextBox.TabIndex = 1;
            this.usernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScriptTextBox_KeyDown);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Red;
            this.statusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusLabel.Location = new System.Drawing.Point(32, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(350, 58);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusLabel.Visible = false;
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(32, 178);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(350, 260);
            this.resultTextBox.TabIndex = 3;
            this.resultTextBox.Text = "";
            // 
            // optionsListBox
            // 
            this.optionsListBox.CheckOnClick = true;
            this.optionsListBox.FormattingEnabled = true;
            this.optionsListBox.Items.AddRange(new object[] {
            "Nom",
            "Prénom",
            "Identifiant",
            "Adresse E-mail",
            "Nom Technique",
            "Actif",
            "Nom Complet",
            "SID"});
            this.optionsListBox.Location = new System.Drawing.Point(447, 12);
            this.optionsListBox.Name = "optionsListBox";
            this.optionsListBox.Size = new System.Drawing.Size(325, 124);
            this.optionsListBox.TabIndex = 4;
            // 
            // filterList
            // 
            this.filterList.FormattingEnabled = true;
            this.filterList.Items.AddRange(new object[] {
            "Nom",
            "Prénom",
            "Identifiant",
            "Adresse E-mail",
            "Nom Technique",
            "Nom Complet",
            "SID"});
            this.filterList.Location = new System.Drawing.Point(447, 155);
            this.filterList.Name = "filterList";
            this.filterList.Size = new System.Drawing.Size(325, 95);
            this.filterList.TabIndex = 5;
            // 
            // ifMultipleLabel
            // 
            this.ifMultipleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifMultipleLabel.Location = new System.Drawing.Point(447, 279);
            this.ifMultipleLabel.Name = "ifMultipleLabel";
            this.ifMultipleLabel.Size = new System.Drawing.Size(325, 18);
            this.ifMultipleLabel.TabIndex = 6;
            this.ifMultipleLabel.Text = "S\'il y a plusieurs utilisateurs, choisir le quel montrer";
            // 
            // whichNumberUD
            // 
            this.whichNumberUD.Location = new System.Drawing.Point(450, 301);
            this.whichNumberUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.whichNumberUD.Name = "whichNumberUD";
            this.whichNumberUD.Size = new System.Drawing.Size(322, 20);
            this.whichNumberUD.TabIndex = 7;
            this.whichNumberUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.whichNumberUD);
            this.Controls.Add(this.ifMultipleLabel);
            this.Controls.Add(this.filterList);
            this.Controls.Add(this.optionsListBox);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.getUserButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.whichNumberUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getUserButton;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.CheckedListBox optionsListBox;
        private System.Windows.Forms.ListBox filterList;
        private System.Windows.Forms.Label ifMultipleLabel;
        private System.Windows.Forms.NumericUpDown whichNumberUD;
    }
}

