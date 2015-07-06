namespace MSGer.tk
{
    partial class SettingsPanelNetwork
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
            this.network = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.portNum = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).BeginInit();
            this.SuspendLayout();
            // 
            // network
            // 
            this.network.AutoSize = true;
            this.network.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.network.Location = new System.Drawing.Point(9, 8);
            this.network.Name = "network";
            this.network.Size = new System.Drawing.Size(115, 31);
            this.network.TabIndex = 13;
            this.network.Text = "Network";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(12, 51);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 14;
            this.portLabel.Text = "Port:";
            // 
            // portNum
            // 
            this.portNum.Location = new System.Drawing.Point(47, 49);
            this.portNum.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.portNum.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.portNum.Name = "portNum";
            this.portNum.Size = new System.Drawing.Size(77, 20);
            this.portNum.TabIndex = 15;
            this.portNum.Value = new decimal(new int[] {
            4510,
            0,
            0,
            0});
            // 
            // SettingsPanelNetwork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.portNum);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.network);
            this.Name = "SettingsPanelNetwork";
            this.Size = new System.Drawing.Size(450, 108);
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label network;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.NumericUpDown portNum;
    }
}
