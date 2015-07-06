namespace MSGer.tk
{
    partial class SettingsPanelPersonal
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
            this.languageList = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.languageLabel = new System.Windows.Forms.Label();
            this.messageText = new System.Windows.Forms.TextBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.personal = new System.Windows.Forms.Label();
            this.selectimgbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // languageList
            // 
            this.languageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.languageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.languageList.LabelWrap = false;
            this.languageList.Location = new System.Drawing.Point(12, 191);
            this.languageList.MultiSelect = false;
            this.languageList.Name = "languageList";
            this.languageList.Size = new System.Drawing.Size(347, 78);
            this.languageList.TabIndex = 13;
            this.languageList.UseCompatibleStateImageBehavior = false;
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.Location = new System.Drawing.Point(9, 175);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(34, 13);
            this.languageLabel.TabIndex = 12;
            this.languageLabel.Text = "Nyelv";
            // 
            // messageText
            // 
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageText.Location = new System.Drawing.Point(12, 99);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(347, 20);
            this.messageText.TabIndex = 11;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(9, 82);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(41, 13);
            this.messageLabel.TabIndex = 10;
            this.messageLabel.Text = "Üzenet";
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Location = new System.Drawing.Point(12, 52);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(347, 20);
            this.nameText.TabIndex = 9;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(9, 35);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(27, 13);
            this.nameLabel.TabIndex = 8;
            this.nameLabel.Text = "Név";
            // 
            // personal
            // 
            this.personal.AutoSize = true;
            this.personal.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.personal.Location = new System.Drawing.Point(3, 0);
            this.personal.Name = "personal";
            this.personal.Size = new System.Drawing.Size(147, 31);
            this.personal.TabIndex = 7;
            this.personal.Text = "Személyes";
            // 
            // selectimgbtn
            // 
            this.selectimgbtn.Location = new System.Drawing.Point(12, 135);
            this.selectimgbtn.Name = "selectimgbtn";
            this.selectimgbtn.Size = new System.Drawing.Size(138, 23);
            this.selectimgbtn.TabIndex = 14;
            this.selectimgbtn.Text = "Kép kiválasztása...";
            this.selectimgbtn.UseVisualStyleBackColor = true;
            this.selectimgbtn.Click += new System.EventHandler(this.selectimgbtn_Click);
            // 
            // SettingsPanelPersonal
            // 
            this.Controls.Add(this.selectimgbtn);
            this.Controls.Add(this.languageList);
            this.Controls.Add(this.languageLabel);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.personal);
            this.Name = "SettingsPanelPersonal";
            this.Size = new System.Drawing.Size(376, 284);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView languageList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label personal;
        private System.Windows.Forms.Button selectimgbtn;

    }
}