namespace MainMenu {
    partial class MainMenuConfigs {
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
            this.ok_button = new System.Windows.Forms.Button();
            this.visSim = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(420, 207);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 0;
            this.ok_button.Text = "Accept";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // visSim
            // 
            this.visSim.AutoSize = true;
            this.visSim.Location = new System.Drawing.Point(40, 68);
            this.visSim.Name = "visSim";
            this.visSim.Size = new System.Drawing.Size(107, 17);
            this.visSim.TabIndex = 1;
            this.visSim.Text = "Visible Simulation";
            this.visSim.UseVisualStyleBackColor = true;
            this.visSim.CheckedChanged += new System.EventHandler(this.visSim_CheckedChanged);
            // 
            // MainMenuConfigs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 242);
            this.Controls.Add(this.visSim);
            this.Controls.Add(this.ok_button);
            this.Name = "MainMenuConfigs";
            this.Text = "Configurations";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.CheckBox visSim;
    }
}