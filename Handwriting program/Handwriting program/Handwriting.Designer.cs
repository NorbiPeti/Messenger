namespace Handwriting_program
{
    partial class Handwriting
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.colorbtn = new System.Windows.Forms.Button();
            this.erasebtn = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.sendbtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 129);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Handwriting_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Handwriting_MouseDown);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Handwriting_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.colorbtn);
            this.flowLayoutPanel1.Controls.Add(this.erasebtn);
            this.flowLayoutPanel1.Controls.Add(this.trackBar1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 129);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(514, 40);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // colorbtn
            // 
            this.colorbtn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.colorbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.colorbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aqua;
            this.colorbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorbtn.Image = global::Handwriting_program.Properties.Resources.colorpicker;
            this.colorbtn.Location = new System.Drawing.Point(3, 3);
            this.colorbtn.Name = "colorbtn";
            this.colorbtn.Size = new System.Drawing.Size(32, 32);
            this.colorbtn.TabIndex = 0;
            this.colorbtn.UseVisualStyleBackColor = false;
            this.colorbtn.Click += new System.EventHandler(this.colorbtn_Click);
            // 
            // erasebtn
            // 
            this.erasebtn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.erasebtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.erasebtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aqua;
            this.erasebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.erasebtn.Image = global::Handwriting_program.Properties.Resources.erase;
            this.erasebtn.Location = new System.Drawing.Point(41, 3);
            this.erasebtn.Name = "erasebtn";
            this.erasebtn.Size = new System.Drawing.Size(32, 32);
            this.erasebtn.TabIndex = 1;
            this.erasebtn.UseVisualStyleBackColor = true;
            this.erasebtn.Click += new System.EventHandler(this.erasebtn_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(79, 3);
            this.trackBar1.Maximum = 50;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(130, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // sendbtn
            // 
            this.sendbtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.sendbtn.Location = new System.Drawing.Point(439, 0);
            this.sendbtn.Name = "sendbtn";
            this.sendbtn.Size = new System.Drawing.Size(75, 129);
            this.sendbtn.TabIndex = 2;
            this.sendbtn.Text = "send";
            this.sendbtn.UseVisualStyleBackColor = true;
            // 
            // Handwriting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sendbtn);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Handwriting";
            this.Size = new System.Drawing.Size(514, 169);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button colorbtn;
        private System.Windows.Forms.Button erasebtn;
        private System.Windows.Forms.TrackBar trackBar1;
        public System.Windows.Forms.Button sendbtn;

    }
}
