namespace SimUI {
    partial class IPConfigWindow {
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ip1 = new System.Windows.Forms.TextBox();
            this.ip2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ip3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ip4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            // 
            // ip1
            // 
            this.ip1.Location = new System.Drawing.Point(10, 42);
            this.ip1.Name = "ip1";
            this.ip1.Size = new System.Drawing.Size(49, 20);
            this.ip1.TabIndex = 2;
            this.ip1.TextChanged += new System.EventHandler(this.ip1_TextChanged);
            this.ip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip1_KeyPress);
            // 
            // ip2
            // 
            this.ip2.Location = new System.Drawing.Point(83, 42);
            this.ip2.Name = "ip2";
            this.ip2.Size = new System.Drawing.Size(49, 20);
            this.ip2.TabIndex = 3;
            this.ip2.TextChanged += new System.EventHandler(this.ip2_TextChanged);
            this.ip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip2_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(138, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = ".";
            // 
            // ip3
            // 
            this.ip3.Location = new System.Drawing.Point(156, 42);
            this.ip3.Name = "ip3";
            this.ip3.Size = new System.Drawing.Size(49, 20);
            this.ip3.TabIndex = 5;
            this.ip3.TextChanged += new System.EventHandler(this.ip3_TextChanged);
            this.ip3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip3_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(211, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = ".";
            // 
            // ip4
            // 
            this.ip4.Location = new System.Drawing.Point(227, 42);
            this.ip4.Name = "ip4";
            this.ip4.Size = new System.Drawing.Size(49, 20);
            this.ip4.TabIndex = 7;
            this.ip4.TextChanged += new System.EventHandler(this.ip4_TextChanged);
            this.ip4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip4_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Server IP Address:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "LocalHost";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // IPConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 112);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ip4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ip3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ip2);
            this.Controls.Add(this.ip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "IPConfigWindow";
            this.Text = "IPConfigWindow";
            this.Load += new System.EventHandler(this.IPConfigWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ip1;
        private System.Windows.Forms.TextBox ip2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ip3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ip4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
    }
}