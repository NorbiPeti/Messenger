namespace MSGer.tk
{
    partial class ThemedForms
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
            this.containerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.MaximizeButton = new System.Windows.Forms.PictureBox();
            this.MinimizeButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeButton)).BeginInit();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.Location = new System.Drawing.Point(12, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(123, 51);
            this.containerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(13, 11);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Title";
            // 
            // CloseButton
            // 
            this.CloseButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CloseButton.Location = new System.Drawing.Point(122, 1);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(24, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MaximizeButton.Location = new System.Drawing.Point(92, 1);
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.Size = new System.Drawing.Size(24, 23);
            this.MaximizeButton.TabIndex = 3;
            this.MaximizeButton.TabStop = false;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MinimizeButton.Location = new System.Drawing.Point(62, 1);
            this.MinimizeButton.Name = "MinimizeButton";
            this.MinimizeButton.Size = new System.Drawing.Size(24, 23);
            this.MinimizeButton.TabIndex = 4;
            this.MinimizeButton.TabStop = false;
            this.MinimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // ThemedForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(147, 93);
            this.Controls.Add(this.MinimizeButton);
            this.Controls.Add(this.MaximizeButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.containerPanel);
            this.Name = "ThemedForms";
            this.Text = "ThemedForms";
            this.DoubleClick += new System.EventHandler(this.ThemedForms_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ThemedForms_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ThemedForms_MouseUp);
            this.Resize += new System.EventHandler(this.ThemedForms_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private System.Windows.Forms.Panel containerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.PictureBox MaximizeButton;
        private System.Windows.Forms.PictureBox MinimizeButton;
    }
}