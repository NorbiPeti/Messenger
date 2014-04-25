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
            this.recentMsgTextBox.Location = new System.Drawing.Point(147, 63);
            this.recentMsgTextBox.Name = "recentMsgTextBox";
            this.recentMsgTextBox.ReadOnly = true;
            this.recentMsgTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.recentMsgTextBox.Size = new System.Drawing.Size(418, 241);
            this.recentMsgTextBox.TabIndex = 1;
            this.recentMsgTextBox.Text = "";
            this.recentMsgTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            this.recentMsgTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OpenLink);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(564, 422);
            this.Controls.Add(this.recentMsgTextBox);
            this.Controls.Add(this.messageTextBox);
            this.Name = "ChatForm";
            this.Text = "Beszélgetés";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Khendys.Controls.ExRichTextBox messageTextBox;
        private Khendys.Controls.ExRichTextBox recentMsgTextBox;
    }
}