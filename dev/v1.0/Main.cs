﻿using System;
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
        //some globals
        string eulocation; //the location of the game
        //lists of organized gamedata for the controls
        List<List<string>> Religions = new List<List<string>>();
        List<List<string>> Cultures = new List<List<string>>();
        List<List<string>> NIdeas = new List<List<string>>();
        List<string> Ideas = new List<string>();
        //list for the saved countries
        List<CountryData> savedcountries = new List<CountryData>();
        //important files, these are saved in a string array in order for the sub editers to change stuff wich the main editor can read and write
        List<string> NIdeasFile1;//00_country_ideas.txt
        List<string> NIdeasFile2;//zz_group_ideas.txt
        List<string> NIdeasFile3;//zzz_default_idea.txt
        List<string> CulturesFile;
        List<string> ReligionsFile;
        List<string> GovernmentFile;
        List<string> TechgroupFile;
        List<string> BIdeasFile;
        List<string> Regions;

        SyntaxOptions options = new SyntaxOptions();

        int ReadSyntax(int linenum, List<string> file) //used to read from curly bracket to curly bracket, returns the line number of the closing bracket, if end of file then returns 0
        {
            for (; linenum < file.Count; linenum++)
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

            LoadCombobox(cmb_government, LoadSyntaxFirstLevel(GovernmentFile));

            //load the religions

            //first read the religion groups

            LoadCombobox(cmb_religiongroup, LoadSyntaxFirstLevel(ReligionsFile));
            LoadCombobox(cmb_prefered_religion, LoadSyntaxFirstLevel(ReligionsFile));

            //now that we know the religiongroups, we can read the religions themselves.

            Religions = LoadSyntaxSecondLevel(ReligionsFile);

            //now load the culture groups

            LoadCombobox(cmb_culturegroup, LoadSyntaxFirstLevel(CulturesFile));

            //load the  cultures into their groups

            Cultures = LoadSyntaxSecondLevel(CulturesFile);

            //load the technology groups

            LoadCombobox(cmb_technology, LoadSyntaxSecondLevel(TechgroupFile)[0]);

            //load the basic ideagroups

            List<string> ideas = LoadSyntaxFirstLevel(BIdeasFile);
            LoadCombobox(cmb_idea1, ideas);
            LoadCombobox(cmb_idea2, ideas);
            LoadCombobox(cmb_idea3, ideas);
            LoadCombobox(cmb_idea4, ideas);
            LoadCombobox(cmb_idea5, ideas);
            LoadCombobox(cmb_idea6, ideas);
            LoadCombobox(cmb_idea7, ideas);
            LoadCombobox(cmb_idea8, ideas);
            LoadCombobox(cmb_idea9, ideas);
            LoadCombobox(cmb_Bidea, ideas);

            //load the national idea groups
            LoadCombobox(cmb_nationalideasgroup, LoadSyntaxFirstLevel(NIdeasFile1).Concat(LoadSyntaxFirstLevel(NIdeasFile2)).ToList());
            //add an entry for the default group
            cmb_nationalideasgroup.Items.Add("default_idea");

            //load the ideas themselves into the global list

            NIdeas = LoadSyntaxSecondLevel(NIdeasFile1).Concat((LoadSyntaxSecondLevel(NIdeasFile2)).Concat(LoadSyntaxSecondLevel(NIdeasFile3)).ToList()).ToList();

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
                //load the files into memory
                NIdeasFile1 = File.ReadAllLines(eulocation + "\\common\\ideas\\00_country_ideas.txt", Encoding.Default).ToList();
                NIdeasFile2 = File.ReadAllLines(eulocation + "\\common\\ideas\\zz_group_ideas.txt", Encoding.Default).ToList();
                NIdeasFile3 = File.ReadAllLines(eulocation + "\\common\\ideas\\zzz_default_idea.txt", Encoding.Default).ToList();
                CulturesFile = File.ReadAllLines(eulocation + "\\common\\cultures\\00_cultures.txt", Encoding.Default).ToList();
                ReligionsFile = File.ReadAllLines(eulocation + "\\common\\religions\\00_religion.txt", Encoding.Default).ToList();
                GovernmentFile = File.ReadAllLines(eulocation + "\\common\\governments\\00_governments.txt", Encoding.Default).ToList();
                TechgroupFile = File.ReadAllLines(eulocation + "\\common\\technology.txt", Encoding.Default).ToList();
                BIdeasFile = File.ReadAllLines(eulocation + "\\common\\ideas\\00_basic_ideas.txt", Encoding.Default).ToList();
                Regions = File.ReadAllLines(eulocation + "\\map\\region.txt", Encoding.Default).ToList();
                //other options
                LoadCountryOptions();
            }

        }

        void LoadCountryControls(CountryData country)
        {
            cmb_religiongroup.Text = "";
            cmb_culturegroup.Text = "";
            cmb_prefered_religion.Text = "";
            cmb_nationalideasgroup.Text = "";
            cmb_nationalideas.Items.Clear();

            txt_tag.Text = country.Tag;
            txt_gfx.Text = country.Gfx;
            cmb_nationalideasgroup.Text = country.Nationalideagroup;

            cmb_religiongroup.Text = country.Religiongroup;
            cmb_culture.Text = country.Culture;
            cmb_culturegroup.Text = country.Culturegroup;
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
            cmb_idea9.Text = country.Ideas[8];
            txt_colour_r.Text = country.Color[0].ToString();
            txt_colour_g.Text = country.Color[1].ToString();
            txt_colour_b.Text = country.Color[2].ToString();
            foreach (string unit in country.Units)
            {
                lb_units.Items.Add(unit);
            }
        }

        CountryData SaveCountryControls()
        {
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
            country.Ideas = new string[9] { cmb_idea1.Text, cmb_idea2.Text, cmb_idea3.Text, cmb_idea4.Text, cmb_idea5.Text, cmb_idea6.Text, cmb_idea7.Text, cmb_idea8.Text, cmb_idea9.Text };
            country.Nationalideagroup = cmb_nationalideasgroup.Text;
            country.Culture = cmb_culture.Text;
            country.Religion = cmb_religion.Text;
            country.PreferedReligion = cmb_prefered_religion.Text;
            country.Techgroup = cmb_technology.Text;
            country.Capital = Convert.ToInt32(txt_capital.Text);
            country.Mercantilism = decimal.Parse(txt_mercantilism.Text, CultureInfo.InvariantCulture);
            country.Government = cmb_government.Text;
            country.Culturegroup = cmb_culturegroup.Text;
            country.Religiongroup = cmb_religiongroup.Text;

            return country;
        }

        CountryData LoadCountry(string countryname)//loads the details of the given country and gives back a country data
        {
            CountryData country = null;
            country = savedcountries.Find(item => item.Name == cmb_countries.Text);
            if (country != null)
            {
                return country;
            }

            if (country != null) //if country has been found in savedcountries list then load the country from this list
            {
                LoadCountryControls(country);
            }
            else
            {
                //create the datatype in wich the information will be loaded in
                country = new CountryData();
                country.Ideas = new string[9];
                //load the tag
                string[] file = File.ReadAllLines(eulocation + "\\common\\country_tags\\00_countries.txt", Encoding.Default);
                foreach (string rawline in file)
                {
                    string line = SplitComments(rawline)[0];
                    if (line == "")
                    {
                        continue;
                    }
                    //extract the country name (just line.contains is not enough because of dubble names eg munster and IRE_munster
                    string[] parts = line.Split('/');
                    string selectedcountry = "";
                    foreach (string part in parts)
                    { 
                        if(part.Contains(".txt"))
                        {
                            selectedcountry = part.Split('.')[0];
                            break;
                        }
                    }

                    if (selectedcountry == countryname)
                    {
                        if (line.Contains('\t')) //sometimes there is a tab and sometimes there is a space to format the line so we need to distuigish this. (wtf paradox?!)
                        {
                            country.Tag = line.Split('\t')[0];
                        }
                        else
                        {
                            country.Tag = line.Split(' ')[0];
                        }

                        break;
                    }
                }

                //read the info from the history/countries/<tag> - <name>.txt file
                //because paradox in all their wisdom choose to not use spaces in the common/countries folder for names but do use it in the history folder (seriously guys wtf?!), we need to load all the filenames from the history file and find the right one using the tag
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
                                country.Government = value;
                                break;
                            case "mercantilism":
                                country.Mercantilism = decimal.Parse(value, CultureInfo.InvariantCulture);
                                break;
                            case "technology_group":
                                country.Techgroup = value;
                                break;
                            case "religion":                        
                                country.Religion = value;
                                break;
                            case "primary_culture":
                                country.Culture = value;
                                break;
                            case "capital":
                                country.Capital = Convert.ToInt32(value);
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
                        country.Gfx = line.Split('=')[1].Trim();
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
                        country.Color = new int[3] { Convert.ToInt32(colours[0]), Convert.ToInt32(colours[1]), Convert.ToInt32(colours[2]) };
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
                            country.Ideas[ideanum] = line.Trim('\t');
                            ideanum++;
                        }
                        continue;
                    }

                    if (line.Contains("historical_units"))
                    {
                        i++;
                        List<string> units = new List<string>();
                        for (; !file[i].Contains('}'); i++)
                        {
                            line = file[i].Split('#')[0].Trim('\t');
                            if (line == "")
                            {
                                continue;
                            }
                            units.Add(line);
                        }
                        country.Units = units;
                    }
                    if (line.Contains("preferred_religion"))
                    {
                        country.PreferedReligion = line.Split('=')[1].Trim();
                    }
                }
                //the culture and religion groups can be related from already loaded data

                for (int i = 0; i < Cultures.Count; i++)
                {
                    if (Cultures[i].Contains(country.Culture))
                    {
                        country.Culturegroup = cmb_culturegroup.Items[i].ToString();
                        break;
                    }
                }

                for (int i = 0; i < Religions.Count; i++)
                {
                    if (Religions[i].Contains(country.Religion))
                    {
                        country.Religiongroup = cmb_religiongroup.Items[i].ToString();
                        break;
                    }
                }
                //the capitalregion
                country.Capitalregion = FindRegion(country.Capital);

                //read the national ideas, this only loads the names of the ideas, the ideas themselves will be loaded and editted in the edit screen
                string ideatag = SearchNationalIdeas(country.Tag);
                if (ideatag != "")
                {
                    country.Nationalideagroup = ideatag;
                }
                else
                {                                   
                    ideatag = SearchNationalGroup(country);
                    if (ideatag != "")
                    {
                        country.Nationalideagroup = ideatag;
                    }
                    else
                    {
                        country.Nationalideagroup = "default_ideas";
                    }
                }
            }

            return country;
        }

        string FindRegion(int province) //finds the name of the region the given province is in returns an empty string if the region is not found.
        {
            
            for (int i = 0; i < Regions.Count(); i++)
            {
                string region = "";
                string line = SplitComments(Regions[i])[0];
                if (line == "")
                {
                    continue;
                }
                if (line.Contains("{"))
                {
                    region = line.Split('=')[0].Trim();
                    for (; i < Regions.Count(); i++)
                    {
                        line = SplitComments(Regions[i])[0];
                        if (line == "")
                        {
                            continue;
                        }
                        if (line.Contains("}"))
                        {
                            break;
                        }
                        if(line.Contains(province.ToString()))
                        {
                            return region;
                        }
                    }
                }
            }
            return "";
        }

        bool Logic(string type, CountryData country, List<string> file, int i, bool internal_block) //returns true if the logic is true and false when logic is false or an error occured
        {
            for (; i < file.Count(); i++)
            {
                string line = SplitComments(file[i])[0].Trim('\t').Trim();
                if (line == "")
                {
                    continue;
                }
                if ((line.Contains("OR") || line.Contains("AND") || line.Contains("NOT")) && line.Contains("{") && !internal_block)
                {
                    bool iblock = false;
                    if(!(line.Contains('{') && line.Contains('}')))
                    {
                        i++;
                    }
                    if (line.Contains('{') && line.Contains('}'))
                    {
                        iblock = true;
                    }
                    if (Logic(line.Trim('\t').Split('=')[0].Trim().ToLower(), country, file, i,iblock))
                    {
                        switch (type)
                        {
                            case "or":
                                return true;
                            case "and":
                                //do nothing
                                break;
                            case "not":
                                return false;
                            default:
                                return false;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case "or":
                                //do nothing
                                break;
                            case "and":
                                return false;
                            case "not":
                                //do nothing
                                break;
                            default:
                                return false;
                        }
                    }
                    if (!(line.Contains('{') && line.Contains('}')))
                    {
                        i = ReadSyntax(i + 1, file);
                    }                    
                    continue;
                }
                if (line.Contains("trigger"))
                {
                    continue;
                }
                if (line.Contains('}') && !internal_block)
                {
                    break;
                }
                string property;
                string value;
                if (internal_block) //check if its a one line block
                {
                    property = line.Split('=')[1].Trim().Trim('{').Trim();
                    value = line.Split('=')[2].Trim().Trim('}').Trim();
                }
                else
                {
                    property = line.Split('=')[0].Trim();
                    value = line.Split('=')[1].Trim();
                }
                switch (property)
                { 
                    case "tag":
                        if (country.Tag == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "primary_culture":
                        if (country.Culture == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "culture_group":
                        if (country.Culturegroup == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "religion":
                        if (country.Religion == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "religion_group":
                        if (country.Religiongroup == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "government":
                        if ((country.Government == value) || ((country.Government == "theocratic_government") && (value == "theocracy")))
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                    case "region":
                        if (country.Capitalregion == value)
                        {
                            switch (type)
                            {
                                case "or":
                                    return true;
                                case "and":
                                    //do nothing
                                    break;
                                case "not":
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "or":
                                    //do nothing
                                    break;
                                case "and":
                                    return false;
                                case "not":
                                    //do nothing
                                    break;
                                default:
                                    return false;
                            }
                        }
                        break;
                }
                if (line.Contains('{') && line.Contains('}'))
                {
                    break;
                }
            }
            switch (type)
            {
                case "or":
                    return false;
                case "and":
                    return true;
                case "not":
                    return true;
                default:
                    return false;
            }
        }

        string SearchNationalGroup(CountryData country) //searches for the ideagroup from the zz_group_ideas.txt, returns the name of the group if found
        {
            List<string> file = NIdeasFile2;
            for (int i = 0; i < file.Count; i++)
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

                    for (int z = i; z < file.Count; z++)
                    {
                        line = file[z].TrimEnd('\t');
                        if (line.Contains("trigger") && line.Contains('\t'))
                        {
                            if (Logic("and", country, file, z+1, false))
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

        string SearchNationalIdeas(string tag) //searches the tag of an ideagroup of the selected country, returns that tag if found and empty string otherwise
        {
            List<string> file = NIdeasFile1;
            for (int i = 0; i < file.Count; i++)
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

                    for (int z = i; z < file.Count; z++)
                    {
                        line = file[z];
                        if (line.Contains("tag") && line.Contains('\t'))
                        {
                            if (line.Split('=')[1].Trim() == tag)
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

        List<string> LoadSyntaxFirstLevel(List<string> file) //parse the items in the first level of code blocks, returns null on syntax error
        {
            List<string> items = new List<string>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];
                string comment = SplitComments(file[i])[1];

                if (line == "")
                {
                    continue;
                }

                if (line.Contains("{"))
                {
                    items.Add(line.Split('=')[0].Trim());
                    i = ReadSyntax(i + 1, file);
                    if (i == 0)
                    {
                        return null;
                    }
                }
            }

            return items;
        }

        List<List<string>> LoadSyntaxSecondLevel(List<string> file) //parse the items in the second level of code blocks. this will return of list of lists that contains the second level item. the order of the list is the same as the file. returns null on syntax error
        {
            List<List<string>> items = new List<List<string>>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];
                if (line.Contains('{'))
                {
                    List<string> blockitems = new List<string>();
                    i++;
                    for (; i < file.Count; i++)
                    {
                        line = SplitComments(file[i])[0];
                        if (line.Contains("}") && !line.TrimEnd('\t').Contains("\t"))
                        {
                            break;
                        }

                        if (line.Contains("{"))
                        {
                            blockitems.Add(line.Split('=')[0].Trim(' ', '\t'));
                            if (!line.Contains("}")) //check if there is a opening and closing statement on the same line. in that case there is no need for a readsyntax call
                            {
                                i = ReadSyntax(i + 1, file);
                                if (i == 0)
                                {
                                    MessageBox.Show("Syntax Error", "GODVERDOMME");
                                    return null;
                                }
                            }

                        }
                    }
                    items.Add(blockitems);
                }
            }

            return items;
        }

        List<string> LoadSyntaxSecondLevel(List<string> file, string name) //this will load the second level items of a block with specified name, returns null on syntax error
        {
            List<string> items = new List<string>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];
                string comment = SplitComments(file[i])[1];

                if (line == "")
                {
                    continue;
                }

                if (line.Contains(name) && line.Contains('{'))
                {
                    for (int z = i; z < file.Count; z++)
                    {
                        line = file[z];
                        if (line.Contains('{') && line.Contains('\t'))
                        {
                            string[] parts = line.Split(' ');
                            string item = parts[0].TrimStart('\t');

                            items.Add(item);

                            if (!line.Contains("}")) //check if the block is not closed at the same line
                            {
                                z = ReadSyntax(z + 1, file);
                                if (z == 0)
                                {
                                    return null;
                                }
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

            return items;
        }

        List<string> ExtractFirstLevelBlock(List<string> file, string blockname) //extracts the codeblock of a given name, returns null when not found
        {
            List<string> Block = new List<string>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];

                if (line.Contains(blockname) && line.Contains("{") && !line.Contains("\t"))
                {
                    for (int j = i + 1; j < file.Count; j++)
                    {
                        line = SplitComments(file[j])[0];

                        if (line == "")
                        {
                            continue;
                        }

                        if (line.Contains("}") && !line.TrimEnd('\t').Contains("\t"))
                        {
                            break;
                        }

                        Block.Add(line.Remove(0,1));
                    }
                    return Block;
                }
            }

            return null;
        }

        void LoadCombobox(ComboBox cmb, List<string> items) //loads a list into combobox items
        {
            cmb.Items.Clear();
            foreach (string item in items)
            {
                cmb.Items.Add(item);
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
                    string comment = SplitComments(original[i])[1];
                    string line = SplitComments(original[i])[0];

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
                            if (idea != "")
                            {
                                writer.WriteLine("\t" + idea);                                
                            }
                        }
                        i = ReadSyntax(i + 1, original.ToList());                      
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
                    //load old file and create new one
                    string filelocation = savelocation + "\\" + modname.Trim() + "\\common\\ideas\\00_country_ideas.txt";
                    File.Delete(filelocation);
                    writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                    //first delete all the old tag references
                    DeleteCountryTag(country, NIdeasFile1, writer);
                    writer.Close();
                    //then reload the file
                    original = File.ReadAllLines(filelocation, Encoding.Default);
                    File.Delete(filelocation);
                    writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                    //and begin editing for adding the country
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
                        //load old file and create new one
                        File.Delete(filelocation);
                        writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                        //first delete all the old tag references
                        DeleteCountryTag(country, NIdeasFile2, writer);
                        writer.Close();
                        //then reload the file
                        original = File.ReadAllLines(filelocation, Encoding.Default);
                        File.Delete(filelocation);
                        writer = new StreamWriter(new FileStream(filelocation, FileMode.Append, FileAccess.Write), Encoding.Default);
                        //and begin editing for adding the country
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
                        writer = new StreamWriter(new FileStream(savelocation + "\\" + modname.Trim() + "\\common\\ideas\\00_country_ideas.txt", FileMode.Append, FileAccess.Write), Encoding.Default);
                        DeleteCountryTag(country, NIdeasFile1, writer);
                        writer.Close();

                    }
                }
            }
            
        }

        void DeleteCountryTag(CountryData country, List<string> original, StreamWriter writer) //deletes the country tag from given file
        {
            foreach (string rawline in original)
            {
                string line = SplitComments(rawline)[0];
                if (!line.Contains("tag = " + country.Tag))
                {
                    writer.WriteLine(rawline);
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
            cmb_culture.Enabled = !cmb_culture.Enabled;
            cmb_religion.Enabled = !cmb_religion.Enabled;
            txt_mercantilism.Enabled = !txt_mercantilism.Enabled;
            cmb_nationalideas.Enabled = !cmb_nationalideas.Enabled;
            cmb_idea1.Enabled = !cmb_idea1.Enabled;
            cmb_idea2.Enabled = !cmb_idea2.Enabled;
            cmb_idea3.Enabled = !cmb_idea3.Enabled;
            cmb_idea4.Enabled = !cmb_idea4.Enabled;
            cmb_idea5.Enabled = !cmb_idea5.Enabled;
            cmb_idea6.Enabled = !cmb_idea6.Enabled;
            cmb_idea7.Enabled = !cmb_idea7.Enabled;
            cmb_idea8.Enabled = !cmb_idea8.Enabled;
            cmb_idea9.Enabled = !cmb_idea9.Enabled;
            lb_units.Enabled = !lb_units.Enabled;
            cmb_units.Enabled = !cmb_units.Enabled;
            bt_up.Enabled = !bt_up.Enabled;
            bt_down.Enabled = !bt_down.Enabled;
            bt_delete.Enabled = !bt_delete.Enabled;
            bt_add_unit.Enabled = !bt_add_unit.Enabled;
            cmb_prefered_religion.Enabled = !cmb_prefered_religion.Enabled;
        }

        List<string> InsertBlock(List<string> originalfile, List<string> block, string blockname)//inserts a block into a file at the right place, returns the resulting file
        {
            List<string> file = new List<string>();

            for (int i = 0; i < originalfile.Count(); i++)
            {
                string line = SplitComments(originalfile[i])[0];

                if (line.Contains(blockname))
                {
                    file.Add(originalfile[i]);                    
                    for (int z = 0; z < block.Count(); z++)
                    {
                        file.Add("\t" + block[z]);
                    }
                    i = ReadSyntax(i + 1, originalfile);
                    file.Add("}");
                }
                else
                {
                    file.Add(originalfile[i]);
                }
            }

            return file;
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
                bt_save_mod.Enabled = true;
                bt_edit_culturegroup.Enabled = true;
                bt_edit_governments.Enabled = true;
                bt_edit_idea.Enabled = true;
                bt_edit_nationalideasgroup.Enabled = true;
                bt_edit_religiongroup.Enabled = true;
                bt_edit_techgroup.Enabled = true;
                cmb_culturegroup.Enabled = true;
                cmb_government.Enabled = true;
                cmb_Bidea.Enabled = true;
                cmb_nationalideasgroup.Enabled = true;
                cmb_religiongroup.Enabled = true;
                cmb_technology.Enabled = true;
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
            CountryData country = SaveCountryControls();
            //adding it to the list
            savedcountries.Add(country);
            MessageBox.Show("Country " + cmb_countries.Text + " saved. Dont forget to save the whole mod!", "Done");
        }

        private void bt_loadc_Click(object sender, EventArgs e)
        {
            if (cmb_countries.Text == "")
            {
                MessageBox.Show("Select a country first", "Error");
                return;
            }
            lb_units.Items.Clear();
            try
            {
                LoadCountryControls(LoadCountry(cmb_countries.Text));
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
            cmb_religion.Text = "";
            LoadCombobox(cmb_religion, Religions[cmb_religiongroup.SelectedIndex]);

            //List<string> religions = Religions[cmb_religiongroup.SelectedIndex];
            //foreach (string religion in religions)
            //{
            //    if (religion != "dummy") //dont print the dummy
            //    {
            //        cmb_religion.Items.Add(religion);
            //    }
                
            //}
        }

        private void cmb_culturegroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_culture.Items.Clear();
            //cmb_culture.Text = "";
            LoadCombobox(cmb_culture,Cultures[cmb_culturegroup.SelectedIndex]);
        //    List<string> cultures = Cultures[cmb_culturegroup.SelectedIndex];
        //    foreach (string culture in cultures)
        //    {
        //        if (culture != "dummy") //dont print the dummy
        //        {
        //            cmb_culture.Items.Add(culture);
        //        }

        //    }
        }

        private void cmb_nationalideasgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_nationalideas.Items.Clear();
            cmb_nationalideas.Text = "";
            LoadCombobox(cmb_nationalideas, NIdeas[cmb_nationalideasgroup.SelectedIndex]);
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

                //NationalIdeaEditor form = new NationalIdeaEditor(ideas, cmb_nationalideasgroup.Text, cmb_nationalideas.Text, eulocation);
                //form.ShowDialog();
            }
            else
            {
                MessageBox.Show("First select an idea to edit, you can change this selection in the ideaeditor", "Error");
            }
        }

        private void bt_edit_nationalideasgroup_Click(object sender, EventArgs e)
        {
            if (cmb_nationalideasgroup.Text != "")
            {
                List<string> file;
                int filenumber;
                //first find out in wich file the ideagroup is in
                if (cmb_nationalideasgroup.Text.Split('_')[0].Length == 3) //country ideas
                {
                    filenumber = 1;
                    file = NIdeasFile1;
                }
                else
                {
                    if (cmb_nationalideasgroup.Text == "default_ideas")//default ideas
                    {
                        filenumber = 3;
                        file = NIdeasFile3;
                    }
                    else //group ideas
                    {
                        filenumber = 2;
                        file = NIdeasFile2;
                    }
                }

                SubEditor editor = new SubEditor("National Ideas", cmb_nationalideasgroup.Text, "Propertie", "Propertie", ExtractFirstLevelBlock(file, cmb_nationalideasgroup.Text), new string[] { "start", "bonus", "trigger", "free" }, options.modifiers);
                editor.ShowDialog();
                switch (filenumber)
                { 
                    case 1:
                        NIdeasFile1 = InsertBlock(NIdeasFile1, editor.Result, cmb_nationalideasgroup.Text);
                        break;

                    case 2:
                        NIdeasFile2 = InsertBlock(NIdeasFile2, editor.Result, cmb_nationalideasgroup.Text);
                        break;

                    case 3:
                        NIdeasFile3 = InsertBlock(NIdeasFile3, editor.Result, cmb_nationalideasgroup.Text);
                        break;
                }
                editor.Dispose();
            }
            else
            {
                MessageBox.Show("Please choose a national idea group first", "Error");
            }

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
                string modfolder = savelocation + "\\" + modname.Trim();
                //create the directory structure needed for the files
                Directory.CreateDirectory(modfolder + "\\common\\countries");
                Directory.CreateDirectory(modfolder + "\\common\\ideas");
                Directory.CreateDirectory(modfolder + "\\history\\countries");
                Directory.CreateDirectory(modfolder + "\\common\\cultures");
                Directory.CreateDirectory(modfolder + "\\common\\religions");
                Directory.CreateDirectory(modfolder + "\\common\\governments");
                //write all the configuration files suchs as governments and ideas etc
                File.WriteAllLines(modfolder + "\\common\\ideas\\00_basic_ideas.txt",BIdeasFile,Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\ideas\\00_country_ideas.txt", NIdeasFile1, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\ideas\\zz_group_ideas.txt", NIdeasFile2, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\ideas\\zzz_default_ideas.txt", NIdeasFile3, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\cultures\\00_cultures.txt", CulturesFile, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\religions\\00_religion.txt", ReligionsFile, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\governments\\00_governments.txt", GovernmentFile, Encoding.Default);
                File.WriteAllLines(modfolder + "\\common\\technology.txt", TechgroupFile, Encoding.Default);
                //save the countries:
                foreach (CountryData country in savedcountries)
                {
                    SaveCountry(country, savelocation, modname);
                }
                //report back that it is done:
                MessageBox.Show("Mod " + modname + " is succesfull saved to: " + savelocation);
            }
        }

        private void bt_edit_religiongroup_Click(object sender, EventArgs e)
        {
            if (cmb_religiongroup.Text != "")
            {
                SubEditor editor = new SubEditor("Religions", cmb_religiongroup.Text, "Religion", "Properties", ExtractFirstLevelBlock(ReligionsFile, cmb_religiongroup.Text), new string[] { "defender_of_faith", "crusade_name" }, new string[] { "papacy", "date", "has_patriarchs", "misguided_heretic", "uses_piety", "annex" });
                editor.ShowDialog();
                ReligionsFile = InsertBlock(ReligionsFile, editor.Result, cmb_religiongroup.Text);
                editor.Dispose();
            }
            else
            {
                MessageBox.Show("Please choose a religiongroup first", "Error");
            }
            
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void bt_edit_culturegroup_Click(object sender, EventArgs e)
        {
            if (cmb_culturegroup.Text != "")
            {
                SubEditor editor = new SubEditor("Cultures", cmb_culturegroup.Text, "Culture", "Properties", ExtractFirstLevelBlock(CulturesFile, cmb_culturegroup.Text), new string[] { "graphical_culture", "dynasty_names", "test" }, new string[] { "primary", "dynasty_names", "test" });
                editor.ShowDialog();
                CulturesFile = InsertBlock(CulturesFile, editor.Result, cmb_culturegroup.Text);
                editor.Dispose();
            }
            else
            {
                MessageBox.Show("Please choose a culturegroup first", "Error");
            }
        }

        private void bt_edit_techgroup_Click(object sender, EventArgs e)
        {

            SubEditor editor = new SubEditor("Techgroups", "", "Techgroup", "Properties", ExtractFirstLevelBlock(TechgroupFile, "groups"), new string[] { }, new string[] { "modifier", "start_level", "cav_to_inf_ratio", "power" });
            editor.ShowDialog();
            TechgroupFile = InsertBlock(TechgroupFile, editor.Result, "groups");
            editor.Dispose();
            
        }

        private void bt_edit_governments_Click(object sender, EventArgs e)
        {
            if (cmb_government.Text != "")
            {
                SubEditor editor = new SubEditor("Government", cmb_government.Text, "Properties", "Properties", ExtractFirstLevelBlock(GovernmentFile, cmb_government.Text), new string[] { "monarchy", "valid_for_new_country", "ai_will_do", "global_spy_defence", "core_creation", "global_manpower_modifier", "land_morale", "army_tradition", "duration", "republic", "republic_name", "tolerance_heretic", "tolerance_heathen" , "royal_marriage", "production_efficiency", "stability_cost_modifier", "allow_convert", "prestige" , "tribal", "nomad", "relations_decay_of_me", "global_prov_trade_power_modifier", "global_revolt_risk", "technology_cost"}, new string[] { "factor", "modifier" });
                editor.ShowDialog();
                GovernmentFile = InsertBlock(GovernmentFile, editor.Result, cmb_government.Text);
                editor.Dispose();
            }
            else
            {
                MessageBox.Show("Please choose a Government first", "Error");
            }
        }

        private void bt_edit_idea_Click(object sender, EventArgs e)
        {
            if (cmb_Bidea.Text != "")
            {
                SubEditor editor = new SubEditor("Idea Group",cmb_Bidea.Text,"Ideas","P roperties", ExtractFirstLevelBlock(BIdeasFile, cmb_Bidea.Text), new string[] { "category", "bonus", "ai_will_do", "trigger" }, options.modifiers);
                editor.ShowDialog();
                BIdeasFile = InsertBlock(BIdeasFile, editor.Result, cmb_Bidea.Text);
                editor.Dispose();
            }
            else
            {
                MessageBox.Show("Please choose an ideagroup first");
            }
        }
    }
}
