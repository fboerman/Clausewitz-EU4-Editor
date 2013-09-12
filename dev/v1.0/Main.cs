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
using System.Globalization;

namespace v1._0
{
    
    public partial class main : Form
    {
        string eulocation; //the location of the game
        List<List<string>> Religions = new List<List<string>>();
        List<List<string>> Cultures = new List<List<string>>();
        List<string> Ideas = new List<string>();
        List<CountryData> savedcountries = new List<CountryData>();

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
                                    cmb_prefered_religion.Items.Add(religion);
                                    z = ReadSyntax(z + 1, file);
                                    if (z == 0)
                                    {
                                        MessageBox.Show("Invalid syntax in religion file", "error");
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
                if (line.Contains('{') && !line.Contains('\t'))
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

            //load the unittypes
            string[] folder = Directory.GetFiles(eulocation + "\\common\\units\\");
            foreach (string filename in folder)
            {
                string unit = Path.GetFileNameWithoutExtension(filename);
                cmb_units.Items.Add(unit);
            }

        }

        void LoadGame() //loads the options for countries into the controls
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select the folder where eu4 is installed";
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
            }

        }

        void LoadCountry()//loads the details of the selected country into the controls
        {
            cmb_religiongroup.Text = "";
            cmb_culturegroup.Text = "";
            CountryData country = null;
            try
            {
                country = savedcountries.Find(item => item.Name == cmb_countries.Text);
            }
            catch
            { 
                //country is not yet in the list so read from file
            }

            if (country != null) //if country has been found in savedcountries list then load the country from this list
            {
                txt_tag.Text = country.Tag;
                txt_gfx.Text = country.Gfx;
                cmb_nationalideasgroup.Text = country.Nationalideagroup;
                cmb_culture.Text = country.Culture;
                cmb_religion.Text = country.Religion;
                cmb_prefered_religion.Text = country.PreferedReligion;
                cmb_technology.Text = country.Techgroup;
                txt_capital.Text = country.Capital.ToString();
                txt_mercantilism.Text = country.Mercantilism.ToString();
                cmb_government.Text = country.Government;
                cmb_idea1.Text = country.Ideas[0];
                cmb_idea2.Text = country.Ideas[1];
                cmb_idea3.Text = country.Ideas[2];
                cmb_idea4.Text = country.Ideas[3];
                cmb_idea5.Text = country.Ideas[4];
                cmb_idea6.Text = country.Ideas[5];
                cmb_idea7.Text = country.Ideas[6];
                cmb_idea8.Text = country.Ideas[7];
                txt_colour_r.Text = country.Color[0].ToString();
                txt_colour_g.Text = country.Color[1].ToString();
                txt_colour_b.Text = country.Color[2].ToString();
                foreach (string unit in country.Units)
                {
                    lb_units.Items.Add(unit);
                }
            }
            else
            {
                if (cmb_countries.Text == "")//when there is no country selected
                {
                    MessageBox.Show("No country selected.", "Error");
                    return;
                }
                cmb_prefered_religion.Text = "";
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
                for (int i = 0; i < file.Length; i++)
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
                        continue;
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
                        continue;
                    }
                    if (line.Contains("historical_idea_groups"))
                    {
                        i++;
                        int ideanum = 0;
                        for (; !file[i].Contains('}'); i++)
                        {
                            rawline = file[i];
                            line = rawline.Split('#')[0];
                            if (line == "")
                            {
                                continue;
                            }
                            ideanum++;
                            string idea = line.Trim('\t');
                            if (Ideas.Contains(idea))
                            {
                                switch (ideanum)
                                {
                                    case 1:
                                        cmb_idea1.Text = idea;
                                        break;
                                    case 2:
                                        cmb_idea2.Text = idea;
                                        break;
                                    case 3:
                                        cmb_idea3.Text = idea;
                                        break;
                                    case 4:
                                        cmb_idea4.Text = idea;
                                        break;
                                    case 5:
                                        cmb_idea5.Text = idea;
                                        break;
                                    case 6:
                                        cmb_idea6.Text = idea;
                                        break;
                                    case 7:
                                        cmb_idea7.Text = idea;
                                        break;
                                    case 8:
                                        cmb_idea8.Text = idea;
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid idea " + ideanum + " in history file", "Error");
                            }
                        }
                        continue;
                    }
                    if (line.Contains("historical_units"))
                    {
                        i++;
                        for (; !file[i].Contains('}'); i++)
                        {
                            line = file[i].Split('#')[0].Trim('\t');
                            if (line == "")
                            {
                                continue;
                            }
                            lb_units.Items.Add(line);
                        }
                    }
                    if (line.Contains("preferred_religion"))
                    {
                        cmb_prefered_religion.Text = line.Split('=')[1].Trim();
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
                        cmb_nationalideasgroup.Text = ideatag;
                        if (ideatag == "") //if no ideagroup has been found load the default ideas
                        {
                            cmb_nationalideas.Items.Clear();
                            LoadNationalIdeas("default_ideas", File.ReadAllLines(eulocation + "\\common\\ideas\\zzz_default_idea.txt"));
                            cmb_nationalideasgroup.Text = "default_ideas";
                        }
                        else
                        {
                            LoadNationalIdeas(ideatag, File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt"));
                        }
                    }
                }

                //clear the selected idea
                cmb_nationalideas.Text = "";
            }
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
                            if ((government == "" || government == cmb_government.Text || (government == "theocracy" && cmb_government.Text == "theocratic_government")) && (religion_group == "" || religion_group == cmb_religiongroup.Text) && (culture_group == "" || culture_group == cmb_culturegroup.Text) && (tag == "" || txt_tag.Text == tag) && OR.Count == 0)
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
            string[] file = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt");
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
                            if (idea != "trigger")
                            {
                                cmb_nationalideas.Items.Add(idea);
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

        void SaveCountry(CountryData country, string savelocation, string modname) //saves a country to the modfolder. the ideafiles need to be copied over first
        {
            //save the history/countries file
            //because paradox in all their wisdom choose to not use spaces in the common/countries folder for names but do use it in the history folder, we need to load all the filenames from the history file and find the right one using the tag
            string[] dir = Directory.GetFiles(eulocation + "\\history\\countries\\");
            string filename = null;
            foreach (string countryfile in dir)
            {
                if (Path.GetFileNameWithoutExtension(countryfile).Contains(country.Tag))
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
                //load the original file and itirate through it to copy it to the new file filling in the selected country details
                string[] original = File.ReadAllLines(eulocation + "\\history\\countries\\" + filename, Encoding.Default);
                StreamWriter writer = new StreamWriter(new FileStream(savelocation + "\\" + modname.Trim() + "\\history\\countries\\" + filename, FileMode.Append, FileAccess.Write), Encoding.Default); //the actual writer
                int i = 0;
                for (; i < original.Length; i++) //itirate through the original and put in the same order the value and keywords in the new modfile
                {
                    string comment = "";
                    string line = "";

                    line = SplitComments(original[i])[0];
                    comment = SplitComments(original[i])[1];


                    if (line.Contains("government")) //check wich property this is
                    {
                        writer.WriteLine("government = " + country.Government + comment); //insert the type, the value and the optional comments into the new mod file
                        continue;
                    }

                    if (line.Contains("mercantilism"))
                    {
                        writer.WriteLine("mercantilism = " + country.Mercantilism + comment);
                        continue;
                    }

                    if (line.Contains("technology_group"))
                    {
                        writer.WriteLine("technology_group = " + country.Techgroup + comment);
                        continue;
                    }

                    if (line.Contains("religion"))
                    {
                        writer.WriteLine("religion = " + country.Religion + comment);
                        continue;
                    }

                    if (line.Contains("primary_culture"))
                    {
                        writer.WriteLine("primary_culture = " + country.Culture + comment);
                        continue;
                    }
                    if (line.Contains("capital"))
                    {
                        writer.WriteLine("capital = " + country.Capital + comment);
                        continue;
                    }

                    if (line == "") //if the first block is over and the history part begins, then break out of the first loop
                    {
                        break;
                    }
                }

                for (i++; i < original.Length; i++) //copy over the history part for government stuff and rulers
                {
                    writer.WriteLine(original[i]);
                }
                writer.Close();

                //now save the common/countries/<name>.txt file
                //first load the original
                original = File.ReadAllLines(eulocation + "\\common\\countries\\" + country.Name + ".txt", Encoding.Default);
                writer = new StreamWriter(new FileStream(savelocation + "\\" + modname.Trim() + "\\common\\countries\\" + country.Name + ".txt", FileMode.Append, FileAccess.Write), Encoding.Default);
                for (i = 0; i < original.Length; i++)
                {
                    string comment = SplitComments(original[i])[0];
                    string line = SplitComments(original[i])[1];

                    //if its a empty line then just copy that over
                    if (original[i].Trim(' ', '\t') == "")
                    {
                        writer.WriteLine("");
                        continue;
                    }

                    //writing the actual file
                    if (line.Contains("preferred_religion"))
                    {
                        if (country.PreferedReligion != "")
                        {
                            writer.WriteLine("preferred_religion = " + country.PreferedReligion);
                        }
                        continue;
                    }

                    if (line.Contains("graphical_culture"))
                    {
                        writer.WriteLine("graphical_culture = " + country.Gfx + comment);
                        continue;
                    }

                    if (line.Contains("color"))
                    {
                        writer.WriteLine("color = { " + country.Color[0] + "  " + country.Color[1] + "  " + country.Color[2] + " }" + comment);
                        continue;
                    }

                    if (line.Contains("historical_idea_groups"))
                    {
                        writer.WriteLine("historical_idea_groups = {" + comment);
                        foreach (string idea in country.Ideas)
                        {
                            writer.WriteLine("\t" + idea);
                        }
                        i = i + 10;
                    }

                    if (line.Contains("historical_units"))
                    {
                        writer.WriteLine("historical_units = {" + comment);
                        foreach (string unit in country.Units)
                        {
                            writer.WriteLine("\t" + unit);
                        }
                        writer.WriteLine("}");
                        for (; !original[i].Contains('}'); i++)
                        { 
                            //search the closing tag   
                        }
                        i++; //skip over the closing tag because we already wrote it
                    }

                    //if there is no editable stuff then just copy over the original
                    writer.WriteLine(original[i]);
                }
                writer.Close();

                //now save the ideagroup
                //first check if its a country idea group or group idea or default idea
                if (country.Nationalideagroup.Split('_')[0].Length == 3 && country.Nationalideagroup != "default_ideas") //3 letter tag in front of the name so its a specific country idea
                {
                    string filelocation = savelocation + "\\" + modname.Trim() + "\\common\\ideas\\00_country_ideas.txt";
                    original = File.ReadAllLines(filelocation, Encoding.Default);
                    File.Delete(filelocation);
                    writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                    bool found = false;
                    i = 0;
                    for (; i < original.Length; i++)
                    {
                        //splitting the comments
                        string line = SplitComments(original[i])[0];
                        string comment = SplitComments(original[i])[1];

                        //if the ideagroup is reached start searching for the trigger, otherwise copy over the original
                        if (line.Contains(country.Nationalideagroup))
                        {
                            found = true;
                            writer.WriteLine(original[i]); //write the beginning of the ideagroup to the new file
                            //look for the trigger
                            int j = i + 1;
                            for (; original[j] != "}"; j++)
                            {
                                line = SplitComments(original[j])[0];
                                comment = SplitComments(original[j])[1];

                                if (line.Contains("trigger"))
                                {
                                    //write the triggerline:
                                    writer.WriteLine(original[j]);
                                    j++;
                                    //then write the or statement, since we are going to add a country its easiest to just always add a OR statement
                                    writer.WriteLine("\t\tOR = {");
                                    for (; !original[j].Contains('}'); j++)//copy the original triggers into the statement
                                    {
                                        line = SplitComments(original[j])[0];
                                        comment = SplitComments(original[j])[1];

                                        if (!line.Contains("OR"))
                                        {
                                            if (!line.Contains("\t\t\t"))
                                            {
                                                writer.WriteLine("\t" + original[j]);
                                            }
                                            else
                                            {
                                                writer.WriteLine(original[j]);
                                            }

                                        }
                                    }
                                    //loop has ended so we are at the end of the original trigger
                                    //now add our own country and close the or statement and the trigger
                                    writer.WriteLine("\t\t\ttag = " + country.Tag);
                                    writer.WriteLine("\t\t}");
                                    writer.WriteLine("\t}");
                                    j++;
                                }
                                else
                                {
                                    //if not the trigger then just copy
                                    writer.WriteLine(original[j]);
                                }
                            }
                            i = j;
                        }
                        else
                        {
                            //if not the right idea group then just copy
                            writer.WriteLine(original[i]);
                        }
                    }

                    writer.Close();

                    if (!found)
                    {
                        //if group is not found then display error message
                        MessageBox.Show("Idea group not found in appropriated file", "Error");
                    }
                }
                else
                {
                    if (country.Nationalideagroup != "default_ideas") //not default and not country specific so its in the group file
                    {
                        string filelocation = savelocation + "\\" + modname.Trim() + "\\common\\ideas\\zz_group_ideas.txt";
                        original = File.ReadAllLines(filelocation, Encoding.Default);
                        File.Delete(filelocation);
                        writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                        bool found = false;
                        i = 0;
                        for (; i < original.Length; i++)
                        {
                            //splitting the comments
                            string line = SplitComments(original[i])[0];
                            string comment = SplitComments(original[i])[1];

                            //if the ideagroup is reached start searching for the trigger, otherwise copy over the original
                            if (line.Contains(country.Nationalideagroup))
                            {
                                found = true;
                                writer.WriteLine(original[i]); //write the beginning of the ideagroup to the new file
                                //look for the trigger
                                int j = i + 1;
                                for (; original[j] != "}"; j++)
                                {
                                    line = SplitComments(original[j])[0];
                                    comment = SplitComments(original[j])[1];

                                    if (line.Contains("trigger"))
                                    {
                                        //write the triggerline:
                                        writer.WriteLine(original[j]);
                                        j++;
                                        //now write the OR and AND construction to add our own tag
                                        writer.WriteLine("\t\tOR = {");
                                        writer.WriteLine("\t\t\tAND = {");
                                        bool extrablock = false;
                                        for (; !original[j].Contains('}'); j++)//copy the original triggers into the statement
                                        {
                                            if (SplitComments(original[i])[0].Contains('{'))
                                            {
                                                extrablock = true;
                                            }
                                            writer.WriteLine("\t\t" + original[j]);
                                        }
                                        //loop has ended so we are at the end of the original trigger
                                        //now close the AND and add our own tag, then end the OR and the triggger
                                        if (extrablock)
                                        {
                                            writer.WriteLine("\t\t\t\t}");
                                        }
                                        writer.WriteLine("\t\t\t}");
                                        writer.WriteLine("\t\t\ttag = " + country.Tag);
                                        writer.WriteLine("\t\t}");
                                        writer.WriteLine("\t}");
                                        j++;
                                    }
                                    else
                                    {
                                        //if not the trigger then just copy
                                        writer.WriteLine(original[j]);
                                    }
                                }
                                i = j;
                            }
                            else
                            {
                                //if not the right idea group then just copy
                                writer.WriteLine(original[i]);
                            }
                        }

                        writer.Close();
                        if (!found)
                        {
                            //if group is not found then display error message
                            MessageBox.Show("Idea group not found in appropriated file", "Error");
                        }

                    }
                    else
                    {
                        //if its default ideas then delete all the instances of the tag in other files, however a country will can still fall under a zz_group_ideas.txt entry, i will not change this because those ideas are always better then default_ideas
                        //first the 00_country_ideas.txt file
                        original = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt", Encoding.Default);
                        writer = new StreamWriter(new FileStream(savelocation + "\\" + modname.Trim() + "\\common\\ideas\\00_country_ideas.txt", FileMode.Append, FileAccess.Write), Encoding.Default);
                        foreach (string line in original)
                        {
                            if (!line.Contains("tag = " + country.Tag))
                            {
                                writer.WriteLine(line);
                            }
                        }
                        writer.Close();

                    }
                }
            }
            
        }

        string[] SplitComments(string rawline) //splits the line from the comment, returns the line on 0 and the comment on 1
        {
            string[] split = { "", "" };

            if (rawline.Split('#').Length > 1)
            {
                split[0] = rawline.Split('#')[0];
                split[1] = "#" + rawline.Split('#')[1];
            }
            else
            {
                split[0] = rawline;
            }

            return split;
        }

        void FlipControls() //enables or disables the controls
        {
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
            bt_new_nationalideasgroup.Enabled = !bt_new_nationalideasgroup.Enabled;
            bt_edit_nationalideasgroup.Enabled = !bt_edit_nationalideasgroup.Enabled;
            bt_edit_nationalidea.Enabled = !bt_edit_nationalidea.Enabled;
            lb_units.Enabled = !lb_units.Enabled;
            cmb_units.Enabled = !cmb_units.Enabled;
            bt_up.Enabled = !bt_up.Enabled;
            bt_down.Enabled = !bt_down.Enabled;
            bt_delete.Enabled = !bt_delete.Enabled;
            bt_add_unit.Enabled = !bt_add_unit.Enabled;
            cmb_prefered_religion.Enabled = !cmb_prefered_religion.Enabled;
        }

        public main()
        {
            InitializeComponent();
        }

        private void bt_load_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGame();
                bt_load.Enabled = false;
                bt_loadc.Enabled = true;
                bt_save_country.Enabled = true;
                cmb_countries.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Gamefiles not found in this directory.", "Error");
            }
        }

        private void bt_save_country_Click(object sender, EventArgs e)
        {
            //add country object with all data to the country list to be saved to the mod folder
            //first remove the country if its already in the list
            savedcountries.RemoveAll(item => item.Tag == txt_tag.Text);
            //creating the dataobject
            CountryData country = new CountryData();
            country.Name = cmb_countries.Text;
            country.Tag = txt_tag.Text;
            int[] colours = new int[3];
            colours[0] = Convert.ToInt32(txt_colour_r.Text);
            colours[1] = Convert.ToInt32(txt_colour_g.Text);
            colours[2] = Convert.ToInt32(txt_colour_b.Text);
            country.Color = colours;
            country.Gfx = txt_gfx.Text;
            country.Units = lb_units.Items.OfType<string>().ToList();
            country.Ideas = new string[8] { cmb_idea1.Text, cmb_idea2.Text, cmb_idea3.Text, cmb_idea4.Text, cmb_idea5.Text, cmb_idea6.Text, cmb_idea7.Text, cmb_idea8.Text };
            country.Nationalideagroup = cmb_nationalideasgroup.Text;
            country.Culture = cmb_culture.Text;
            country.Religion = cmb_religion.Text;
            country.PreferedReligion = cmb_prefered_religion.Text;
            country.Techgroup = cmb_technology.Text;
            country.Capital = Convert.ToInt32(txt_capital.Text);
            country.Mercantilism = decimal.Parse(txt_mercantilism.Text, CultureInfo.InvariantCulture);
            country.Government = cmb_government.Text;
            //adding it to the list
            savedcountries.Add(country);
            MessageBox.Show("Country " + cmb_countries.Text + " saved. Dont forget to save the whole mod!", "Done");
        }

        private void bt_loadc_Click(object sender, EventArgs e)
        {
            lb_units.Items.Clear();
            try 
            {
                LoadCountry();
                if (txt_capital.Enabled == false)
                {
                    FlipControls();
                }
            }
            catch
            {
                MessageBox.Show("Country files not found in selected directory.", "Error");
            }
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

        private void cmb_nationalideasgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_nationalideas.Items.Clear();
            cmb_nationalideas.Text = "";

            if (cmb_nationalideasgroup.Text == "default_ideas")
            {
                LoadNationalIdeas("default_ideas", File.ReadAllLines(eulocation + "\\common\\ideas\\zzz_default_idea.txt"));
            }
            else
            {
                LoadNationalIdeas(cmb_nationalideasgroup.Text, File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt"));
                if (cmb_nationalideas.Items.Count == 0)
                {
                    LoadNationalIdeas(cmb_nationalideasgroup.Text, File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt"));
                }
            }
        }

        private void bt_up_Click(object sender, EventArgs e)
        {
            int selectedIndex = lb_units.SelectedIndex;
            if (selectedIndex > 0 & selectedIndex != -1)
            {
                lb_units.Items.Insert(selectedIndex - 1, lb_units.Items[selectedIndex]);
                lb_units.Items.RemoveAt(selectedIndex + 1);
                lb_units.SelectedIndex = selectedIndex - 1;
            }
        }

        private void bt_down_Click(object sender, EventArgs e)
        {
            int selectedIndex = lb_units.SelectedIndex;
            if (selectedIndex < lb_units.Items.Count - 1 & selectedIndex != -1)
            {
                lb_units.Items.Insert(selectedIndex + 2, lb_units.Items[selectedIndex]);
                lb_units.Items.RemoveAt(selectedIndex);
                lb_units.SelectedIndex = selectedIndex + 1;

            }
        }

        private void bt_add_unit_Click(object sender, EventArgs e)
        {
            if (!lb_units.Items.Contains(cmb_units.Text))
            {
                lb_units.Items.Add(cmb_units.Text);
            }
            else
            {
                MessageBox.Show("Unit is already in list", "Error");
            }
        }

        private void bt_delete_Click(object sender, EventArgs e)
        {
            lb_units.Items.RemoveAt(lb_units.SelectedIndex);
        }

        private void bt_edit_nationalidea_Click(object sender, EventArgs e)
        {
            List<string> ideas = new List<string>();
            List<string> groups = new List<string>();

            if (cmb_nationalideas.Text != "")
            {
                foreach (string group in cmb_nationalideasgroup.Items)
                {
                    groups.Add(group);
                }

                foreach (string idea in cmb_nationalideas.Items)
                {
                    ideas.Add(idea);
                }

                NationalIdeaEditor form = new NationalIdeaEditor(ideas, cmb_nationalideasgroup.Text, cmb_nationalideas.Text, eulocation);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("First select an idea to edit, you can change this selection in the ideaeditor", "Error");
            }
        }

        private void bt_edit_nationalideasgroup_Click(object sender, EventArgs e)
        {
            List<string> groups = new List<string>();
            foreach (string group in cmb_nationalideasgroup.Items)
            {
                groups.Add(group);
            }
            NationalIdeaGroupEditor form = new NationalIdeaGroupEditor(groups, cmb_nationalideasgroup.Text, eulocation);
            form.ShowDialog();
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_save_mod_Click(object sender, EventArgs e)
        {
            //ask for mod name
            InputBox popup = new InputBox("Please specify a name for you mod");
            popup.ShowDialog();
            string modname = popup.answer;
            popup.Dispose();
            //ask for savelocation
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select the folder where you want to save your modifications";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string savelocation = fbd.SelectedPath;
                //create the mod file
                string[] file = {
                                    "name=" + modname,
                                    "path=mod/" + modname.Trim()
                                };
                File.WriteAllLines(savelocation + "\\" + modname.Trim() + ".mod", file);
                //create the modfolder
                Directory.CreateDirectory(savelocation + "\\" + modname.Trim());
                //create the directory structure needed for the country files
                Directory.CreateDirectory(savelocation + "\\" + modname.Trim() + "\\common\\countries");
                Directory.CreateDirectory(savelocation + "\\" + modname.Trim() + "\\common\\ideas");
                Directory.CreateDirectory(savelocation + "\\" + modname.Trim() + "\\history\\countries");
                //copy the idea files if they are not already there
                try
                {
                    File.Copy(eulocation + "\\common\\ideas\\00_country_ideas.txt", savelocation + "\\" + modname.Trim() + "\\common\\ideas\\00_country_ideas.txt");
                }
                catch
                { 
                    //the file already exists, in this case just do nothing, the savefunction will use the existing file
                }
                try
                {
                    File.Copy(eulocation + "\\common\\ideas\\zz_group_ideas.txt", savelocation + "\\" + modname.Trim() + "\\common\\ideas\\zz_group_ideas.txt");
                }
                catch
                {
                    //the file already exists, in this case just do nothing, the savefunction will use the existing file
                }
                //save the countries:
                foreach (CountryData country in savedcountries)
                {
                    SaveCountry(country, savelocation, modname);
                }
            }
        }
    }
}
