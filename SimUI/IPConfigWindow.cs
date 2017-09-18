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
    public partial class IPConfigWindow : Form {

        String address;

        public IPConfigWindow() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            address = ip1.Text + "." + ip2.Text + "." + ip3.Text + "." + ip4.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e) {
            address = "127.0.0.1";
            this.DialogResult = DialogResult.OK;
        }

        public String getTheAddressResult() {
            return address;
        }

        private void IPConfigWindow_Load(object sender, EventArgs e) {
            this.ActiveControl = ip1;
        }

        private void ip1_TextChanged(object sender, EventArgs e) {
            if (ip1.Text.Length == 3) {
                ip2.Select();
            }
        }

        private void ip2_TextChanged(object sender, EventArgs e) {
            if (ip2.Text.Length == 3) {
                ip3.Select();
            }
        }

        private void ip3_TextChanged(object sender, EventArgs e) {
            if (ip3.Text.Length == 3) {
                ip4.Select();
            }
        }

        private void ip4_TextChanged(object sender, EventArgs e) {
            if (ip4.Text.Length == 3) {
                button1.Select();
            }
        }

        private void ip1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter) {
                e.Handled = true;
                button2.PerformClick();
            }
        }

        private void ip2_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void ip3_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void ip4_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
        }

    }
}
