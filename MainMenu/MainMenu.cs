using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

using Generals;
using General_Logger;


public struct simConfigParameters {
    public string userName;             // User name
    public string preDefPositions;
    public string evName;
    
    public bool humInter;
    public bool aggeval;
    public bool othhalfeval;
    public bool clusteval;
    public bool transportEval;
    public bool hConType;               // Human Control Type
    public bool rndRefPos;
    public bool addRefOb;               // Black Object
    public bool rndTnpPos;
    public bool addTransObj;            // Add transport object..

    public int usePreDefP;
    public int colorObjects;
    public int objectsPerColor;
    public int objectsPerColorHidden;   // 
    public int amountBots;
    public int startingAlg;
    public int maxSimTime;
    public int simRepeat;    
    public int RefORad;
    public int RefO_X;
    public int RefO_Y;
    public int RefColor;
    public int amountTranspObj;         // Amount of transport objects...
    public int TnpORad;
    public int TnpO_X;
    public int TnpO_Y;
    public int TnpColor;
    public int TnpWeight;
    public int AlgFolVer;
    public int AlgClustVer;             // Alg-COj-Ver

    // ****** These values need to be defined in the batch file ******
    public bool arenaType;              // True = Circular, False = Square...

    public int arenaX;
    public int arenaY;
    public int arenaR;
};

namespace MainMenu {

    public partial class mainMenu : Form {

        General gen = new General();
        Summary sLog = new Summary();
        Video_Builder vLog = new Video_Builder();

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;

        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

        simConfigParameters sCP;

        bool cLoaded = false;                           // Other configuration loaded...
        bool saveVideo = false;
        bool saveImages = false;

        string cmdLoc = "Commands";
        string posLoc = "Positions";
        string fileMName = "JJDoe";
        string ownMark = "Var1";

        bool simulationVisible = true;
        int startTime_ms = 10000;
        int timeStep_ms = 10000;
        int framesJump = 5;                             //  Jump between frames process and frames ignored...
        int flagConfig = int.MaxValue;

        String userUniqueID = "";
        String[] simArgs = new String[4];

        Thread logger;

        List<string> configs = new List<string>();      // 0-Last Config file uesd...

        public mainMenu() {
            try {
                InitializeComponent();
            } catch (Exception ex) {
                using (StreamWriter outfile = new StreamWriter(@".\MainMenu_Error.txt")) {
                    outfile.Write(ex.Message.ToString());
                }
            }
        }

        ~mainMenu() { 
            
        }

        private void mainMenu_Load(object sender, EventArgs e) {
            prog1.Checked = true;
            remcontrols.Enabled = false;
            remcontrols.Visible = false;
            logControls.Enabled = false;
            logControls.Visible = false;

            cmdfolder.Text = "Logger\\Commands";
            posfolder.Text = "Logger\\Positions";
            fileLocation.Text = "Logger\\Positions";

            var fileStream = new FileStream(@"Data\MainConfig.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream)) {
                string line;
                while ((line = streamReader.ReadLine()) != null) {
                    //int pos = line.LastIndexOf(":") + 2;
                    //configs.Add(line.Substring(pos, line.Length - pos));
                    configs.Add(line);
                }
            }
            try { 
                configName.Text = configs[0];       // Set last configuration used...
                //ready = load_scp_data(Environment.CurrentDirectory + "\\UserData\\" + configs[0]);
                load_scp_data(Environment.CurrentDirectory + "\\UserData\\" + configs[0]);
                fileMName = configs[1];
                startTime_ms = Convert.ToInt32(configs[2]);
                timeStep_ms = Convert.ToInt32(configs[3]);
                flagConfig = Convert.ToInt32(configs[4]);
                ownMark = configs[5];
                simulationVisible = Convert.ToBoolean(configs[6]);
                framesJump = Convert.ToInt32(configs[7]);
                saveImages = Convert.ToBoolean(configs[8]);
                saveVideo = Convert.ToBoolean(configs[9]);
            }catch(Exception ex){
                configName.Text = "JJDoe";       // Set last configuration used...
                fileMName = "Tester";
                startTime_ms = 10000;
                timeStep_ms = 1000;
                flagConfig = int.MaxValue;
                ownMark = "Var1";
                simulationVisible = true;
                framesJump = 5;
                saveImages = true;
                saveVideo = true;
                MessageBox.Show(ex.Message);
            }
            fileMarkName.Text = fileMName;
            ownmark.Text = ownMark;
            run_exe.Enabled = false;

            WindowState = FormWindowState.Maximized;
            
            // To work later...
            notes.Enabled = false;
            button2.Enabled = false;

        }

