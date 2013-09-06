using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace v1._0
{
    public partial class NationalIdeaEditor : Form
    {
        string eulocation;

        void LoadIdea() //loads the actual ideainfo from file
        {
            txt_edit.Text = "";

            string[] file = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string rawline = file[i];
                string line = rawline.Split('#')[0];

                if(line == "")
                {
                    continue;
                }

                if(line.Contains(txt_group.Text))
                {
                    for (int j = i + 1; (!file[j].Contains('}') || file[j].Contains('\t')); j++)
                    {
                        rawline = file[j];
                        line = rawline.Split('#')[0];

                        if (line == "")
                        {
                            continue;
                        }

                        if (line.Contains(cmb_ideas.Text))
                        {
                            for (int q = j + 1; !file[q].Contains('}'); q++)
                            {
                                rawline = file[q];
                                line = rawline.Split('#')[0];

                                if (line == "")
                                {
                                    continue;
                                }

                                txt_edit.Text = txt_edit.Text + "\n" + line.Trim('\t');
                                
                            }
                            return;
                        }
                    }
                    MessageBox.Show("Idea not found in group", "Error");
                    return;
                }
            }
            MessageBox.Show("Ideagroup not found", "Error");
            return;

        }

        void SaveIdea()
        { 
            
        } //saves the ideainfo to file

        public NationalIdeaEditor(List<string> ideas, string selectedgroup, string selectedidea, string Eulocation)
        {
            InitializeComponent();

            eulocation = Eulocation;

            foreach (string idea in ideas)
            {
                cmb_ideas.Items.Add(idea);
            }

            txt_group.Text = selectedgroup;
            cmb_ideas.Text = selectedidea;

            LoadIdea();
        }


        private void bt_save_Click(object sender, EventArgs e)
        {
            SaveIdea();
        }

        private void bt_load_Click(object sender, EventArgs e)
        {
            LoadIdea();
        }
    }
}
