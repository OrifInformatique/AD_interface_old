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
            this.username = new System.Windows.Forms.TextBox();
            this.label_username = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.line = new System.Windows.Forms.Label();
            this.label_groups = new System.Windows.Forms.Label();
            this.groups_list = new System.Windows.Forms.TextBox();
            this.application_statue = new System.Windows.Forms.TextBox();
            this.label_check_access = new System.Windows.Forms.Label();
            this.lb_applications = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(77, 6);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(150, 20);
            this.username.TabIndex = 0;
            this.username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchEnter);
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(12, 9);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(59, 13);
            this.label_username.TabIndex = 1;
            this.label_username.Text = "Identifiant :";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(233, 4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 2;
            this.btn_search.Text = "Rechercher";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.Search);
            // 
            // output
            // 
            this.output.AcceptsTab = true;
            this.output.Location = new System.Drawing.Point(15, 55);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.output.Size = new System.Drawing.Size(293, 162);
            this.output.TabIndex = 3;
            // 
            // line
            // 
            this.line.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.line.Location = new System.Drawing.Point(15, 40);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(293, 2);
            this.line.TabIndex = 4;
            // 
            // label_groups
            // 
            this.label_groups.AutoSize = true;
            this.label_groups.Location = new System.Drawing.Point(314, 35);
            this.label_groups.Name = "label_groups";
            this.label_groups.Size = new System.Drawing.Size(47, 13);
            this.label_groups.TabIndex = 5;
            this.label_groups.Text = "Groupes";
            // 
            // groups_list
            // 
            this.groups_list.Location = new System.Drawing.Point(314, 55);
            this.groups_list.Multiline = true;
            this.groups_list.Name = "groups_list";
            this.groups_list.ReadOnly = true;
            this.groups_list.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.groups_list.Size = new System.Drawing.Size(212, 162);
            this.groups_list.TabIndex = 6;
            // 
            // application_statue
            // 
            this.application_statue.AcceptsTab = true;
            this.application_statue.Location = new System.Drawing.Point(532, 130);
            this.application_statue.Multiline = true;
            this.application_statue.Name = "application_statue";
            this.application_statue.ReadOnly = true;
            this.application_statue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.application_statue.Size = new System.Drawing.Size(212, 87);
            this.application_statue.TabIndex = 8;
            // 
            // label_check_access
            // 
            this.label_check_access.AutoSize = true;
            this.label_check_access.Location = new System.Drawing.Point(529, 35);
            this.label_check_access.Name = "label_check_access";
            this.label_check_access.Size = new System.Drawing.Size(130, 13);
            this.label_check_access.TabIndex = 9;
            this.label_check_access.Text = "Vérifier accès applications";
            // 
            // lb_applications
            // 
            this.lb_applications.FormattingEnabled = true;
            this.lb_applications.Location = new System.Drawing.Point(532, 55);
            this.lb_applications.Name = "lb_applications";
            this.lb_applications.Size = new System.Drawing.Size(212, 69);
            this.lb_applications.TabIndex = 10;
            this.lb_applications.SelectedIndexChanged += new System.EventHandler(this.lb_applications_SelectedIndexChanged);
            // 
            // RetreiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_check_access);
            this.Controls.Add(this.application_statue);
            this.Controls.Add(this.groups_list);
            this.Controls.Add(this.label_groups);
            this.Controls.Add(this.line);
            this.Controls.Add(this.output);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.label_username);
            this.Controls.Add(this.username);
            this.Controls.Add(this.lb_applications);
            this.Name = "RetreiveForm";
            this.Text = "Voir les informations d\'un utilisateur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.Label line;
        private System.Windows.Forms.Label label_groups;
        private System.Windows.Forms.TextBox groups_list;
        private System.Windows.Forms.TextBox application_statue;
        private System.Windows.Forms.Label label_check_access;
        private System.Windows.Forms.ListBox lb_applications;
    }
}

