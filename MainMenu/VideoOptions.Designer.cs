namespace MainMenu {
    partial class VideoOptions {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.accept = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.jFr = new System.Windows.Forms.TextBox();
            this.simages = new System.Windows.Forms.CheckBox();
            this.svideo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // accept
            // 
            this.accept.Location = new System.Drawing.Point(223, 101);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 0;
            this.accept.Text = "Accept";
            this.accept.UseVisualStyleBackColor = true;
            this.accept.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Jump Between Frames:";
            // 
            // jFr
            // 
            this.jFr.Location = new System.Drawing.Point(149, 28);
            this.jFr.Name = "jFr";
            this.jFr.Size = new System.Drawing.Size(50, 20);
            this.jFr.TabIndex = 2;
            this.jFr.Text = "5";
            this.jFr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.jFr_KeyPress);
            this.jFr.Leave += new System.EventHandler(this.jFr_Leave);
            // 
            // simages
            // 
            this.simages.AutoSize = true;
            this.simages.Location = new System.Drawing.Point(29, 62);
            this.simages.Name = "simages";
            this.simages.Size = new System.Drawing.Size(88, 17);
            this.simages.TabIndex = 3;
            this.simages.Text = "Save Images";
            this.simages.UseVisualStyleBackColor = true;
            // 
            // svideo
            // 
            this.svideo.AutoSize = true;
            this.svideo.Location = new System.Drawing.Point(29, 85);
            this.svideo.Name = "svideo";
            this.svideo.Size = new System.Drawing.Size(81, 17);
            this.svideo.TabIndex = 4;
            this.svideo.Text = "Save Video";
            this.svideo.UseVisualStyleBackColor = true;
            // 
            // VideoOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 136);
            this.Controls.Add(this.svideo);
            this.Controls.Add(this.simages);
            this.Controls.Add(this.jFr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accept);
            this.Name = "VideoOptions";
            this.Text = "VideoOptions";
            this.Load += new System.EventHandler(this.VideoOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox jFr;
        private System.Windows.Forms.CheckBox simages;
        private System.Windows.Forms.CheckBox svideo;
    }
}