namespace MSGer.tk
{
    partial class ChatForm
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
            this.messageTextBox = new MSGer.tk.ExExRichTextBox();
            this.recentMsgTextBox = new Khendys.Controls.ExRichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusLabel = new Khendys.Controls.ExRichTextBox();
            this.partnerMsg = new Khendys.Controls.ExRichTextBox();
            this.partnerName = new Khendys.Controls.ExRichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.BackColor = System.Drawing.Color.White;
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.DetectUrls = false;
            this.messageTextBox.ForeColor = System.Drawing.Color.Black;
            this.messageTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.messageTextBox.Location = new System.Drawing.Point(147, 310);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(418, 75);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            this.messageTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            this.messageTextBox.TextChanged += new System.EventHandler(this.MessageTextChanged);
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendMessage);
            // 
            // recentMsgTextBox
            // 
            this.recentMsgTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recentMsgTextBox.BackColor = System.Drawing.Color.White;
            this.recentMsgTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.recentMsgTextBox.ForeColor = System.Drawing.Color.Black;
            this.recentMsgTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.recentMsgTextBox.Location = new System.Drawing.Point(147, 76);
            this.recentMsgTextBox.Name = "recentMsgTextBox";
            this.recentMsgTextBox.ReadOnly = true;
            this.recentMsgTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.recentMsgTextBox.Size = new System.Drawing.Size(418, 228);
            this.recentMsgTextBox.TabIndex = 1;
            this.recentMsgTextBox.Text = "";
            this.recentMsgTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            this.recentMsgTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OpenLink);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 422);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Controls.Add(this.partnerMsg);
            this.panel2.Controls.Add(this.partnerName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(141, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(423, 70);
            this.panel2.TabIndex = 4;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusLabel.ForeColor = System.Drawing.Color.Black;
            this.statusLabel.HiglightColor = Khendys.Controls.RtfColor.White;
            this.statusLabel.Location = new System.Drawing.Point(9, 52);
            this.statusLabel.Multiline = false;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(390, 16);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "statusLabel";
            this.statusLabel.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // partnerMsg
            // 
            this.partnerMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.partnerMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.partnerMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.partnerMsg.ForeColor = System.Drawing.Color.Black;
            this.partnerMsg.HiglightColor = Khendys.Controls.RtfColor.White;
            this.partnerMsg.Location = new System.Drawing.Point(9, 30);
            this.partnerMsg.Multiline = false;
            this.partnerMsg.Name = "partnerMsg";
            this.partnerMsg.Size = new System.Drawing.Size(390, 24);
            this.partnerMsg.TabIndex = 4;
            this.partnerMsg.Text = "partnerMsg";
            this.partnerMsg.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // partnerName
            // 
            this.partnerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.partnerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.partnerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.partnerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.partnerName.ForeColor = System.Drawing.Color.Black;
            this.partnerName.HiglightColor = Khendys.Controls.RtfColor.White;
            this.partnerName.Location = new System.Drawing.Point(9, 7);
            this.partnerName.Multiline = false;
            this.partnerName.Name = "partnerName";
            this.partnerName.ReadOnly = true;
            this.partnerName.Size = new System.Drawing.Size(399, 25);
            this.partnerName.TabIndex = 3;
            this.partnerName.Text = "partnerName";
            this.partnerName.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(564, 422);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.recentMsgTextBox);
            this.Controls.Add(this.messageTextBox);
            this.Name = "ChatForm";
            this.Text = "Beszélgetés";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExExRichTextBox messageTextBox;
        private Khendys.Controls.ExRichTextBox recentMsgTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public Khendys.Controls.ExRichTextBox partnerName;
        public Khendys.Controls.ExRichTextBox partnerMsg;
        public Khendys.Controls.ExRichTextBox statusLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}