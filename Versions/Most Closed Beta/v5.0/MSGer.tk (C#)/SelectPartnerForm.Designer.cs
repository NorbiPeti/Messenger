namespace MSGer.tk
{
    partial class SelectPartnerForm
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
            this.partnerList = new GlacialComponents.Controls.GlacialList();
            this.selectedPartners = new System.Windows.Forms.TextBox();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.titleText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // partnerList
            // 
            this.partnerList.AllowColumnResize = true;
            this.partnerList.AllowMultiselect = false;
            this.partnerList.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.partnerList.AlternatingColors = false;
            this.partnerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.partnerList.AutoHeight = true;
            this.partnerList.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.partnerList.BackgroundStretchToFit = true;
            this.partnerList.ControlStyle = GlacialComponents.Controls.GLControlStyles.Normal;
            this.partnerList.FullRowSelect = true;
            this.partnerList.GridColor = System.Drawing.Color.LightGray;
            this.partnerList.GridLines = GlacialComponents.Controls.GLGridLines.gridBoth;
            this.partnerList.GridLineStyle = GlacialComponents.Controls.GLGridLineStyles.gridSolid;
            this.partnerList.GridTypes = GlacialComponents.Controls.GLGridTypes.gridOnExists;
            this.partnerList.HeaderHeight = 0;
            this.partnerList.HeaderVisible = false;
            this.partnerList.HeaderWordWrap = false;
            this.partnerList.HotColumnTracking = false;
            this.partnerList.HotItemTracking = false;
            this.partnerList.HotTrackingColor = System.Drawing.Color.LightGray;
            this.partnerList.HoverEvents = false;
            this.partnerList.HoverTime = 1;
            this.partnerList.ImageList = null;
            this.partnerList.ItemHeight = 18;
            this.partnerList.ItemWordWrap = false;
            this.partnerList.Location = new System.Drawing.Point(13, 26);
            this.partnerList.Name = "partnerList";
            this.partnerList.Selectable = true;
            this.partnerList.SelectedTextColor = System.Drawing.Color.White;
            this.partnerList.SelectionColor = System.Drawing.Color.DarkBlue;
            this.partnerList.ShowBorder = true;
            this.partnerList.ShowFocusRect = false;
            this.partnerList.Size = new System.Drawing.Size(359, 286);
            this.partnerList.SortType = GlacialComponents.Controls.SortTypes.InsertionSort;
            this.partnerList.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.partnerList.TabIndex = 0;
            // 
            // selectedPartners
            // 
            this.selectedPartners.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedPartners.Location = new System.Drawing.Point(13, 319);
            this.selectedPartners.Multiline = true;
            this.selectedPartners.Name = "selectedPartners";
            this.selectedPartners.Size = new System.Drawing.Size(359, 103);
            this.selectedPartners.TabIndex = 1;
            // 
            // okbtn
            // 
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbtn.Location = new System.Drawing.Point(215, 428);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 2;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            // 
            // cancelbtn
            // 
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(297, 429);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 3;
            this.cancelbtn.Text = "Mégse";
            this.cancelbtn.UseVisualStyleBackColor = true;
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.Location = new System.Drawing.Point(13, 7);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(53, 13);
            this.titleText.TabIndex = 4;
            this.titleText.Text = "Unknown";
            // 
            // SelectPartnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.titleText);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.selectedPartners);
            this.Controls.Add(this.partnerList);
            this.Name = "SelectPartnerForm";
            this.Text = "SelectPartnerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GlacialComponents.Controls.GlacialList partnerList;
        private System.Windows.Forms.TextBox selectedPartners;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.Label titleText;
    }
}