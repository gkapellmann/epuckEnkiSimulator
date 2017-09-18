using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Generals;

namespace SimUI {
    public partial class GUIConfig : Form {

        General gen = new General();

        int ctmW = 0;
        int ctmH = 0;
        bool[] enControls = new bool[8];                //0-Alg0Gos, 1-Alg1Agg, 2-Alg2Fol, 3-Alg3ObC, 4-Alg4CTr, 5-Directions, 6-Camera, 7-IRSensors...
        bool advHumCntl;

        public GUIConfig() {
            InitializeComponent();
        }

        private void CamConfig_Load(object sender, EventArgs e){
            for (int i = 0; i < enControls.Length; i++) { 
                loadAndSetEnables(i, enControls[i]);
            }
            customH.Enabled = false;
            customW.Enabled = false;
        }

        private void loadAndSetEnables(int i, bool val) {
            switch(i){
                case 0:
                    alg0gos.Checked = val;
                    break;
                case 1:
                    alg1agg.Checked = val;
                    break;
                case 2:
                    alg2fol.Checked = val;
                    break;
                case 3:
                    alg3Clust.Checked = val;
                    break;
                case 4:
                    alg4tra.Checked = val;
                    break;
                case 5:
                    directions.Checked = val;
                    break;
                case 6:
                    camen.Checked = val;
                    break;
                case 7:
                    iren.Checked = val;
                    break;
                default:
                    break;
            }
        }

        private void ok_Click(object sender, EventArgs e) {
            if (presetConfigs.SelectedIndex == 0){
                try { 
                    ctmW = Convert.ToInt32(customW.Text);
                    ctmH = Convert.ToInt32(customH.Text);
                }catch(Exception ex){
                    MessageBox.Show("Error","Error: " + ex.StackTrace);
                }
            } 
            statusFile("Status.txt");
            this.DialogResult = DialogResult.OK;
        }

        private void customW_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void customH_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void presetConfigs_SelectedIndexChanged(object sender, EventArgs e) {
            if(presetConfigs.SelectedIndex == 0){
                customH.Enabled = true;
                customW.Enabled = true;
            }else{
                customH.Enabled = false;
                customW.Enabled = false;
            }
        }

        // ****** Txt File Functions ***************************************************************

        private void statusFile(string nameFile) {
            List<string> param = new List<string>();
            param.Add("Gossip: *" + gen.translateBool(enControls[0]));
            param.Add("Aggregation: *" + gen.translateBool(enControls[1]));
            param.Add("Follower: *" + gen.translateBool(enControls[2]));
            param.Add("Object CLustering: *" + gen.translateBool(enControls[3]));
            param.Add("Collective Transport: *" + gen.translateBool(enControls[4]));
            param.Add("Directions: *" + gen.translateBool(enControls[5]));
            param.Add("Camera: *" + gen.translateBool(enControls[6]));
            param.Add("IR Sensors: *" + gen.translateBool(enControls[7]));
            param.Add("Camera Resolution: -" + presetConfigs.SelectedIndex);
            File.WriteAllLines(@nameFile, param);                               // Saves all the string elements in a same file (Encoding UTF-8)...
        }        

        private void alg0gos_CheckedChanged(object sender, EventArgs e) {
            enControls[0] = alg0gos.Checked;
        }

        private void alg1agg_CheckedChanged(object sender, EventArgs e) {
            enControls[1] = alg1agg.Checked;
        }

        private void alg2fol_CheckedChanged(object sender, EventArgs e) {
            enControls[2] = alg2fol.Checked;
        }

        private void alg3Clust_CheckedChanged(object sender, EventArgs e) {
            enControls[3] = alg3Clust.Checked;
        }

        private void alg4tra_CheckedChanged(object sender, EventArgs e) {
            enControls[4] = alg4tra.Checked;
        }

        private void directions_CheckedChanged(object sender, EventArgs e) {
            enControls[5] = directions.Checked;
        }

        private void camen_CheckedChanged(object sender, EventArgs e) {
            enControls[6] = camen.Checked;
        }

        private void iren_CheckedChanged(object sender, EventArgs e) {
            enControls[7] = iren.Checked;
        }

        private void advMotion_CheckedChanged(object sender, EventArgs e) {
            advHumCntl = advMotion.Checked;
        }

        // ******  Gets & Sets ********************************************************************

        public void setAlg0GosEn(bool x) {
            enControls[0] = x;
        }

        public void setAlg1AggEn(bool x) {
            enControls[1] = x;
        }

        public void setAlg2FolEn(bool x) {
            enControls[2] = x;
        }

        public void setAlg3ObCEn(bool x) { 
            enControls[3] = x;
        }

        public void setAlg4CTrEn(bool x) {
            enControls[4] = x;
        }

        public void setDirectionsEn(bool x) {
            enControls[5] = x;
        }

        public void setCameraEn(bool x) {
            enControls[6] = x;
        }

        public void setIRSensorsEn(bool x) {
            enControls[7] = x;
        }

        public void setAdvMotion(bool x) {
            advHumCntl = x;
        }

        public void setCameraRes(int x) {
            presetConfigs.SelectedIndex = x;
        }

        public int getCameraRes(){
            return presetConfigs.SelectedIndex;
        }

        public int getNewCamHeight() {
            return ctmH;
        }

        public int getNewCamWidth() {
            return ctmW;
        }

        public bool getAlg0GosEn() {
            return enControls[0];
        }

        public bool getAlg1AggEn() {
            return enControls[1];
        }

        public bool getAlg2FolEn() {
            return enControls[2];
        }

        public bool getAlg3ObCEn() {
            return enControls[3];
        }

        public bool getAlg4CTrEn() {
            return enControls[4];
        }

        public bool getDirectionsEn() {
            return enControls[5];
        }

        public bool getCameraEn() {
            return enControls[6];
        }

        public bool getIRSensorsEn() {
            return enControls[7];
        }

        public bool getAdvMotion() {
            return advHumCntl;
        }

    }
}
