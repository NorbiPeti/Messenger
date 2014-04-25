namespace MSGer.tk
{
    partial class AddPartner
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.searchbtn = new System.Windows.Forms.Button();
            this.glacialList1 = new GlacialComponents.Controls.GlacialList();
            this.gobtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ismerős felvétele";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(18, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Név/E-mail/Felhasználónév";
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Location = new System.Drawing.Point(18, 106);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(270, 20);
            this.nameText.TabIndex = 2;
            // 
            // searchbtn
            // 
            this.searchbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchbtn.Location = new System.Drawing.Point(297, 104);
            this.searchbtn.Name = "searchbtn";
            this.searchbtn.Size = new System.Drawing.Size(75, 23);
            this.searchbtn.TabIndex = 3;
            this.searchbtn.Text = "Keresés";
            this.searchbtn.UseVisualStyleBackColor = true;
            this.searchbtn.Click += new System.EventHandler(this.searchbtn_Click);
            // 
            // glacialList1
            // 
            this.glacialList1.AllowColumnResize = true;
            this.glacialList1.AllowMultiselect = false;
            this.glacialList1.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glacialList1.AlternatingColors = false;
            this.glacialList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glacialList1.AutoHeight = true;
            this.glacialList1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.glacialList1.BackgroundStretchToFit = true;
            glColumn1.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn1.CheckBoxes = false;
            glColumn1.ImageIndex = -1;
            glColumn1.Name = "UserName";
            glColumn1.NumericSort = false;
            glColumn1.Text = "Felhasználónév";
            glColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            glColumn1.Width = 200;
            this.glacialList1.Columns.AddRange(new GlacialComponents.Controls.GLColumn[] {
            glColumn1});
            this.glacialList1.ControlStyle = GlacialComponents.Controls.GLControlStyles.Normal;
            this.glacialList1.FullRowSelect = true;
            this.glacialList1.GridColor = System.Drawing.Color.LightGray;
            this.glacialList1.GridLines = GlacialComponents.Controls.GLGridLines.gridBoth;
            this.glacialList1.GridLineStyle = GlacialComponents.Controls.GLGridLineStyles.gridSolid;
            this.glacialList1.GridTypes = GlacialComponents.Controls.GLGridTypes.gridOnExists;
            this.glacialList1.HeaderHeight = 22;
            this.glacialList1.HeaderVisible = true;
            this.glacialList1.HeaderWordWrap = false;
            this.glacialList1.HotColumnTracking = false;
            this.glacialList1.HotItemTracking = true;
            this.glacialList1.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glacialList1.HoverEvents = false;
            this.glacialList1.HoverTime = 1;
            this.glacialList1.ImageList = null;
            this.glacialList1.ItemHeight = 17;
            this.glacialList1.ItemWordWrap = false;
            this.glacialList1.Location = new System.Drawing.Point(18, 152);
            this.glacialList1.Name = "glacialList1";
            this.glacialList1.Selectable = true;
            this.glacialList1.SelectedTextColor = System.Drawing.Color.White;
            this.glacialList1.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glacialList1.ShowBorder = true;
            this.glacialList1.ShowFocusRect = false;
            this.glacialList1.Size = new System.Drawing.Size(354, 272);
            this.glacialList1.SortType = GlacialComponents.Controls.SortTypes.InsertionSort;
            this.glacialList1.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glacialList1.TabIndex = 4;
            this.glacialList1.Text = "glacialList1";
            this.glacialList1.Click += new System.EventHandler(this.glacialList1_Click);
            // 
            // gobtn
            // 
            this.gobtn.Location = new System.Drawing.Point(297, 427);
            this.gobtn.Name = "gobtn";
            this.gobtn.Size = new System.Drawing.Size(75, 23);
            this.gobtn.TabIndex = 5;
            this.gobtn.Text = "Felvétel";
            this.gobtn.UseVisualStyleBackColor = true;
            this.gobtn.Click += new System.EventHandler(this.gobtn_Click);
            // 
            // AddPartner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.gobtn);
            this.Controls.Add(this.glacialList1);
            this.Controls.Add(this.searchbtn);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddPartner";
            this.Text = "Ismerős felvétele";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Button searchbtn;
        private GlacialComponents.Controls.GlacialList glacialList1;
        private System.Windows.Forms.Button gobtn;
    }
}