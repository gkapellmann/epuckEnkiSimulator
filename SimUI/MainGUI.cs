using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Net.Sockets; 
using System.Net;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using InTheHand.Net.Mime;
using InTheHand.Net;

using Generals;

namespace SimUI {

        delegate void RefreshProgressDelegate(int position);
        delegate void SetTextCallback(string text);                             // Text Delegates...
        delegate void EnableButton(Button thisButton, bool state);              // Enable State Delegates...
        delegate void SetCheckboxEnable(CheckBox chBox, bool state);            // Checkbox Enable Delegates...
        delegate void SetAlgsEnable(RadioButton thisRadioButton, bool state);   // RadioButton Enable Delegates...
        delegate void CheckAlgsHandler(RadioButton cb, bool isChecked);         // RadioButton Checked Delegates...
        delegate void ListDelegate(ListViewItem info);                          // List Items Delegate...

    public partial class Swarmer : Form {

        General gen = new General();

        IPConfigWindow ipObject;
        GUIConfig gUIObject;
        SwarmConfig swarmConfig;

        volatile bool preSelectedBot = false;           // If the user wants to connect to a specific robot...
        volatile bool bTKeepDiscovering = true;         // The bluetooth keeps discoverying after completition...
        volatile bool recvWorkerSimul = true;           // Simulation worker flag...
        volatile bool recvWorkerReal = true;            // Real robot worker flag...
        volatile bool possibleCalc = false;

        private bool stuckImage = false;
        private bool gotAnswer = false;                 // First communication achieved...
        private bool isComEnabled = false;              // Is communication enabled and stable?...
        private bool isSwarmConfigured = false;
        private bool connectedToDevice = false;         // Is the connection established?
        private bool selectedItemActive = false;        // Is a robot selected from the list?...
        private bool humanCEStatus = false;             // Status of the Human Control selector...
        private bool imageInProcess = false;            // Is an image been received...
        private bool stoppedRobot = false;
        private bool alg0JustChecked = false;           // Gossip...
        private bool alg1JustChecked = false;           // Aggregation...
        private bool alg2JustChecked = false;           // Follower...
        private bool alg3JustChecked = false;           // Object Clustering...
        private bool alg4JustChecked = false;           // Collective Transport...

        int j = 0;
        int nPixIndex = 0;
        int nImgWidth = 0;
        int nImgHeight = 0;
        int imThisPuck = -1;                            // ID of the current connected robot...
        int clusterSize = 0;                            // Size of the cluster reported by the robot...
        int swarmSize = 0;                              // Total size of the swarm...
        int numberOfSwapedBots = 0;                     // Number of times the suer changed of robots...
        int newcamIndex = 0;
        int newCamW = 80;
        int newCamH = 60;
        int robotChosen = 0;                            // Robot in "devices" list to be connected, chosen randomly...
        int biggestcluster = 0;                         // Record of the biggest cluster detected...
        int posDecoX = 0;
        int posDecoY = 0;
        int posDecoD = 0;

        byte[] socketBuffer;                            // Socket buffer...
        byte[] robotBuffer;                             // Robot buffer...

        int[,] integersPixels;

        bool[] enCtls = new bool[9];                    // 0-Alg0Gos, 1-Alg1Agg, 2-Alg2Fol, 3-Alg3ObC, 4-Alg4CTr, 5-Directions, 6-Camera, 7-IRSensors, 8-Camera Resolution...
        bool advHumCntl;
        bool robotListActive = false;

        string selectBot = "";

        List<int> myClusters;                           // Size of reported cluster...
        List<int> myUsedIDs;                            // ID's of identified robots...

        List<BluetoothDeviceInfo> devices;

        Socket senderSock;
        SocketPermission permission;
        IPAddress ipAddr = IPAddress.None;
        Bitmap eImage;

        NetworkStream BTstream;
        Random robotID;

        BluetoothRadio myRadio;
        BluetoothEndPoint localEndpoint;
        BluetoothClient localClient;
        BluetoothComponent localComponent;
        BluetoothAddress mac;
        //Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");

        BackgroundWorker SimulationWorker = new BackgroundWorker();
        BackgroundWorker RobotWorker = new BackgroundWorker();

        struct selectedListBot {
            public int listIndex;
            public string name;
            public string macAddr;
            public bool paired;
        }; 
        selectedListBot sBot;

        Point[] linePoints;

        public Swarmer() {
            try {
                InitializeComponent();
                myClusters = new List<int>();
                myUsedIDs = new List<int>();
                socketBuffer = new byte[Constants.socketBufferSize];
                robotBuffer = new byte[Constants.robotBufferSize];
                devices = new List<BluetoothDeviceInfo>();
            } catch (Exception ex) {
                using (StreamWriter outfile = new StreamWriter(@".\Error.txt")) {
                    outfile.Write(ex.Message.ToString());
                }
            }
        }

        ~Swarmer() {
            // release leader if selected...
            // stopt connection if opened...
        }

        private void Swarmer_Load(object sender, EventArgs e) {
            robotID = new Random();
            linePoints = new Point[9];

            // ****** Bluetooth Activation ***************************************************
            myRadio = BluetoothRadio.PrimaryRadio;
            if (myRadio == null) {
                realbotserver.Enabled = false;
            }

            // ******          
            setConnectedEnables(false);
            setAllRequestedBotEnables(false);
            countBots.Enabled = false;

            // ****** Update GUI Status ******************************************************
            try {
                int count = 0;
                var fileStream = new FileStream("Status.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream)) {
                    string line;
                    while ((line = streamReader.ReadLine()) != null) {
                        if (line.Contains("*")) {
                            int pos = line.LastIndexOf("*") + 1;
                            if (Convert.ToInt32(line.Substring(pos, line.Length - pos)) == 1) {
                                enCtls[count] = true;
                            } else {
                                enCtls[count] = false;
                            }
                            count++;
                        }
                        if(line.Contains("-")){
                            int pos = line.LastIndexOf("-") + 1;
                            newcamIndex = Convert.ToInt32(line.Substring(pos, line.Length - pos));
                            setCameraPresetResolutions(newcamIndex);
                        }
                        //MessageBox.Show(line.Substring(pos, line.Length - pos));
                    }
                }
                setControlEnables();
            } catch (Exception ex) {
                for (int i = 0; i < enCtls.Length; i++ ) {
                    enCtls[i] = true;
                }
                MessageBox.Show("Update Status Error", "There was an error reading the statusd file, previous configuration not updated...",MessageBoxButtons.OK, MessageBoxIcon.Error);
                using (StreamWriter outfile = new StreamWriter(@".\Update_Error.txt")) {
                    outfile.Write(ex.Message.ToString());
                }
            }
            try {
                var fileStream2 = new FileStream(@"..\Data\InitConfig.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream2)) {
                    string line;
                    while ((line = streamReader.ReadLine()) != null) {
                        if (line.Contains("Human Control Type")) {
                            int pos = line.LastIndexOf(":") + 1;
                            if (Convert.ToInt32(line.Substring(pos, line.Length - pos)) == 1) {
                                advHumCntl = true;
                            } else {
                                advHumCntl = false;
                            }
                        }
                    }
                }
            }catch(Exception ex2){
                MessageBox.Show("Update Status Error", "There was an error reading the statusd file, previous configuration not updated...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                using (StreamWriter outfile = new StreamWriter(@".\Update_Error_B.txt")) {
                    outfile.Write(ex2.Message.ToString());
                }
            }
            updateGuiLabels();

            // ****** Set background workers *************************************************
            Application.DoEvents();
            RobotWorker.WorkerSupportsCancellation = true;
            RobotWorker.WorkerReportsProgress = false;
            RobotWorker.DoWork += new DoWorkEventHandler(RobotWorker_DoWork);
            RobotWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RobotWorker_RunWorkerCompleted);

