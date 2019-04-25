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
            this.getItemButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.optionsListBox = new System.Windows.Forms.CheckedListBox();
            this.filterList = new System.Windows.Forms.ListBox();
            this.ifMultipleLabel = new System.Windows.Forms.Label();
            this.whichNumberUD = new System.Windows.Forms.NumericUpDown();
            this.multipleCheckBox = new System.Windows.Forms.CheckBox();
            this.userRButton = new System.Windows.Forms.RadioButton();
            this.computerRButton = new System.Windows.Forms.RadioButton();
            this.ownWindowButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.whichNumberUD)).BeginInit();
            this.SuspendLayout();
            // 
            // getItemButton
            // 
            this.getItemButton.Location = new System.Drawing.Point(35, 113);
            this.getItemButton.Name = "getItemButton";
            this.getItemButton.Size = new System.Drawing.Size(170, 41);
            this.getItemButton.TabIndex = 0;
            this.getItemButton.Text = "Obtenir les informations de l\'utilisateur";
            this.getItemButton.UseVisualStyleBackColor = true;
            this.getItemButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(35, 83);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(347, 20);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScriptTextBox_KeyDown);
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
            this.resultTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.resultTextBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            "Nom Complete",
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
            "Nom Complete",
            "SID",
            "Actif"});
            this.filterList.Location = new System.Drawing.Point(447, 155);
            this.filterList.Name = "filterList";
            this.filterList.Size = new System.Drawing.Size(325, 108);
            this.filterList.TabIndex = 5;
            // 
            // ifMultipleLabel
            // 
            this.ifMultipleLabel.Enabled = false;
            this.ifMultipleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifMultipleLabel.Location = new System.Drawing.Point(447, 279);
            this.ifMultipleLabel.Name = "ifMultipleLabel";
            this.ifMultipleLabel.Size = new System.Drawing.Size(325, 18);
            this.ifMultipleLabel.TabIndex = 6;
            this.ifMultipleLabel.Text = "S\'il y a plusieurs utilisateurs, choisir le quel montrer";
            // 
            // whichNumberUD
            // 
            this.whichNumberUD.Enabled = false;
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
            // multipleCheckBox
            // 
            this.multipleCheckBox.Enabled = false;
            this.multipleCheckBox.Location = new System.Drawing.Point(450, 327);
            this.multipleCheckBox.Name = "multipleCheckBox";
            this.multipleCheckBox.Size = new System.Drawing.Size(322, 20);
            this.multipleCheckBox.TabIndex = 8;
            this.multipleCheckBox.Text = "Voir le tout";
            this.multipleCheckBox.UseVisualStyleBackColor = true;
            this.multipleCheckBox.CheckedChanged += new System.EventHandler(this.MultipleCheckBox_CheckedChanged);
            // 
            // userRButton
            // 
            this.userRButton.Checked = true;
            this.userRButton.Location = new System.Drawing.Point(450, 363);
            this.userRButton.Name = "userRButton";
            this.userRButton.Size = new System.Drawing.Size(322, 20);
            this.userRButton.TabIndex = 9;
            this.userRButton.TabStop = true;
            this.userRButton.Text = "Utilisateurs";
            this.userRButton.UseVisualStyleBackColor = true;
            this.userRButton.CheckedChanged += new System.EventHandler(this.UserRButton_CheckedChanged);
            // 
            // computerRButton
            // 
            this.computerRButton.Location = new System.Drawing.Point(450, 389);
            this.computerRButton.Name = "computerRButton";
            this.computerRButton.Size = new System.Drawing.Size(322, 20);
            this.computerRButton.TabIndex = 10;
            this.computerRButton.Text = "Ordinateurs";
            this.computerRButton.UseVisualStyleBackColor = true;
            this.computerRButton.CheckedChanged += new System.EventHandler(this.ComputerRButton_CheckedChanged);
            // 
            // ownWindowButton
            // 
            this.ownWindowButton.Location = new System.Drawing.Point(211, 113);
            this.ownWindowButton.Name = "ownWindowButton";
            this.ownWindowButton.Size = new System.Drawing.Size(170, 41);
            this.ownWindowButton.TabIndex = 11;
            this.ownWindowButton.Text = "Montrer les informations sur une autre fenêtre";
            this.ownWindowButton.UseVisualStyleBackColor = true;
            this.ownWindowButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ownWindowButton);
            this.Controls.Add(this.computerRButton);
            this.Controls.Add(this.userRButton);
            this.Controls.Add(this.multipleCheckBox);
            this.Controls.Add(this.whichNumberUD);
            this.Controls.Add(this.ifMultipleLabel);
            this.Controls.Add(this.filterList);
            this.Controls.Add(this.optionsListBox);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.getItemButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.whichNumberUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getItemButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.CheckedListBox optionsListBox;
        private System.Windows.Forms.ListBox filterList;
        private System.Windows.Forms.Label ifMultipleLabel;
        private System.Windows.Forms.NumericUpDown whichNumberUD;
        private System.Windows.Forms.CheckBox multipleCheckBox;
        private System.Windows.Forms.RadioButton userRButton;
        private System.Windows.Forms.RadioButton computerRButton;
        private System.Windows.Forms.Button ownWindowButton;
    }
}

