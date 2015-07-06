namespace MSGer.tk
{
    partial class SettingsPanelPacks
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
            this.Scripterbtn = new System.Windows.Forms.Button();
            this.packs = new System.Windows.Forms.Label();
            this.themedesignerbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Scripterbtn
            // 
            this.Scripterbtn.Location = new System.Drawing.Point(12, 41);
            this.Scripterbtn.Name = "Scripterbtn";
            this.Scripterbtn.Size = new System.Drawing.Size(83, 23);
            this.Scripterbtn.TabIndex = 13;
            this.Scripterbtn.Text = "Szkriptíró";
            this.Scripterbtn.UseVisualStyleBackColor = true;
            this.Scripterbtn.Click += new System.EventHandler(this.Scripterbtn_Click);
            // 
            // packs
            // 
            this.packs.AutoSize = true;
            this.packs.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.packs.Location = new System.Drawing.Point(6, 7);
            this.packs.Name = "packs";
            this.packs.Size = new System.Drawing.Size(89, 31);
            this.packs.TabIndex = 12;
            this.packs.Text = "Packs";
            // 
            // themedesignerbtn
            // 
            this.themedesignerbtn.Location = new System.Drawing.Point(12, 70);
            this.themedesignerbtn.Name = "themedesignerbtn";
            this.themedesignerbtn.Size = new System.Drawing.Size(83, 23);
            this.themedesignerbtn.TabIndex = 14;
            this.themedesignerbtn.Text = "Tématervező";
            this.themedesignerbtn.UseVisualStyleBackColor = true;
            this.themedesignerbtn.Click += new System.EventHandler(this.themedesignerbtn_Click);
            // 
            // SettingsPanelPacks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.themedesignerbtn);
            this.Controls.Add(this.Scripterbtn);
            this.Controls.Add(this.packs);
            this.Name = "SettingsPanelPacks";
            this.Size = new System.Drawing.Size(189, 138);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Scripterbtn;
        private System.Windows.Forms.Label packs;
        private System.Windows.Forms.Button themedesignerbtn;
    }
}
