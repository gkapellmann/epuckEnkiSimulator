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
    public partial class NewConfig : Form {

        General gen = new General();

        simConfigParameters localsCP;

        public NewConfig() {
            InitializeComponent();
            localsCP.userName = "JJDoe";
            localsCP.preDefPositions = "None";
            localsCP.evName = "Empty";

            localsCP.humInter = false;
            localsCP.aggeval = false;
            localsCP.othhalfeval = false;
            localsCP.clusteval = false;
            localsCP.hConType = false;
            localsCP.transportEval = false;
            localsCP.addRefOb = false;
            localsCP.addTransObj = false;
            localsCP.rndRefPos = false;
            localsCP.rndTnpPos = false;

            localsCP.usePreDefP = 0;
            localsCP.colorObjects = 0;
            localsCP.objectsPerColor = 0;
            localsCP.objectsPerColorHidden = 0;
            localsCP.amountBots = 20;
            localsCP.startingAlg = 1;
            localsCP.maxSimTime = 0;
            localsCP.simRepeat = 0;
            localsCP.RefORad = 5;
            localsCP.RefO_X = 20;
            localsCP.RefO_Y = 20;
            localsCP.RefColor = 0;               // Black...
            localsCP.amountTranspObj = 0;        // Amount of transport objects...
            localsCP.TnpORad = 15;
            localsCP.TnpO_X = 20;
            localsCP.TnpO_Y = 20;
            localsCP.TnpColor = 2;               // Blue...
            localsCP.TnpWeight = 30;
            localsCP.AlgFolVer = 0;
            localsCP.AlgClustVer = 1;

            localsCP.arenaType = false;
            localsCP.arenaX = 400;
            localsCP.arenaY = 300;
            localsCP.arenaR = 170;
        }

        public NewConfig(simConfigParameters x) {
            InitializeComponent();
            localsCP = x;
        }

        private void NewConfig_Load(object sender, EventArgs e) {
            cName.Text = localsCP.userName;
            prePositions.Text = localsCP.preDefPositions;
            enviroFileName.Text = localsCP.evName;

            hinter.Checked = localsCP.humInter;
            aggEval.Checked = localsCP.aggeval;
            othHalfEval.Checked = localsCP.othhalfeval;
            clustEval.Checked = localsCP.clusteval;
            transEval.Checked = localsCP.transportEval;
            humConType.Checked = localsCP.hConType;
            refobj.Checked = localsCP.addRefOb;
            addTrnsport.Checked = localsCP.addTransObj;
            refrandpos.Checked = localsCP.rndRefPos;
            tnprndpos.Checked = localsCP.rndTnpPos;

            //enviro.SelectedIndex = localsCP.environment;
            numbofcol.SelectedIndex = localsCP.colorObjects;
            objPerCol.Text = localsCP.objectsPerColor.ToString();
            mwgOtherObj.Text = localsCP.objectsPerColorHidden.ToString();    // objectsPerColorHidden
            aofBots.Text = localsCP.amountBots.ToString();
            algorithm.SelectedIndex = localsCP.startingAlg;
            sTime.Text = localsCP.maxSimTime.ToString();
            repeats.Text = localsCP.simRepeat.ToString();
            refposrad.Text = localsCP.RefORad.ToString();
            refposx.Text = localsCP.RefO_X.ToString();
            refposy.Text = localsCP.RefO_Y.ToString();
            refColor.SelectedIndex = localsCP.RefColor;
            trnspObjs.SelectedIndex = localsCP.amountTranspObj;  // ****** Update ******
            tnpR.Text = localsCP.TnpORad.ToString();
            tnpX.Text = localsCP.TnpO_X.ToString();
            tnpY.Text = localsCP.TnpO_Y.ToString();
            tnpColor.SelectedIndex = localsCP.TnpColor;
            tnpW.Text = localsCP.TnpWeight.ToString();
            folversion.SelectedIndex = localsCP.AlgFolVer;
            clustversions.SelectedIndex = localsCP.AlgClustVer;

            // ****** Batch values *******
            circArena.Checked = localsCP.arenaType;

            arenax.Text = localsCP.arenaX.ToString();
            arenay.Text = localsCP.arenaY.ToString();
            arenarad.Text = localsCP.arenaR.ToString();
            
            // ****** GUI Setup ******
            circSquareArenaEnabled();
            refObjEnabled();
            tnpObjEnabled();
        }

        private void closeandsave_Click(object sender, EventArgs e) {
            localsCP.userName = cName.Text;
            // preDefPositions
            // enviroName

            localsCP.humInter = hinter.Checked;
            localsCP.aggeval = aggEval.Checked;
            localsCP.othhalfeval = othHalfEval.Checked;
            localsCP.clusteval = clustEval.Checked;
            localsCP.transportEval = transEval.Checked;
            localsCP.hConType = humConType.Checked;
            localsCP.addRefOb = refobj.Checked;
            localsCP.addTransObj = addTrnsport.Checked;
            localsCP.rndRefPos = refrandpos.Checked;
            localsCP.rndTnpPos = tnprndpos.Checked;

            if (prePositions.Text.Equals("None")) {
                localsCP.usePreDefP = 0;
            } else {
                localsCP.usePreDefP = 1;
            }
            //localsCP.environment = 0;                                           // Future use...****************
            localsCP.colorObjects = numbofcol.SelectedIndex;
            localsCP.objectsPerColor = Convert.ToInt32(objPerCol.Text);
            localsCP.objectsPerColorHidden = Convert.ToInt32(mwgOtherObj.Text);
            if (localsCP.objectsPerColorHidden > localsCP.objectsPerColor) {
                localsCP.objectsPerColorHidden = localsCP.objectsPerColor;
            } else if (localsCP.objectsPerColorHidden < 0) {
                localsCP.objectsPerColorHidden = 0;
            }
            localsCP.amountBots = Convert.ToInt32(aofBots.Text);
            localsCP.startingAlg = algorithm.SelectedIndex;
            localsCP.maxSimTime = Convert.ToInt32(sTime.Text);
            localsCP.simRepeat = Convert.ToInt32(repeats.Text);
            localsCP.RefORad = Convert.ToInt32(refposrad.Text);
            localsCP.RefO_X = Convert.ToInt32(refposx.Text);
            localsCP.RefO_Y = Convert.ToInt32(refposy.Text);
            localsCP.RefColor = refColor.SelectedIndex;
            localsCP.amountTranspObj = trnspObjs.SelectedIndex;
            localsCP.TnpORad = Convert.ToInt32(tnpR.Text);
            localsCP.TnpO_X = Convert.ToInt32(tnpX.Text);
            localsCP.TnpO_Y = Convert.ToInt32(tnpY.Text);
            localsCP.TnpColor = tnpColor.SelectedIndex;
            localsCP.TnpWeight = Convert.ToInt32(tnpW.Text);
            localsCP.AlgFolVer = folversion.SelectedIndex;
            localsCP.AlgClustVer = clustversions.SelectedIndex;

            localsCP.arenaType = circArena.Checked;
            localsCP.arenaX = Convert.ToInt32(arenax.Text);
            localsCP.arenaY = Convert.ToInt32(arenay.Text);
            localsCP.arenaR = Convert.ToInt32(arenarad.Text);
         
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        // ****** Bool controls ******

        private void aggEval_CheckedChanged(object sender, EventArgs e) {
            
        }

        private void othHalfEval_CheckedChanged(object sender, EventArgs e) {
            //if (othHalfEval.Checked) {
            //    othhalfeval = true;
            //} else {
            //    othhalfeval = false;
            //}
        }

        private void clustEval_CheckedChanged(object sender, EventArgs e) {
            //if (clustEval.Checked) {
            //    clusteval = true;
            //} else {
            //    clusteval = false;
            //}
        }

        private void transEval_CheckedChanged(object sender, EventArgs e) {
            if (transEval.Checked) {
                tnpR.Enabled = true;
                tnpX.Enabled = true;
                tnpY.Enabled = true;
                tnpColor.Enabled = true;
                tnpW.Enabled = true;
            } else {
                tnpR.Enabled = false;
                tnpX.Enabled = false;
                tnpY.Enabled = false;
                tnpColor.Enabled = false;
                tnpW.Enabled = false;
            }
        }

        private void refobj_CheckedChanged(object sender, EventArgs e) {
            refObjEnabled();
        }

        private void addTrnsport_CheckedChanged(object sender, EventArgs e) {
            tnpObjEnabled();
        }

        private void humConType_CheckedChanged(object sender, EventArgs e) {
            //if (humConType.Checked) {
            //    humConT = true;
            //} else {
            //    humConT = false;
            //}
        }
        
        // ******

        private void algorithm_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.startingAlg = algorithm.SelectedIndex;
        }

        private void enviro_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.environment = enviro.SelectedIndex;
        }

        private void folversion_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.AlgFolVer = folversion.SelectedIndex;
        }

        private void clustversions_SelectedIndexChanged(object sender, EventArgs e) {
            //clustVer = clustversions.SelectedIndex;
        }

        private void numbofcol_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.colorObjects = numbofcol.SelectedIndex;
        }

        private void loadConfig_Click(object sender, EventArgs e) {
            OpenFileDialog oFDialog = new OpenFileDialog();
            oFDialog.Title = "Load Pre-Defined Positions";
            oFDialog.Filter = "Positions (CSV) |*.csv*";
            oFDialog.InitialDirectory = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Data");
            if (oFDialog.ShowDialog() == DialogResult.OK) {                                         // Test result.
                string file = oFDialog.FileName;
                int pos = file.LastIndexOf("\\") + 1;
                file = file.Substring(pos, file.Length - pos);
                pos = file.LastIndexOf(".");
                file = file.Substring(0, pos);
                prePositions.Text = file;
                localsCP.preDefPositions = file;
            }
        }

        private void loadenviro_Click(object sender, EventArgs e) {
            OpenFileDialog oFDialog = new OpenFileDialog();
            oFDialog.Title = "Load Environment Setting";
            oFDialog.Filter = "Environments |*.txt*";
            oFDialog.InitialDirectory = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Data\Environments");
            if (oFDialog.ShowDialog() == DialogResult.OK) {                                         // Test result.
                string file = oFDialog.FileName;
                int pos = file.LastIndexOf("\\") + 1;
                file = file.Substring(pos, file.Length - pos);
                pos = file.LastIndexOf(".");
                file = file.Substring(0, pos);
                enviroFileName.Text = file;
                localsCP.evName = file;
            }
        }

        private void randPos_Click(object sender, EventArgs e) {
            string file = "None";
            prePositions.Text = file;
            localsCP.preDefPositions = file;
        }

        private void circArena_CheckedChanged(object sender, EventArgs e) {
            circSquareArenaEnabled();
        }

        private void circSquareArenaEnabled() {
            if (circArena.Checked) {          // Circular arena...
                arenarad.Enabled = true;
                arenax.Enabled = false;
                arenay.Enabled = false;
            } else {                          // Squared Arena...
                arenarad.Enabled = false;
                arenax.Enabled = true;
                arenay.Enabled = true;
            }
        }

        private void refObjEnabled() {
            if (refobj.Checked) {
                refColor.Enabled = true;
                refposx.Enabled = true;
                refposy.Enabled = true;
                refposrad.Enabled = true;
                refrandpos.Enabled = true;
            } else {
                refColor.Enabled = false;
                refposx.Enabled = false;
                refposy.Enabled = false;
                refposrad.Enabled = false;
                refrandpos.Enabled = false;
            }
        }

        private void tnpObjEnabled() {
            if (addTrnsport.Checked) {
                tnpX.Enabled = true;
                tnpY.Enabled = true;
                tnpR.Enabled = true;
                tnpW.Enabled = true;
                tnpColor.Enabled = true;
                tnprndpos.Enabled = true;
            } else {
                tnpX.Enabled = false;
                tnpY.Enabled = false;
                tnpR.Enabled = false;
                tnpW.Enabled = false;
                tnpColor.Enabled = false;
                tnprndpos.Enabled = false;
            }
        }

        // ****** Text Control Functions **********************************************

        private void arenax_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void arenay_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void arenarad_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void aofBots_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void aofBots_Leave(object sender, EventArgs e) {
            localsCP.amountBots = Convert.ToInt32(aofBots.Text);
            if (localsCP.amountBots > 100) {
                localsCP.amountBots = 100;
            } else if (localsCP.amountBots == 0) {
                localsCP.amountBots = 1;
            }
            aofBots.Text = localsCP.amountBots.ToString();
        }

        private void repeats_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void sTime_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void objPerCol_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void mwgSize_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void mwgOtherObj_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void mwgOtherObj_Leave(object sender, EventArgs e) {
            localsCP.objectsPerColorHidden = Convert.ToInt32(mwgOtherObj.Text);
            if (localsCP.objectsPerColorHidden > localsCP.objectsPerColor) {
                localsCP.objectsPerColorHidden = localsCP.objectsPerColor;
            } else if (localsCP.objectsPerColorHidden < 0) {
                localsCP.objectsPerColorHidden = 0;
            }
        }

        // ****** Reference Object ********************************************
        private void refposx_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }
        
        private void refposx_Leave(object sender, EventArgs e) {
            localsCP.RefO_X = Convert.ToInt32(refposx.Text);
            if (localsCP.RefO_X > localsCP.arenaX - localsCP.RefORad) {
                localsCP.RefO_X = localsCP.arenaX - localsCP.RefORad;
            } else if (localsCP.RefO_X <= localsCP.RefORad) {
                localsCP.RefO_X = localsCP.RefORad;
            }
            refposx.Text = localsCP.RefO_X.ToString();
        }

        private void refposy_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void refposy_Leave(object sender, EventArgs e) {
            localsCP.RefO_Y = Convert.ToInt32(refposy.Text);
            if (localsCP.RefO_Y > localsCP.arenaY - localsCP.RefORad) {
                localsCP.RefO_Y = localsCP.arenaY - localsCP.RefORad;
            } else if (localsCP.RefO_Y <= localsCP.RefORad) {
                localsCP.RefO_Y = localsCP.RefORad;
            }
            refposy.Text = localsCP.RefO_Y.ToString();
        }

        private void refposrad_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void refposrad_Leave(object sender, EventArgs e) {
            localsCP.RefORad = Convert.ToInt32(refposrad.Text);
        }

        private void refColor_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.RefColor = refColor.SelectedIndex;
        }

        // ****** Transport Object ********************************************
        private void trnspObjs_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.amountTranspObj = trnspObjs.SelectedIndex;
        }

        private void tnpColor_SelectedIndexChanged(object sender, EventArgs e) {
            //localsCP.TnpColor = tnpColor.SelectedIndex;
        }

        private void tnpX_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void tnpX_Leave(object sender, EventArgs e) {
            localsCP.TnpO_X = Convert.ToInt32(tnpX.Text);
            if (localsCP.TnpO_X > localsCP.arenaX - localsCP.TnpORad) {
                localsCP.TnpO_X = localsCP.arenaX - localsCP.TnpORad;
            } else if (localsCP.TnpO_X <= localsCP.TnpORad) {
                localsCP.TnpO_X = localsCP.TnpORad;
            }
            tnpX.Text = localsCP.TnpO_X.ToString();
        }

        private void tnpY_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void tnpY_Leave(object sender, EventArgs e) {
            localsCP.TnpO_Y = Convert.ToInt32(tnpY.Text);
            if (localsCP.TnpO_Y > localsCP.arenaY - localsCP.TnpORad) {
                localsCP.TnpO_Y = localsCP.arenaY - localsCP.TnpORad;
            } else if (localsCP.TnpO_Y <= localsCP.TnpORad) {
                localsCP.TnpO_Y = localsCP.TnpORad;
            }
            tnpY.Text = localsCP.TnpO_Y.ToString();
        }

        private void tnpR_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        private void tnpR_Leave(object sender, EventArgs e) {
            localsCP.TnpORad = Convert.ToInt32(tnpR.Text);
        }

        private void tnpW_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = gen.checkForOnlyNumbers(e);
        }

        // ****** Gets & Sets ******

        public simConfigParameters variables {
            get{
                return this.localsCP;
            }
            set{
                this.localsCP = value;
            }
        }

        }
}
