namespace PowerShellUI1
{
    partial class AdvancedRetreiveForm
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
            this.WarningLabel = new System.Windows.Forms.Label();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.DisplayBelow = new System.Windows.Forms.Button();
            this.DisplayWindow = new System.Windows.Forms.Button();
            this.ResultTextBox = new System.Windows.Forms.RichTextBox();
            this.WhichOne = new System.Windows.Forms.NumericUpDown();
            this.IfMultipleLabel = new System.Windows.Forms.Label();
            this.MultipleCheckBox = new System.Windows.Forms.CheckBox();
            this.CriteriaListBox = new System.Windows.Forms.ListBox();
            this.DisplayCheckBox = new System.Windows.Forms.CheckedListBox();
            this.SearchCriteria = new System.Windows.Forms.Label();
            this.DisplayWhat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WhichOne)).BeginInit();
            this.SuspendLayout();
            // 
            // WarningLabel
            // 
            this.WarningLabel.BackColor = System.Drawing.Color.Salmon;
            this.WarningLabel.ForeColor = System.Drawing.Color.Red;
            this.WarningLabel.Location = new System.Drawing.Point(12, 9);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(264, 74);
            this.WarningLabel.TabIndex = 0;
            this.WarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarningLabel.Visible = false;
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(12, 86);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(264, 20);
            this.SearchTextBox.TabIndex = 1;
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // DisplayBelow
            // 
            this.DisplayBelow.Location = new System.Drawing.Point(12, 113);
            this.DisplayBelow.Name = "DisplayBelow";
            this.DisplayBelow.Size = new System.Drawing.Size(125, 49);
            this.DisplayBelow.TabIndex = 2;
            this.DisplayBelow.Text = "Obtenir les informations de l\'utilisateur";
            this.DisplayBelow.UseVisualStyleBackColor = true;
            this.DisplayBelow.Click += new System.EventHandler(this.UpdateResultTextBox);
            // 
            // DisplayWindow
            // 
            this.DisplayWindow.Location = new System.Drawing.Point(151, 112);
            this.DisplayWindow.Name = "DisplayWindow";
            this.DisplayWindow.Size = new System.Drawing.Size(125, 50);
            this.DisplayWindow.TabIndex = 3;
            this.DisplayWindow.Text = "Montrer les informations dans une fenêtre";
            this.DisplayWindow.UseVisualStyleBackColor = true;
            this.DisplayWindow.Click += new System.EventHandler(this.DisplayResultWindow);
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ResultTextBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResultTextBox.Location = new System.Drawing.Point(12, 168);
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ReadOnly = true;
            this.ResultTextBox.Size = new System.Drawing.Size(264, 270);
            this.ResultTextBox.TabIndex = 4;
            this.ResultTextBox.Text = "";
            // 
            // WhichOne
            // 
            this.WhichOne.Location = new System.Drawing.Point(283, 417);
            this.WhichOne.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.WhichOne.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WhichOne.Name = "WhichOne";
            this.WhichOne.Size = new System.Drawing.Size(258, 20);
            this.WhichOne.TabIndex = 5;
            this.WhichOne.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WhichOne.Click += new System.EventHandler(this.OnChangeDisplay);
            // 
            // IfMultipleLabel
            // 
            this.IfMultipleLabel.Enabled = false;
            this.IfMultipleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IfMultipleLabel.Location = new System.Drawing.Point(282, 394);
            this.IfMultipleLabel.Name = "IfMultipleLabel";
            this.IfMultipleLabel.Size = new System.Drawing.Size(506, 20);
            this.IfMultipleLabel.TabIndex = 7;
            this.IfMultipleLabel.Text = "S\'il y a plusieurs utilisateurs, choisir le quel montrer";
            // 
            // MultipleCheckBox
            // 
            this.MultipleCheckBox.Enabled = false;
            this.MultipleCheckBox.Location = new System.Drawing.Point(547, 416);
            this.MultipleCheckBox.Name = "MultipleCheckBox";
            this.MultipleCheckBox.Size = new System.Drawing.Size(241, 20);
            this.MultipleCheckBox.TabIndex = 8;
            this.MultipleCheckBox.Text = "Voir le tout";
            this.MultipleCheckBox.UseVisualStyleBackColor = true;
            this.MultipleCheckBox.Click += new System.EventHandler(this.OnChangeDisplay);
            // 
            // CriteriaListBox
            // 
            this.CriteriaListBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CriteriaListBox.FormattingEnabled = true;
            this.CriteriaListBox.ItemHeight = 14;
            this.CriteriaListBox.Location = new System.Drawing.Point(285, 35);
            this.CriteriaListBox.Name = "CriteriaListBox";
            this.CriteriaListBox.Size = new System.Drawing.Size(256, 354);
            this.CriteriaListBox.TabIndex = 9;
            // 
            // DisplayCheckBox
            // 
            this.DisplayCheckBox.CheckOnClick = true;
            this.DisplayCheckBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisplayCheckBox.FormattingEnabled = true;
            this.DisplayCheckBox.Location = new System.Drawing.Point(547, 35);
            this.DisplayCheckBox.Name = "DisplayCheckBox";
            this.DisplayCheckBox.Size = new System.Drawing.Size(249, 349);
            this.DisplayCheckBox.TabIndex = 10;
            this.DisplayCheckBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnChangeDisplay);
            // 
            // SearchCriteria
            // 
            this.SearchCriteria.Location = new System.Drawing.Point(285, 9);
            this.SearchCriteria.Name = "SearchCriteria";
            this.SearchCriteria.Size = new System.Drawing.Size(256, 23);
            this.SearchCriteria.TabIndex = 11;
            this.SearchCriteria.Text = "Critère de recherche";
            // 
            // DisplayWhat
            // 
            this.DisplayWhat.Location = new System.Drawing.Point(548, 9);
            this.DisplayWhat.Name = "DisplayWhat";
            this.DisplayWhat.Size = new System.Drawing.Size(248, 23);
            this.DisplayWhat.TabIndex = 12;
            this.DisplayWhat.Text = "Valeurs à montrer";
            // 
            // AdvancedRetreiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DisplayWhat);
            this.Controls.Add(this.SearchCriteria);
            this.Controls.Add(this.DisplayCheckBox);
            this.Controls.Add(this.CriteriaListBox);
            this.Controls.Add(this.MultipleCheckBox);
            this.Controls.Add(this.IfMultipleLabel);
            this.Controls.Add(this.WhichOne);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.DisplayWindow);
            this.Controls.Add(this.DisplayBelow);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.WarningLabel);
            this.Name = "AdvancedRetreiveForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Voir les informations avancées d\'un utilisateur";
            ((System.ComponentModel.ISupportInitialize)(this.WhichOne)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button DisplayBelow;
        private System.Windows.Forms.Button DisplayWindow;
        private System.Windows.Forms.RichTextBox ResultTextBox;
        private System.Windows.Forms.NumericUpDown WhichOne;
        private System.Windows.Forms.Label IfMultipleLabel;
        private System.Windows.Forms.CheckBox MultipleCheckBox;
        private System.Windows.Forms.ListBox CriteriaListBox;
        private System.Windows.Forms.CheckedListBox DisplayCheckBox;
        private System.Windows.Forms.Label SearchCriteria;
        private System.Windows.Forms.Label DisplayWhat;
    }
}