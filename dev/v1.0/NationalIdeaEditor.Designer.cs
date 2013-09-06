namespace v1._0
{
    partial class NationalIdeaEditor
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
            this.txt_edit = new System.Windows.Forms.TextBox();
            this.bt_load = new System.Windows.Forms.Button();
            this.cmb_ideas = new System.Windows.Forms.ComboBox();
            this.bt_save = new System.Windows.Forms.Button();
            this.txt_group = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_edit
            // 
            this.txt_edit.Location = new System.Drawing.Point(149, 38);
            this.txt_edit.Multiline = true;
            this.txt_edit.Name = "txt_edit";
            this.txt_edit.Size = new System.Drawing.Size(191, 196);
            this.txt_edit.TabIndex = 0;
            // 
            // bt_load
            // 
            this.bt_load.Location = new System.Drawing.Point(34, 92);
            this.bt_load.Name = "bt_load";
            this.bt_load.Size = new System.Drawing.Size(75, 23);
            this.bt_load.TabIndex = 1;
            this.bt_load.Text = "Load";
            this.bt_load.UseVisualStyleBackColor = true;
            this.bt_load.Click += new System.EventHandler(this.bt_load_Click);
            // 
            // cmb_ideas
            // 
            this.cmb_ideas.FormattingEnabled = true;
            this.cmb_ideas.Location = new System.Drawing.Point(12, 65);
            this.cmb_ideas.Name = "cmb_ideas";
            this.cmb_ideas.Size = new System.Drawing.Size(121, 21);
            this.cmb_ideas.TabIndex = 3;
            // 
            // bt_save
            // 
            this.bt_save.Location = new System.Drawing.Point(346, 38);
            this.bt_save.Name = "bt_save";
            this.bt_save.Size = new System.Drawing.Size(75, 23);
            this.bt_save.TabIndex = 4;
            this.bt_save.Text = "Save";
            this.bt_save.UseVisualStyleBackColor = true;
            this.bt_save.Click += new System.EventHandler(this.bt_save_Click);
            // 
            // txt_group
            // 
            this.txt_group.Enabled = false;
            this.txt_group.Location = new System.Drawing.Point(12, 39);
            this.txt_group.Name = "txt_group";
            this.txt_group.Size = new System.Drawing.Size(121, 20);
            this.txt_group.TabIndex = 8;
            // 
            // NationalIdeaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 273);
            this.Controls.Add(this.txt_group);
            this.Controls.Add(this.bt_save);
            this.Controls.Add(this.cmb_ideas);
            this.Controls.Add(this.bt_load);
            this.Controls.Add(this.txt_edit);
            this.Name = "NationalIdeaEditor";
            this.Text = "NationalIdeaEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_edit;
        private System.Windows.Forms.Button bt_load;
        private System.Windows.Forms.ComboBox cmb_ideas;
        private System.Windows.Forms.Button bt_save;
        private System.Windows.Forms.TextBox txt_group;
    }
}