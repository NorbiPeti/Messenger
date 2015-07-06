namespace MSGer.tk
{
    partial class ThemeDesigner
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
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.colorRadiobtn = new System.Windows.Forms.RadioButton();
            this.imageRadiobtn = new System.Windows.Forms.RadioButton();
            this.scriptRadiobtn = new System.Windows.Forms.RadioButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.scriptLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // okbtn
            // 
            this.okbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbtn.Location = new System.Drawing.Point(21, 292);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 1;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // cancelbtn
            // 
            this.cancelbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(102, 292);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 2;
            this.cancelbtn.Text = "Mégse";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tervezés";
            // 
            // colorRadiobtn
            // 
            this.colorRadiobtn.AutoSize = true;
            this.colorRadiobtn.Location = new System.Drawing.Point(12, 67);
            this.colorRadiobtn.Name = "colorRadiobtn";
            this.colorRadiobtn.Size = new System.Drawing.Size(91, 17);
            this.colorRadiobtn.TabIndex = 4;
            this.colorRadiobtn.TabStop = true;
            this.colorRadiobtn.Text = "colorRadiobtn";
            this.colorRadiobtn.UseVisualStyleBackColor = true;
            this.colorRadiobtn.CheckedChanged += new System.EventHandler(this.colorRadiobtn_CheckedChanged);
            // 
            // imageRadiobtn
            // 
            this.imageRadiobtn.AutoSize = true;
            this.imageRadiobtn.Location = new System.Drawing.Point(12, 120);
            this.imageRadiobtn.Name = "imageRadiobtn";
            this.imageRadiobtn.Size = new System.Drawing.Size(96, 17);
            this.imageRadiobtn.TabIndex = 5;
            this.imageRadiobtn.TabStop = true;
            this.imageRadiobtn.Text = "imageRadiobtn";
            this.imageRadiobtn.UseVisualStyleBackColor = true;
            this.imageRadiobtn.CheckedChanged += new System.EventHandler(this.imageRadiobtn_CheckedChanged);
            // 
            // scriptRadiobtn
            // 
            this.scriptRadiobtn.AutoSize = true;
            this.scriptRadiobtn.Location = new System.Drawing.Point(12, 222);
            this.scriptRadiobtn.Name = "scriptRadiobtn";
            this.scriptRadiobtn.Size = new System.Drawing.Size(93, 17);
            this.scriptRadiobtn.TabIndex = 6;
            this.scriptRadiobtn.TabStop = true;
            this.scriptRadiobtn.Text = "scriptRadiobtn";
            this.scriptRadiobtn.UseVisualStyleBackColor = true;
            this.scriptRadiobtn.CheckedChanged += new System.EventHandler(this.scriptRadiobtn_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 90);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 24);
            this.panel1.TabIndex = 7;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 143);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(165, 73);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // scriptLocation
            // 
            this.scriptLocation.Location = new System.Drawing.Point(12, 258);
            this.scriptLocation.Name = "scriptLocation";
            this.scriptLocation.Size = new System.Drawing.Size(166, 20);
            this.scriptLocation.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Szkript helye:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Téma neve:";
            // 
            // domainUpDown1
            // 
            this.domainUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainUpDown1.Location = new System.Drawing.Point(90, 41);
            this.domainUpDown1.Name = "domainUpDown1";
            this.domainUpDown1.Size = new System.Drawing.Size(87, 20);
            this.domainUpDown1.TabIndex = 13;
            this.domainUpDown1.Text = "New theme";
            this.domainUpDown1.SelectedItemChanged += new System.EventHandler(this.domainUpDown1_SelectedItemChanged);
            this.domainUpDown1.TextChanged += new System.EventHandler(this.domainUpDown1_TextChanged);
            // 
            // ThemeDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 327);
            this.Controls.Add(this.domainUpDown1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.scriptLocation);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.scriptRadiobtn);
            this.Controls.Add(this.imageRadiobtn);
            this.Controls.Add(this.colorRadiobtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ThemeDesigner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ThemeDesigner";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThemeDesigner_FormClosing);
            this.Load += new System.EventHandler(this.ThemeDesigner_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton colorRadiobtn;
        private System.Windows.Forms.RadioButton imageRadiobtn;
        private System.Windows.Forms.RadioButton scriptRadiobtn;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox scriptLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
    }
}