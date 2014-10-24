namespace MSGer.tk
{
    partial class PartnerInformation
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nameTextBox = new Khendys.Controls.ExRichTextBox();
            this.messageTextBox = new Khendys.Controls.ExRichTextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.userName1 = new System.Windows.Forms.Label();
            this.userName2 = new System.Windows.Forms.Label();
            this.userID1 = new System.Windows.Forms.Label();
            this.userID2 = new System.Windows.Forms.Label();
            this.email2 = new System.Windows.Forms.Label();
            this.email1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.nameTextBox.Location = new System.Drawing.Point(119, 12);
            this.nameTextBox.Multiline = false;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(445, 30);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Text = "ShownName";
            this.nameTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.messageTextBox.Location = new System.Drawing.Point(119, 57);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(445, 55);
            this.messageTextBox.TabIndex = 2;
            this.messageTextBox.Text = "User Message\nLine";
            this.messageTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(119, 38);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(63, 13);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "StatusLabel";
            // 
            // userName1
            // 
            this.userName1.AutoSize = true;
            this.userName1.Location = new System.Drawing.Point(12, 120);
            this.userName1.Name = "userName1";
            this.userName1.Size = new System.Drawing.Size(63, 13);
            this.userName1.TabIndex = 4;
            this.userName1.Text = "UserName1";
            // 
            // userName2
            // 
            this.userName2.AutoSize = true;
            this.userName2.Location = new System.Drawing.Point(119, 120);
            this.userName2.Name = "userName2";
            this.userName2.Size = new System.Drawing.Size(63, 13);
            this.userName2.TabIndex = 5;
            this.userName2.Text = "UserName2";
            // 
            // userID1
            // 
            this.userID1.AutoSize = true;
            this.userID1.Location = new System.Drawing.Point(12, 135);
            this.userID1.Name = "userID1";
            this.userID1.Size = new System.Drawing.Size(46, 13);
            this.userID1.TabIndex = 6;
            this.userID1.Text = "UserID1";
            // 
            // userID2
            // 
            this.userID2.AutoSize = true;
            this.userID2.Location = new System.Drawing.Point(119, 135);
            this.userID2.Name = "userID2";
            this.userID2.Size = new System.Drawing.Size(46, 13);
            this.userID2.TabIndex = 7;
            this.userID2.Text = "UserID2";
            // 
            // email2
            // 
            this.email2.AutoSize = true;
            this.email2.Location = new System.Drawing.Point(119, 150);
            this.email2.Name = "email2";
            this.email2.Size = new System.Drawing.Size(38, 13);
            this.email2.TabIndex = 9;
            this.email2.Text = "Email2";
            // 
            // email1
            // 
            this.email1.AutoSize = true;
            this.email1.Location = new System.Drawing.Point(12, 150);
            this.email1.Name = "email1";
            this.email1.Size = new System.Drawing.Size(38, 13);
            this.email1.TabIndex = 8;
            this.email1.Text = "Email1";
            // 
            // PartnerInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 273);
            this.Controls.Add(this.email2);
            this.Controls.Add(this.email1);
            this.Controls.Add(this.userID2);
            this.Controls.Add(this.userID1);
            this.Controls.Add(this.userName2);
            this.Controls.Add(this.userName1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Name = "PartnerInformation";
            this.Text = "PartnerInformation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Khendys.Controls.ExRichTextBox nameTextBox;
        private Khendys.Controls.ExRichTextBox messageTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label userName1;
        private System.Windows.Forms.Label userName2;
        private System.Windows.Forms.Label userID1;
        private System.Windows.Forms.Label userID2;
        private System.Windows.Forms.Label email2;
        private System.Windows.Forms.Label email1;
    }
}