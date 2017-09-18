using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenu {
    public partial class MainMenuConfigs : Form {
        bool simV;
        
        public MainMenuConfigs() {
            InitializeComponent();
        }

        public MainMenuConfigs(bool x) {
            InitializeComponent();
            visSim.Checked = x;
        }

        private void ok_button_Click(object sender, EventArgs e) {
            simV = visSim.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void visSim_CheckedChanged(object sender, EventArgs e) {
            simV = visSim.Checked;
        }

        public bool simVariableState {
            get {
                return simV;
            }
            set {
                simV = value;
            }
        }

    }
}
