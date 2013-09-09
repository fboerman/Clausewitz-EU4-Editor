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
    public partial class NationalIdeaGroupEditor : Form
    {
        string eulocation;

        int ReadSyntax(int linenum, string[] file) //used to read from curly bracket to curly bracket, returns the line number of the closing bracket, if end of file then returns 0
        {
            for (; linenum < file.Length; linenum++)
            {
                string line = file[linenum];
                if (line.Contains('}') && !line.Contains('{'))
                {
                    return linenum;
                }
                else
                {
                    if (line.Contains('{') && !line.Contains('}'))
                    {
                        linenum = ReadSyntax(linenum + 1, file);
                    }
                }
            }

            return 0;
        }

        void LoadNationalIdeas(string tag, string[] file) //loads the national ideas of a country of the given tag/name of ideagroup in one of the ideas files
        {
            int ideanum = 1;

            for (int i = 0; i < file.Length; i++)
            {
                string[] parts = file[i].Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                string line = parts[0];
                if (line.Contains(tag) && line.Contains('{'))
                {
                    for (int z = i; z < file.Length; z++)
                    {
                        line = file[z];
                        if (line.Contains('{') && line.Contains('\t'))
                        {
                            parts = line.Split(' ');
                            string idea = parts[0].TrimStart('\t');
                            if (idea != "trigger" && idea != "start" && idea != "bonus")
                            {
                                switch (ideanum)
                                { 
                                    case 1:
                                        txt_idea1.Text = idea;
                                        break;
                                    case 2:
                                        txt_idea2.Text = idea;
                                        break;
                                    case 3:
                                        txt_idea3.Text = idea;
                                        break;
                                    case 4:
                                        txt_idea4.Text = idea;
                                        break;
                                    case 5:
                                        txt_idea5.Text = idea;
                                        break;
                                    case 6:
                                        txt_idea6.Text = idea;
                                        break;
                                    case 7:
                                        txt_idea7.Text = idea;
                                        break;
                                }
                                ideanum++;
                            }
                            z = ReadSyntax(z + 1, file);
                            if (z == 0)
                            {
                                MessageBox.Show("Invalid syntax in country_ideas file", "Error");
                                return;
                            }
                        }
                        else
                        {
                            if (line.Contains('}') && !line.Contains('\t'))
                            {
                                i = z;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }

        void LoadControls() //loads the controls
        {
            txt_idea1.Text = "";
            txt_idea2.Text = "";
            txt_idea3.Text = "";
            txt_idea4.Text = "";
            txt_idea5.Text = "";
            txt_idea6.Text = "";
            txt_idea7.Text = "";

            LoadNationalIdeas(cmb_groups.Text, File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt"));
            if (txt_idea1.Text == "")
            {
                LoadNationalIdeas(cmb_groups.Text, File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt"));
                if (txt_idea1.Text == "")
                {
                    LoadNationalIdeas("default_ideas", File.ReadAllLines(eulocation + "\\common\\ideas\\zzz_default_idea.txt"));
                }
            }
        }

        public NationalIdeaGroupEditor(List<string> groups, string selectedgroup, string location)
        {
            InitializeComponent();
            foreach (string group in groups)
            {
                cmb_groups.Items.Add(group);
            }
            cmb_groups.Text = selectedgroup;
            eulocation = location;
            LoadControls();
        }

        private void bt_load_Click(object sender, EventArgs e)
        {
            LoadControls();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {

        }
    }
}
