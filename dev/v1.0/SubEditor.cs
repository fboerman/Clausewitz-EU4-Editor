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
    public partial class SubEditor : Form
    {
        List<string> File;
        List<List<string>> thirdlevel;

        public List<string> Result
        {
            get
            {
                return File;
            }
        }

        void LoadCombobox(ComboBox cmb, List<string> items) //loads a list into combobox items
        {
            cmb.Items.Clear();
            foreach (string item in items)
            {
                cmb.Items.Add(item);
            }
        }

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
                    if (!line.Contains("}"))
                    {
                        i = ReadSyntax(i + 1, file);
                        if (i == 0)
                        {
                            return null;
                        }
                    }
                }
            }

            return items;
        }

        List<List<string>> LoadSyntaxWPSecondLevel(List<string> file) //parse the items in the second level of code blocks. this will return of list of lists that contains the second level item. the order of the list is the same as the file. returns null on syntax error. this functions returns also nonblockproperties
        {
            List<List<string>> items = new List<List<string>>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];

                if (line == "")
                {
                    continue;
                }

                if(line.Contains("{") && !line.TrimEnd('\t').Contains("\t"))
                {
                    string nextline = SplitComments(file[i + 1])[0];
                    if (!nextline.Contains('=')) //check if the current block holds properties or just a bunch of lines with info
                    {
                        continue;
                    }
                    else
                    {
                        List<string> block = ExtractFirstLevelBlock(file, line.Split('=')[0].Trim('\t', ' '));
                        items.Add(LoadSyntaxFirstLevel(block).Concat(ExtractProperties(block)).ToList());
                    }
                }
            }

            return items;
        }

        List<string> ExtractFirstLevelBlock(List<string> file, string blockname) //extracts the codeblock of a given name, returns null when not found.
        {
            List<string> Block = new List<string>();

            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];

                if (line.Contains(blockname) && line.Contains("{") && !line.Contains("\t"))
                {
                    if (!line.Contains('}'))
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

                            Block.Add(line.Remove(0, 1));
                        }
                        return Block;
                    }
                    else
                    {
                        Block.Add(line.Split('=')[1].TrimStart(' ').TrimEnd(' ').Trim('{','}'));
                        return Block;
                    }
                }
            }

            return null;
        }

        List<string> ExtractProperties(List<string> file) //extracts the properties defined in the topblock of the given file
        {
            List<string> properties = new List<string>();

            for (int i = 0; i < file.Count; i++)
            { 
                string line = SplitComments(file[i])[0];

                if (line.Contains('=') && !line.Contains('{'))
                {
                    properties.Add(line.Split('=')[0].Trim(' ', '\t'));
                }
                else
                {
                    if (line.Contains('{') && !line.Contains('}'))
                    {
                        i = ReadSyntax(i + 1, file);
                    }
                }
            }

            return properties;
        }

        public SubEditor(string subjectname, string groupname, string secondlevelname, string thirdlevelname, List<string> file, string[] addstuffsecondlevel, string[] addstuffthirdlevel)
        {
            InitializeComponent();

            File = file;
            this.Text = (subjectname + " subeditor");
            txt_group.Text = groupname;
            lbl_group.Text = subjectname + ":";
            lbl_secondlevel.Text = secondlevelname + ":";
            lbl_thirdlevel.Text = thirdlevelname + ":";
            LoadCombobox(cmb_second, LoadSyntaxFirstLevel(File).Concat(ExtractProperties(file)).ToList());
            thirdlevel = LoadSyntaxWPSecondLevel(file);
            LoadCombobox(cmb_addstuff_second, addstuffsecondlevel.ToList());
            LoadCombobox(cmb_addstuff_third, addstuffthirdlevel.ToList());
            bt_edit_names.Text = "Edit " + subjectname + " groups";
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmb_second_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmb_second.Text = "";
            txt_edit.Text = "";
            if (cmb_second.SelectedIndex < thirdlevel.Count)
            {
                //there is a third level
                cmb_third.Enabled = true;
                cmb_addstuff_third.Enabled = true;
                bt_add_third.Enabled = true;
                LoadCombobox(cmb_third, thirdlevel[cmb_second.SelectedIndex]);
            }
            else
            {
                //there is no third level
                cmb_third.Enabled = false;
                cmb_addstuff_third.Enabled = false;
                bt_add_third.Enabled = false;               
                txt_edit.Text = ExtractPropertieValue(File, cmb_second.Text);
            }
        }

        string ExtractPropertieValue(List<string>file,string propertie) //finds the value that belongs to the given propertie. returns that value, returns null when not found  
        {
            if (propertie == "")
            {
                return "";
            }
            for (int i = 0; i < file.Count; i++)
            {
                string line = SplitComments(file[i])[0];

                if (!line.TrimEnd('\t').Contains("\t") && line.Contains(propertie))
                {
                    if (line.Contains("{")) //check if its a block type propertie (eg dynasty names)
                    {
                        string property = "";                       
                        for (i++; i < file.Count(); i++)
                        {
                            line = SplitComments(file[i])[0];

                            if (line.Contains("}"))
                            {
                                return property;
                            }
                            else
                            {
                                property += line.Trim('\t') + Environment.NewLine;
                            }
                        }
                    }
                    else
                    {
                        return line.Split('=')[1].Trim();
                    }
                }
            }

            return null;
        }

        private void cmb_third_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_edit.Text = "";
            List<string> secondblock = ExtractFirstLevelBlock(File, cmb_second.Text);
            List<string> thirdblock = ExtractFirstLevelBlock(secondblock, cmb_third.Text);
            if (thirdblock != null)
            {
                foreach (string line in thirdblock)
                {
                    if (txt_edit.Text == "")
                    {
                        txt_edit.Text = line;
                    }
                    else
                    {
                        txt_edit.Text = txt_edit.Text + Environment.NewLine + line;
                    }
                }
            }
            else
            {
                txt_edit.Text = ExtractPropertieValue(secondblock, cmb_third.Text);
            }
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < File.Count; i++)
            {
                string line = SplitComments(File[i])[0];
                string comment = SplitComments(File[i])[1];

                if (line.Contains(cmb_second.Text))
                {
                    if (cmb_third.Enabled == true)
                    {
                        //block is selected
                        for (int j = i + 1; File[j].TrimEnd('\t') != "}"; j++)
                        {
                            line = SplitComments(File[j])[0];
                            comment = SplitComments(File[j])[1];

                            if (line.Contains(cmb_third.Text))
                            {
                                if (line.Contains("{"))
                                {
                                    //block
                                    j++;
                                    for (int z = 0; z < txt_edit.Lines.Count(); z++)
                                    {
                                        if (File[j + z].TrimEnd('\t') != "\t}")
                                        {
                                            File[j + z] = "\t\t" + txt_edit.Lines[z];
                                        }
                                        else
                                        {
                                            File.Insert(j + z, "\t\t" + txt_edit.Lines[z]);
                                        }
                                    }
                                    return;
                                }
                                else
                                {
                                    //propertie
                                    File[j] = "\t" + cmb_third.Text + " = " + txt_edit.Lines[0];
                                    if (comment != "")
                                    {
                                        File[j] += " #" + comment;
                                    }
                                    return;
                                }
                            }
                        }
                        //function is still running so its a new propertie on the third level
                        i++;
                        if (txt_edit.Lines.Count() > 1) //check if its a block or a one value property
                        {
                            File.Insert(i, "\t" + cmb_third.Text + " = {");
                            i++;
                            for (int z = 0; z < txt_edit.Lines.Count(); z++)
                            {
                                File.Insert(i, "\t\t" + txt_edit.Lines[z]);
                                i++;
                            }
                            File.Insert(i, "\t}");
                            thirdlevel = LoadSyntaxWPSecondLevel(File);
                            return;
                        }
                        else
                        {
                            string newitem = "\t" + cmb_third.Text + " = " + txt_edit.Lines[0];
                            if (comment != "")
                            {
                                newitem += " #" + comment;
                            }
                            File.Insert(i, newitem);
                        }
                        thirdlevel = LoadSyntaxWPSecondLevel(File);
                        return;
                    }
                    else
                    {
                        //propertie is selected
                        File[i] = cmb_second.Text + " = " + txt_edit.Lines[0];
                        if (comment != "")
                        {
                            File[i] += " #" + comment;
                        }

                        return;
                    }
                }
            }
            //function is still running so adding new item on secondlevel
            if (txt_edit.Lines.Count() > 1)
            {                
                File.Add(cmb_addstuff_second.Text + " = {");
                for (int z = 0; z < txt_edit.Lines.Count(); z++)
                {
                    File.Add("\t" + txt_edit.Lines[z]);
                }
                File.Add("}");
                LoadCombobox(cmb_second, LoadSyntaxFirstLevel(File).Concat(ExtractProperties(File)).ToList());
            }
            else
            {
                File.Add(cmb_second.Text + " = " + txt_edit.Lines[0]);
            }
            return;
        }

        bool AddStuff(ComboBox cmb_add, ComboBox cmb_main) //adds stuff to specified level, returns true if succesfull and false if item is already in the list
        {
            foreach (string item in cmb_main.Items)
            {
                if (item == cmb_add.Text)
                {
                    MessageBox.Show("Item is already in file", "Error");
                    cmb_main.Text = cmb_add.Text;
                    return false;
                }
            }
            cmb_main.Items.Add(cmb_add.Text);
            cmb_main.Text = cmb_add.Text;
            txt_edit.Text = "";
            return true;
        }

        private void bt_add_third_Click(object sender, EventArgs e)
        {
            AddStuff(cmb_addstuff_third, cmb_third);
            cmb_third_SelectedIndexChanged(null, null);
        }

        private void bt_add_second_Click(object sender, EventArgs e)
        {
            AddStuff(cmb_addstuff_second, cmb_second);            
            cmb_second_SelectedIndexChanged(null, null);
        }

        private void bt_delete_Click(object sender, EventArgs e)
        {
            
        }

    }
}