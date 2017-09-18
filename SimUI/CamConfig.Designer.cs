namespace SimUI {
    partial class GUIConfig {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUIConfig));
            this.ok = new System.Windows.Forms.Button();
            this.presetConfigs = new System.Windows.Forms.ComboBox();
            this.customW = new System.Windows.Forms.TextBox();
            this.customH = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.alg1agg = new System.Windows.Forms.CheckBox();
            this.alg2fol = new System.Windows.Forms.CheckBox();
            this.alg3Clust = new System.Windows.Forms.CheckBox();
            this.alg4tra = new System.Windows.Forms.CheckBox();
            this.alg0gos = new System.Windows.Forms.CheckBox();
            this.directions = new System.Windows.Forms.CheckBox();
            this.camen = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.iren = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.advMotion = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(463, 245);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "Accept";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // presetConfigs
            // 
            this.presetConfigs.FormattingEnabled = true;
            this.presetConfigs.Items.AddRange(new object[] {
            "Custom",
            "80 x 60",
            "120 x 90",
            "160 x 120",
            "200 x 150",
            "640 x 480"});
            this.presetConfigs.Location = new System.Drawing.Point(410, 50);
            this.presetConfigs.Name = "presetConfigs";
            this.presetConfigs.Size = new System.Drawing.Size(123, 21);
            this.presetConfigs.TabIndex = 1;
            this.presetConfigs.SelectedIndexChanged += new System.EventHandler(this.presetConfigs_SelectedIndexChanged);
            // 
            // customW
            // 
            this.customW.Location = new System.Drawing.Point(410, 93);
            this.customW.Name = "customW";
            this.customW.Size = new System.Drawing.Size(53, 20);
            this.customW.TabIndex = 2;
            this.customW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.customW_KeyPress);
            // 
            // customH
            // 
            this.customH.Location = new System.Drawing.Point(410, 119);
            this.customH.Name = "customH";
            this.customH.Size = new System.Drawing.Size(53, 20);
            this.customH.TabIndex = 3;
            this.customH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.customH_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(469, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(407, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Image Size Ratio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(410, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Custom";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(204, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(469, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Width";
            // 
            // alg1agg
            // 
            this.alg1agg.AutoSize = true;
            this.alg1agg.Location = new System.Drawing.Point(12, 55);
            this.alg1agg.Name = "alg1agg";
            this.alg1agg.Size = new System.Drawing.Size(83, 17);
            this.alg1agg.TabIndex = 10;
            this.alg1agg.Text = "Aggregation";
            this.alg1agg.UseVisualStyleBackColor = true;
            this.alg1agg.CheckedChanged += new System.EventHandler(this.alg1agg_CheckedChanged);
            // 
            // alg2fol
            // 
            this.alg2fol.AutoSize = true;
            this.alg2fol.Location = new System.Drawing.Point(12, 78);
            this.alg2fol.Name = "alg2fol";
            this.alg2fol.Size = new System.Drawing.Size(65, 17);
            this.alg2fol.TabIndex = 11;
            this.alg2fol.Text = "Follower";
            this.alg2fol.UseVisualStyleBackColor = true;
            this.alg2fol.CheckedChanged += new System.EventHandler(this.alg2fol_CheckedChanged);
            // 
            // alg3Clust
            // 
            this.alg3Clust.AutoSize = true;
            this.alg3Clust.Location = new System.Drawing.Point(12, 101);
            this.alg3Clust.Name = "alg3Clust";
            this.alg3Clust.Size = new System.Drawing.Size(106, 17);
            this.alg3Clust.TabIndex = 12;
            this.alg3Clust.Text = "Object Clustering";
            this.alg3Clust.UseVisualStyleBackColor = true;
            this.alg3Clust.CheckedChanged += new System.EventHandler(this.alg3Clust_CheckedChanged);
            // 
            // alg4tra
            // 
            this.alg4tra.AutoSize = true;
            this.alg4tra.Location = new System.Drawing.Point(12, 124);
            this.alg4tra.Name = "alg4tra";
            this.alg4tra.Size = new System.Drawing.Size(120, 17);
            this.alg4tra.TabIndex = 13;
            this.alg4tra.Text = "Collective Transport";
            this.alg4tra.UseVisualStyleBackColor = true;
            this.alg4tra.CheckedChanged += new System.EventHandler(this.alg4tra_CheckedChanged);
            // 
            // alg0gos
            // 
            this.alg0gos.AutoSize = true;
            this.alg0gos.Location = new System.Drawing.Point(12, 32);
            this.alg0gos.Name = "alg0gos";
            this.alg0gos.Size = new System.Drawing.Size(58, 17);
            this.alg0gos.TabIndex = 14;
            this.alg0gos.Text = "Gossip";
            this.alg0gos.UseVisualStyleBackColor = true;
            this.alg0gos.CheckedChanged += new System.EventHandler(this.alg0gos_CheckedChanged);
            // 
            // directions
            // 
            this.directions.AutoSize = true;
            this.directions.Location = new System.Drawing.Point(9, 181);
            this.directions.Name = "directions";
            this.directions.Size = new System.Drawing.Size(123, 17);
            this.directions.TabIndex = 15;
            this.directions.Text = "Direction Commands";
            this.directions.UseVisualStyleBackColor = true;
            this.directions.CheckedChanged += new System.EventHandler(this.directions_CheckedChanged);
            // 
            // camen
            // 
            this.camen.AutoSize = true;
            this.camen.Location = new System.Drawing.Point(9, 204);
            this.camen.Name = "camen";
            this.camen.Size = new System.Drawing.Size(62, 17);
            this.camen.TabIndex = 16;
            this.camen.Text = "Camera";
            this.camen.UseVisualStyleBackColor = true;
            this.camen.CheckedChanged += new System.EventHandler(this.camen_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Algorithms";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape3,
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(550, 280);
            this.shapeContainer1.TabIndex = 18;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape3
            // 
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 200;
            this.lineShape3.X2 = 540;
            this.lineShape3.Y1 = 25;
            this.lineShape3.Y2 = 25;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 10;
            this.lineShape2.X2 = 150;
            this.lineShape2.Y1 = 175;
            this.lineShape2.Y2 = 175;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 10;
            this.lineShape1.X2 = 150;
            this.lineShape1.Y1 = 26;
            this.lineShape1.Y2 = 26;
            // 
            // iren
            // 
            this.iren.AutoSize = true;
            this.iren.Location = new System.Drawing.Point(9, 227);
            this.iren.Name = "iren";
            this.iren.Size = new System.Drawing.Size(78, 17);
            this.iren.TabIndex = 19;
            this.iren.Text = "IR Sensors";
            this.iren.UseVisualStyleBackColor = true;
            this.iren.CheckedChanged += new System.EventHandler(this.iren_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Robot Abilities";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Camera Configuration";
            // 
            // advMotion
            // 
            this.advMotion.AutoSize = true;
            this.advMotion.Location = new System.Drawing.Point(204, 187);
            this.advMotion.Name = "advMotion";
            this.advMotion.Size = new System.Drawing.Size(110, 17);
            this.advMotion.TabIndex = 22;
            this.advMotion.Text = "Advanced Motion";
            this.advMotion.UseVisualStyleBackColor = true;
            this.advMotion.CheckedChanged += new System.EventHandler(this.advMotion_CheckedChanged);
            // 
            // GUIConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 280);
            this.Controls.Add(this.advMotion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.iren);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.camen);
            this.Controls.Add(this.directions);
            this.Controls.Add(this.alg0gos);
            this.Controls.Add(this.alg4tra);
            this.Controls.Add(this.alg3Clust);
            this.Controls.Add(this.alg2fol);
            this.Controls.Add(this.alg1agg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customH);
            this.Controls.Add(this.customW);
            this.Controls.Add(this.presetConfigs);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUIConfig";
            this.Text = "GUI Configuration";
            this.Load += new System.EventHandler(this.CamConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.ComboBox presetConfigs;
        private System.Windows.Forms.TextBox customW;
        private System.Windows.Forms.TextBox customH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox alg1agg;
        private System.Windows.Forms.CheckBox alg2fol;
        private System.Windows.Forms.CheckBox alg3Clust;
        private System.Windows.Forms.CheckBox alg4tra;
        private System.Windows.Forms.CheckBox alg0gos;
        private System.Windows.Forms.CheckBox directions;
        private System.Windows.Forms.CheckBox camen;
        private System.Windows.Forms.Label label2;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.CheckBox iren;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.Label label6;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox advMotion;
    }
}