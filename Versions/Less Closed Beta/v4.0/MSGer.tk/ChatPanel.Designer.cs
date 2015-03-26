namespace MSGer.tk
{
    partial class ChatPanel
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

        #region Component Designer generated code

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
            this.partnerName = new Khendys.Controls.ExRichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.showicons = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.masterPanel = new System.Windows.Forms.Panel();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.masterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.BackColor = System.Drawing.Color.White;
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.DetectUrls = false;
            this.messageTextBox.ForeColor = System.Drawing.Color.Black;
            this.messageTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.messageTextBox.Location = new System.Drawing.Point(0, 3);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(391, 113);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            this.messageTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            this.messageTextBox.TextChanged += new System.EventHandler(this.MessageTextChanged);
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendMessage);
            // 
            // recentMsgTextBox
            // 
            this.recentMsgTextBox.BackColor = System.Drawing.Color.White;
            this.recentMsgTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.recentMsgTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentMsgTextBox.ForeColor = System.Drawing.Color.Black;
            this.recentMsgTextBox.HiglightColor = Khendys.Controls.RtfColor.White;
            this.recentMsgTextBox.Location = new System.Drawing.Point(0, 0);
            this.recentMsgTextBox.Name = "recentMsgTextBox";
            this.recentMsgTextBox.ReadOnly = true;
            this.recentMsgTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.recentMsgTextBox.Size = new System.Drawing.Size(391, 160);
            this.recentMsgTextBox.TabIndex = 4;
            this.recentMsgTextBox.Text = "";
            this.recentMsgTextBox.TextColor = Khendys.Controls.RtfColor.Black;
            this.recentMsgTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OpenLink);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 363);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.partnerName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(391, 39);
            this.panel2.TabIndex = 0;
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
            this.partnerName.Size = new System.Drawing.Size(337, 25);
            this.partnerName.TabIndex = 3;
            this.partnerName.Text = "partnerName";
            this.partnerName.TextColor = Khendys.Controls.RtfColor.Black;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // showicons
            // 
            this.showicons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showicons.AutoSize = true;
            this.showicons.Location = new System.Drawing.Point(2, 126);
            this.showicons.Name = "showicons";
            this.showicons.Size = new System.Drawing.Size(76, 17);
            this.showicons.TabIndex = 1;
            this.showicons.Text = "showicons";
            this.showicons.UseVisualStyleBackColor = true;
            this.showicons.CheckedChanged += new System.EventHandler(this.showicons_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(313, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "handwriting";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(564, 363);
            this.splitContainer1.SplitterDistance = 169;
            this.splitContainer1.TabIndex = 7;
            this.splitContainer1.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.showicons);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.messageTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(391, 363);
            this.splitContainer2.SplitterDistance = 203;
            this.splitContainer2.TabIndex = 2;
            this.splitContainer2.TabStop = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.recentMsgTextBox);
            this.splitContainer3.Size = new System.Drawing.Size(391, 203);
            this.splitContainer3.SplitterDistance = 39;
            this.splitContainer3.TabIndex = 2;
            this.splitContainer3.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImage = global::MSGer.tk.Properties.Resources.Menü_2;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(564, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // masterPanel
            // 
            this.masterPanel.Controls.Add(this.splitContainer1);
            this.masterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterPanel.Location = new System.Drawing.Point(0, 24);
            this.masterPanel.Name = "masterPanel";
            this.masterPanel.Size = new System.Drawing.Size(564, 363);
            this.masterPanel.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // ChatPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.masterPanel);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ChatPanel";
            this.Size = new System.Drawing.Size(564, 387);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.masterPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Khendys.Controls.ExRichTextBox messageTextBox;
        private Khendys.Controls.ExRichTextBox recentMsgTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public Khendys.Controls.ExRichTextBox partnerName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox showicons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel masterPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    }
}
