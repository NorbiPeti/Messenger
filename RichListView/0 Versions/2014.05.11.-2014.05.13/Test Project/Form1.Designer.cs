namespace Test_Project
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SzNPProjects.RichListViewColumn richListViewColumn1 = new SzNPProjects.RichListViewColumn();
            SzNPProjects.RichListViewColumn richListViewColumn2 = new SzNPProjects.RichListViewColumn();
            this.richListView1 = new SzNPProjects.RichListView();
            this.SuspendLayout();
            // 
            // richListView1
            // 
            this.richListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richListView1.AutoScroll = true;
            this.richListView1.BackColor = System.Drawing.Color.Black;
            this.richListView1.ColumnAutoFill = true;
            richListViewColumn1.Text = "Na most már elvileg működik";
            richListViewColumn1.Width = 100;
            richListViewColumn2.Text = "Újabb oszlop";
            richListViewColumn2.Width = 100;
            this.richListView1.Columns = new SzNPProjects.RichListViewColumn[] {
        richListViewColumn1,
        richListViewColumn2};
            this.richListView1.ForeColor = System.Drawing.Color.Aqua;
            this.richListView1.HeaderHeight = 0;
            this.richListView1.ItemHeight = 50;
            this.richListView1.Location = new System.Drawing.Point(12, 12);
            this.richListView1.Name = "richListView1";
            this.richListView1.Size = new System.Drawing.Size(762, 415);
            this.richListView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(786, 439);
            this.Controls.Add(this.richListView1);
            this.ForeColor = System.Drawing.Color.Aqua;
            this.Name = "Form1";
            this.Text = "Test Form";
            this.ResumeLayout(false);

        }

        #endregion

        private SzNPProjects.RichListView richListView1;



    }
}

