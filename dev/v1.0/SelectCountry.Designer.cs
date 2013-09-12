namespace v1._0
{
    partial class SelectCountry
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
            this.lbl_question = new System.Windows.Forms.Label();
            this.lb_countries = new System.Windows.Forms.ListBox();
            this.bt_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_question
            // 
            this.lbl_question.AutoSize = true;
            this.lbl_question.Location = new System.Drawing.Point(12, 9);
            this.lbl_question.Name = "lbl_question";
            this.lbl_question.Size = new System.Drawing.Size(218, 13);
            this.lbl_question.TabIndex = 0;
            this.lbl_question.Text = "Please select the countries you want to save";
            // 
            // lb_countries
            // 
            this.lb_countries.FormattingEnabled = true;
            this.lb_countries.Location = new System.Drawing.Point(15, 25);
            this.lb_countries.Name = "lb_countries";
            this.lb_countries.Size = new System.Drawing.Size(120, 277);
            this.lb_countries.TabIndex = 1;
            // 
            // bt_ok
            // 
            this.bt_ok.Location = new System.Drawing.Point(60, 308);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(75, 23);
            this.bt_ok.TabIndex = 2;
            this.bt_ok.Text = "Ok";
            this.bt_ok.UseVisualStyleBackColor = true;
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            // 
            // SelectCountry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 399);
            this.Controls.Add(this.bt_ok);
            this.Controls.Add(this.lb_countries);
            this.Controls.Add(this.lbl_question);
            this.Name = "SelectCountry";
            this.Text = "SelectCountry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_question;
        private System.Windows.Forms.ListBox lb_countries;
        private System.Windows.Forms.Button bt_ok;
    }
}