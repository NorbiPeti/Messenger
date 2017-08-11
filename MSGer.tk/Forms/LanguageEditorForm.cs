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
    public partial class LanguageEditorForm : Form
    {
        public LanguageEditorForm()
        {
            InitializeComponent();
            this.Text = Language.Translate(Language.StringID.LanguageEditor);
            label1.Text = this.Text;
            domainUpDown1.Items.AddRange(Language.Languages);
            listView1.Items.AddRange(Language.CurrentLanguage.Strings.Select(entry => new ListViewItem(new string[] { entry.Key, entry.Value })).ToArray());
            cancelbtn.Text = Language.Translate(Language.StringID.Button_Cancel);
            domainUpDown1.SelectedItemChanged += DomainUpDown1_SelectedItemChanged;
            domainUpDown1.TextChanged += DomainUpDown1_TextChanged;
        }

        private void DomainUpDown1_TextChanged(object sender, EventArgs e)
        {
            var selectedlang = domainUpDown1.Items.OfType<Language>().FirstOrDefault(entry => entry.ToString() == domainUpDown1.Text);
            if (selectedlang != null)
                domainUpDown1.SelectedItem = selectedlang;
        }

        private void DomainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void domainUpDown1_Leave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
