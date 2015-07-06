using GlacialComponents.Controls;
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
    public partial class SelectPartnerForm : ThemedForms
    {
        public SelectPartnerForm(ToolStripMenuItem SelectPartnerSender) //paraméter: 2014.09.06.
        {
            InitializeComponent();
            this.Text = SelectPartnerSender.Text; //2014.02.28.
            titleText.Text = SelectPartnerSender.Text;

            cancelbtn.Text = Language.Translate(Language.StringID.Button_Cancel);

            partnerList.Items.Clear();
            for (int x = 0; x < UserInfo.KnownUsers.Count; x++) //Partners
            {
                if (!UserInfo.KnownUsers[x].IsPartner)
                    continue;
                try
                {
                    partnerList.Items.Add(UserInfo.KnownUsers[x].UserName);
                }
                catch (NullReferenceException)
                {
                    break;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    break;
                }
            }
            partnerList.Columns[0].Width = partnerList.ClientSize.Width - 5;
        }

        private void FormResizeFinish(object sender, EventArgs e)
        {
            partnerList.Columns[0].Width = partnerList.ClientSize.Width - 5;
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < partnerList.Items.Count; i++)
            {
                if (partnerList.Items[i].SubItems[0].Checked)
                {
                    int p = selectedPartners.Text.IndexOf(partnerList.Items[i].SubItems[0].Text + ";");
                    if (p == -1)
                    {
                        selectedPartners.Text += partnerList.Items[i].SubItems[0].Text + ";";
                    }
                }
                else
                {
                    int p = selectedPartners.Text.IndexOf(partnerList.Items[i].SubItems[0].Text + ";");
                    if (p != -1) //Eltávolitja a nem kiválasztott partner előfordulását a listából - Megcsinálom visszafelé is
                        selectedPartners.Text = selectedPartners.Text.Remove(p, partnerList.Items[i].SubItems[0].Text.Length + 1); //Eltávolitja a ;-t is
                }
            }
        }

        private void ItemDoubleClick(object sender, EventArgs e)
        {
            int a = partnerList.HotItemIndex;
            if (partnerList.Items[a].SubItems[0].Checked)
                partnerList.Items[a].SubItems[0].Checked = false;
            else
                partnerList.Items[a].SubItems[0].Checked = true;
            ItemClicked(sender, e);
        }

        public string[] Partners;
        private void okbtn_Click(object sender, EventArgs e)
        {
            while (selectedPartners.Text.Contains(' '))
            { //Eltávolitja a szóközöket
                int x = selectedPartners.Text.IndexOf(' ');
                selectedPartners.Text.Remove(x, 1);
            }
            Partners = selectedPartners.Text.Split(';');
        }
    }
}
