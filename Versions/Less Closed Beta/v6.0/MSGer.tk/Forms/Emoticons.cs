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
    public partial class Emoticons : Form
    { //2015.06.06.
        public Emoticons()
        {
            InitializeComponent();
            label1.Text = Language.Translate(Language.StringID.Emoticons); //TODO: A ...-t a menüknél mindig adja hozzá
            this.Text = label1.Text;
            addbtn.Text = Language.Translate(Language.StringID.Add); //TODO: A teljesen megegyező string-eket jelezze
            removebtn.Text = Language.Translate(Language.StringID.Remove);
            closebtn.Text = Language.Translate(Language.StringID.Close);
            //cancelbtn.Text = Language.Translate(Language.StringID.Button_Cancel);
            label2.Text = Language.Translate(Language.StringID.SelectImage);
            button1.Text = Language.Translate(Language.StringID.Modify);
            label3.Text = Language.Translate(Language.StringID.Text);
            domainUpDown1.Items.Clear();
            domainUpDown1.Items.AddRange(TextFormat.TextFormats);
            if (domainUpDown1.Items.Count == 0)
                domainUpDown1.SelectedIndex = -1;
            else
                domainUpDown1.SelectedIndex = 0;
            SelectedTextFormat = domainUpDown1.SelectedIndex;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            SelectedTextFormat = domainUpDown1.SelectedIndex;
        }

        //private void SelectTextFormat(int index)
        private int selectedtextformat;
        private int SelectedTextFormat
        {
            get
            {
                return selectedtextformat;
            }
            set
            {
                selectedtextformat = value;
                if (value == -1)
                    return;
                flowLayoutPanel1.Controls.Clear();
                SelectedEmoticon = 0;
                if (TextFormat.TextFormats.Count > 0)
                    //flowLayoutPanel1.Controls.AddRange(TextFormat.TextFormats[value].Emoticons.Select(entry => new PictureBox { Image = entry.Frames[0].Clone() as Image }).ToArray());
                    flowLayoutPanel1.Controls.AddRange(TextFormat.TextFormats[value].Emoticons.Select(entry => new PictureBox { Image = entry.Clone() as Image }).ToArray());
                int i = 0;
                foreach (PictureBox pictbox in flowLayoutPanel1.Controls)
                {
                    pictbox.Click += Emoticons_Click;
                    pictbox.Tag = i;
                    pictbox.Size = pictbox.Image.Size;
                    i++;
                }
            }
        }

        void Emoticons_Click(object sender, EventArgs e)
        {
            SelectedEmoticon = (int)(sender as Control).Tag;
        }

        //private void SelectEmoticon(int index)
        private int selectedemoticon;
        private int SelectedEmoticon
        {
            get
            {
                return selectedemoticon;
            }
            set
            {
                SaveEmoticonIfChanged();
                if (value == selectedemoticon)
                    return;
                if (value >= flowLayoutPanel1.Controls.Count)
                {
                    pictureBox1.Image = null;
                    initialtext = "";
                    textBox1.Text = "";
                }
                else
                {
                    pictureBox1.Image = (flowLayoutPanel1.Controls[value] as PictureBox).Image;
                    initialtext = TextFormat.TextFormats[SelectedTextFormat].Emoticons[value].Value;
                }
                textBox1.Text = initialtext;
                selectedemoticon = value;
            }
        }

        private bool imagechanged = false;
        private void SaveEmoticonIfChanged()
        {
            bool save = false;
            if (pictureBox1.Image == null || textBox1.Text == "")
                return;
            if (SelectedEmoticon >= flowLayoutPanel1.Controls.Count)
            {
                flowLayoutPanel1.Controls.Add(new PictureBox { Tag = flowLayoutPanel1.Controls.Count, Image = pictureBox1.Image.Clone() as Image, Size = pictureBox1.Image.Size });
                flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1].Click += Emoticons_Click;
                if (SelectedTextFormat == -1)
                {
                    TextFormat.TextFormats.Add(PackManager.Add<TextFormat>(domainUpDown1.Text + ".npack"));
                    selectedtextformat = TextFormat.TextFormats.Count - 1;
                    save = true;
                }
                var emoticon = new Emoticon(textBox1.Text);
                //emoticon.Frames.Add(new Bitmap(pictureBox1.Image));
                emoticon.Image = new Bitmap(pictureBox1.Image); //2015.07.05.
                TextFormat.TextFormats[SelectedTextFormat].Emoticons.Add(emoticon);
            }
            else if (textBox1.Text != initialtext || imagechanged)
            {
                TextFormat.TextFormats[SelectedTextFormat].Emoticons[SelectedEmoticon].Value = textBox1.Text;
                //TextFormat.TextFormats[SelectedTextFormat].Emoticons[SelectedEmoticon].Frames.Clear();
                TextFormat.TextFormats[SelectedTextFormat].Emoticons[SelectedEmoticon].Image.Dispose(); //2015.07.05.
                //TextFormat.TextFormats[SelectedTextFormat].Emoticons[SelectedEmoticon].Frames.Add(new Bitmap(pictureBox1.Image));
                TextFormat.TextFormats[SelectedTextFormat].Emoticons[SelectedEmoticon].Image = new Bitmap(pictureBox1.Image); //2015.07.05.
                initialtext = textBox1.Text;
                imagechanged = false;
                (flowLayoutPanel1.Controls[SelectedEmoticon] as PictureBox).Image = pictureBox1.Image;
                save = true;
            }

            if (save)
                PackManager.Save<TextFormat>(TextFormat.TextFormats[SelectedTextFormat]);
        }

        private string initialtext;
        private void domainUpDown1_TextChanged(object sender, EventArgs e)
        {
            if (domainUpDown1.SelectedIndex == -1)
            {
                flowLayoutPanel1.Controls.Clear();
                TextFormat tf = domainUpDown1.Items.ToArray().Cast<TextFormat>().FirstOrDefault(entry => entry.ToString() == domainUpDown1.Text);
                if (tf != default(TextFormat))
                    domainUpDown1.SelectedItem = tf;
            }
            SelectedEmoticon = 0;
            imagechanged = false;
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            /*if (SelectedEmoticon == flowLayoutPanel1.Controls.Count)
                SaveEmoticonIfChanged();*/
            SelectedEmoticon = flowLayoutPanel1.Controls.Count;
        }

        private void removebtn_Click(object sender, EventArgs e)
        {
            if (SelectedTextFormat == -1 || SelectedEmoticon > TextFormat.TextFormats[SelectedTextFormat].Emoticons.Count)
                return;
            if (MessageBox.Show(Language.Translate(Language.StringID.AreYouSure), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TextFormat.TextFormats[SelectedTextFormat].Emoticons.RemoveAt(SelectedEmoticon);
                flowLayoutPanel1.Controls.RemoveAt(SelectedEmoticon); //TO!DO: TextFormat törlés
                if (TextFormat.TextFormats[SelectedTextFormat].Emoticons.Count == 0)
                {
                    PackManager.Remove<TextFormat>(TextFormat.TextFormats[SelectedTextFormat]); //2015.06.14.
                    TextFormat.TextFormats.RemoveAt(SelectedTextFormat); //2015.06.14.
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Program.LoadImageFromFile(openFileDialog1.FileName);
                imagechanged = true;
                SaveEmoticonIfChanged();
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Emoticons_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveEmoticonIfChanged();
            Program.MainF.contactList.Items.Clear();
            Program.MainF.LoadPartnerList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SaveEmoticonIfChanged();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            SaveEmoticonIfChanged();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SaveEmoticonIfChanged();
        }
    }
}
