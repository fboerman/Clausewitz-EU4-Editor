using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace v1._0
{
    public partial class InputBox : Form
    {

        public string answer
        {
            get
            {
                return txt_answer.Text;
            }
        }

        public InputBox(string question)
        {
            InitializeComponent();
            lbl_question.Text = question;
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
