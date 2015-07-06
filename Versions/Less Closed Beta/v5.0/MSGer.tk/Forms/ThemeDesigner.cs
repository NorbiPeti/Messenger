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
        public ThemeDesigner()
        {
            InitializeComponent();
        }

        private void ThemeDesigner_Load(object sender, EventArgs e)
        {
            SetWindow<MainForm>();
        }

        //private void SetWindow<T>() where T : ThemedForms
        private void SetWindow<T>() where T : Control
        {
            T form = Activator.CreateInstance<T>();
            //form.ThemedForms_Load(null, null);
            //designerPanel.Controls.AddRange(form.GetOriginalControls().Cast<Control>().ToArray());
            Control[] controls;
            if (!form.GetType().IsSubclassOf(typeof(ThemedForms))) //2015.05.24.
                controls = form.Controls.Cast<Control>().ToArray(); //2015.05.24.
            else //2015.05.24.
                controls = ((ThemedForms)(object)form).Controls.Cast<Control>().ToArray(); //2015.05.24.
            designerPanel.Controls.AddRange(controls); //2015.05.24.
            foreach (Control control in designerPanel.Controls)
                control.Enabled = false; //2015.05.24.
            form.Controls.Clear(); //2015.05.24. - Őrizze meg a control-okat
            form.Dispose();
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            //TODO
            this.Close(); //2015.05.24.
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close(); //2015.05.24.
        }
    }
}
