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
            this.hidebtn = new System.Windows.Forms.Button();
            this.showbtn = new System.Windows.Forms.Button();
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
            this.richListView1.HeaderHeight = 20;
            this.richListView1.ItemHeight = 50;
            this.richListView1.Location = new System.Drawing.Point(12, 12);
            this.richListView1.Name = "richListView1";
            this.richListView1.Size = new System.Drawing.Size(762, 381);
            this.richListView1.TabIndex = 0;
            this.richListView1.ItemClicked += new System.EventHandler<int>(this.richListView1_ItemClicked);
            this.richListView1.ItemDoubleClicked += new System.EventHandler<int>(this.richListView1_ItemDoubleClicked);
            // 
            // hidebtn
            // 
            this.hidebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hidebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hidebtn.Location = new System.Drawing.Point(305, 404);
            this.hidebtn.Name = "hidebtn";
            this.hidebtn.Size = new System.Drawing.Size(75, 23);
            this.hidebtn.TabIndex = 1;
            this.hidebtn.Text = "Hide";
            this.hidebtn.UseVisualStyleBackColor = true;
            this.hidebtn.Click += new System.EventHandler(this.hidebtn_Click);
            // 
            // showbtn
            // 
            this.showbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showbtn.Location = new System.Drawing.Point(386, 404);
            this.showbtn.Name = "showbtn";
            this.showbtn.Size = new System.Drawing.Size(75, 23);
            this.showbtn.TabIndex = 2;
            this.showbtn.Text = "Show";
            this.showbtn.UseVisualStyleBackColor = true;
            this.showbtn.Click += new System.EventHandler(this.showbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(786, 439);
            this.Controls.Add(this.showbtn);
            this.Controls.Add(this.hidebtn);
            this.Controls.Add(this.richListView1);
            this.ForeColor = System.Drawing.Color.Aqua;
            this.Name = "Form1";
            this.Text = "Test Form";
            this.ResumeLayout(false);

        }

        #endregion

        private SzNPProjects.RichListView richListView1;
        private System.Windows.Forms.Button hidebtn;
        private System.Windows.Forms.Button showbtn;



    }
}