        private void mainMenu_FormClosed(object sender, FormClosedEventArgs e) {
            if (IsProcessOpen("PlaygroundQT_ST")) {
                closeRunningProcess("PlaygroundQT_ST");
            }
            if (IsProcessOpen("SimUI")) {
                closeRunningProcess("SimUI");
            }
            if (logger != null) { 
                if (logger.IsAlive) {
                    logger.Join();
                }
            }
            Thread.Sleep(100);                              // Needed for the background workers to end...
            try {
                configs[0] = configName.Text;       
                configs[1] = fileMName;
                configs[2] = startTime_ms.ToString();
                configs[3] = timeStep_ms.ToString();
                configs[4] = flagConfig.ToString();
                configs[5] = ownMark;
                configs[6] = simulationVisible.ToString();
                configs[7] = framesJump.ToString();
                configs[8] = saveImages.ToString();
                configs[9] = saveVideo.ToString();
                string path = @"Data\MainConfig.txt";
                File.WriteAllLines(path, configs);
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        // ****** Inteface Controls ****************************************************************

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            MainMenuConfigs form;
            form = new MainMenuConfigs(simulationVisible);
            var result = form.ShowDialog();
            if (result == DialogResult.OK) {
                simulationVisible = form.simVariableState;
            }
        }

        private void prog1_CheckedChanged(object sender, EventArgs e) {
            if (prog1.Checked) {
                simControls.Visible = true;
                simControls.Enabled = true;
            } else {
                simControls.Visible = false;
                simControls.Enabled = false;
            }
        }

        private void prog2_CheckedChanged(object sender, EventArgs e) {
            if (prog2.Checked) {
                remcontrols.Visible = true;
                remcontrols.Enabled = true;
            } else {
                remcontrols.Visible = false;
                remcontrols.Enabled = false;
            }
        }

        private void prog3_CheckedChanged(object sender, EventArgs e) {
            if (prog3.Checked) {
                logControls.Visible = true;
                logControls.Enabled = true;
            } else {
                logControls.Visible = false;
                logControls.Enabled = false;
            }
        }

        private void loadSimArgs() {
            try {
                StreamReader file = new StreamReader(@"Data\ArgsConfig.txt");
                String line = "";
                int next = 0;
                while ((line = file.ReadLine()) != null) {
                    simArgs[next] = line;
                    next++;
                }
                file.Close();
            } catch (Exception ex) {
                simArgs[0] = "0";
                simArgs[1] = "100";
                simArgs[2] = "100";
                simArgs[3] = "100";
            }
        }

        private void simulator_exe_Click(object sender, EventArgs e) {                              // Start a separate process (.exe) and catch the return value...
            if (prog1.Checked) {                                                                    // Run the simulator...
                if (!IsProcessOpen("PlaygroundQT_ST")) {
                    Process ProSim = new Process();
                    ProcessStartInfo stInfo = new ProcessStartInfo();
                    stInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    loadSimArgs();                                                                  // Loads the arguments to run the simulations...
                    if (simArgs[0].Equals("0")) {                                                   // Square or circle arena...
                        stInfo.Arguments = String.Format("{0} {1} {2} {3} {4}", simArgs[0], simArgs[1], simArgs[2], userUniqueID, sCP.evName);       // Square...
                    } else {
                        stInfo.Arguments = String.Format("{0} {1} {2} {3} {4}", simArgs[0], simArgs[3], "0", userUniqueID, sCP.evName);              // Circle...
                    }
                    stInfo.FileName = @"Data\DLL_Dependencies\QtDLLs\PlaygroundQT_ST.exe";
                    ProSim = Process.Start(stInfo);
                    if (!simulationVisible) {
                        bool nextStep = false;
                        while (!nextStep) {
                            Process[] pname = Process.GetProcessesByName("PlaygroundQT_ST");
                            if (pname.Length != 0) {
                                nextStep = true;
                            }
                            Thread.Sleep(5);
                        }
                        for (int i = 0; i < 5; i++) {
                            Thread.Sleep(100);
                            this.BringToFront();
                            this.Activate();
                        }
                    }
                    toolStripStatusLabel1.Text = "Enki Simulator running...";
                } else {
                    MessageBox.Show("Already running");
                }
            } else if (prog2.Checked) {                                                             // Run the remote control...
                Process ProRem = new Process();
                ProcessStartInfo stInfo = new ProcessStartInfo();
                stInfo.WorkingDirectory = "SimGUI";
                stInfo.FileName = "SimUI.exe";
                ProRem = Process.Start(stInfo);
                toolStripStatusLabel1.Text = "Remote running...";
            } 
        }

        // ****** Simulator ************************************************************************

        private void loadConfig_Click(object sender, EventArgs e) {                                 // Load an already existing configuration...
            OpenFileDialog oFDialog = new OpenFileDialog();
            oFDialog.Title = "Open a previously saved configuration";
            oFDialog.Filter = "Configuration File |*.txt*";
            oFDialog.InitialDirectory = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"UserData");
            if (oFDialog.ShowDialog() == DialogResult.OK) {                                         // Test result.
                string file = oFDialog.FileName;
                cLoaded = load_scp_data(file);                                                      // Load data into the structure register...
                if (cLoaded) {
                    int pos = file.LastIndexOf("\\") + 1;                                           // Clean the file string to get only the name...                                         
                    file = file.Substring(pos, file.Length - pos);                                  // ******
                    configName.Text = file;                                                         // Save the file name...
                    configs[0] = file;                                                              // Save for logging...
                    toolStripStatusLabel1.Text = "Configuration loaded correctly.";
                    showSummary(sCP);
                } else { 
                    toolStripStatusLabel1.Text = "Configuration not loaded. Check log for error details.";
                }
            }
            saveConfig.Enabled = false;
        }

