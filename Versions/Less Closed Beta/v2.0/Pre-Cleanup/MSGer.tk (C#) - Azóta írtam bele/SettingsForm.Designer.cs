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
            GlacialComponents.Controls.GLColumn glColumn2 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLItem glItem2 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem2 = new GlacialComponents.Controls.GLSubItem();
            this.glacialList1 = new GlacialComponents.Controls.GlacialList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.personal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.messageText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            glColumn2.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn2.CheckBoxes = false;
            glColumn2.ImageIndex = -1;
            glColumn2.Name = "Column1";
            glColumn2.NumericSort = false;
            glColumn2.Text = "Column";
            glColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn2.Width = 115;
            this.glacialList1.Columns.AddRange(new GlacialComponents.Controls.GLColumn[] {
            glColumn2});
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
            glSubItem2.Text = "Személyes";
            glItem2.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem2});
            glItem2.Text = "Személyes";
            this.glacialList1.Items.AddRange(new GlacialComponents.Controls.GLItem[] {
            glItem2});
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Név";
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Location = new System.Drawing.Point(12, 52);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(354, 20);
            this.nameText.TabIndex = 2;
            // 
            // messageText
            // 
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageText.Location = new System.Drawing.Point(12, 99);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(354, 20);
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
    }
}