namespace MSGer.tk
{
    partial class SettingsForm
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
            GlacialComponents.Controls.GLColumn glColumn1 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLItem glItem1 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem1 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLItem glItem2 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem2 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLItem glItem3 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem3 = new GlacialComponents.Controls.GLSubItem();
            this.glacialList1 = new GlacialComponents.Controls.GlacialList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.isserver = new System.Windows.Forms.CheckBox();
            this.technical = new System.Windows.Forms.Label();
            this.chatwindowTabs = new System.Windows.Forms.CheckBox();
            this.chatwindow = new System.Windows.Forms.CheckBox();
            this.layout = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.messageText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.personal = new System.Windows.Forms.Label();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glacialList1
            // 
            this.glacialList1.AllowColumnResize = true;
            this.glacialList1.AllowMultiselect = false;
            this.glacialList1.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glacialList1.AlternatingColors = false;
            this.glacialList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.glacialList1.AutoHeight = true;
            this.glacialList1.BackColor = System.Drawing.Color.White;
            this.glacialList1.BackgroundStretchToFit = true;
            glColumn1.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn1.CheckBoxes = false;
            glColumn1.ImageIndex = -1;
            glColumn1.Name = "Column1";
            glColumn1.NumericSort = false;
            glColumn1.Text = "Column";
            glColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn1.Width = 115;
            this.glacialList1.Columns.AddRange(new GlacialComponents.Controls.GLColumn[] {
            glColumn1});
            this.glacialList1.ControlStyle = GlacialComponents.Controls.GLControlStyles.Normal;
            this.glacialList1.ForeColor = System.Drawing.Color.Black;
            this.glacialList1.FullRowSelect = true;
            this.glacialList1.GridColor = System.Drawing.Color.LightGray;
            this.glacialList1.GridLines = GlacialComponents.Controls.GLGridLines.gridBoth;
            this.glacialList1.GridLineStyle = GlacialComponents.Controls.GLGridLineStyles.gridSolid;
            this.glacialList1.GridTypes = GlacialComponents.Controls.GLGridTypes.gridOnExists;
            this.glacialList1.HeaderHeight = 0;
            this.glacialList1.HeaderVisible = false;
            this.glacialList1.HeaderWordWrap = false;
            this.glacialList1.HotColumnTracking = false;
            this.glacialList1.HotItemTracking = true;
            this.glacialList1.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glacialList1.HoverEvents = false;
            this.glacialList1.HoverTime = 1;
            this.glacialList1.ImageList = null;
            this.glacialList1.ItemHeight = 17;
            glItem1.BackColor = System.Drawing.Color.White;
            glItem1.ForeColor = System.Drawing.Color.Black;
            glItem1.RowBorderColor = System.Drawing.Color.Black;
            glItem1.RowBorderSize = 0;
            glSubItem1.BackColor = System.Drawing.Color.Empty;
            glSubItem1.Checked = false;
            glSubItem1.ForceText = false;
            glSubItem1.ForeColor = System.Drawing.Color.Black;
            glSubItem1.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem1.ImageIndex = -1;
            glSubItem1.Text = "Személyes";
            glItem1.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem1});
            glItem1.Text = "Személyes";
            glItem2.BackColor = System.Drawing.Color.White;
            glItem2.ForeColor = System.Drawing.Color.Black;
            glItem2.RowBorderColor = System.Drawing.Color.Black;
            glItem2.RowBorderSize = 0;
            glSubItem2.BackColor = System.Drawing.Color.Empty;
            glSubItem2.Checked = false;
            glSubItem2.ForceText = false;
            glSubItem2.ForeColor = System.Drawing.Color.Black;
            glSubItem2.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem2.ImageIndex = -1;
            glSubItem2.Text = "Kinézet";
            glItem2.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem2});
            glItem2.Text = "Kinézet";
            glItem3.BackColor = System.Drawing.Color.White;
            glItem3.ForeColor = System.Drawing.Color.Black;
            glItem3.RowBorderColor = System.Drawing.Color.Black;
            glItem3.RowBorderSize = 0;
            glSubItem3.BackColor = System.Drawing.Color.Empty;
            glSubItem3.Checked = false;
            glSubItem3.ForceText = false;
            glSubItem3.ForeColor = System.Drawing.Color.Black;
            glSubItem3.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem3.ImageIndex = -1;
            glSubItem3.Text = "Technikai";
            glItem3.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem3});
            glItem3.Text = "Technikai";
            this.glacialList1.Items.AddRange(new GlacialComponents.Controls.GLItem[] {
            glItem1,
            glItem2,
            glItem3});
            this.glacialList1.ItemWordWrap = false;
            this.glacialList1.Location = new System.Drawing.Point(12, 12);
            this.glacialList1.Name = "glacialList1";
            this.glacialList1.Selectable = true;
            this.glacialList1.SelectedTextColor = System.Drawing.Color.White;
            this.glacialList1.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glacialList1.ShowBorder = true;
            this.glacialList1.ShowFocusRect = false;
            this.glacialList1.Size = new System.Drawing.Size(120, 418);
            this.glacialList1.SortType = GlacialComponents.Controls.SortTypes.InsertionSort;
            this.glacialList1.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glacialList1.TabIndex = 0;
            this.glacialList1.Text = "glacialList1";
            this.glacialList1.Click += new System.EventHandler(this.glacialList1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.isserver);
            this.panel1.Controls.Add(this.technical);
            this.panel1.Controls.Add(this.chatwindowTabs);
            this.panel1.Controls.Add(this.chatwindow);
            this.panel1.Controls.Add(this.layout);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.messageText);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nameText);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.personal);
            this.panel1.Location = new System.Drawing.Point(139, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 391);
            this.panel1.TabIndex = 1;
            // 
            // isserver
            // 
            this.isserver.AutoSize = true;
            this.isserver.Location = new System.Drawing.Point(12, 405);
            this.isserver.Name = "isserver";
            this.isserver.Size = new System.Drawing.Size(217, 17);
            this.isserver.TabIndex = 11;
            this.isserver.Text = "Szerver mód (port forwarding szükséges)";
            this.isserver.UseVisualStyleBackColor = true;
            // 
            // technical
            // 
            this.technical.AutoSize = true;
            this.technical.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.technical.Location = new System.Drawing.Point(6, 371);
            this.technical.Name = "technical";
            this.technical.Size = new System.Drawing.Size(131, 31);
            this.technical.TabIndex = 10;
            this.technical.Text = "Technikai";
            // 
            // chatwindowTabs
            // 
            this.chatwindowTabs.AutoSize = true;
            this.chatwindowTabs.Enabled = false;
            this.chatwindowTabs.Location = new System.Drawing.Point(50, 329);
            this.chatwindowTabs.Name = "chatwindowTabs";
            this.chatwindowTabs.Size = new System.Drawing.Size(194, 17);
            this.chatwindowTabs.TabIndex = 9;
            this.chatwindowTabs.Text = "A beszélgetések fülekbe rendezése";
            this.chatwindowTabs.UseVisualStyleBackColor = true;
            // 
            // chatwindow
            // 
            this.chatwindow.AutoSize = true;
            this.chatwindow.Location = new System.Drawing.Point(12, 305);
            this.chatwindow.Name = "chatwindow";
            this.chatwindow.Size = new System.Drawing.Size(272, 17);
            this.chatwindow.TabIndex = 8;
            this.chatwindow.Text = "A beszélgetések jelenjenek meg külön ablak(ok)ban";
            this.chatwindow.UseVisualStyleBackColor = true;
            // 
            // layout
            // 
            this.layout.AutoSize = true;
            this.layout.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.layout.Location = new System.Drawing.Point(6, 270);
            this.layout.Name = "layout";
            this.layout.Size = new System.Drawing.Size(105, 31);
            this.layout.TabIndex = 7;
            this.layout.Text = "Kinézet";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.LabelWrap = false;
            this.listView1.Location = new System.Drawing.Point(12, 148);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(121, 97);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Nyelv";
            // 
            // messageText
            // 
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageText.Location = new System.Drawing.Point(12, 99);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(337, 20);
            this.messageText.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Üzenet";
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Location = new System.Drawing.Point(12, 52);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(337, 20);
            this.nameText.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Név";
            // 
            // personal
            // 
            this.personal.AutoSize = true;
            this.personal.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.personal.Location = new System.Drawing.Point(3, 0);
            this.personal.Name = "personal";
            this.personal.Size = new System.Drawing.Size(147, 31);
            this.personal.TabIndex = 0;
            this.personal.Text = "Személyes";
            // 
            // okbtn
            // 
            this.okbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbtn.Location = new System.Drawing.Point(368, 410);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 2;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // cancelbtn
            // 
            this.cancelbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(449, 410);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 3;
            this.cancelbtn.Text = "Mégse";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 442);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.glacialList1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GlacialComponents.Controls.GlacialList glacialList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label personal;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label layout;
        private System.Windows.Forms.CheckBox chatwindowTabs;
        private System.Windows.Forms.CheckBox chatwindow;
        private System.Windows.Forms.Label technical;
        private System.Windows.Forms.CheckBox isserver;
    }
}