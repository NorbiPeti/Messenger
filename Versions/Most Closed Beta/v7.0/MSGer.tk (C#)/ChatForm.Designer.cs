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
            this.messageTextBox = new Khendys.Controls.ExRichTextBox();
            this.recentMsgTextBox = new Khendys.Controls.ExRichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.partnerMsg = new System.Windows.Forms.Label();
            this.partnerName = new System.Windows.Forms.Label();
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
            this.panel2.Controls.Add(this.partnerMsg);
            this.panel2.Controls.Add(this.partnerName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(141, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(423, 70);
            this.panel2.TabIndex = 4;
            // 
            // partnerMsg
            // 
            this.partnerMsg.AutoSize = true;
            this.partnerMsg.ForeColor = System.Drawing.Color.Black;
            this.partnerMsg.Location = new System.Drawing.Point(9, 38);
            this.partnerMsg.Name = "partnerMsg";
            this.partnerMsg.Size = new System.Drawing.Size(60, 13);
            this.partnerMsg.TabIndex = 1;
            this.partnerMsg.Text = "partnerMsg";
            // 
            // partnerName
            // 
            this.partnerName.AutoSize = true;
            this.partnerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.partnerName.ForeColor = System.Drawing.Color.Black;
            this.partnerName.Location = new System.Drawing.Point(7, 13);
            this.partnerName.Name = "partnerName";
            this.partnerName.Size = new System.Drawing.Size(136, 25);
            this.partnerName.TabIndex = 0;
            this.partnerName.Text = "partnerName";
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
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Khendys.Controls.ExRichTextBox messageTextBox;
        private Khendys.Controls.ExRichTextBox recentMsgTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label partnerMsg;
        private System.Windows.Forms.Label partnerName;
    }
}