        private void saveConfig_Click(object sender, EventArgs e) {
            SaveFileDialog oFDialog = new SaveFileDialog();
            oFDialog.Title = "Save configuration";
            oFDialog.Filter = "Configuration File |*.txt*";
            oFDialog.InitialDirectory = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"UserData");
            if (oFDialog.ShowDialog() == DialogResult.OK) {                                         // Test result.
                string file = oFDialog.FileName;
                if(!file.Contains(".txt")){
                    file += ".txt";
                }
                createInitFile(sCP, file, true);
            }
        }

        private void newConfig_Click(object sender, EventArgs e) {                                  // Receive the new configuration format...
            NewConfig form;
            if (cLoaded) {
                form = new NewConfig(sCP);
            } else {
                form = new NewConfig();
            }
            var result = form.ShowDialog();
            if (result == DialogResult.OK) {
                sCP = form.variables;
                saveConfig.Enabled = true;
                configName.Text = sCP.userName;
                createInitFile(sCP, "Data\\InitConfig.txt", false);
                string fN = "Data\\Environments\\" + sCP.evName + ".txt";
                copyFileToLocation(fN, "Data\\Environment.txt");
                updateArgumentsConfig(sCP);
                showSummary(sCP);
            }
        }

        private bool load_scp_data(string theFileName) {
            bool correct = false;
            string line;
            try { 
                StreamReader file = new StreamReader(theFileName);
                while ((line = file.ReadLine()) != null) {
                    try {
                        if(line.Contains(":")){
                            int configIndex = Convert.ToInt32(line.Substring(0, 2));
                            int pos = line.IndexOf(":") + 1;                                    // Finds the position of ":" and sets the index in the next position...
                            string temp = line.Substring(pos, line.Length - pos);               // We get the rest of the line...
                            temp = temp.Trim();                                                 // Gets rid of the sapces before and after the first and last characters (gets rid of \r\n too)... 
                            temp = temp.Trim(new Char[] { ' ', '*' });                          // Erase empty spaces and * if existant...
                            fill_struct(configIndex, temp);                                     // Fill the SCP struct with the value...
                        }else if(line.Contains("-")){
                            int pos = line.IndexOf("-") + 1;                                    // Finds the position of "-" and sets the index in the next position...
                            string temp = line.Substring(pos, line.Length - pos);               // We get the rest of the line...
                            temp = temp.Trim();
                            if(line.Contains("X")){
                                sCP.arenaX = Convert.ToInt32(temp);
                            }else if (line.Contains("Y")) {
                                sCP.arenaY = Convert.ToInt32(temp);
                            }else if (line.Contains("R")) {
                                sCP.arenaR = Convert.ToInt32(temp);
                            } else if (line.Contains("Type")) {
                                if (temp.Equals("1")) {
                                    sCP.arenaType = true;
                                } else {
                                    sCP.arenaType = false;
                                }
                            }
                        }
                    } catch (ArgumentOutOfRangeException e) {
                        //MessageBox.Show("Empty line...");
                    } catch (FormatException e){
                        //MessageBox.Show("Usless Line, for now...");
                    } catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                    }
                }
                createInitFile(sCP, @"Data\InitConfig.txt", false);
                string fN = "Data\\Environments\\" + sCP.evName + ".txt";
                copyFileToLocation(fN, "Data\\Environment.txt");
                updateArgumentsConfig(sCP);
                correct = true;
                file.Close();    
            }catch(FileNotFoundException){
                //MessageBox.Show("Old file not found...");
            }catch(Exception ex){
                MessageBox.Show(ex.ToString());
                correct = false;
            }
            return correct;
        }

        // ****** Remote Control *******************************************************************

        // ****** Logger ***************************************************************************

        private void LogAccept_Click(object sender, EventArgs e) {
            imageWorkProgress.Value = 0;
            fileMName = fileMarkName.Text;
            ownMark = ownmark.Text;
            cmdLoc = Path.Combine(System.Windows.Forms.Application.StartupPath, cmdfolder.Text);
            posLoc = Path.Combine(System.Windows.Forms.Application.StartupPath, posfolder.Text);

            logger = new Thread(logProcess);
            logger.Start();
            //sLog.process_Summary(this, cmdLoc, posLoc, fileMName, ownMark, startTime_ms, timeStep_ms, flagConfig);
            //try {
            //    logSum.Text = File.ReadAllText(@"Data\Logger_Summary.txt");                 // Read all the .txt file...
            //} catch (Exception ex) {
            //    MessageBox.Show("Log information not found...");
            //}
            //MessageBox.Show("Commands and Positions logging finished!", "Logging", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void logProcess() {
            sLog.process_Summary(this, cmdLoc, posLoc, fileMName, ownMark, startTime_ms, timeStep_ms, flagConfig);
            try {
                logSum.Invoke((System.Action)delegate { logSum.Text = File.ReadAllText(@"Data\Logger_Summary.txt"); });
            } catch (Exception ex) {
                MessageBox.Show("Log information not found...");
            }
        }

        private void changecmd_Click(object sender, EventArgs e) {
            FolderBrowserDialog fDialog = new FolderBrowserDialog();
            fDialog.Description = "Select folder where the command logs can be found.";
            fDialog.SelectedPath = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Logger");
            if (fDialog.ShowDialog() == DialogResult.OK) {
                //String localPath = gen.getLocalFolderDir(fDialog.SelectedPath);
                //cmdfolder.Text = localPath;
                //cmdLoc = localPath;
                cmdfolder.Text = fDialog.SelectedPath;
                cmdLoc = fDialog.SelectedPath;
            }
        }

        private void changepos_Click(object sender, EventArgs e) {
            FolderBrowserDialog fDialog = new FolderBrowserDialog();
            fDialog.Description = "Select folder where the position logs can be found.";
            fDialog.SelectedPath = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Logger");
            if (fDialog.ShowDialog() == DialogResult.OK) {
                //String localPath = gen.getLocalFolderDir(fDialog.SelectedPath);
                //posfolder.Text = localPath;
                //posLoc = localPath;
                posfolder.Text = fDialog.SelectedPath;
                posLoc = fDialog.SelectedPath;
            }
        }

        private void logConfig_Click(object sender, EventArgs e) {
            LogConfigs form;
            form = new LogConfigs(startTime_ms, timeStep_ms, flagConfig);
            var result = form.ShowDialog();
            if (result == DialogResult.OK) {
                flagConfig = form.flags;
                startTime_ms = form.startTime;
                timeStep_ms = form.stepTime;
            }
        }

        // ****** Algorithms ***********************************************************************

        private void Algorithms_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

        }

        private void Algorithms_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {  // Runs in the main thread!!!...
            if ((e.Cancelled == true)) {
                //MessageBox.Show("Worker cancelled...");
            } else if (!(e.Error == null)) {
                //MessageBox.Show("Worker error...");
            } else {
                toolStripStatusLabel1.Text = " ";
                //MessageBox.Show("Worker finished...");
            }
        }

        // ****** Txt File Functions ***************************************************************

        private void createInitFile(simConfigParameters v, string nameFile, bool op) {   // op-true = User Data Profile, op-false = Initconfig file...
            List<string> param = new List<string>();
            if(op){
                param.Add("Arena Size X-" + v.arenaX.ToString());
                param.Add("Arena Size Y-" + v.arenaY.ToString());
                param.Add("Arena Size R-" + v.arenaR.ToString());
                param.Add("Arena Type-" + translateBool(v.arenaType));
                param.Add(" ");
            }
            param.AddRange(prepareSummary(v, true));
            File.WriteAllLines(@nameFile, param);                               // Saves all the string elements in a same file (Encoding UTF-8)...
        }

        private void copyFileToLocation(string envFile, string nameFile) {
            List<string> lines = new List<string>();
            string line;

            StreamReader file = new StreamReader(envFile);
            while ((line = file.ReadLine()) != null) {
                lines.Add(line);
            }
            File.WriteAllLines(@nameFile, lines);                               // Saves all the string elements in a same file (Encoding UTF-8)...
        }

        private void updateArgumentsConfig(simConfigParameters d) {
            List<string> lines = new List<string>();
            lines.Add(translateBool(d.arenaType));
            lines.Add(d.arenaX.ToString());
            lines.Add(d.arenaY.ToString());
            lines.Add(d.arenaR.ToString());
            File.WriteAllLines("Data\\ArgsConfig.txt", lines);                               // Saves all the string elements in a same file (Encoding UTF-8)...
        }

        private void showSummary(simConfigParameters v) {
            List<string> param = new List<string>();
            if (v.arenaType) {
                param.Add("Circular Arena:");
                param.Add("Arena Size R-" + v.arenaR.ToString());
            } else {
                param.Add("Squared Arena:");
                param.Add("Arena Size X-" + v.arenaX.ToString());
                param.Add("Arena Size Y-" + v.arenaY.ToString());
            }
            param.Add(" ");
            param.AddRange(prepareSummary(v, false));
            sumary.Text = string.Join(Environment.NewLine, param.ToArray());
        }

        List<string> prepareSummary(simConfigParameters v, bool op) {       // False = To be seen, True = To iniFile...
            List<string> param = new List<string>();            
            param.Add("==== Environment Settings ====");
            param.Add("00_User name: *" + v.userName);
            param.Add("01_Scenario: *" + v.evName);
            param.Add("02_Environment: 0"); // + v.environment.ToString());
            param.Add("03_Max Simulation Time: " + v.maxSimTime.ToString());
            param.Add("04_Repetitions: " + v.simRepeat.ToString());
            if (op) {
                param.Add("05_Human Interaction: " + translateBool(v.humInter));
                param.Add("06_Human Control Type: " + translateBool(v.hConType));
                param.Add(" ");
                param.Add("==== Robots Settings ====");
                param.Add("07_Amount of Robots: " + v.amountBots.ToString());
                param.Add("08_positionsFileName: *" + v.preDefPositions);
                param.Add("09_Predefined positions: " + v.usePreDefP.ToString());
                param.Add("10_Starting Algorithm: " + v.startingAlg.ToString());
                param.Add("11_Alg. Follower Version: " + v.AlgFolVer.ToString());
                param.Add("12_Alg. Obj. Cluster Version: " + v.AlgClustVer.ToString());
                param.Add(" ");
                param.Add("==== Object Clustering Settings ====");
                param.Add("13_Color Objects: " + v.colorObjects.ToString());
                param.Add("14_Objects per color: " + v.objectsPerColor.ToString());
                param.Add("15_Objects per color hidden: " + v.objectsPerColorHidden.ToString());
                param.Add(" ");
                param.Add("==== Evaluations Settings ====");
                param.Add("16_Aggregation Evaluation: " + translateBool(v.aggeval));
                param.Add("17_Other Half Evaluation: " + translateBool(v.othhalfeval));
                param.Add("18_Cluster Evaluation: " + translateBool(v.clusteval));
                param.Add("19_Transport Evaluation: " + translateBool(v.transportEval));
                param.Add(" ");
                param.Add("==== Reference Object Settings ====");
                param.Add("20_Reference Object: " + translateBool(v.addRefOb));
                param.Add("21_Reference Object Random Position: " + translateBool(v.rndRefPos));
                param.Add("22_Reference Object Radious: " + v.RefORad.ToString());
                param.Add("23_Reference Object Position X: " + v.RefO_X.ToString());
                param.Add("24_Reference Object Position Y: " + v.RefO_Y.ToString());
                param.Add("25_Reference Object Color: " + v.RefColor.ToString());
                param.Add(" ");
                param.Add("==== Transport Object Settings ====");
                param.Add("26_Amount of Transport Objects: " + v.amountTranspObj.ToString());
                param.Add("27_Transport Object: " + translateBool(v.addTransObj));
                param.Add("28_Transport Object Random Position: " + translateBool(v.rndTnpPos));
                param.Add("29_Transport Object Radious: " + v.TnpORad.ToString());
                param.Add("30_Transport Object Position X: " + v.TnpO_X.ToString());
                param.Add("31_Transport Object Position Y: " + v.TnpO_Y.ToString());
                param.Add("32_Transport Object Weight: " + v.TnpWeight.ToString());
                param.Add("33_Transport Object Color: " + v.TnpColor.ToString());
            } else {
                param.Add("05_Human Interaction: " + boolToActivInactive(v.humInter));
                param.Add("06_Human Control Type: " + boolToSimplAdvanced(v.hConType));
                param.Add(" ");
                param.Add("==== Robots Settings ====");
                param.Add("07_Amount of Robots: " + v.amountBots.ToString());
                param.Add("08_positionsFileName: *" + v.preDefPositions);
                param.Add("09_Predefined positions: " + v.usePreDefP.ToString());
                param.Add("10_Starting Algorithm: " + v.startingAlg.ToString());
                param.Add("11_Alg. Follower Version: " + v.AlgFolVer.ToString());
                param.Add("12_Alg. Obj. Cluster Version: " + v.AlgClustVer.ToString());
                if(v.colorObjects != 0){
                    param.Add(" ");
                    param.Add("==== Object Clustering Settings ====");
                    param.Add("13_Color Objects: " + v.colorObjects.ToString());
                    param.Add("14_Objects per color: " + v.objectsPerColor.ToString());
                    param.Add("15_Objects per color hidden: " + v.objectsPerColorHidden.ToString());
                }
                param.Add(" ");
                param.Add("==== Evaluations Settings ====");
                param.Add("16_Aggregation Evaluation: " + boolToActivInactive(v.aggeval));
                param.Add("17_Other Half Evaluation: " + boolToActivInactive(v.othhalfeval));
                param.Add("18_Cluster Evaluation: " + boolToActivInactive(v.clusteval));
                param.Add("19_Transport Evaluation: " + boolToActivInactive(v.transportEval));
                if (v.addRefOb) {
                    param.Add(" ");
                    param.Add("==== Reference Object Settings ====");
                    param.Add("20_Reference Object: " + translateBool(v.addRefOb));
                    param.Add("22_Reference Object Radious: " + v.RefORad.ToString());
                    if (v.rndRefPos) {
                        param.Add("23_24_Reference Random Position...");
                    } else {
                        param.Add("23_Reference Object Position X: " + v.RefO_X.ToString());
                        param.Add("24_Reference Object Position Y: " + v.RefO_Y.ToString());    
                    }
                    param.Add("25_Reference Object Color: " + v.RefColor.ToString());
                }
                if (v.addTransObj) {
                    param.Add(" ");
                    param.Add("==== Transport Object Settings ====");
                    param.Add("26_Amount of Transport Objects: " + v.amountTranspObj.ToString());
                    param.Add("27_Transport Object: " + translateBool(v.addTransObj));
                    param.Add("29_Transport Object Radious: " + v.TnpORad.ToString());
                    if (v.rndTnpPos) {
                        param.Add("30_31_Reference Random Position...");
                    }else{
                        param.Add("30_Transport Object Position X: " + v.TnpO_X.ToString());
                        param.Add("31_Transport Object Position Y: " + v.TnpO_Y.ToString());
                    }
                    param.Add("32_Transport Object Weight: " + v.TnpWeight.ToString());
                    param.Add("33_Transport Object Color: " + v.TnpColor.ToString());
                }
            }            
            return param;
        }

        // ****** Internal use *********************************************************************

        private void fill_struct(int count, string temp) {
            int convData = 0;
            if (count == 0 || count == 1 || count == 8) {
                // Dont convert...
            } else {
                convData = Convert.ToInt32(temp);
            }
            switch (count) {
                case 0:                                 // 00_User name
                    sCP.userName = temp;
                    break;
                case 1:                                 // 01_Scenario
                    sCP.evName = temp;
                    break;
                case 2:                                 // 02_Environment
                    //sCP.environment = convData;
                    break;
                case 3:                                 // 03_Max Simulation Time
                    sCP.maxSimTime = convData;
                    break;
                case 4:                                 // 04_Repetitions
                    sCP.simRepeat = convData;
                    break;
                case 5:                                 // 05_Human Interaction
                    sCP.humInter = translateToBool(convData);
                    break;
                case 6:                                 // 06_Human Control Type
                    sCP.hConType = translateToBool(convData);
                    break;
                case 7:                                 // 07_Amount of Robots
                    sCP.amountBots = convData;
                    break;
                case 8:                                 // 08_positionsFileName
                    sCP.preDefPositions = temp;
                    break;
                case 9:                                 // 09_Predefined positions
                    sCP.usePreDefP = convData;
                    break;
                case 10:                                // 10_Starting Algorithm
                    sCP.startingAlg = convData;
                    break;
                case 11:                                // 11_Alg. Follower Version
                    sCP.AlgFolVer = convData;
                    break;
                case 12:                                // 12_Alg. Obj. Cluster Version
                    sCP.AlgClustVer = convData;
                    break;
                case 13:                                // 13_Color Objects
                    sCP.colorObjects = convData;
                    break;
                case 14:                                // 14_Objects per color
                    sCP.objectsPerColor = convData;
                    break;
                case 15:                                // 15_Objects per color hidden
                    sCP.objectsPerColorHidden = convData;
                    break;
                case 16:                                // 16_Aggregation Evaluation
                    sCP.aggeval = translateToBool(convData);
                    break;
                case 17:                                // 17_Other Half Evaluation
                    sCP.othhalfeval = translateToBool(convData);
                    break;
                case 18:                                // 18_Cluster Evaluation
                    sCP.clusteval = translateToBool(convData);
                    break;
                case 19:                                // 19_Transport Evaluation
                    sCP.transportEval = translateToBool(convData);
                    break;
                case 20:                                // 20_Reference Object
                    sCP.addRefOb = translateToBool(convData);
                    break;
                case 21:                                // 21_Reference Object Random Position
                    sCP.rndRefPos = translateToBool(convData);
                    break;
                case 22:                                // 22_Reference Object Radious
                    sCP.RefORad = convData;
                    break;
                case 23:                                // 23_Reference Object Position X
                    sCP.RefO_X = convData;
                    break;
                case 24:                                // 24_Reference Object Position Y
                    sCP.RefO_Y = convData;
                    break;
                case 25:                                // 25_Reference Object Color
                    sCP.RefColor = convData;
                    break;
                case 26:                                // 26_Amount of Transport Objects
                    sCP.amountTranspObj = convData;
                    break;
                case 27:                                // 27_Transport Object
                    sCP.addTransObj = translateToBool(convData);
                    break;
                case 28:                                // 28_Transport Object Random Position
                    sCP.rndTnpPos = translateToBool(convData);
                    break;
                case 29:                                // 29_Transport Object Radious
                    sCP.TnpORad = convData;
                    break;
                case 30:                                // 30_Transport Object Position X
                    sCP.TnpO_X = convData;
                    break;
                case 31:                                // 31_Transport Object Position Y
                    sCP.TnpO_Y = convData;
                    break;
                case 32:                                // 32_Transport Object Weight
                    sCP.TnpWeight = convData;
                    break;
                case 33:                                // 33_Transport Object Color
                    sCP.TnpColor = convData;
                    break;
                default:
                    break;
            }
        }

        private string translateBool(bool value) {
            if (value) {
                return "1";
            } else {
                return "0";
            }
        }

        private string boolToActivInactive(bool value) {
            if (value) {
                return "Active";
            } else {
                return "Inactive";
            }
        }

        private string boolToSimplAdvanced(bool value) {
            if (value) {
                return "Advanced";
            } else {
                return "Simple";
            }
        }

        private bool IsProcessOpen(string name) {                        // Be sure to not add the .exe to the name you provide...
            foreach (Process clsProcess in Process.GetProcesses()) {
                if (clsProcess.ProcessName.Contains(name)) {
                    return true;
                }
            }
            return false;
        }

        private bool translateToBool(int i) {
            bool a;
            if (i == 1) {
                a = true;
            } else {
                a = false;
            }
            return a;
        }

        private void closeRunningProcess(string name) {                        // Be sure to not add the .exe to the name you provide...
            foreach (Process theProcess in Process.GetProcesses()) {
                if (theProcess.ProcessName.Contains(name)) {
                    theProcess.CloseMainWindow();
                    theProcess.Close();
                }
            }
        }

        private void usSet_CheckedChanged(object sender, EventArgs e) {
            if (usSet.Checked) {
                run_exe.Enabled = true;
                userUniqueID = userPersID.Text;
                userPersID.Enabled = false;
            } else {
                run_exe.Enabled = false;
                userPersID.Enabled = true;
            }
        }

        public void updateLoggerPBStats(int min, int max, int step) {
            logProgressBar.Minimum = min;
            logProgressBar.Maximum = max;
            logProgressBar.Step = step;
        }

        public void updateLoggerPBPerformStep() {
            logProgressBar.PerformStep();
        }

        public void updateLoggerPBValue(int x) {
            logProgressBar.Value = x;
        }

        // ****** Excel Editor *********************************************************************

        private void button2_Click(object sender, EventArgs e) {                                    // Creates an excel sheet and fills it... 
            //if (xlApp == null) {
            //    Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
            //    return;
            //}
            //xlApp.Visible = true;

            //Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            //Worksheet ws = (Worksheet)wb.Worksheets[1];

            //if (ws == null) {
            //    Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
            //}

            //// Select the Excel cells, in the range c1 to c7 in the worksheet.
            //Range aRange = ws.get_Range("C1", "C7");

            //if (aRange == null) {
            //    Console.WriteLine("Could not get a range. Check to be sure you have the correct versions of the office DLLs.");
            //}

            //// Fill the cells in the C1 to C7 range of the worksheet with the number 6.
            //Object[] args = new Object[1];
            //args[0] = 6;
            //aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);

            //// Change the cells in the C1 to C7 range of the worksheet to the number 8.
            //aRange.Value2 = 8;
        }
    
        private void configName_TextChanged(object sender, EventArgs e) {

        }

        private void notes_Click(object sender, EventArgs e) {

        }

        // ****** Video Builder ********************************************************************

        private void processVideo_Click(object sender, EventArgs e) {
            vLog.process_Video(this, fileLocation.Text, framesJump, saveVideo, saveImages); 
        }

        private void goToFiLoc_Click(object sender, EventArgs e) {
            OpenFileDialog oFDialog = new OpenFileDialog();
            oFDialog.Title = "Select file to convert into video.";
            oFDialog.Filter = "CSV files|*.csv";
            oFDialog.InitialDirectory = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Logger\Positions");
            DialogResult result = oFDialog.ShowDialog();            // Show the dialog.
            if (result == DialogResult.OK) {                        // Test result.
                fileLocation.Text = oFDialog.FileName;
            }
            //processVideo.Enabled = true;
        }

        private void vidoptions_Click(object sender, EventArgs e) {
            VideoOptions form;
            form = new VideoOptions(framesJump, saveImages, saveVideo);
            var result = form.ShowDialog();
            if (result == DialogResult.OK) {
                framesJump = form.jmpFr;
                saveImages = form.simg;
                saveVideo = form.svid;
            }
        }

        public void updateImageShower(Image img){
            imageShower.Image = img;
        }

        public void updateVideoPBStats(int min, int max, int step) {
            imageWorkProgress.Minimum = min;
            imageWorkProgress.Maximum = max;
            imageWorkProgress.Step = step;
        }

        public void updateVideoPBPerformStep() {
            imageWorkProgress.PerformStep();
        }

        public void updateVideoPBValue(int x) {
            imageWorkProgress.Value = x;
        }

    }
}