using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimUI {
    public partial class SwarmConfig : Form {

        int swarmS = 0;
        
        public SwarmConfig() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                swarmS = Convert.ToInt32(sS.Text);
                if (swarmS < 0) {
                    swarmS = 0;
                }
            }catch (Exception ex) {
                swarmS = 0;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void CheckEnter(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                button1.PerformClick();
            }
        }

        public int getSwarmSize() {
            return swarmS;
        }

        private void SwarmConfig_Load(object sender, EventArgs e) {
            this.ActiveControl = sS;
        }

    }
}
