namespace v1._0
{
    partial class InputBox
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
            this.txt_answer = new System.Windows.Forms.TextBox();
            this.bt_ok = new System.Windows.Forms.Button();
            this.lbl_question = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_answer
            // 
            this.txt_answer.Location = new System.Drawing.Point(12, 25);
            this.txt_answer.Name = "txt_answer";
            this.txt_answer.Size = new System.Drawing.Size(209, 20);
            this.txt_answer.TabIndex = 0;
            // 
            // bt_ok
            // 
            this.bt_ok.Location = new System.Drawing.Point(227, 23);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(75, 23);
            this.bt_ok.TabIndex = 1;
            this.bt_ok.Text = "Ok";
            this.bt_ok.UseVisualStyleBackColor = true;
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            // 
            // lbl_question
            // 
            this.lbl_question.AutoSize = true;
            this.lbl_question.Location = new System.Drawing.Point(9, 9);
            this.lbl_question.Name = "lbl_question";
            this.lbl_question.Size = new System.Drawing.Size(49, 13);
            this.lbl_question.TabIndex = 2;
            this.lbl_question.Text = "Question";
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 85);
            this.Controls.Add(this.lbl_question);
            this.Controls.Add(this.bt_ok);
            this.Controls.Add(this.txt_answer);
            this.Name = "InputBox";
            this.Text = "Question";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_answer;
        private System.Windows.Forms.Button bt_ok;
        private System.Windows.Forms.Label lbl_question;
    }
}