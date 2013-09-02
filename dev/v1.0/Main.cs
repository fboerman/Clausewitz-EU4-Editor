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
    public partial class main : Form
    {
        string eulocation; //the location of the game
        List<List<string>> Religions = new List<List<string>>();
        List<List<string>> Cultures = new List<List<string>>();
        List<string> Ideas = new List<string>();

        void FlipControls() //enables or disables the controls
        {
            bt_loadc.Enabled = !bt_loadc.Enabled;
            cmb_countries.Enabled = !cmb_countries.Enabled;
            bt_save.Enabled = !bt_save.Enabled;
            txt_capital.Enabled = !txt_capital.Enabled;
            txt_colour_b.Enabled = !txt_colour_b.Enabled;
            txt_colour_g.Enabled = !txt_colour_g.Enabled;
            txt_colour_r.Enabled = !txt_colour_r.Enabled;
            txt_gfx.Enabled = !txt_gfx.Enabled;
            cmb_culturegroup.Enabled = !cmb_culturegroup.Enabled;
            cmb_culture.Enabled = !cmb_culture.Enabled;
            cmb_religiongroup.Enabled = !cmb_religiongroup.Enabled;
            cmb_religion.Enabled = !cmb_religion.Enabled;
            cmb_technology.Enabled = !cmb_technology.Enabled;
            cmb_government.Enabled = !cmb_government.Enabled;
            txt_tag.Enabled = !txt_tag.Enabled;
            txt_mercantilism.Enabled = !txt_mercantilism.Enabled;
            cmb_nationalideasgroup.Enabled = !cmb_nationalideasgroup.Enabled;
            cmb_nationalideas.Enabled = !cmb_nationalideas.Enabled;
            cmb_idea1.Enabled = !cmb_idea1.Enabled;
            cmb_idea2.Enabled = !cmb_idea2.Enabled;
            cmb_idea3.Enabled = !cmb_idea3.Enabled;
            cmb_idea4.Enabled = !cmb_idea4.Enabled;
            cmb_idea5.Enabled = !cmb_idea5.Enabled;
            cmb_idea6.Enabled = !cmb_idea6.Enabled;
            cmb_idea7.Enabled = !cmb_idea7.Enabled;
            cmb_idea8.Enabled = !cmb_idea8.Enabled;
        }

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

        void LoadCountryOptions() //loads all the possible religions, cultures, governments and technology groups
        {
            //load the government types
            string[] file = File.ReadAllLines(eulocation + "\\common\\governments\\00_governments.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line
                {
                    if (line.Contains('{'))
                    {
                        string[] parts = line.Split(' ');
                        cmb_government.Items.Add(parts[0]);
                        i = ReadSyntax(i + 1, file);
                        if (i == 0)
                        {
                            MessageBox.Show("Invalid syntax in government file", "error");
                            return;
                        }
                    }
                }
            }

            //load the religions

            //first read the religion groups
            file = File.ReadAllLines(eulocation + "\\common\\religions\\00_religion.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line and not a religiongroup
                {
                    if (line.Contains('{'))
                    {
                        string[] parts = line.Split(' ');
                        cmb_religiongroup.Items.Add(parts[0]);
                        List<string> group = new List<string>();
                        group.Add("dummy"); //create a dummy entry to make sure the list is properly initialized.
                        Religions.Add(group);
                        i = ReadSyntax(i + 1, file);
                        if (i == 0)
                        {
                            MessageBox.Show("Invalid syntax in religion file", "error");
                            return;
                        }
                    }
                }
            }

            //now that we know the religiongroups, we can read the religions themselves.
            int j = 0;
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line and not a religiongroup
                {
                    
                    foreach (string group in cmb_religiongroup.Items) //go through all the religiongroups
                    {
                        if (line.Contains(group))//check if the current line is the start of a new religiongroup, then read the codeblock of this group
                        {
                            for (int z = i; z < file.Length; z++)
                            {
                                line = file[z];
                                if (line.Contains('{') && line.Contains('\t'))
                                {
                                    string[] parts = line.Split(' ');
                                    string religion = parts[0].TrimStart('\t');
                                    Religions[j].Add(religion);
                                    z = ReadSyntax(z + 1, file);
                                    if (z == 0)
                                    {
                                        MessageBox.Show("Invalid syntax in religion file", "error");
                                        return;
                                    }
                                }
                                else
                                { 
                                    if(line.Contains('}') && !line.Contains('\t'))
                                    {
                                        i = z;
                                        break;
                                    }
                                }
                            }
                            j++;
                            break;
                        }
                    }

                }
            }

            //first load the culture groups
            file = File.ReadAllLines(eulocation + "\\common\\cultures\\00_cultures.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line and not a religiongroup
                {
                    if (line.Contains('{'))
                    {
                        string[] parts = line.Split(' ');
                        string culturegroup = parts[0].TrimEnd('=').TrimEnd('\t');
                        cmb_culturegroup.Items.Add(culturegroup);
                        List<string> group = new List<string>();
                        group.Add("dummy"); //create a dummy entry to make sure the list is properly initialized.
                        Cultures.Add(group);
                        i = ReadSyntax(i + 1, file);
                        if (i == 0)
                        {
                            MessageBox.Show("Invalid syntax in culture file", "error");
                            return;
                        }
                    }
                }
            }

            //load the  cultures into their groups
            j = 0;
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line and not a religiongroup
                {

                    foreach (string group in cmb_culturegroup.Items) //go through all the religiongroups
                    {
                        if (line.Contains(group))//check if the current line is the start of a new religiongroup, then read the codeblock of this group
                        {
                            for (int z = i + 1; z < file.Length; z++)
                            {
                                line = file[z];
                                if (line.Contains('{') && line.Contains('\t') && !line.Contains("dynasty_names"))
                                {
                                    string[] parts = line.Split(' ');
                                    string culture = parts[0].TrimStart('\t').TrimEnd('\t');
                                    Cultures[j].Add(culture);
                                    z = ReadSyntax(z + 1, file);
                                    if (z == 0)
                                    {
                                        MessageBox.Show("Invalid syntax in cultures file", "error");
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
                            j++;
                            break;
                        }
                    }

                }
            }

            //load the technology groups
            file = File.ReadAllLines(eulocation + "\\common\\technology.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                if (!line.Contains('#')) //not a comment line
                {
                    if (line.Contains('{') && line.Contains('\t'))
                    {
                        string[] parts = line.Split(' ');
                        cmb_technology.Items.Add(parts[0].TrimStart('\t'));
                        i = ReadSyntax(i + 1, file);
                        if (i == 0)
                        {
                            MessageBox.Show("Invalid syntax in technology file", "Error");
                            return;
                        }
                    }
                }
            }

            //load the basic ideagroups
            file = File.ReadAllLines(eulocation + "\\common\\ideas\\00_basic_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                string[] parts = line.Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                line = parts[0];
                if(line.Contains('{') && !line.Contains('\t'))
                {
                    parts = line.Split(' ');
                    string idea = parts[0].TrimStart('\t');
                    Ideas.Add(idea);
                    cmb_idea1.Items.Add(idea);
                    cmb_idea2.Items.Add(idea);
                    cmb_idea3.Items.Add(idea);
                    cmb_idea4.Items.Add(idea);
                    cmb_idea5.Items.Add(idea);
                    cmb_idea6.Items.Add(idea);
                    cmb_idea7.Items.Add(idea);
                    cmb_idea8.Items.Add(idea);

                    i = ReadSyntax(i + 1, file);
                    if (i == 0)
                    {
                        MessageBox.Show("Invalid syntax in basic ideas file", "Error");
                        return;
                    } 
                }
            }

            //load the national idea groups
            //first the individual national ideas
            file = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                string[] parts = line.Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                line = parts[0];
                if (line.Contains('{') && !line.Contains('\t'))
                {
                    parts = line.Split(' ');
                    string idea = parts[0].TrimStart('\t');
                    cmb_nationalideasgroup.Items.Add(idea);
                    i = ReadSyntax(i + 1, file);
                    if (i == 0)
                    {
                        MessageBox.Show("Invalid syntax in national ideas file", "Error");
                        return;
                    }
                }
            }

            //second the idea groups that are generic for for example an entire continent or parts of a culturegroup
            file = File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string line = file[i];
                string[] parts = line.Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                line = parts[0];
                if (line.Contains('{') && !line.Contains('\t'))
                {
                    parts = line.Split(' ');
                    string idea = parts[0].TrimStart('\t');
                    cmb_nationalideasgroup.Items.Add(idea);
                    i = ReadSyntax(i + 1, file);
                    if (i == 0)
                    {
                        MessageBox.Show("Invalid syntax in national ideas file", "Error");
                        return;
                    }
                }
            }
            //and lastly an entry for the default idea group
            cmb_nationalideasgroup.Items.Add("default_idea");

        }

        void LoadGame() //loads the options for countries into the controls
        {
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK) //let the user specify the location of eu4
            {
                //counry names:
                eulocation = fbd.SelectedPath;
                string[] dir = Directory.GetFiles(fbd.SelectedPath + "\\common\\countries\\"); //get a list of all the countrie.txt files
                foreach (string file in dir) //iterate over this list and extract the country name
                {
                    string country = Path.GetFileNameWithoutExtension(file);
                    cmb_countries.Items.Add(country);
                }
                //other options
                LoadCountryOptions();
                bt_load.Enabled = false;
                bt_loadc.Enabled = true;
                cmb_countries.Enabled = true;
            }

        }

        void LoadCountry()//loads the details of the selected country into the controls
        {
            if (cmb_countries.Text == "")//when there is no country selected
            {
                MessageBox.Show("No country selected.", "Error");
                return;
            }
            //load the tag
            string[] file = File.ReadAllLines(eulocation + "\\common\\country_tags\\00_countries.txt");
            foreach (string rawline in file)
            {
                string line = rawline.Split('#')[0];
                if (line == "")
                {
                    continue;
                }
                if (line.Contains(cmb_countries.Text))
                {
                    if (line.Contains('\t'))
                    {
                        string[] parts = line.Split('\t');
                        txt_tag.Text = parts[0];
                    }
                    else
                    {
                        string[] parts = line.Split(' ');
                        txt_tag.Text = parts[0];
                    }

                    break;
                }
            }

            //read the info from the history/countries/<tag> - <name>.txt file
            //because paradox in all their wisdom choose to not use spaces in the common/countries folder for names but do use it in the history folder, we need to load all the filenames from the history file and find the right one using the tag
            string[] dir = Directory.GetFiles(eulocation + "\\history\\countries\\");
            string filename = null;
            foreach (string countryfile in dir)
            {
                if (Path.GetFileNameWithoutExtension(countryfile).Contains(txt_tag.Text))
                {
                    filename = Path.GetFileName(countryfile);
                    break;
                }
            }
            if (filename == null)
            {
                MessageBox.Show("Selected country has no entry in the history/countries folder", "FileError");
            }
            else
            {
                file = File.ReadAllLines(eulocation + "\\history\\countries\\" + filename);
                foreach (string line in file)
                {
                    if (line == "") //temprary while the monarch and generals history is not loaded to controls yet
                    {
                        break;
                    }
                    //check if the line is commented as a whole
                    string[] parts = line.Split('#');
                    if (parts[0] == "")
                    {
                        continue;
                    }
                    parts = parts[0].Split('=');
                    string property = parts[0].Trim(' ', '\t');
                    string value = parts[1].Trim(' ', '\t');
                    switch (property)
                    {
                        case "government":
                            cmb_government.Text = value;
                            break;
                        case "mercantilism":
                            txt_mercantilism.Text = value;
                            break;
                        case "technology_group":
                            cmb_technology.Text = value;
                            break;
                        case "religion":

                            for (int i = 0; i < Religions.Count; i++)
                            {
                                if (Religions[i].Contains(value))
                                {
                                    cmb_religiongroup.SelectedIndex = i;
                                    break;
                                }
                            }
                            cmb_religion.Text = value;
                            if (cmb_religiongroup.Text == "")
                            {
                                MessageBox.Show("Invalid religion", "Syntax Error");
                            }
                            break;
                        case "primary_culture":

                            for (int i = 0; i < Cultures.Count; i++)
                            {
                                if (Cultures[i].Contains(value))
                                {
                                    cmb_culturegroup.SelectedIndex = i;
                                    break;
                                }
                            }
                            cmb_culture.Text = value;
                            if (cmb_culturegroup.Text == "")
                            {
                                MessageBox.Show("Invalid culture", "Syntax Error");
                            }
                            break;
                        case "capital":

                            txt_capital.Text = value;
                            break;
                    }
                }
            }
            //read the info from the common/countries/<name>.txt file
            file = File.ReadAllLines(eulocation + "\\common\\countries\\" + cmb_countries.Text + ".txt");
            for (int i = 0; i < file.Length; i++ )
            {
                string rawline = file[i];
                string line = rawline.Split('#')[0];
                if (line == "")
                {
                    continue;
                }
                if (line.Contains("graphical_culture")) //just extract the value after the = char
                {
                    txt_gfx.Text = line.Split('=')[1].Trim();
                }
                if (line.Contains("color")) //if its the color line then split it at spaces and determine wich part is numeric
                {
                    List<string> colours = new List<string>();
                    string[] parts = line.Split(' ');
                    foreach (string part in parts)
                    {
                        int num;
                        if (int.TryParse(part, out num))//numeric check
                        {
                            colours.Add(part);
                        }
                    }
                    txt_colour_r.Text = colours[0];
                    txt_colour_g.Text = colours[1];
                    txt_colour_b.Text = colours[2];

                }
                if (line.Contains("historical_idea_groups"))
                {
                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    string idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea1.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 1 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea2.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 2 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea3.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 3 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea4.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 4 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea5.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 5 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea6.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 6 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea7.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 7 in history file", "Error");
                    }

                    i++;
                    rawline = file[i];
                    line = rawline.Split('#')[0];
                    if (line == "")
                    {
                        continue;
                    }
                    idea = line.Trim('\t');
                    if (Ideas.Contains(idea))
                    {
                        cmb_idea8.Text = line.Trim('\t');
                    }
                    else
                    {
                        MessageBox.Show("Invalid idea 8 in history file", "Error");
                    }
                }
            }

            cmb_nationalideasgroup.Text = "";
            cmb_nationalideas.Items.Clear();
            //read the national ideas this only loads the names of the ideas, the ideas themselvers will be loaded and editted in the edit screen
            LoadNationalIdeas(txt_tag.Text, File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt")); //try and load the national ideas from the tag
            cmb_nationalideasgroup.Text = txt_tag.Text + "_ideas";
            if (cmb_nationalideas.Items.Count == 0)//check if ideas have been loaded, if not then search if the tag is in the trigger of an ideagroup
            {
                string ideatag = SearchNationalIdeas();
                if (ideatag != "") //check if search resulted in anything
                {
                    LoadNationalIdeas(ideatag, File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt"));
                    cmb_nationalideasgroup.Text = ideatag;
                }
                else //if not then look into the file that is loaded by the engine after the 00_country_ideas.txt file: zz_group_ideas.txt
                {
                    ideatag = SearchNationalGroup();
                    LoadNationalIdeas(ideatag, File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt"));
                    cmb_nationalideasgroup.Text = ideatag;
                    if (ideatag == "") //if no ideagroup has been found load the default ideas
                    {
                        cmb_nationalideas.Items.Clear();
                        LoadNationalIdeas("default_ideas", File.ReadAllLines(eulocation + "\\common\\ideas\\zzz_default_idea.txt"));
                        cmb_nationalideasgroup.Text = "default_ideas";
                    }
                }
            }


            FlipControls();
        }

        string SearchNationalGroup() //searches for the ideagroup from the zz_group_ideas.txt, returns the name of the group if found
        {
            string[] file = File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string[] parts = file[i].Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                string line = parts[0].TrimEnd('\t');
                if (line.Contains("ideas") && line.Contains('{'))
                {
                    string ideatag = line.Split(' ')[0];

                    for (int z = i; z < file.Length; z++)
                    {
                        line = file[z].TrimEnd('\t');
                        if (line.Contains("trigger") && line.Contains('\t'))
                        {
                            List<string> OR = new List<string>();
                            string government = "";
                            string culture_group = "";
                            string religion_group = "";
                            string tag = "";

                            for (int j = z + 1; j < file.Length; j++)
                            {
                                line = file[j];
                                if (line.Contains('='))
                                {
                                    string keyword = line.Trim('\t').Split('=')[0].Trim();
                                    string value = line.Trim('\t').Split('=')[1].Trim();

                                    switch (keyword)
                                    {
                                        case "OR":
                                            for (int q = j + 1; q < file.Length; q++)
                                            {
                                                line = file[q];
                                                if (line.Contains('='))
                                                {
                                                    keyword = line.Trim('\t').Split('=')[0].Trim();
                                                    value = line.Trim('\t').Split('=')[1].Trim();
                                                    switch (keyword)
                                                    {
                                                        case "government":
                                                            OR.Add("government:" + value);
                                                            break;
                                                        case "religion_group":
                                                            OR.Add("religion_group:" + value);
                                                            break;
                                                        case "culture_group":
                                                            OR.Add("culture_group:" + value);
                                                            break;
                                                        case "tag":
                                                            OR.Add("tag:" + value);
                                                            break;
                                                    }
                                                }
                                                if (line.Contains('}'))
                                                {
                                                    line = "";
                                                    j = q;
                                                    break;
                                                }
                                            }
                                            break;
                                        case "government":
                                            government = value;
                                            break;
                                        case "religion_group":
                                            religion_group = value;
                                            break;
                                        case "culture_group":
                                            culture_group = value;
                                            break;
                                        case "tag":
                                            tag = value;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (line.Contains('}'))
                                    {
                                        z = j;
                                        break;
                                    }
                                }
                            }
                            //after parsing all the info in trigger check if the conditions are true.
                            //first check with the OR list
                            foreach (string requirement in OR)
                            {
                                string keyword = requirement.Split(':')[0];
                                string value = requirement.Split(':')[1];
                                switch (keyword)
                                { 
                                    case "government":
                                        if (cmb_government.Text == value)
                                        {
                                            if ((religion_group == "" || religion_group == cmb_religiongroup.Text) && (culture_group == "" || culture_group == cmb_culturegroup.Text) && (tag == "" || tag == txt_tag.Text))
                                            {
                                                return ideatag;
                                            }
                                        }
                                        break;
                                    case "religion_group":
                                        if (cmb_religiongroup.Text == value)
                                        {
                                            if ((government == "" || government == cmb_government.Text || (government == "theocracy" && cmb_government.Text == "theocratic_government")) && (culture_group == "" || culture_group == cmb_culturegroup.Text) && (tag == "" || tag == txt_tag.Text))
                                            {
                                                return ideatag;
                                            }
                                        }
                                        break;
                                    case "culture_group":
                                        if (cmb_culturegroup.Text == value)
                                        {
                                            if ((government == "" || government == cmb_government.Text || (government == "theocracy" && cmb_government.Text == "theocratic_government")) && (religion_group == "" || religion_group == cmb_religiongroup.Text) && (tag == "" || tag == txt_tag.Text))
                                            {
                                                return ideatag;
                                            }
                                        }
                                        break;
                                    case "tag":
                                        if (txt_tag.Text == value)
                                        {
                                            if ((government == "" || government == cmb_government.Text || (government == "theocracy" && cmb_government.Text == "theocratic_government")) && (religion_group == "" || religion_group == cmb_religiongroup.Text) && (culture_group == "" || culture_group == cmb_culturegroup.Text))
                                            {
                                                return ideatag;
                                            }
                                        }
                                        break;
                                }
                            }
                            //if there is no OR block then check the individual requirements
                            if ((government == "" || government == cmb_government.Text || (government == "theocracy" && cmb_government.Text == "theocratic_government") ) && (religion_group == "" || religion_group == cmb_religiongroup.Text) && (culture_group == "" || culture_group == cmb_culturegroup.Text) && (tag == "" || txt_tag.Text == tag) && OR.Count == 0) 
                            {
                                return ideatag;
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
                    
                }
            }
            return "";
        }

        string SearchNationalIdeas() //searches the tag of an ideagroup of the selected country, returns that tag if found and empty string otherwise
        {
            string []file = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt");
            for (int i = 0; i < file.Length; i++)
            {
                string[] parts = file[i].Split('#');
                if (parts[0] == "")
                {
                    continue;
                }
                string line = parts[0];
                if (line.Contains("ideas") && line.Contains('{'))
                {
                    string ideatag = line.Split(' ')[0];

                    for (int z = i; z < file.Length; z++)
                    {
                        line = file[z];
                        if (line.Contains("tag") && line.Contains('\t'))
                        {
                            if (line.Trim('\t').Split('=')[1].Trim() == txt_tag.Text)
                            {
                                return ideatag;
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
            return "";
        }

        void LoadNationalIdeas(string tag, string[] file) //loads the national ideas of a country of the given tag/name of ideagroup in one of the ideas files
        {
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
                            cmb_nationalideas.Items.Add(idea);
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

        void SaveCountry() //saves the controls to a mod folder
        {
            FlipControls();
        }

        public main()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_load_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            SaveCountry();
        }

        private void saveCountryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCountry();
        }

        private void bt_loadc_Click(object sender, EventArgs e)
        {
            LoadCountry();
        }

        private void cmb_religiongroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_religion.Items.Clear();
            cmb_religion.Text = null;
            List<string> religions = Religions[cmb_religiongroup.SelectedIndex];
            foreach (string religion in religions)
            {
                if (religion != "dummy") //dont print the dummy
                {
                    cmb_religion.Items.Add(religion);
                }
                
            }
        }

        private void cmb_culturegroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_culture.Items.Clear();
            cmb_culture.Text = null;
            List<string> cultures = Cultures[cmb_culturegroup.SelectedIndex];
            foreach (string culture in cultures)
            {
                if (culture != "dummy") //dont print the dummy
                {
                    cmb_culture.Items.Add(culture);
                }

            }
        }
    }
}
