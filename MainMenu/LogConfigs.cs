using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Generals;

namespace MainMenu {
    public partial class LogConfigs : Form {

        General gen = new General();
        
        public LogConfigs(int startT, int stepT, int fc) {
            InitializeComponent();
            flags = fc;
            starttime.Text = startT.ToString();
            steptime.Text = stepT.ToString();
            starttime_s.Text = (startT / 1000).ToString();
            steptime_s.Text = (stepT / 1000).ToString();
        }

        public int flags { get; set; }
        public int startTime { get; set; }
        public int stepTime { get; set; }

        private void LogConfigs_Load(object sender, EventArgs e) {
            if(gen.IsBitSet(flags,0)){
                time_ms.Checked = true;
            }
            if (gen.IsBitSet(flags, 1)) {
                time_s.Checked = true;
            }
            if (gen.IsBitSet(flags, 2)) {
                allclust.Checked = true;
            }
            if (gen.IsBitSet(flags, 3)) {
                bigclust.Checked = true;
            }
            if (gen.IsBitSet(flags, 4)) {
                secArea.Checked = true;
            }
            if (gen.IsBitSet(flags, 5)) {
                redObj.Checked = true;
            }
            if (gen.IsBitSet(flags, 6)) {
                blueobj.Checked = true;
            }
            if (gen.IsBitSet(flags, 7)) {
                greenobj.Checked = true;
            }
            if (gen.IsBitSet(flags, 8)) {
                purpleobj.Checked = true;
            }
            if (gen.IsBitSet(flags, 9)) {
                averageobj.Checked = true;
            }
            if (gen.IsBitSet(flags, 10)) {
                xpos.Checked = true;
            }
            if (gen.IsBitSet(flags, 11)) {
                ypos.Checked = true;
            }
            if (gen.IsBitSet(flags, 12)) {
                apos.Checked = true;
            }
            if (gen.IsBitSet(flags, 13)) {
                titles.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            startTime = Convert.ToInt32(starttime.Text);
            stepTime = Convert.ToInt32(steptime.Text);
            flags = 0;
            if (time_ms.Checked) {
                flags |= (1 << 0);
            } 
            if (time_s.Checked) {
                flags |= (1 << 1);
            }
            if (allclust.Checked) {
                flags |= (1 << 2);
            }
            if (bigclust.Checked) {
                flags |= (1 << 3);
            }
            if (secArea.Checked) {
                flags |= (1 << 4);
            }
            if (redObj.Checked) {
                flags |= (1 << 5);
            }
            if (blueobj.Checked) {
                flags |= (1 << 6);
            }
            if (greenobj.Checked) {
                flags |= (1 << 7);
            }
            if (purpleobj.Checked) {
                flags |= (1 << 8);
            }
            if (averageobj.Checked) {
                flags |= (1 << 9);
            }
            if (xpos.Checked) {
                flags |= (1 << 10);
            }
            if (ypos.Checked) {
                flags |= (1 << 11);
            }
            if (apos.Checked) {
                flags |= (1 << 12);
            }
            if (titles.Checked) {
                flags |= (1 << 13);
            }
            //MessageBox.Show(flags.ToString());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void starttime_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void starttime_s_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void starttime_s_TextChanged(object sender, EventArgs e) {
            try { 
                int temp = Convert.ToInt32(starttime_s.Text);
                temp *= 1000;
                starttime.Text = temp.ToString();
            }catch(Exception ex){
                starttime.Text = "0";
            }
        }

        private void starttime_s_Leave(object sender, EventArgs e) {
            if(starttime_s.Text.Equals("")){
                starttime_s.Text = "10";
            }
        }

        private void steptime_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void steptime_s_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void steptime_s_TextChanged(object sender, EventArgs e) {
            try {
                int temp = Convert.ToInt32(steptime_s.Text);
                temp *= 1000;
                steptime.Text = temp.ToString();
            } catch (Exception ex) {
                steptime.Text = "0";
            }
        }

        private void steptime_s_Leave(object sender, EventArgs e) {
            if (steptime_s.Text.Equals("")) {
                steptime_s.Text = "1";
            }
        }

    }
}
