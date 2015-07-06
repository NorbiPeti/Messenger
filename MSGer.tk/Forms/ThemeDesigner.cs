using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public partial class ThemeDesigner : Form
    { //2015.05.23.
        private bool Stop = false; //2015.07.03.
        public ThemeDesigner()
        {
            InitializeComponent();
            if (ThemedForms.ThemeDesignerRunning != null)
            { //2015.07.03.
                ThemedForms.ThemeDesignerRunning.Focus();
                Stop = true;
                return;
            }
            domainUpDown1.Items.Clear(); //2015.07.03.
            domainUpDown1.Items.AddRange(Theme.Themes); //2015.07.03.
            if (domainUpDown1.Items.Count > 0)
                domainUpDown1.SelectedIndex = 0; //2015.07.03.
            if (domainUpDown1.SelectedItem != null) //2015.07.03.
                SelectedTheme = (Theme)domainUpDown1.SelectedItem;
            SelectedType = null; //2015.07.03. - Reset
        }

        public Theme SelectedTheme { get; set; } //2015.07.03.
        private Type selectedtype; //2015.07.03.
        public Type SelectedType
        { //2015.07.03.
            get
            {
                return selectedtype;
            }
            set
            {
                if (SelectedTheme == null)
                {
                    SelectedTheme = PackManager.Add<Theme>(domainUpDown1.Text);
                }
                if (value == null)
                {
                    colorRadiobtn.Enabled = false;
                    panel1.Enabled = false;
                    panel1.BackColor = Color.Black;
                    imageRadiobtn.Enabled = false;
                    pictureBox1.Enabled = false;
                    pictureBox1.Image = null;
                    scriptRadiobtn.Enabled = false;
                    label2.Enabled = false;
                    scriptLocation.Enabled = false;
                    scriptLocation.Text = "";
                }
                else
                {
                    if (!SelectedTheme.Controls.ContainsKey(value))
                    {
                        SelectedTheme.Controls.Add(value, new ThemeControl(Color.Black, Color.Black)); //2015.07.05.
                    }
                    colorRadiobtn.Enabled = true; //2015.07.05.
                    imageRadiobtn.Enabled = true; //2015.07.05.
                    scriptRadiobtn.Enabled = true; //2015.07.05.
                    SetThemeControl(SelectedTheme.Controls[value].ControlType, value); //2015.07.04.
                }
                selectedtype = value;
            }
        }
        private void ThemeDesigner_Load(object sender, EventArgs e)
        {
            if (Stop) //2015.07.03.
                this.Close();
            ThemedForms.ThemeDesignerRunning = this; //2015.07.03.
        }

        private bool InternalCheck = false; //2015.07.05.
        private void SetThemeControl(ThemeControlTypes controltype, Type selectedtype = null) //<-- 2015.07.04. - Áthelyezve ide a SelectedType.set()-ből
        {
            InternalCheck = true; //2015.07.05.
            if (selectedtype == null)
                selectedtype = SelectedType; //2015.07.04.
            switch (controltype)
            {
                case ThemeControlTypes.Colors:
                    colorRadiobtn.Checked = true; //2015.07.05.
                    panel1.Enabled = true;
                    panel1.BackColor = SelectedTheme.Controls[selectedtype].Color; //2015.07.05.
                    pictureBox1.Enabled = false;
                    pictureBox1.Image = null;
                    label2.Enabled = false;
                    scriptLocation.Enabled = false;
                    scriptLocation.Text = "";
                    break;
                case ThemeControlTypes.Dynamic:
                    panel1.Enabled = false;
                    panel1.BackColor = Color.Black;
                    pictureBox1.Enabled = false;
                    pictureBox1.Image = null;
                    scriptRadiobtn.Checked = true; //2015.07.05.
                    label2.Enabled = true;
                    scriptLocation.Enabled = true;
                    scriptLocation.Text = SelectedTheme.Controls[selectedtype].Script;
                    break;
                case ThemeControlTypes.Image:
                    panel1.Enabled = false;
                    panel1.BackColor = Color.Black;
                    imageRadiobtn.Checked = true; //2015.07.05.
                    pictureBox1.Enabled = true;
                    pictureBox1.Image = SelectedTheme.Controls[selectedtype].Image;
                    label2.Enabled = false;
                    scriptLocation.Enabled = false;
                    scriptLocation.Text = "";
                    break;
            }
            InternalCheck = false; //2015.07.05.
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            //TO!DO
            Language.ReloadLangs(); //2015.07.05. - Lényegében csak az ablakokat frissíti
            PackManager.Save<Theme>(SelectedTheme); //2015.07.03.
            this.Close(); //2015.05.24.
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close(); //2015.05.24.
        }

        private void ThemeDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThemedForms.ThemeDesignerRunning == this)
                ThemedForms.ThemeDesignerRunning = null; //2015.07.03.
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel1.BackColor; //2015.07.03.
            if (colorDialog1.ShowDialog() == DialogResult.OK) //2015.07.05.
                panel1.BackColor = colorDialog1.Color; //2015.07.05.
        }

        private void domainUpDown1_TextChanged(object sender, EventArgs e)
        { //2015.07.03.
            if (domainUpDown1.SelectedIndex == -1)
            {
                SelectedType = null;
                SelectedTheme = null;
                Theme theme = domainUpDown1.Items.ToArray().Cast<Theme>().FirstOrDefault(entry => entry.ToString() == domainUpDown1.Text);
                if (theme != default(Theme))
                    domainUpDown1.SelectedItem = theme;
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            SelectedTheme = (Theme)domainUpDown1.SelectedItem; //2015.07.03.
        }

        private void colorRadiobtn_CheckedChanged(object sender, EventArgs e)
        { //2015.07.04.
            if (!InternalCheck) //<-- 2015.07.05.
                SetThemeControl(ThemeControlTypes.Colors);
        }

        private void imageRadiobtn_CheckedChanged(object sender, EventArgs e)
        { //2015.07.04.
            if (!InternalCheck) //<-- 2015.07.05.
                SetThemeControl(ThemeControlTypes.Image);
        }

        private void scriptRadiobtn_CheckedChanged(object sender, EventArgs e)
        { //2015.07.04.
            if (!InternalCheck) //<-- 2015.07.05.
                SetThemeControl(ThemeControlTypes.Dynamic);
        }
    }
}
