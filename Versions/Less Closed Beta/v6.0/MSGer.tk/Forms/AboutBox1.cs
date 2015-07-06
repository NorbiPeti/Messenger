using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    partial class AboutBox1 : ThemedForms
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format(Language.Translate(Language.StringID.About), AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format(Language.Translate(Language.StringID.About_Version), AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;

            labelLicenseLink.Text = "https://www.gnu.org/copyleft/gpl.html"; //2014.04.18. - Frissitve: 2014.04.25.
            List<string> desc = new List<string>(); //2014.04.18.
            desc.Add(Language.Translate(Language.StringID.About_Programmer)); //2014.04.18.
            desc.Add("SzNP");
            desc.Add("http://sznp.tk");
            desc.Add("");
            desc.Add(Language.Translate(Language.StringID.About_SpecialThanks));
            desc.Add("Jonathan Kay");
            desc.Add("http://messengergeek.com");
            desc.Add(Language.Translate(Language.StringID.About_SpecThanks1));
            desc.Add("");
            desc.Add(Language.Translate(Language.StringID.About_SpecThanks2));
            desc.Add("");
            desc.Add("Allen Anderson");
            desc.Add("http://www.codeproject.com/Articles/4012/C-List-View-v");
            desc.Add(Language.Translate(Language.StringID.About_SpecThanks3));
            desc.Add("");
            desc.Add("Khendys Gordon");
            desc.Add("http://www.codeproject.com/Articles/4544/Insert-Plain-Text-and-Images-into-RichTextBox-at-R");
            desc.Add(Language.Translate(Language.StringID.About_SpecThanks4));
            textBoxDescription.Lines = desc.ToArray();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void labelLicenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(labelLicenseLink.Text);
        }
    }
}
