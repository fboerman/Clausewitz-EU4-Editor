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
    public partial class SelectCountry : Form
    {
        public List<string> selectedcountries
        {
            get
            {
                return lb_countries.SelectedItems.OfType<string>().ToList(); ;
            }
        }

        public SelectCountry(List<string> Countries)
        {
            InitializeComponent();

            foreach (string country in Countries)
            {
                lb_countries.Items.Add(country);
            }
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