            // Create one SocketPermission for socket access restrictions 
            SimulationWorker.WorkerSupportsCancellation = true;
            SimulationWorker.WorkerReportsProgress = false;
            SimulationWorker.DoWork += new DoWorkEventHandler(SimulationWorker_DoWork);
            SimulationWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SimulationWorker_RunWorkerCompleted);
            try {
                permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, "", SocketPermission.AllPorts);
                permission.Demand();                                                                    // Ensures the code to have permission to access a Socket...
            } catch (Exception exc) {
                MessageBox.Show(exc.ToString());
            }

            camBitShape.BringToFront();
            L0to1.Paint += new PaintEventHandler(this.L0to1_Paint);
            L1to2.Paint += new PaintEventHandler(this.L0to1_Paint);
            L2to3.Paint += new PaintEventHandler(this.L0to1_Paint);
            L3to4.Paint += new PaintEventHandler(this.L0to1_Paint);
            L4to5.Paint += new PaintEventHandler(this.L0to1_Paint);
            L5to6.Paint += new PaintEventHandler(this.L0to1_Paint);
            L6to7.Paint += new PaintEventHandler(this.L0to1_Paint);
            L7to0.Paint += new PaintEventHandler(this.L0to1_Paint);            
            loadPointsInit();

            posDecoX = posDeco.Location.X;
            posDecoY = posDeco.Location.Y;
            //prepareRobotsList();

            biggestcluster = 0;
            imgPuck.Image = Image.FromFile(@"..\Data\Resources\CamIcon.png");
        }

        private void Swarmer_FormClosed(object sender, FormClosedEventArgs e) {
            
        } 

        // ****** GUI activities and calculations ****************************************

        private void setCameraPresetResolutions(int x) {
            switch (x) {
                case 1:
                    newCamW = 80;
                    newCamH = 60;
                    break;
                case 2:
                    newCamW = 120;
                    newCamH = 90;
                    break;
                case 3:
                    newCamW = 160;
                    newCamH = 120;
                    break;
                case 4:
                    newCamW = 200;
                    newCamH = 150;
                    break;
                case 5:
                    newCamW = 640;
                    newCamH = 480;
                    break;
                default:
                    break;
            }
        }

        private void calcNewXYPosition(double mag, int sector) {         // Sector = Cuadrant of sensor, A=true - B=false...
            mag = Constants.m * mag + Constants.b;  // Rescaling...
            switch(sector){
                case 0:
                case 8:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious + (int)(mag * Constants.sinAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious - (int)(mag * Constants.cosAlpha);
                    break;
                case 1:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious + (int)(mag * Constants.cosAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious - (int)(mag * Constants.sinAlpha);
                    break;
                case 2:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious + (int)(mag * Constants.cosAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious + (int)(mag * Constants.sinAlpha);
                    break;
                case 3:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious + (int)(mag * Constants.sinAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious + (int)(mag * Constants.cosAlpha);
                    break;
                case 4:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious - (int)(mag * Constants.sinAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious + (int)(mag * Constants.cosAlpha);
                    break;
                case 5:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious - (int)(mag * Constants.cosAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious + (int)(mag * Constants.sinAlpha);
                    break;
                case 6:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious - (int)(mag * Constants.cosAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious - (int)(mag * Constants.sinAlpha);
                    break;
                case 7:
                    linePoints[sector].X = robotShape.Location.X + Constants.circleRadious - (int)(mag * Constants.sinAlpha);
                    linePoints[sector].Y = robotShape.Location.Y + Constants.circleRadious - (int)(mag * Constants.cosAlpha);
                    break;
                default:
                    break;
            }
        }

        private void loadPoints(string[] magnitudes){
            for (int i = 0; i < magnitudes.Length; i++) {
                calcNewXYPosition(Convert.ToDouble(magnitudes[i]), i);
            }
            calcNewXYPosition(Convert.ToDouble(magnitudes[0]), 8);
        }

        private void loadPointsInit() {
            for (int i = 0; i < linePoints.Length; i++) {
                calcNewXYPosition(0, i);    // Inital position...
            }
        }

        private void setConnectedEnables(bool x) {
            SetButtonEnable(reqPuck, x);
            SetButtonEnable(tmCheck, x);
        }

        private void setRequestedBotEnables(bool[] x) {
            SetCheckBoxEnabled(mControl, humanCEStatus);     
            SetAlgsState(algor0, x[0]);
            SetAlgsState(algor1, x[1]);
            SetAlgsState(algor2, x[2]);
            SetAlgsState(algor3, x[3]);
            SetAlgsState(algor4, x[4]);
            SetButtonEnable(upBtn, x[5]);
            SetButtonEnable(downBtn, x[5]);
            SetButtonEnable(leftBtn, x[5]);
            SetButtonEnable(rightBtn, x[5]);
            SetButtonEnable(stopBtn, x[5]);
            SetButtonEnable(imgreq, x[6]);
            SetButtonEnable(fbset, x[7]);    
        }

        private void setAllRequestedBotEnables(bool x) {
            SetCheckBoxEnabled(mControl, x);
            SetAlgsState(algor0, x);
            SetAlgsState(algor1, x);
            SetAlgsState(algor2, x);
            SetAlgsState(algor3, x);
            SetAlgsState(algor4, x);
            SetButtonEnable(upBtn, x);
            SetButtonEnable(downBtn, x);
            SetButtonEnable(leftBtn, x);
            SetButtonEnable(rightBtn, x);
            SetButtonEnable(stopBtn, x);
            SetButtonEnable(imgreq, x);
            SetButtonEnable(fbset, x);
        }

        private void resetButtonsBackcolor() {
            upBtn.BackColor = SystemColors.Control;
            downBtn.BackColor = SystemColors.Control;
            leftBtn.BackColor = SystemColors.Control;
            rightBtn.BackColor = SystemColors.Control;
            stopBtn.BackColor = SystemColors.Control;
        }

        private void cleanInterphase() {
            SetCamBitShapeColor(Color.White);
            eps0.Text = "-----";
            eps1.Text = "-----";
            eps2.Text = "-----";
            eps3.Text = "-----";
            eps4.Text = "-----";
            eps5.Text = "-----";
            eps6.Text = "-----";
            eps7.Text = "-----";
            bigclustever.Text = "...";
            myCluster.Text = "...";
            amoChng.Text = "...";
            robIden.Text = "...";
        }

        private void createImageFromInt() {
            int stride = nImgWidth * 4;
            unsafe {
                fixed (int* intPtr = &integersPixels[0, 0]) {
                    eImage = new Bitmap(nImgWidth, nImgHeight, stride, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }
        }

        private void parseIPAddress(String ipAddress) {
            try {
                ipAddr = IPAddress.Parse(ipAddress);
            } catch (ArgumentNullException e) {
                MessageBox.Show("ArgumentNullException caught!!!", "Source : " + e.Source + "\n" + "Message : " + e.Message);
            } catch (FormatException e) {
                MessageBox.Show("FormatException caught!!!", "Source : " + e.Source + "\n" + "Message : " + e.Message);
            } catch (Exception e) {
                MessageBox.Show("Exception caught!!!", "Source : " + e.Source + "\n" + "Message : " + e.Message);
            }
        }

        private void askForIPAddress() {
            String stringAddress = " ";
            ipObject = new IPConfigWindow();
            DialogResult dr = ipObject.ShowDialog(this);
            if (dr == DialogResult.Cancel) {
                stringAddress = "127.0.0.1";
                ipObject.Close();
            } else if (dr == DialogResult.OK) {
                stringAddress = ipObject.getTheAddressResult();
                ipObject.Close();
            }
            if (stringAddress != " ") {
                parseIPAddress(stringAddress);
            }
        }

        private void configureGUI() {
            gUIObject = new GUIConfig();
            gUIObject.setAlg0GosEn(enCtls[0]);
            gUIObject.setAlg1AggEn(enCtls[1]);
            gUIObject.setAlg2FolEn(enCtls[2]);
            gUIObject.setAlg3ObCEn(enCtls[3]);
            gUIObject.setAlg4CTrEn(enCtls[4]);
            gUIObject.setDirectionsEn(enCtls[5]);
            gUIObject.setCameraEn(enCtls[6]);
            gUIObject.setIRSensorsEn(enCtls[7]);
            gUIObject.setAdvMotion(advHumCntl);
            gUIObject.setCameraRes(newcamIndex);
            DialogResult dr = gUIObject.ShowDialog(this);
            if (dr == DialogResult.Cancel) {
                gUIObject.Close();
            } else if (dr == DialogResult.OK) {
                newcamIndex = gUIObject.getCameraRes();
                if (newcamIndex == 0) {
                    newCamW = gUIObject.getNewCamWidth();
                    newCamH = gUIObject.getNewCamHeight();
                } else {
                    setCameraPresetResolutions(newcamIndex);
                }
                enCtls[0] = gUIObject.getAlg0GosEn();
                enCtls[1] = gUIObject.getAlg1AggEn();
                enCtls[2] = gUIObject.getAlg2FolEn();
                enCtls[3] = gUIObject.getAlg3ObCEn();
                enCtls[4] = gUIObject.getAlg4CTrEn();
                enCtls[5] = gUIObject.getDirectionsEn();
                enCtls[6] = gUIObject.getCameraEn();
                enCtls[7] = gUIObject.getIRSensorsEn();
                advHumCntl = gUIObject.getAdvMotion();
                gUIObject.Close();
            }
            updateGuiLabels();
        }

        private void configureSwarm() {
            swarmConfig = new SwarmConfig();
            DialogResult dr = swarmConfig.ShowDialog(this);
            if (dr == DialogResult.Cancel) {
                swarmConfig.Close();
            } else if (dr == DialogResult.OK) {
                swarmSize = swarmConfig.getSwarmSize();
                isSwarmConfigured = true;
                swarmConfig.Close();
            }
        }

        private void RefreshProgress(int value) {
            if (this == null) return;
            imgProg.Value = value;
        }

        private void configProgress(int x) {
            if (this == null) return;
            imgProg.Maximum = x;
        }

        private void prepareRobotsList(int op) {
            robotsList.Columns.Add("Robot", 55);
            if (op == 1) {                                  // Simulator table...
                robotsList.Columns.Add("Algorithm", 70);
                //robotsList.Columns.Add("", 60);
            } else {                                        // Real robots table...
                robotsList.Columns.Add("Connection", 70);
                robotsList.Columns.Add("Paired", 60);
            }   
        }

        private void setControlEnables() { 
            
        }

        private void updatePosDecoPosition(int degree, bool reset) {
            if (reset) {
                posDecoD = 0;
                posDecoX = Constants.XPDecoOriginal;
                posDecoY = Constants.YPDecoOriginal - 25;
            } else {
                posDecoD += degree;
                if (posDecoD >= 360) {
                    // Calc the difference remaining...
                    posDecoD = 0;
                }
                if(posDecoD < 0){
                    posDecoD = 360;
                }
                posDecoX = Constants.XPDecoOriginal + (int)(25 * Math.Sin(posDecoD * Constants.r2dFactor));
                posDecoY = Constants.YPDecoOriginal - (int)(25 * Math.Cos(posDecoD * Constants.r2dFactor));
            }     
            posDeco.Location = new Point(posDecoX, posDecoY);
        }

        private void directionsSet(int op) {
            if (!imageInProcess) {
                switch (op) {
                    case 0:         // Stop...
                        stoppedRobot = true;
                        stopBtn.BackColor = SystemColors.ControlDark;
                        if (simserver.Checked) {
                            sendDataToServer("stop");
                        } else {
                            sendDataToRobot(" ");
                        }
                        break;
                    case 1:         // Forward...
                        stoppedRobot = false;
                        upBtn.BackColor = SystemColors.ControlDark;
                        if (simserver.Checked) {
                            sendDataToServer("up");
                        } else {
                            sendDataToRobot("w");
                        }
                        updatePosDecoPosition(0, true);
                        break;
                    case 2:         // Backward...
                        stoppedRobot = false;
                        downBtn.BackColor = SystemColors.ControlDark;
                        if (simserver.Checked) {
                            sendDataToServer("down");
                        } else {
                            sendDataToRobot("s");
                        }
                        updatePosDecoPosition(0, true);
                        break;
                    case 3:         // Left...
                        leftBtn.BackColor = SystemColors.ControlDark;
                        if (algor0.Checked || stoppedRobot) {
                            updatePosDecoPosition(-Constants.rotFactor, false);
                            Thread thread = new Thread(new ThreadStart(WaitAndReset));
                            thread.Start();
                        }
                        if (simserver.Checked) {
                            sendDataToServer("left");
                        } else {
                            sendDataToRobot("a");
                        }
                        break;
                    case 4:         // Right...
                        rightBtn.BackColor = SystemColors.ControlDark;
                        if (algor0.Checked || stoppedRobot) {
                            updatePosDecoPosition(Constants.rotFactor, false);
                            Thread thread = new Thread(new ThreadStart(WaitAndReset));
                            thread.Start();
                        } 
                        if (simserver.Checked) {
                            sendDataToServer("right");
                        } else {
                            sendDataToRobot("d");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void updateGuiLabels() {
            if (advHumCntl) {
                contype.Text = "Advanced Motion";
            } else {
                contype.Text = "Standar Motion";
            }
            if (enCtls[0]) {
                countBots.Visible = true;
                label4.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label10.Visible = true;
                myCluster.Visible = true;
                bigclustever.Visible = true;
                robIden.Visible = true;
                amoChng.Visible = true;
            } else {
                countBots.Visible = false;
                label4.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label10.Visible = false;
                myCluster.Visible = false;
                bigclustever.Visible = false;
                robIden.Visible = false;
                amoChng.Visible = false;
            }
        }

        private void sensorReadings_CheckedChanged(object sender, EventArgs e) {
            s7.Visible = sensorReadings.Checked;
            s6.Visible = sensorReadings.Checked;
            s5.Visible = sensorReadings.Checked;
            s4.Visible = sensorReadings.Checked;
            s3.Visible = sensorReadings.Checked;
            s2.Visible = sensorReadings.Checked;
            s1.Visible = sensorReadings.Checked;
            s0.Visible = sensorReadings.Checked;
            eps7.Visible = sensorReadings.Checked;
            eps6.Visible = sensorReadings.Checked;
            eps5.Visible = sensorReadings.Checked;
            eps4.Visible = sensorReadings.Checked;
            eps3.Visible = sensorReadings.Checked;
            eps2.Visible = sensorReadings.Checked;
            eps1.Visible = sensorReadings.Checked;
            eps0.Visible = sensorReadings.Checked;
        }

        private void resetGuiSensorsLines() {
            loadPointsInit();
            SetS0LinePos();
            SetS1LinePos();
            SetS2LinePos();
            SetS3LinePos();
            SetS4LinePos();
            SetS5LinePos();
            SetS6LinePos();
            SetS7LinePos();
        }

        private void resetGUISettings() {
            fbset.Text = "Sensors On";
            resetButtonsBackcolor();                    // Clear all buttons...
            cleanInterphase();
            Thread.Sleep(10);
            reqPuck.Text = "Request Bot";
            conBotLbl.Text = "None...";
            imgPuck.Image = Image.FromFile(@"..\Data\Resources\CamIcon.png");
            imThisPuck = -1;
            isComEnabled = false;
            setAllRequestedBotEnables(false);
        }

        // ****** Simulator functions ******************************************

        private void connectToSimulator() {
            if (!connectedToDevice) {
                if (ipAddr == IPAddress.None) {
                    askForIPAddress();
                }
                try {
                    recvWorkerSimul = true;
                    IPHostEntry ipHost = Dns.GetHostEntry("");                                          // Resolves a host name to an IPHostEntry instance     
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Constants.port);                     // Creates a network endpoint 
                    senderSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create one Socket object to setup Tcp connection 
                    senderSock.ReceiveTimeout = 1000;
                    senderSock.NoDelay = false;                                                         // Using the Nagle algorithm 
                    senderSock.Connect(ipEndPoint);                                                     // Establishes a connection to a remote host 
                    if (SimulationWorker.IsBusy != true) {
                        SimulationWorker.WorkerSupportsCancellation = true;
                        SimulationWorker.RunWorkerAsync();
                    }
                    setConnectedEnables(true);
                    string initMessage = "C" + newCamW.ToString() + "." + newCamH.ToString() + "." + gen.translateBool(advHumCntl) + ".";
                    sendDataToServer(initMessage);
                    gUIConfigurationToolStripMenuItem.Enabled = false;
                    connect.Text = "Disconnect";
                    conIndicator.Text = "Connection established";
                    connectedToDevice = true;
                    robotListActive = false;
                    connectGroup.Enabled = false;
                    prepareRobotsList(1);
                } catch (Exception exc) {
                    MessageBox.Show(exc.ToString());
                }
            } else {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to disconnect from the server?", "Disconnect from server", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes) {
                    commonDisconnectCommands();
                    Thread.Sleep(100);
                    recvWorkerSimul = false;
                    sendDataToServer("notify");
                    countBots.Enabled = false;
                    gUIConfigurationToolStripMenuItem.Enabled = true;
                    myUsedIDs.Clear();
                    myClusters.Clear();
                    robotsList.Clear();
                    numberOfSwapedBots = 0;
                    swarmSize = 0;
                    connectedToDevice = false;
                    connectGroup.Enabled = true;
                    setConnectedEnables(false);
                    selectedItemActive = false;
                } else if (dialogResult == DialogResult.No) {
                    //do something else
                }
            }
        }

        private void commonDisconnectCommands() {
            if (fbset.Text == "Sensors Off") {
                sendDataToServer("fboff");
                fbset.Text = "Sensors On";
            }
            sendDataToServer("retAlg");
            resetGUISettings();
            sendDataToServer("nolead");
        }

        private void SimulationWorker_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (recvWorkerSimul) {
                if ((worker.CancellationPending == true)) {
                    e.Cancel = true;
                    break;
                } else {
                    Thread.Sleep(30);
                    ReceiveDataFromServer();
                }
            }
        }

        private void SimulationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {      // Runs in the main thread!!!...
            if ((e.Cancelled == true)) {
                //MessageBox.Show("Worker cancelled...");
            } else if (!(e.Error == null)) {
                //MessageBox.Show("Worker error...");
            } else {
                //MessageBox.Show("Worker finished...");
            }
            //MessageBox.Show("Closing Socket...");
            senderSock.Shutdown(SocketShutdown.Both);                                   // Disables sends and receives on a Socket... 
            senderSock.Disconnect(true);
            senderSock.Close();                                                         // Closes the Socket connection and releases all resources...

            connect.Text = "Connect";
            conIndicator.Text = "Disconnected";
        }

        private void requestBotFromSimulation() {
            if (reqPuck.Text.Equals("Request Bot")) {   // Getting a new Robot...
                reqPuck.Text = "Release Bot";
                if (selectedItemActive) {
                    sendDataToServer("lead_select." + selectBot);
                }else{
                    sendDataToServer("lead");
                }
                setRequestedBotEnables(enCtls);
                mControl.Enabled = false;
                numberOfSwapedBots++;
                isComEnabled = true;
                robotListActive = false;
            } else {                                    // Disconnecting from robot...
                robotsList.SelectedItems.Clear();
                selectedItemActive = false;
                commonDisconnectCommands();
                resetGuiSensorsLines();
                robotListActive = true;
            }
        }

        private bool processKeyPressRobotCommandsSimulator(Keys keyData) {
            switch (keyData) {
                case Keys.Up:               // Capture up arrow key...
                    if (enCtls[5]) { 
                        if (algor0.Checked != true) {
                            directionsSet(1);
                            mControl.Checked = true;
                            mControl.Enabled = true;
                            humanCEStatus = true;
                        }
                    }
                    return true;
                case Keys.Down:             // Capture down arrow key...
                    if (enCtls[5]) {
                        if (algor0.Checked != true) {
                            directionsSet(2);
                            mControl.Checked = true;
                            mControl.Enabled = true;
                            humanCEStatus = true;
                        }
                    }
                    return true;
                case Keys.Left:         // Capture left arrow key...
                    if (enCtls[5]) {
                        directionsSet(3);
                        mControl.Checked = true;
                        mControl.Enabled = true;
                        humanCEStatus = true;
                    }
                    return true;
                case Keys.Right:        // Capture right arrow key...
                    if (enCtls[5]) { 
                        directionsSet(4);
                        mControl.Checked = true;
                        mControl.Enabled = true;
                        humanCEStatus = true;
                    }
                    return true;
                case Keys.Space:        // Capture space key...
                    if (enCtls[5]) {
                        if (algor0.Checked != true) {
                            directionsSet(0);
                            mControl.Checked = true;
                            mControl.Enabled = true;
                            humanCEStatus = true;
                        }
                    }
                    return true;
                case Keys.D0:           // Capture 0 key (Gossip Algorithm)...
                    if(!imageInProcess){
                        if (!algor0.Checked && enCtls[0]) {
                            algor0.Checked = true;
                        }
                    }
                    return true;
                case Keys.D1:           // Capture 1 key (Aggregation Algorithm)...
                    if (!imageInProcess) {
                        if (!algor1.Checked && enCtls[1]) {
                            algor1.Checked = true;
                        }
                    }
                    return true;
                case Keys.D2:           // Capture 2 key (Follower Algorithm)...
                    if (!imageInProcess) {
                        if (!algor2.Checked && enCtls[2]) {
                            algor2.Checked = true;
                        }
                    }
                    return true;
                case Keys.D3:           // Capture 3 key (Object Clustering Algorithm)...
                    if (!imageInProcess) {
                        if (!algor3.Checked && enCtls[3]) {
                            algor3.Checked = true;
                        }
                    }
                    return true;
                case Keys.D4:           // Capture 4 key (Collective Transport Algorithm)...
                    if (!imageInProcess) {
                        if (!algor4.Checked && enCtls[4]) {
                            algor4.Checked = true;
                        }
                    }
                    return true;
                case Keys.A:
                    if (!imageInProcess) {
                        if (mControl.Enabled = true && mControl.Checked == true) {
                            resetButtonsBackcolor();
                            if (simserver.Checked) {
                                sendDataToServer("retAlg");
                            } else {
                                sendDataToRobot("q");
                            }
                            mControl.Checked = false;
                            mControl.Enabled = false;
                            humanCEStatus = false;
                        }
                    }
                    return true;
                case Keys.F:
                    if (!imageInProcess) {
                        if (fbset.Enabled && enCtls[7]) {
                            fbset.PerformClick();
                        }
                    }
                    return true;
                case Keys.I:
                    if (!imageInProcess) {
                        if (imgreq.Enabled && enCtls[6]) {
                            imgreq.PerformClick();
                        }
                    }
                    return true;
                case Keys.S:
                    if (!imageInProcess) {
                        if (reqPuck.Text.Equals("Release Bot") && reqPuck.Enabled) {
                            reqPuck.PerformClick();
                        }
                    }
                    return true;
                case Keys.Z:
                    if (!imageInProcess) {
                        if (countBots.Enabled && enCtls[0]) { 
                            countBots.PerformClick();
                        }
                    }
                    return true;
            }
            return false;
        }

        private bool processKeyPressGUICommands(Keys keyData) {
            switch (keyData) {
                case Keys.C:
                    connect.PerformClick();
                    return true;
                case Keys.L:
                case Keys.R:
                    if (reqPuck.Text.Equals("Request Bot") && reqPuck.Enabled) {
                        reqPuck.PerformClick();
                    }
                    return true;
            }
            return false;
        }

        private void sendDataToServer(string theMessageToSend) {
            try {
                byte[] msg = Encoding.ASCII.GetBytes(theMessageToSend);
                senderSock.Send(msg);                                       // Sends data to a connected Socket...
            } catch (Exception exc) {
                MessageBox.Show(exc.ToString());
            }
        }

        private void ReceiveDataFromServer() {
            try {
                int bytesRec = senderSock.Receive(socketBuffer);                        // Receives data from a bound Socket...
                string MessageRd = Encoding.ASCII.GetString(socketBuffer, 0, bytesRec); // Converts byte array to string...
                while (!MessageRd.Substring(bytesRec - 4, 4).Equals("Dend")) {          // Continues to read the data till data isn't available...
                    bytesRec = senderSock.Receive(socketBuffer);
                    MessageRd += Encoding.ASCII.GetString(socketBuffer, 0, bytesRec);
                    if (!recvWorkerSimul) {
                        break;
                    }
                }
                //SetGenText(MessageRd);
                switch (MessageRd.Substring(0, 1)) {
                    case "a":                                           // Algorithm running was received...
                        BotAlgorithmReceived(MessageRd);
                        break;
                    case "c":                                           // Camera bit was received...
                        if (MessageRd.Substring(1, 1).Equals("1")) {
                            SetCamBitShapeColor(Color.Black);
                        } else {
                            SetCamBitShapeColor(Color.White);
                        }
                        break;
                    case "f":                                           // Good reset ack...
                        processProperReset(MessageRd);
                        break;
                    case "g":                                           // Image data was received...
                        prcessShowImage(MessageRd);
                        break;
                    case "i":                                           // IR data was received...
                        setSensorData(MessageRd);
                        break;
                    case "m":                                           // Image info was received...
                        imageInProcess = true;                          // Avoid any other possible message to be sent if an image is been received...
                        prcessInfoImage(MessageRd);
                        break;
                    case "n":                                           // ID of robot was received...
                        BotNumberReceived(MessageRd);
                        break;
                    case "r":
                        setRequestedBotEnables(enCtls);                 // Enable interfase after a correct reset...
                        setConnectedEnables(true);      
                        break;
                    case "t":                                           // Text data was received...
                        SetGenText(MessageRd);
                        break;
                    case "u":                                           // Count of robots was received...
                        setNumberOfLocalRobots(MessageRd);
                        break;
                    case "1":
                        MessageBox.Show("Simulation max time reached.", "Time maxed out!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        resetGUISettings();
                        robotsList.Clear();                             // Clear the listview...
                        prepareRobotsList(1);
                        break;
                    case "2":
                        MessageBox.Show("The swarm completed the task!", "Objective Accomplished!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    default:                                            // Non important data was received...
                        break;
                }
            } catch (Exception exc) {
                // ****** Once the rror is detected, it gets stuck *********************************************************
                if (stuckImage) {
                    Array.Clear(socketBuffer, 0, socketBuffer.Length);
                    //senderSock.
                    integersPixels = null;
                    imgProg.Value = 0;                                  // Reset load var to zero...
                    imgPuck.Image = Image.FromFile(@"..\Data\Resources\CamIcon.png");
                    sendDataToServer("imgerror");
                    setRequestedBotEnables(enCtls);                     // Re-activate all buttons...
                    setConnectedEnables(true);                          // Re-activate connection buttons...
                    stuckImage = false;
                    imageInProcess = false;
                    MessageBox.Show("An error ocurred while receiveing the image... \n \n " + exc.ToString(), "Image Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ****** Bluetooth functions *******************************************

        private void connectToRealRobots() {
            bool setBTRadio = true;
            bTKeepDiscovering = true;
            if (!connectedToDevice) {
                if (myRadio.LocalAddress == null) {
                    // Warning: LocalAddress is null if the radio is powered-off...
                    setBTRadio = false;
                }
                if (setBTRadio) {
                    mac = myRadio.LocalAddress;
                    localEndpoint = new BluetoothEndPoint(mac, BluetoothService.SerialPort);
                    localClient = new BluetoothClient(localEndpoint);
                    localComponent = new BluetoothComponent(localClient);
                    localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);     // Start discovering devices...
                    localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress);
                    localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete);

                    connect.Text = "Searching...";
                    conIndicator.Text = "Searching for nearby robots...";
                    connectedToDevice = true;
                    robotListActive = false;
                    connectGroup.Enabled = false;
                    //this.Width = Constants.extendedWidth;
                    //lineShape3.X1 = Constants.extendedWidth - Constants.spaceObjects;
                    prepareRobotsList(2);
                } else {
                    MessageBox.Show("Bluetooth radio device not active. /r/n Aborting connection.", "Bluetooth Radio Off...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            } else {
                connectedToDevice = false;
                bTKeepDiscovering = false;              // Do not keep discoverying...
                devices.Clear();                        // Clear the list of discovered devices...
                robotsList.Clear();                     // Clear the listview...
                mac = null;                             // Clear the mac addres of our BT radio...
                localEndpoint = null;
                if (localClient.Connected) {
                    localClient.Close();
                }
                try {
                    localComponent.Dispose();
                } catch (Exception ex) {
                    MessageBox.Show("Local Component exception: " + ex.ToString(), "Exception caught!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                try {
                    localClient.Dispose();
                } catch (Exception ex) {
                    MessageBox.Show("Local Client exception: " + ex.ToString(), "Exception caught!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                connect.Text = "Connect";
                conBotLbl.Text = "...";
                conIndicator.Text = "Disconnected";
                setConnectedEnables(false);
                connectGroup.Enabled = true;
            }
        }

        private void RobotWorker_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            //while (!worker.CancellationPending){
            while (recvWorkerReal) {
                if ((worker.CancellationPending == true)) {
                    e.Cancel = true;
                    break;
                } else {
                    Thread.Sleep(5);
                    ReceiveDataFromRobot();
                }
            }
        }

        private void RobotWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if ((e.Cancelled == true)) {
                //MessageBox.Show("RobotWorker cancelled...");
            } else if (!(e.Error == null)) {
                //MessageBox.Show("RobotWorker error...");
            } else {
                //MessageBox.Show("RobotWorker finished...");
            }
            //MessageBox.Show("RobotWorker closing...");
            
            BTstream.Close();                               // When the worker is not active anymore...
            BTstream.Dispose();
            localClient.Close();                            // Close the stream and the client...
            bTKeepDiscovering = true;
            localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);     // Start discovering robots again...

            refreshRobotList();
            isComEnabled = false;
            setAllRequestedBotEnables(false);
            cleanInterphase();
            reqPuck.Text = "Request Bot";
            conBotLbl.Text = "None...";
        }

        private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e) {
            for (int i = 0; i < e.Devices.Length; i++) {
                discover_and_process_BT(e.Devices[i]);
            }
            //MessageBox.Show("Device count: " + devices.Count, "Message");
        }

        private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e) {
            for (int i = 0; i < e.Devices.Length; i++) {
                discover_and_process_BT(e.Devices[i]);
            }
            if (bTKeepDiscovering) {                        // If not connected or disabled, keep discovering...
                localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
            }
            //MessageBox.Show("The BT Radio found " + e.Devices.Length + " devices...", "Message");
        }

        private void discover_and_process_BT(BluetoothDeviceInfo theBTDev) {
            if (theBTDev.DeviceName.Substring(0, 6).Equals("e-puck")) {         // If its an e-puck...
                if (!devices.Contains(theBTDev)) {                              // If the discovered devices is not already detected...
                    devices.Add(theBTDev);                                      // Add it to the list of devices...
                    ListViewItem nowBT = new ListViewItem(theBTDev.DeviceName.Substring(7));
                    nowBT.SubItems.Add(theBTDev.Connected.ToString());
                    nowBT.SubItems.Add(theBTDev.Authenticated.ToString());
                    robotsList.Items.Add(nowBT);
                }
                if (connectedToDevice) { 
                    setConnectedEnables(true);                                      // Enable request epuck...
                    SetConnectButtonText("Disconnect");                             // Change button to dosconnect...
                }
            }           
        }

        private void refreshRobotList() {
            for (int i = 0; i < devices.Count; i++) {                       // refresh the devices state info...
                devices[i].Refresh();
            }
            robotsList.Clear();                                             // Clear the listview...
            if (simserver.Checked) {
                prepareRobotsList(1);
            } else {
                prepareRobotsList(2);
                if (devices.Count != 0) {                                    // If the discovered devices is not already detected...
                    for (int i = 0; i < devices.Count; i++) {
                        ListViewItem nowBT = new ListViewItem(devices[i].DeviceName.Substring(7));
                        nowBT.SubItems.Add(devices[i].Connected.ToString());
                        nowBT.SubItems.Add(devices[i].Authenticated.ToString());
                        robotsList.Items.Add(nowBT);
                    }
                }
            }
        }
                
        private void requestRealRobot() {
            if (reqPuck.Text.Equals("Request Bot")) {
                bTKeepDiscovering = false;                              // Stop discovering robots...
                if (!localClient.Connected) {            // ***************** Not the best option (Check) *************************
                    localClient = new BluetoothClient(localEndpoint);
                    localComponent = new BluetoothComponent(localClient);
                    //MessageBox.Show("Not Connected...", "Message");
                }
                if (devices.Count > 0) {
                    if (preSelectedBot) {
                        robotChosen = sBot.listIndex;                   // A specific robot...
                        //MessageBox.Show("Selected..." + robotChosen.ToString(), "Message");
                    }else{
                        robotChosen = robotID.Next(0, devices.Count);   // A random robot... (+1 so that the upper bound has a chance to be chosen)
                        //MessageBox.Show("Random..." + robotChosen.ToString(), "Message");
                    }
                    if (!devices[robotChosen].Authenticated) {
                        pairWithDevice(devices[robotChosen]);

                        //MessageBox.Show("Pairing...", "Process");
                        while (!devices[robotChosen].Authenticated) {   // Wait for pairing...
                            devices[robotChosen].Refresh();
                            Thread.Sleep(1000);
                        }
                        //MessageBox.Show("Paired", "Process");
                    }
                    refreshRobotList();
                    connectWithRobot(devices[robotChosen]);
                } else {
                    MessageBox.Show("No robots found...", "Message");
                }
            } else {
                disconnectFromRobot();
                robotListActive = true;
            }
        }

        private void pairWithDevice(BluetoothDeviceInfo btDev) {
            if (BluetoothSecurity.PairRequest(btDev.DeviceAddress, getDevicePin(btDev.DeviceName))) {
                if (btDev.Authenticated) {
                    //MessageBox.Show("Authenticated & Paired device...", "Message");
                } else {
                    //MessageBox.Show("Authenticated but NOT Paired device...", "Message");
                }
            } else {
                MessageBox.Show("Not paired device...", "Message");
            }
        }

        private void connectWithRobot(BluetoothDeviceInfo btDevice) {
            if (btDevice.Authenticated) {
                localClient.SetPin(getDevicePin(btDevice.DeviceName));  // set pin of device to connect with
                //localClient.BeginConnect(btDevice.DeviceAddress, mUUID, new AsyncCallback(Connect), btDevice);
                localClient.BeginConnect(btDevice.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), btDevice);
            } else {
                MessageBox.Show("Connection failed...", "Message");
            }
        }

        private void Connect(IAsyncResult result) {
            bool stopConnect = false;
            int timeout = 0;
            if (result.IsCompleted) {
                recvWorkerReal = true;
                //SetBotNumberText(getDevicePin(devices[robotChosen].DeviceName));
                SetReqButtonText("Connecting...");
                while(!localClient.Connected){                          // Give some time for localclient to connect...
                    Thread.Sleep(100);
                    timeout++;
                    if(timeout > 20){                                   // Time to wait for connection in seconds...
                        MessageBox.Show("Connection failed...", "Message");
                        stopConnect = true;
                        break;
                    }
                }
                if (!stopConnect) {
                    Thread.Sleep(50);                                       // And some time to get stable...
                    BTstream = localClient.GetStream();
                    if (RobotWorker.IsBusy != true) {
                        RobotWorker.WorkerSupportsCancellation = true;
                        RobotWorker.RunWorkerAsync();                       // Start the thread to receive data from robot...
                    }
                    Thread.Sleep(450);                                      // And some time to get stable...
                    isComEnabled = true;
                    while (!gotAnswer) {                                    // Tell the robots they are connected... 
                        Thread.Sleep(500);                                  // Wait for an answer...
                        sendDataToRobot("c");                               // If not, repeat the call...
                    }
                    gotAnswer = false;
                    numberOfSwapedBots++;
                    SetReqButtonText("Release Bot");
                    refreshRobotList();
                    robotListActive = false;
                } else {
                    MessageBox.Show("Connection stoped...", "Message");
                    try {
                        localClient.Close();
                        bTKeepDiscovering = true;
                        localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);     // Start discovering robots again...
                        SetReqButtonText("Request Bot");
                    } catch(Exception ex) {
                        MessageBox.Show("Local Client exception: " + ex.ToString(), "Exception caught!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            sendDataToRobot("c"); 
        }

        private void disconnectFromRobot() {
            conBotLbl.Text = "None...";
            sendDataToRobot(".");               // Send we are about to disconnect...
            recvWorkerReal = false;             // Stop backgroundworker to receive data...
        }

        private bool processKeyPressRobotCommandsReal(Keys keyData) {
            switch (keyData) {
                case Keys.Up:               // Capture up arrow key...
                    if (enCtls[5]) {
                        if (algor0.Checked != true) {
                            directionsSet(1);
                        } 
                    }
                    return true;
                case Keys.Down:             // Capture down arrow key...
                    if (enCtls[5]) {
                        if (algor0.Checked != true) {
                            directionsSet(2);
                        } 
                    }
                    return true;
                case Keys.Left:         // Capture left arrow key...
                    if (enCtls[5]) {
                        directionsSet(3); 
                    }
                    return true;
                case Keys.Right:        // Capture right arrow key...
                    if (enCtls[5]) { 
                        directionsSet(4);
                    }
                    return true;
                case Keys.Space:        // Capture space key...
                    if (enCtls[5]) { 
                        if (algor0.Checked != true) {
                            directionsSet(0);
                        }
                    }
                    return true;
                case Keys.D0:           // Capture 0 key... (Gossip)
                    if (algor0.Checked && enCtls[0]) {
                        sendDataToRobot("0");
                    } else {
                        algor0.Checked = true;
                    }
                    return true;
                case Keys.D1:           // Capture 1 key... (Aggregation)
                    if (algor1.Checked && enCtls[1]) {
                        sendDataToRobot("1");
                    } else {
                        algor1.Checked = true;
                    }
                    return true;
                case Keys.D2:           // Capture 2 key... (Follower)
                    if (algor2.Checked && enCtls[2]) {
                        sendDataToRobot("2");
                    } else {
                        algor2.Checked = true;
                    }
                    return true;
                case Keys.D3:           // Capture 3 key (Object Clustering Algorithm)...
                    if (enCtls[3]) {
                        if (algor3.Checked && enCtls[3]) {
                            sendDataToRobot("3");
                        } else {
                            algor3.Checked = true;
                        }
                    }
                    return true;
                case Keys.F:
                    if (fbset.Enabled && enCtls[7]) {
                        fbset.PerformClick();
                    }
                    return true;
                case Keys.I:
                    if (imgreq.Enabled && enCtls[6]) {
                        imgreq.PerformClick();
                    }
                    return true;
                case Keys.S:
                    if (reqPuck.Text.Equals("Release Bot") && reqPuck.Enabled) {
                        reqPuck.PerformClick();
                    }
                    return true;
                case Keys.Z:
                    if (countBots.Enabled && enCtls[0]) {
                        countBots.PerformClick();
                    }
                    return true;
            }
            return false;
        }

        private void ReceiveDataFromRobot() {
            if (BTstream.CanRead) {
                int numberOfBytesRead = 0;
                byte[] myReadBuffer = new byte[Constants.robotBufferSize];                          // How many bytes are there to read...
                if (BTstream.DataAvailable) {
                    string MessageBot = " ";                                                        
                    while (!MessageBot.Substring(MessageBot.Length - 1, 1).Equals("!")) {           // Continues to read the data till data isn't available...
                        numberOfBytesRead = BTstream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        if (numberOfBytesRead > 0) {
                            MessageBot += Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
                            if (!recvWorkerReal) {
                                break;
                            }
                        }
                    }
                    MessageBot.Trim();
                    if(MessageBot.Contains("i")){
                        gotAnswer = true;
                        SetBotNumberText(MessageBot.Substring(1, MessageBot.Length - 2));
                        setRequestedBotEnables(enCtls);
                    }
                }
            }
        }

        private void sendDataToRobot(string message) {
            if (BTstream.CanWrite) {
                byte[] data_conf = Encoding.ASCII.GetBytes(message);
                BTstream.Write(data_conf, 0, data_conf.Length);
                BTstream.Flush();
            }
        }

        private string getDevicePin(string name) {
            name = name.Substring(7);
            return name;
        }

        // ****** Thread Command Helpers ****************************************

        private void prcessInfoImage(string str) {
            int srtIndex = str.IndexOf("m");
            int endIndex = str.IndexOf("P");
            nPixIndex = Convert.ToInt32(str.Substring(srtIndex + 1, endIndex - srtIndex - 1));    // Number of pixels...
            srtIndex = endIndex;
            endIndex = str.IndexOf("W");
            nImgWidth = Convert.ToInt32(str.Substring(srtIndex + 1, endIndex - srtIndex - 1));    // Width of image...
            srtIndex = endIndex;
            endIndex = str.IndexOf("H");
            nImgHeight = Convert.ToInt32(str.Substring(srtIndex + 1, endIndex - srtIndex - 1));    // Height of image...
            sendDataToServer("nextLine");
            integersPixels = new int[nImgHeight, nImgWidth];
            this.Invoke(new RefreshProgressDelegate(configProgress), nImgHeight);
        }

        private void prcessShowImage(string str) {
            str = str.Remove(0, 1);                     // Remove the starting 'g'...
            if (str.Substring(0, 1).Equals("r")) {
                stuckImage = true;
                for (int i = 0; i < nImgWidth; i++) {
                    int posg = str.IndexOf("g");
                    int posb = str.IndexOf("b");
                    int posp = str.IndexOf(".");
                    byte rPix = Convert.ToByte(str.Substring(1, posg - 1));                 // Get red color...;
                    byte gPix = Convert.ToByte(str.Substring(posg + 1, posb - posg - 1));   // Get green color...;
                    byte bPix = Convert.ToByte(str.Substring(posb + 1, posp - posb - 1));   // Get blue color...;
                    byte[] bgra = new byte[] { bPix, gPix, rPix, 255 };
                    integersPixels[j, i] = BitConverter.ToInt32(bgra, 0);
                    str = str.Remove(0, posp + 1);
                }
                j++;
                sendDataToServer("nextLine");
            } else {                                    // Image was completely received...
                stuckImage = false;
                createImageFromInt();
                imgPuck.Image = eImage;
                integersPixels = null;
                setRequestedBotEnables(enCtls);         // Re-activate all buttons...
                setConnectedEnables(true);              // Re-activate connection buttons...
                imageInProcess = false;                 // Let the user send directions again...
                j = 0;
                if (fbset.Text.Equals("Sensors Off")) {
                    sendDataToServer("fbon");
                }
            }
            this.Invoke(new RefreshProgressDelegate(RefreshProgress), j);
        }

        private void setSensorData(string str) {
            string[] data = new string[8];
            for (int i = 0; i < 8; i++) {
                int foundS1 = str.IndexOf("/");
                int foundS2 = str.IndexOf("/", foundS1 + 1);
                int dot = str.IndexOf(".");
                data[i] = str.Substring(foundS1 + 1, dot - foundS1 - 1);
                str = str.Remove(foundS1, foundS2 - foundS1);
            }

            loadPoints(data);
            SetSen0Text(data[0]);
            L0to1.Parent.BeginInvoke(new Action(SetS0LinePos));
            SetSen1Text(data[1]);
            L1to2.Parent.BeginInvoke(new Action(SetS1LinePos));
            SetSen2Text(data[2]);
            L2to3.Parent.BeginInvoke(new Action(SetS2LinePos));
            SetSen3Text(data[3]);
            L3to4.Parent.BeginInvoke(new Action(SetS3LinePos));
            SetSen4Text(data[4]);
            L4to5.Parent.BeginInvoke(new Action(SetS4LinePos));
            SetSen5Text(data[5]);
            L5to6.Parent.BeginInvoke(new Action(SetS5LinePos));
            SetSen6Text(data[6]);
            L6to7.Parent.BeginInvoke(new Action(SetS6LinePos));
            SetSen7Text(data[7]);
            L7to0.Parent.BeginInvoke(new Action(SetS7LinePos));
        }

        private void setNumberOfLocalRobots(string str) {
            int x;
            if (str.IndexOf("D") == 2) {
                x = 1;
            } else if (str.IndexOf("D") == 3) {
                x = 2;
            } else {
                x = 3;
            }
            clusterSize = Convert.ToInt32(str.Substring(1, x));
            SetCounttext(str.Substring(1, x));
            //Console.WriteLine("In thread the possibleCalc is " + possibleCalc.ToString());
            possibleCalc = true;
            //Console.WriteLine("In thread the possibleCalc is " + possibleCalc.ToString());
        }

        private void BotNumberReceived(string str) {
            int x;
            if (str.IndexOf("D") == 2) {
                x = 1;
            } else if (str.IndexOf("D") == 3) {
                x = 2;
            } else {
                x = 3;
            }
            imThisPuck = Convert.ToInt32(str.Substring(1, x));
            if (!myUsedIDs.Contains(imThisPuck)) {                              // If the list doesn't contain the identified robot...
                myUsedIDs.Add(imThisPuck);                                      // Add the ID to the list...
                myClusters.Add(0);                                              // As no reported cluster yet, we set it to 0...
            }
            SetBotNumberText(imThisPuck.ToString());
        }

        private void BotAlgorithmReceived(string str) {
            ListViewItem nowBT = new ListViewItem(imThisPuck.ToString());
            if (str.Substring(1, 1).Equals("0")) {          // Gossip...
                nowBT.SubItems.Add("Gossip");
                CheckAlgsHand(algor0, true);
            } else if (str.Substring(1, 1).Equals("1")) {   // Aggregation...
                nowBT.SubItems.Add("Clustering");
                CheckAlgsHand(algor1, true);
            } else if (str.Substring(1, 1).Equals("2")) {   // Follower...
                nowBT.SubItems.Add("Follower");
                CheckAlgsHand(algor2, true);
            } else if (str.Substring(1, 1).Equals("3")) {   // Clustering...
                nowBT.SubItems.Add("Obj. Cluster");
                CheckAlgsHand(algor3, true);
            }
            AddListItem(nowBT);
        }

        private void processProperReset(string str) {
            string message = str.Substring(1, str.Length-5);
            if (message.Equals("NotGood")) {
                MessageBox.Show("Impossible to Reset...", "Other users are still connected to the server.");
            } else if (message.Equals("GoodReset")) {
                biggestcluster = 0;
                numberOfSwapedBots = 0;
                myUsedIDs.Clear();
                myClusters.Clear();
                robotsList.Clear();                         // Clear the listview...
                prepareRobotsList(1);
            } 
        }

        public void WaitAndReset() {
            try {
                Thread.Sleep(100);
                resetButtonsBackcolor();
            } catch (Exception ex) {
                // log errors
            }
        }

        // ****** Thread Handlers ***********************************************

        private void CheckAlgsHand(RadioButton cb, bool isChecked) {
            try {
                if (cb.InvokeRequired) {
                    CheckAlgsHandler d = new CheckAlgsHandler(CheckAlgsHand);
                    this.Invoke(d, new object[] { cb, isChecked });
                } else {
                    cb.Checked = isChecked;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetAlgsState(RadioButton thisRadioButton, bool state) {
            try {
                if (thisRadioButton.InvokeRequired) {
                    SetAlgsEnable d = new SetAlgsEnable(SetAlgsState);
                    this.Invoke(d, new object[] { thisRadioButton, state });
                } else {
                    thisRadioButton.Enabled = state;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetCheckBoxEnabled(CheckBox chBox, bool state) {
            try {
                if (chBox.InvokeRequired) {
                    SetCheckboxEnable d = new SetCheckboxEnable(SetCheckBoxEnabled);
                    this.Invoke(d, new object[] { chBox, state });
                } else {
                    chBox.Enabled = state;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetButtonEnable(Button thisButton, bool state) {
            try {
                if (thisButton.InvokeRequired) {
                    EnableButton d = new EnableButton(SetButtonEnable);
                    this.Invoke(d, new object[] { thisButton, state });
                } else {
                    thisButton.Enabled = state;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetConnectButtonText(string text) {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.connect.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetConnectButtonText);
                this.Invoke(d, new object[] { text });
            } else {
                this.connect.Text = text;
            }
        }

        private void SetReqButtonText(string text) {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.reqPuck.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetReqButtonText);
                this.Invoke(d, new object[] { text });
            } else {
                this.reqPuck.Text = text;
            }
        }

        private void SetBotNumberText(string text) {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.conBotLbl.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetBotNumberText);
                this.Invoke(d, new object[] { text });
            } else {
                this.conBotLbl.Text = text;
            }
        }
        
        private void SetSen0Text(string text) {
            if (this.eps0.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen0Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps0.Text = text;
            }
        }

        private void SetSen1Text(string text) {
            if (this.eps1.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen1Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps1.Text = text;
            }
        }

        private void SetSen2Text(string text) {
            if (this.eps2.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen2Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps2.Text = text;
            }
        }

        private void SetSen3Text(string text) {
            if (this.eps3.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen3Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps3.Text = text;
            }
        }

        private void SetSen4Text(string text) {
            if (this.eps4.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen4Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps4.Text = text;
            }
        }

        private void SetSen5Text(string text) {
            if (this.eps5.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen5Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps5.Text = text;
            }
        }

        private void SetSen6Text(string text) {
            if (this.eps6.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen6Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps6.Text = text;
            }
        }

        private void SetSen7Text(string text) {
            if (this.eps7.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetSen7Text);
                this.Invoke(d, new object[] { text });
            } else {
                this.eps7.Text = text;
            }
        }

        private void SetCounttext(string text) {
            if (this.myCluster.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetCounttext);
                this.Invoke(d, new object[] { text });
            } else {
                this.myCluster.Text = text;
            }
        }

        private void SetGenText(string text) {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label8.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetGenText);
                this.Invoke(d, new object[] { text });
            } else {
                this.label8.Text = text;
            }
        }

        private void AddListItem(ListViewItem info) {
            if (this.robotsList.InvokeRequired) {
                ListDelegate call = new ListDelegate(AddListItem);
                this.Invoke(call, new object[] { info });
            } else {
                ListViewItem foundItem = robotsList.FindItemWithText(info.Text);
                if (foundItem != null) {
                    robotsList.Items.RemoveAt(foundItem.Index);
                    robotsList.Items.Add(info);
                } else {
                    robotsList.Items.Add(info);
                }
                //robotsList.Sorting = SortOrder.Ascending;
            }
        }

        private void SetCamBitShapeColor(Color x) {
            camBitShape.Parent.BeginInvoke(((Action)(() => camBitShape.BackColor = x)));
        }

        private void SetS0LinePos() {
            L0to1.X1 = linePoints[0].X;
            L0to1.Y1 = linePoints[0].Y;
            L0to1.X2 = linePoints[1].X;
            L0to1.Y2 = linePoints[1].Y;
        }

        private void SetS1LinePos() {
            L1to2.X1 = linePoints[1].X;
            L1to2.Y1 = linePoints[1].Y;
            L1to2.X2 = linePoints[2].X;
            L1to2.Y2 = linePoints[2].Y;
        }

        private void SetS2LinePos() {
            L2to3.X1 = linePoints[2].X;
            L2to3.Y1 = linePoints[2].Y;
            L2to3.X2 = linePoints[3].X;
            L2to3.Y2 = linePoints[3].Y;
        }

        private void SetS3LinePos() {
            L3to4.X1 = linePoints[3].X;
            L3to4.Y1 = linePoints[3].Y;
            L3to4.X2 = linePoints[4].X;
            L3to4.Y2 = linePoints[4].Y;
        }

        private void SetS4LinePos() {
            L4to5.X1 = linePoints[4].X;
            L4to5.Y1 = linePoints[4].Y;
            L4to5.X2 = linePoints[5].X;
            L4to5.Y2 = linePoints[5].Y;
        }

        private void SetS5LinePos() {
            L5to6.X1 = linePoints[5].X;
            L5to6.Y1 = linePoints[5].Y;
            L5to6.X2 = linePoints[6].X;
            L5to6.Y2 = linePoints[6].Y;
        }

        private void SetS6LinePos() {
            L6to7.X1 = linePoints[6].X;
            L6to7.Y1 = linePoints[6].Y;
            L6to7.X2 = linePoints[7].X;
            L6to7.Y2 = linePoints[7].Y;
        }

        private void SetS7LinePos() {
            L7to0.X1 = linePoints[7].X;
            L7to0.Y1 = linePoints[7].Y;
            L7to0.X2 = linePoints[0].X;
            L7to0.Y2 = linePoints[0].Y;
        }

        // ****** User Input Requests *******************************************

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool answer;
            if (isComEnabled) {
                resetButtonsBackcolor();
                if (simserver.Checked) {
                    answer = processKeyPressRobotCommandsSimulator(keyData);
                } else {
                    answer = processKeyPressRobotCommandsReal(keyData);
                }
            } else {
                answer = processKeyPressGUICommands(keyData);
            }
            if (answer) {
                return true;
            } else {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void connect_Click(object sender, EventArgs e) {
            if (simserver.Checked) {
                connectToSimulator();
            } else {
                connectToRealRobots();
            }
        }

        private void reqPuck_Click(object sender, EventArgs e) {
            if (algor0.Checked == true) {
                MessageBox.Show("It is not allowed to leave the robot during the -Stop- algorithm.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                if (simserver.Checked) {
                    requestBotFromSimulation();
                } else {
                    requestRealRobot();
                }
            }
        }

        private void mControl_CheckedChanged(object sender, EventArgs e) {
            if (mControl.Checked) {

            } else {
                resetButtonsBackcolor();
                if (simserver.Checked) {
                    sendDataToServer("retAlg");
                } else {
                    sendDataToRobot("q");
                }
                mControl.Enabled = false;
                humanCEStatus = false;
            }
        }

        private void algor0_CheckedChanged(object sender, EventArgs e) {
            if (algor0.Checked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg0");   // Gossip...
                } else {
                    sendDataToRobot("0");
                }
                alg2JustChecked = true;
            }
            if (algor0.Checked) {
                countBots.Enabled = true;
                upBtn.Enabled = false;
                downBtn.Enabled = false;
                stopBtn.Enabled = false;
            } else {
                countBots.Enabled = false;
                upBtn.Enabled = true;
                downBtn.Enabled = true;
                stopBtn.Enabled = true;
            }
        }

        private void algor0_Click(object sender, EventArgs e) {
            if (algor0.Checked && !alg2JustChecked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg0");   // Gossip...
                } else {
                    sendDataToRobot("0");
                }
            }
            if (algor0.Checked) {
                countBots.Enabled = true;
                upBtn.Enabled = false;
                downBtn.Enabled = false;
                stopBtn.Enabled = false;
            } else {
                countBots.Enabled = false;
                upBtn.Enabled = true;
                downBtn.Enabled = true;
                stopBtn.Enabled = true;
            }
            alg2JustChecked = false;
        }
        
        private void algor1_CheckedChanged(object sender, EventArgs e) {
            if (algor1.Checked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg1");   // Aggregation...
                } else {
                    sendDataToRobot("1");
                }
                alg0JustChecked = true;
            }
        }

        private void algor1_Click(object sender, EventArgs e) {
            if (algor1.Checked && !alg0JustChecked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg1");   // Aggregation...
                } else {
                    sendDataToRobot("1");
                }
            }
            alg0JustChecked = false;
        }

        private void algor2_CheckedChanged(object sender, EventArgs e) {
            if (algor2.Checked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg2");   // Follower...
                } else {
                    sendDataToRobot("2");
                }
                alg1JustChecked = true;
            }
        }

        private void algor2_Click(object sender, EventArgs e) {
            if (algor2.Checked && !alg1JustChecked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg2");   // Follower...
                } else {
                    sendDataToRobot("2");
                }
            }
            alg1JustChecked = false;
        }

        private void algor3_CheckedChanged(object sender, EventArgs e) {
            if (algor3.Checked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg3");   // Clustering...
                } else {
                    sendDataToRobot("3");
                }
                alg3JustChecked = true;
            }
        }

        private void algor3_Click(object sender, EventArgs e) {
            if (algor3.Checked && !alg3JustChecked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg3");   // Clustering...
                } else {
                    sendDataToRobot("3");
                }
            }
            alg3JustChecked = false;
        }

        private void algor4_CheckedChanged(object sender, EventArgs e) {
            if (algor4.Checked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg4");   // Transport...
                } else {
                    sendDataToRobot("4");
                }
                alg4JustChecked = true;
            }
        }

        private void algor4_Click(object sender, EventArgs e) {
            if (algor4.Checked && !alg4JustChecked) {
                if (simserver.Checked) {
                    sendDataToServer("Alg4");   // Clustering...
                } else {
                    sendDataToRobot("4");
                }
            }
            alg3JustChecked = false;
        }

        private void analyze_Click(object sender, EventArgs e) {
            int timout = 0;
            List<int> prevCLuster = new List<int>();                                    //

            possibleCalc = false;
            sendDataToServer("count");

            if (!isSwarmConfigured) {
                configureSwarm();                                                       // Ask the user for the total size of the swarm...
            }

            if (swarmSize > 0) {
                while (!possibleCalc) {                                                 // Give some time to wait for some of the servers information...
                    Thread.Sleep(100);
                    timout++;
                    if (timout == 10) {                                                 // Wait a max time of 2000 ms...
                        //MessageBox.Show("Impossible to analyse due to lack of information from the server, try again.", "Server not responding...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
                //Console.WriteLine("After break the possibleCalc is " + possibleCalc.ToString());

                if (!possibleCalc) {                                                    // If data was sent succesfully...
                    if (clusterSize >= swarmSize) {
                        MessageBox.Show("All robots are now clustered!", "Nice!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if(biggestcluster < clusterSize){
                        biggestcluster = clusterSize;
                    }

                    // ****************** To be used later ************************************
                    myClusters[myUsedIDs.IndexOf(imThisPuck)] = clusterSize;            // Set the reported size of the cluster of this robot...                            
                    for (int i = 0; i < myClusters.Count; i++) {
                        if (!prevCLuster.Contains(myClusters[i])) {
                            prevCLuster.Add(myClusters[i]);
                        } else { 
                            // Check the possibility of a equal sized cluster... 
                        }
                    }

                    bigclustever.Text = biggestcluster.ToString();                      // Size of the biggest cluster detected...
                    //myCluster.Text = clusterSize.ToString();                            // Number of robots in current cluster...                      
                    amoChng.Text = numberOfSwapedBots.ToString();                       // How many times has the user changed robots...
                    robIden.Text = myUsedIDs.Count.ToString();                          // How many different robots has the user being connected to...
                } else {
                    //MessageBox.Show("Impossible to analyse due to lack of information from the server, try again.", "Server not responding...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You need to configure the swarm first.", "More information needed...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void upBtn_Click(object sender, EventArgs e) {
            resetButtonsBackcolor();
            directionsSet(1);
            mControl.Checked = true;
            mControl.Enabled = true;
            humanCEStatus = true;
        }

        private void downBtn_Click(object sender, EventArgs e) {
            resetButtonsBackcolor();
            directionsSet(2);
            mControl.Checked = true;
            mControl.Enabled = true;
            humanCEStatus = true;
        }

        private void leftBtn_Click(object sender, EventArgs e) {
            resetButtonsBackcolor();
            directionsSet(3);
            mControl.Checked = true;
            mControl.Enabled = true;
            humanCEStatus = true;
        }

        private void rightBtn_Click(object sender, EventArgs e) {
            resetButtonsBackcolor();
            directionsSet(4);
            mControl.Checked = true;
            mControl.Enabled = true;
            humanCEStatus = true;
        }

        private void stopBtn_Click(object sender, EventArgs e) {
            resetButtonsBackcolor();
            directionsSet(0);
            mControl.Checked = true;
            mControl.Enabled = true;
            humanCEStatus = true;
        }

        private void fbset_Click(object sender, EventArgs e) {
            if (simserver.Checked) {
                if (fbset.Text.Equals("Sensors On")) {
                    //imgreq.Enabled = false;
                    fbset.Text = "Sensors Off";
                    sendDataToServer("fbon");
                } else {
                    //imgreq.Enabled = true;
                    fbset.Text = "Sensors On";
                    sendDataToServer("fboff");
                }
            } else {
                sendDataToRobot("f");
            }
            loadPointsInit();
        }

        private void imgreq_Click(object sender, EventArgs e) {
            if (simserver.Checked) {
                if (fbset.Text.Equals("Sensors Off")) {
                    sendDataToServer("fboff");
                }
                Thread.Sleep(10);
                sendDataToServer("photo");
            } else {
                sendDataToRobot("p");
            }
            setAllRequestedBotEnables(false);
            setConnectedEnables(false);
        }

        private void tmCheck_Click(object sender, EventArgs e) {
            MyInputMessageBox myIMB = new MyInputMessageBox();
            if(myIMB.ShowDialog(this) == DialogResult.OK){
                sendDataToServer("tmStamp-" + myIMB.getTheMessage());
            }
            myIMB.Dispose();
        }

        private void simulatorIPAddressToolStripMenuItem_Click(object sender, EventArgs e) {
            askForIPAddress();
        }

        private void swarmConfigurationToolStripMenuItem_Click(object sender, EventArgs e) {
            configureSwarm();
        }

        private void gUIConfigurationToolStripMenuItem_Click(object sender, EventArgs e) {
            configureGUI();
        }

        private void resetServerToolStripMenuItem_Click(object sender, EventArgs e) {
            // Add (Is there other client connected?)...
            if (reqPuck.Text.Equals("Release Bot")) {
                MessageBox.Show("Unable to reset until release of the current robot has been requested.", "Reset Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }else{
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset the simulation server?", "Reset Simulation Server", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes) {
                    sendDataToServer("reset");
                }
            }
            //setAllRequestedBotEnables(false);          // Wait for server ready...
            //setConnectedEnables(false);            
        }

        private void L0to1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L0to1.X1, L0to1.Y1, L0to1.X2, L0to1.Y2);
        }

        private void L1to2_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L1to2.X1, L1to2.Y1, L1to2.X2, L1to2.Y2);
        }

        private void L2to3_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L2to3.X1, L2to3.Y1, L2to3.X2, L2to3.Y2);
        }

        private void L3to4_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L3to4.X1, L3to4.Y1, L3to4.X2, L3to4.Y2);
        }

        private void L4to5_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L4to5.X1, L4to5.Y1, L4to5.X2, L4to5.Y2);
        }

        private void L5to6_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L5to6.X1, L5to6.Y1, L5to6.X2, L5to6.Y2);
        }

        private void L6to7_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L6to7.X1, L6to7.Y1, L6to7.X2, L6to7.Y2);
        }

        private void L7to0_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Black, L7to0.X1, L7to0.Y1, L7to0.X2, L7to0.Y2);
        }

        private void sensorLines(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawLines(Pens.Black, linePoints);
        }

        private void robotsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            if (robotListActive) {
                if (e.IsSelected) {
                    selectedItemActive = true;
                    selectBot = robotsList.SelectedItems[0].Text;
                    //MessageBox.Show("ItemSelectionChanged - Item selected... \n" + robotsList.SelectedItems[0].Text, "Message");
                } else {
                    selectedItemActive = false;
                }
            }
        }

        private void robotsList_SelectedIndexChanged(object sender, EventArgs e) {
            
        }

        // ****** Txt File Functions ***************************************************************
        
        private void statusFile(string nameFile) {
            List<string> param = new List<string>();
            param.Add("Gossip: *" + gen.translateBool(enCtls[0]));
            param.Add("Aggregation: *" + gen.translateBool(enCtls[1]));
            param.Add("Follower: *" + gen.translateBool(enCtls[2]));
            param.Add("Object CLustering: *" + gen.translateBool(enCtls[3]));
            param.Add("Collective Transport: *" + gen.translateBool(enCtls[4]));
            param.Add("Directions: *" + gen.translateBool(enCtls[5]));
            param.Add("Camera: *" + gen.translateBool(enCtls[6]));
            param.Add("IR Sensors: *" + gen.translateBool(enCtls[7]));
            File.WriteAllLines(@nameFile, param);                               // Saves all the string elements in a same file (Encoding UTF-8)...
        }

        // ****** Gets & Stes Methods ********************************************

    }

    static class Constants {
        public const int normalWidth = 600;
        public const int normalHeight = 550;
        public const int extendedWidth = 800;
        public const int spaceObjects = 30;
        public const int XPDecoOriginal = 165;                                                              // X center of position circle trayectory...
        public const int YPDecoOriginal = 415;                                                              // Y center of position circle trayectory...

        public const int socketBufferSize = 10000;
        public const int robotBufferSize = 1024;
        public const int sensorMaxValue = 4096;
        public const int circleRadious = 30;
        public const int b = 70;
        public const int rotFactor = 2;                                                                     // Degrees to rotate the position deco...

        public const double r2dFactor = Math.PI / 180.0;                                                    // Radians to Degrees factor multiplier...
        
        private const double angle_between_sensors = 45;
        public static readonly double cosAlpha = Math.Cos((Math.PI / 180) * (angle_between_sensors / 2));   // Alpha = 22.5...
        public static readonly double sinAlpha = Math.Sin((Math.PI / 180) * (angle_between_sensors / 2));   // Alpha = 22.5...
        public const double m = -0.00732;                                                                   // m = -0.00732...

        public const ushort port = 3006;
    }
}
