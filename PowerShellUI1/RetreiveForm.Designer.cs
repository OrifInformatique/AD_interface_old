namespace PowerShellUI1
{
    partial class RetreiveForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Username = new System.Windows.Forms.TextBox();
            this.Label_username = new System.Windows.Forms.Label();
            this.Btn_search = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.Line = new System.Windows.Forms.Label();
            this.Label_groups = new System.Windows.Forms.Label();
            this.Groups_list = new System.Windows.Forms.TextBox();
            this.Application_status = new System.Windows.Forms.TextBox();
            this.Label_check_access = new System.Windows.Forms.Label();
            this.Lb_applications = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(77, 6);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(150, 20);
            this.Username.TabIndex = 0;
            this.Username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchEnter);
            // 
            // Label_username
            // 
            this.Label_username.AutoSize = true;
            this.Label_username.Location = new System.Drawing.Point(12, 9);
            this.Label_username.Name = "Label_username";
            this.Label_username.Size = new System.Drawing.Size(59, 13);
            this.Label_username.TabIndex = 1;
            this.Label_username.Text = "Identifiant :";
            // 
            // Btn_search
            // 
            this.Btn_search.Location = new System.Drawing.Point(233, 4);
            this.Btn_search.Name = "Btn_search";
            this.Btn_search.Size = new System.Drawing.Size(75, 23);
            this.Btn_search.TabIndex = 2;
            this.Btn_search.Text = "Rechercher";
            this.Btn_search.UseVisualStyleBackColor = true;
            this.Btn_search.Click += new System.EventHandler(this.Search);
            // 
            // Output
            // 
            this.Output.AcceptsTab = true;
            this.Output.Location = new System.Drawing.Point(15, 55);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(293, 162);
            this.Output.TabIndex = 3;
            // 
            // Line
            // 
            this.Line.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Line.Location = new System.Drawing.Point(15, 40);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(293, 2);
            this.Line.TabIndex = 4;
            // 
            // Label_groups
            // 
            this.Label_groups.AutoSize = true;
            this.Label_groups.Location = new System.Drawing.Point(314, 35);
            this.Label_groups.Name = "Label_groups";
            this.Label_groups.Size = new System.Drawing.Size(47, 13);
            this.Label_groups.TabIndex = 5;
            this.Label_groups.Text = "Groupes";
            // 
            // Groups_list
            // 
            this.Groups_list.Location = new System.Drawing.Point(314, 55);
            this.Groups_list.Multiline = true;
            this.Groups_list.Name = "Groups_list";
            this.Groups_list.ReadOnly = true;
            this.Groups_list.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Groups_list.Size = new System.Drawing.Size(212, 162);
            this.Groups_list.TabIndex = 6;
            // 
            // Application_status
            // 
            this.Application_status.AcceptsTab = true;
            this.Application_status.Location = new System.Drawing.Point(532, 130);
            this.Application_status.Multiline = true;
            this.Application_status.Name = "Application_status";
            this.Application_status.ReadOnly = true;
            this.Application_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Application_status.Size = new System.Drawing.Size(212, 87);
            this.Application_status.TabIndex = 8;
            // 
            // Label_check_access
            // 
            this.Label_check_access.AutoSize = true;
            this.Label_check_access.Location = new System.Drawing.Point(529, 35);
            this.Label_check_access.Name = "Label_check_access";
            this.Label_check_access.Size = new System.Drawing.Size(130, 13);
            this.Label_check_access.TabIndex = 9;
            this.Label_check_access.Text = "Vérifier accès applications";
            // 
            // Lb_applications
            // 
            this.Lb_applications.FormattingEnabled = true;
            this.Lb_applications.Location = new System.Drawing.Point(532, 55);
            this.Lb_applications.Name = "Lb_applications";
            this.Lb_applications.Size = new System.Drawing.Size(212, 69);
            this.Lb_applications.TabIndex = 10;
            this.Lb_applications.SelectedIndexChanged += new System.EventHandler(this.ListAllGroups);
            // 
            // RetreiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Label_check_access);
            this.Controls.Add(this.Application_status);
            this.Controls.Add(this.Groups_list);
            this.Controls.Add(this.Label_groups);
            this.Controls.Add(this.Line);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Btn_search);
            this.Controls.Add(this.Label_username);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.Lb_applications);
            this.Name = "RetreiveForm";
            this.Text = "Voir les informations d\'un utilisateur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.Label Label_username;
        private System.Windows.Forms.Button Btn_search;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.Label Line;
        private System.Windows.Forms.Label Label_groups;
        private System.Windows.Forms.TextBox Groups_list;
        private System.Windows.Forms.TextBox Application_status;
        private System.Windows.Forms.Label Label_check_access;
        private System.Windows.Forms.ListBox Lb_applications;
    }
}

