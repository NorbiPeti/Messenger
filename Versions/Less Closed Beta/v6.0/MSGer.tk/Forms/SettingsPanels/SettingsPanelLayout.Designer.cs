namespace MSGer.tk
{
    partial class SettingsPanelLayout
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
            this.chatwindowTabs = new System.Windows.Forms.CheckBox();
            this.chatwindow = new System.Windows.Forms.CheckBox();
            this.layout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chatwindowTabs
            // 
            this.chatwindowTabs.AutoSize = true;
            this.chatwindowTabs.Enabled = false;
            this.chatwindowTabs.Location = new System.Drawing.Point(51, 69);
            this.chatwindowTabs.Name = "chatwindowTabs";
            this.chatwindowTabs.Size = new System.Drawing.Size(194, 17);
            this.chatwindowTabs.TabIndex = 12;
            this.chatwindowTabs.Text = "A beszélgetések fülekbe rendezése";
            this.chatwindowTabs.UseVisualStyleBackColor = true;
            // 
            // chatwindow
            // 
            this.chatwindow.AutoSize = true;
            this.chatwindow.Location = new System.Drawing.Point(13, 45);
            this.chatwindow.Name = "chatwindow";
            this.chatwindow.Size = new System.Drawing.Size(272, 17);
            this.chatwindow.TabIndex = 11;
            this.chatwindow.Text = "A beszélgetések jelenjenek meg külön ablak(ok)ban";
            this.chatwindow.UseVisualStyleBackColor = true;
            // 
            // layout
            // 
            this.layout.AutoSize = true;
            this.layout.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.layout.Location = new System.Drawing.Point(7, 10);
            this.layout.Name = "layout";
            this.layout.Size = new System.Drawing.Size(105, 31);
            this.layout.TabIndex = 10;
            this.layout.Text = "Kinézet";
            // 
            // SettingsPanelLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chatwindowTabs);
            this.Controls.Add(this.chatwindow);
            this.Controls.Add(this.layout);
            this.Name = "SettingsPanelLayout";
            this.Size = new System.Drawing.Size(346, 122);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chatwindowTabs;
        private System.Windows.Forms.CheckBox chatwindow;
        private System.Windows.Forms.Label layout;
    }
}
