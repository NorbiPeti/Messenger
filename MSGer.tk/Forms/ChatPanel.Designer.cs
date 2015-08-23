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
            style.Dispose(); //2015.08.23.
            styleRecent.Dispose(); //2015.08.23.
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatPanel));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.partnerName = new Khendys.Controls.ExRichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.recentMsgTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.messageTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterPanel = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.recentMsgTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messageTextBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.masterPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer2.Panel2.Controls.Add(this.progressBar1);
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
            // recentMsgTextBox
            // 
            this.recentMsgTextBox.AcceptsTab = false;
            this.recentMsgTextBox.AllowMacroRecording = false;
            this.recentMsgTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recentMsgTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.recentMsgTextBox.AutoIndent = false;
            this.recentMsgTextBox.AutoIndentChars = false;
            this.recentMsgTextBox.AutoIndentExistingLines = false;
            this.recentMsgTextBox.AutoScrollMinSize = new System.Drawing.Size(0, 12);
            this.recentMsgTextBox.BackBrush = null;
            this.recentMsgTextBox.CharHeight = 12;
            this.recentMsgTextBox.CharWidth = 7;
            this.recentMsgTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.recentMsgTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.recentMsgTextBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.recentMsgTextBox.IsReplaceMode = false;
            this.recentMsgTextBox.Location = new System.Drawing.Point(3, 3);
            this.recentMsgTextBox.Name = "recentMsgTextBox";
            this.recentMsgTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.recentMsgTextBox.ReadOnly = true;
            this.recentMsgTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.recentMsgTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("recentMsgTextBox.ServiceColors")));
            this.recentMsgTextBox.ShowLineNumbers = false;
            this.recentMsgTextBox.Size = new System.Drawing.Size(388, 157);
            this.recentMsgTextBox.TabIndex = 4;
            this.recentMsgTextBox.WordWrap = true;
            this.recentMsgTextBox.Zoom = 100;
            this.recentMsgTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.recentMsgTextBox_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.ForeColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(162, 123);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(145, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Value = 100;
            this.progressBar1.Visible = false;
            // 
            // messageTextBox
            // 
            this.messageTextBox.AcceptsTab = false;
            this.messageTextBox.AllowMacroRecording = false;
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.messageTextBox.AutoIndent = false;
            this.messageTextBox.AutoIndentChars = false;
            this.messageTextBox.AutoIndentExistingLines = false;
            this.messageTextBox.AutoScrollMinSize = new System.Drawing.Size(0, 12);
            this.messageTextBox.BackBrush = null;
            this.messageTextBox.CharHeight = 12;
            this.messageTextBox.CharWidth = 7;
            this.messageTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.messageTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.messageTextBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.messageTextBox.IsReplaceMode = false;
            this.messageTextBox.Location = new System.Drawing.Point(0, 2);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.messageTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.messageTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("messageTextBox.ServiceColors")));
            this.messageTextBox.ShowLineNumbers = false;
            this.messageTextBox.Size = new System.Drawing.Size(391, 114);
            this.messageTextBox.TabIndex = 3;
            this.messageTextBox.WordWrap = true;
            this.messageTextBox.Zoom = 100;
            this.messageTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.MessageTextChanged);
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendMessage);
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
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.recentMsgTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messageTextBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.masterPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public Khendys.Controls.ExRichTextBox partnerName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel masterPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private FastColoredTextBoxNS.FastColoredTextBox messageTextBox;
        private FastColoredTextBoxNS.FastColoredTextBox recentMsgTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
