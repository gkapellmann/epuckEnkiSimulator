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
    public partial class VideoOptions : Form {

        General gen = new General();

        public int jmpFr { get; set; }
        public bool svid { get; set; }
        public bool simg { get; set; }

        public VideoOptions(int x, bool y, bool z) {
            InitializeComponent();
            jmpFr = x;
            simg = y;
            svid = z;
        }

        private void VideoOptions_Load(object sender, EventArgs e) {
            jFr.Text = jmpFr.ToString();
            svideo.Checked = svid;
            simages.Checked = simg;
        }

        private void button1_Click(object sender, EventArgs e) {
            jmpFr = Convert.ToInt32(jFr.Text);
            svid = svideo.Checked;
            simg = simages.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void jFr_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void jFr_Leave(object sender, EventArgs e) {
            if (jFr.Text.Equals("")) {
                jFr.Text = "5";
            }
        }

        

    }
}
