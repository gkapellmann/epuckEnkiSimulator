using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using AForge.Video.FFMPEG;

using Generals;

namespace General_Logger {

    class Summary {

        public struct dataIndex {
            public int time;
            public int BCluster;
            public int SecArea;
            public int D0;
            public int D1;
            public int D2;
            public int D3;
            public int Rob0;
        };

        public struct dataAverages {
            public double D0;
            public double final_D0;
            public double D1;
            public double final_D1;
            public double D2;
            public double final_D2;
            public double D3;
            public double final_D3;
            public int counter;
        };

        General gen = new General();

        bool[] selection = new bool[MyConstants.bitswanted];	            // Titles, RobA, RobY, RobX, Dist3, Dist2, Dist1, Dist0, Secret Area, Cluster, Time S, Time Ms
        bool biggestClusters = false;
	    bool secretAreas = false;
	    bool allClusters = false;
	    bool averageMetrics = false;
	    bool Dist0 = false;
	    bool Dist1 = false;
	    bool Dist2 = false;
	    bool Dist3 = false;

	    int robots = 0;
        int totalAmountFiles = 0;

        string dirTXT;
        string dirCSV;
        string mark;

        List<string> cmdTexts = new List<string>();
        List<string> txtSubFolders = new List<string>();
        List<string> csvSubFolders = new List<string>();

        dataIndex dIndex = new dataIndex();
        dataAverages aAvg = new dataAverages();

        public Summary() {                                             // Constructor...
            initCmdTexts();
        }

        private void initCmdTexts(){
	        cmdTexts.Add("Other Command");
	
	        cmdTexts.Add("Move forward");
	        cmdTexts.Add("Move backward");
	        cmdTexts.Add("Rotate left");
	        cmdTexts.Add("Rotate right");
	        cmdTexts.Add("Stop moving");

	        cmdTexts.Add("New robot");		// Plus the robot ID...
	        cmdTexts.Add("lead_select");	// Plus the robot ID...
	        cmdTexts.Add("Release current");

	        cmdTexts.Add("gossip algorithm");
	        cmdTexts.Add("aggregation algorithm");
	        cmdTexts.Add("follower algorithm");
	        cmdTexts.Add("object cluster algorithm");
	        cmdTexts.Add("transport algorithm");

	        cmdTexts.Add("back to algorithm");
	        cmdTexts.Add("new image");
	        cmdTexts.Add("Feedback on");
	        cmdTexts.Add("Feedback off");
	
	        cmdTexts.Add("Checkpoint Saved"); // + the time...

	        cmdTexts.Add("General");
            cmdTexts.Add("tmStamp");
	        // cmdTexts[17] = "user disconnected" - User disconnection + time...
	        // Goals (dependant)...
        }

        public void process_Summary(MainMenu.mainMenu mainForm, string cmdFileLoc, string posFileLoc, string fileMark, string ownMark, int stTime_ms, int tStep_ms, int flags) {

            dirTXT = cmdFileLoc;
            dirCSV = posFileLoc;
            mark = fileMark;

            StreamWriter activityLog = new StreamWriter(@"Data\Logger_Summary.txt");

            for (int i = 0; i < MyConstants.bitswanted; i++) {
                if (gen.GetBit(flags,i)) {
                    selection[i] = true;
                } else {
                    selection[i] = false;
                }
            }

            activityLog.WriteLine("***** Logging all files related to: " + fileMark + " ******\n");
            prepareProgressBar(mainForm);
            // ****** Commands .csv ****************************************************************************************
            activityLog.WriteLine("****** TXT Files *************************************************\n");
            if (Directory.Exists(cmdFileLoc)) {         // If the desired directory exists...
                analyse_txt(mainForm, fileMark, ownMark, activityLog);
            } else {
                MessageBox.Show("Commands directory not found.", "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ****** Positions .csv ***************************************************************************************
            activityLog.WriteLine("****** CSV Files *************************************************\n");
            if (Directory.Exists(posFileLoc)) {         // If the desired directory exists...
                analyse_csv(mainForm, fileMark, ownMark, tStep_ms, stTime_ms, selection, activityLog);
            } else {
                MessageBox.Show("Positions directory not found.", "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            mainForm.updateLoggerPBValue(0);

            activityLog.WriteLine("******************************************************************\n\n");
            activityLog.WriteLine("End of logging process...");

            activityLog.Close();
            activityLog.Dispose();
        }

        private void prepareProgressBar(MainMenu.mainMenu mForm) {
            List<string> txtTemp = new List<string>();
            List<string> csvTemp = new List<string>();
            txtTemp = gen.DirSearch(dirTXT);										// Find subfolders in directory...
            csvTemp = gen.DirSearch(dirCSV);										// Find subfolders in directory...
            if (txtTemp.Count > 0) {							// If there are any files...		
                if (gen.checkFileExist(txtTemp, mark)) {
                    totalAmountFiles = txtTemp.Count;
                }
            }
            if (csvTemp.Count > 0) {							// If there are any files...		
                if (gen.checkFileExist(csvTemp, mark)) {
                    totalAmountFiles += csvTemp.Count;
                }
            }
            mForm.updateLoggerPBStats(0, totalAmountFiles, 1);
        }

        // ****** TXT Command Files ******

        private void analyse_txt(MainMenu.mainMenu mForm, string fileMark, string studyMark, StreamWriter log) {
            int fileCounter = 0;														// Create a file counter...	
            txtSubFolders = gen.DirSearch(dirTXT);										// Find subfolders in directory...
            if (txtSubFolders.Count > 0) {							// If there are any files...		
                if (gen.checkFileExist(txtSubFolders, mark)) {
                    string OutputFolder = "Logger\\Assembled\\" + fileMark + "_" + studyMark + "_Commands_Log";			// Position where the results will be saved...
                    Directory.CreateDirectory(OutputFolder);
                    try {
                        for (int j = 0; j < txtSubFolders.Count; j++) {
                            if (gen.textAfterMark(txtSubFolders[j], "\\").Contains(fileMark)) {
                                string fileName = txtSubFolders[j];
                                string ID = gen.textBeforeMark(gen.textAfterMark(fileName, "\\"), "-");
                                string logNum = gen.textBeforeMark(gen.textAfterMark(fileName, "Log"),".");
                                string pos_file_name = OutputFolder + "\\" + ID + "_" + fileMark + " CL" + logNum + " FNum" + fileCounter.ToString() + ".csv";
                                process_txt(pos_file_name, fileName);
                                log.WriteLine("Processing: " + txtSubFolders[j]);
                                fileCounter++;
                            }
                            mForm.updateLoggerPBPerformStep();
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                    }
                    log.WriteLine("\n");
                }
            } else {
                MessageBox.Show("The file codename was not found...");
            }
        }

        private void process_txt(string pos_file_name, string fileName) { 
            
            int[] cmdCounts = new int[cmdTexts.Count];
            int allMoves = 0;
            int allAlgs = 0;
            int lastIndex = -1;
            int lastSec = 0;

            string data_to_write = "";
            string finalGeneralNotes = "";
            string userComments = "";

            string[] infile = File.ReadAllLines(fileName);
            StreamWriter posLogger = new StreamWriter(pos_file_name);       // Create a report CSV file...

            // ********************************************
            string dataTimes = "";
            string tempDL = "";
            string dataLeader = "";
            // ********************************************
            
            for (int i = 0; i < infile.Length; i++ ) {                      // Go through all the document...
                if(infile[i].Contains("Time [ms]")){
                    bool correctTime = gen.timeConversionTXT(infile[i], ref data_to_write);									// If the desired time is met, process the line...
                    if (correctTime) {
                        bool process = true;
                        int t_Sec = gen.returnTime_S_TXT(infile[i]);
                        int index = detectCommand(infile[i]);

                        // Check for time repetition and for action repetition...
                        if (index == lastIndex) {
                            int secDif = 200;
                            if (secDif > 200) {
                                process = false;
                            }
                        }

                        if(process){
					        if(index == 1 || index == 2 || index == 3 || index == 4 || index == 5){
						        allMoves++;
					        }
					        if(index == 9 || index == 10 || index == 11 || index == 12 || index == 13){
						        allAlgs++;
					        }
					        if(index == 19){
						        if(infile[i].Contains("simulation time reached")){
							        finalGeneralNotes += infile[i] + "\n";
						        }
					        }
					        if(index == 20){
                                userComments += infile[i] + "\n";
					        }
					        cmdCounts[index]++;
				        }

                        lastIndex = index;
                        lastSec = t_Sec;
                    }
                }
            }

            data_to_write = fileName + "\n";
            data_to_write += "Command Log Count \n \n Commands , Repetitions \n";

            for (int i = 0; i < cmdTexts.Count; i++) {
                switch (i) {
                    case 6:
                        cmdCounts[i + 1] += cmdCounts[i];
                        break;
                    case 7:
                        data_to_write += "Changes of leader: ," + cmdCounts[i].ToString() + "\n";
                        break;
                    case 15:
                        data_to_write += "Image Requests: ," + cmdCounts[i].ToString() + "\n";
                        break;
                    case 18:
                        data_to_write += "Checkpoints saved: ," + cmdCounts[i].ToString() + "\n";
                        break;
                    default:
                        data_to_write += cmdTexts[i] + ": ," + cmdCounts[i].ToString() + "\n";
                        break;
                }
            }
            
            data_to_write += "\n";
            data_to_write += "Total move commands: ," + allMoves.ToString() + "\n";
            data_to_write += "Total algorithm changes: ," + allAlgs.ToString() + "\n \n";

            data_to_write += userComments + "\n";
            data_to_write += finalGeneralNotes;
            posLogger.Write(data_to_write);

            posLogger.Close();
            posLogger.Dispose();
        }

        private int detectCommand(string phrase) {
            int cmdVal = 0;
            for(int i=1; i<cmdTexts.Count; i++){
		        if(phrase.Contains(cmdTexts[i])){
			        cmdVal = i;
			        break;
		        }
	        }
            return cmdVal;
        }

        // ****** CSV Position Files ******
        
        private void analyse_csv(MainMenu.mainMenu mForm, string fileMark, string studyMark, int timeStep, int timeStartReference, bool[] selected, StreamWriter log) {
            int fileCounter = 0;														// Create a file counter...	
            csvSubFolders = gen.DirSearch(dirCSV);										// Find subfolders in directory...
            if (csvSubFolders.Count > 0) {							// If there are any files...		
                if (gen.checkFileExist(csvSubFolders, mark)) {
                    string OutputFolder = "Logger\\Assembled\\" + fileMark + "_" + studyMark + "_Positions_Log";			// Position where the results will be saved...
                    Directory.CreateDirectory(OutputFolder);
                    try {
                        for (int j = 0; j < csvSubFolders.Count; j++) {
                            if (gen.textAfterMark(csvSubFolders[j],"\\").Contains(fileMark)) {
                                string fileName = csvSubFolders[j];
                                string ID = gen.textBeforeMark(gen.textAfterMark(fileName, "\\"), "-");
                                string logNum = gen.textBeforeMark(gen.textAfterMark(fileName, "Log"),".");
                                string pos_file_name = OutputFolder + "\\" + ID + "_" + fileMark + " PL" + logNum + " FNum" + fileCounter.ToString() + ".csv";
                                process_csv(pos_file_name, fileName, timeStartReference, timeStep, selected);
                                log.WriteLine("Processing: " + csvSubFolders[j]);
                                fileCounter++;
                            }
                            mForm.updateLoggerPBPerformStep();
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                    }
                    log.WriteLine("\n");
                }
            } else {
                MessageBox.Show("The file codename was not found...");
            }
        }

        private void process_csv(string pos_file_name, string fileName, int t, int tS, bool[] selected) {
            int linePointer = 0;
            string data_to_write = "";
            
            string[] infile = File.ReadAllLines(fileName);
            StreamWriter posLogger = new StreamWriter(pos_file_name);       // Create a report CSV file...

            initTitleVars();
            while (!infile[linePointer].Contains("Time [ms]")) {												                // Find the sections title line...
                linePointer++;
            }
            bool firstValue = true;
            processTitlesLine(infile[linePointer], selected, posLogger);                                                        // dIndex is calculated...
            for (int i = linePointer; i < infile.Length; i++) {
                string[] data = infile[i].Split(',');
                bool correctTime = gen.timeConversionCSV(data, ref data_to_write, ref t, tS, selected[0], selected[1]);	        // If the desired time is met, process the line...
                averageCalculation(correctTime, ref firstValue, data);
                if (correctTime) {
                    // ****** Robot Clustering ***********************************
                    if (allClusters) {
                        if (selected[2]) {
                            data_to_write += (gen.countClusters(data, robots)).ToString() + ",";
                        }
                    }
                    if (biggestClusters) {
                        if (selected[3]) {
                            data_to_write += data[dIndex.BCluster] + ",";
                        }
                    }
                    // ****** Secret Area ****************************************
                    if (secretAreas) {
                        if (selected[4]) {
                            data_to_write += data[dIndex.SecArea] + ",";
                        }
                    }
                    // ****** Object Distances ***********************************
                    if (Dist0) {
                        if (selected[5]) {
                            if (averageMetrics) {
                                data_to_write += aAvg.final_D0.ToString() + ",";
                            } else {
                                data_to_write += data[dIndex.D0] + ",";
                            }
                        }
                    }
                    if (Dist1) {
                        if (selected[6]) {
                            if (averageMetrics) {
                                data_to_write += aAvg.final_D1.ToString() + ",";
                            } else {
                                data_to_write += data[dIndex.D1] + ",";
                            }
                        }
                    }
                    if (Dist2) {
                        if (selected[7]) {
                            if (averageMetrics) {
                                data_to_write += aAvg.final_D2.ToString() + ",";
                            } else {
                                data_to_write += data[dIndex.D2] + ",";
                            }
                        }
                    }
                    if (Dist3) {
                        if (selected[8]) {
                            if (averageMetrics) {
                                data_to_write += aAvg.final_D3.ToString() + ",";
                            } else {
                                data_to_write += data[dIndex.D3] + ",";
                            }
                        }
                    }
                    // ****** Robot Coordinates ***********************************
                    int dataPos = dIndex.Rob0;
                    for (int r = 0; r < robots; r++) {
                        //cout << "Robot " << r << " Coords: X->" << data[dataPos] << " Y->" << data[dataPos+1] << " A->" << data[dataPos+2] << endl;
                        if (selected[10]) {
                            data_to_write += data[dataPos] + ",";
                        }
                        dataPos++;
                        if (selected[11]) {
                            data_to_write += data[dataPos] + ",";
                        }
                        dataPos++;
                        if (selected[12]) {
                            data_to_write += data[dataPos] + ",";
                        }
                        dataPos++;
                    }
                    posLogger.WriteLine(data_to_write);
                }
            }
            posLogger.Close();
            posLogger.Dispose();

        }

        private void averageCalculation(bool cTime, ref bool fValue, string[] data){
	        if(averageMetrics){
		        if(cTime){
			        if(Dist0){
                        aAvg.final_D0 = gen.averageOrSimple(fValue, data[dIndex.D0], aAvg.final_D0, ref aAvg.D0, aAvg.counter);	
			        }
			        if(Dist1){
                        aAvg.final_D1 = gen.averageOrSimple(fValue, data[dIndex.D1], aAvg.final_D1, ref aAvg.D1, aAvg.counter);	
			        }
			        if(Dist2){
                        aAvg.final_D2 = gen.averageOrSimple(fValue, data[dIndex.D2], aAvg.final_D2, ref aAvg.D2, aAvg.counter);	
			        }
			        if(Dist3){
                        aAvg.final_D3 = gen.averageOrSimple(fValue, data[dIndex.D3], aAvg.final_D3, ref aAvg.D3, aAvg.counter);	
			        }
			        /*if(firstValue){
				        aAvg.final_D0 = stod(data[dIndex.D0]);
			        }else{
				        aAvg.final_D0 = aAvg.D0 / aAvg.counter;
			        }
			        aAvg.D0 = 0;*/
			        aAvg.counter = 0;
			        fValue = false;
		        }else{
			        if(!fValue){
				        if(Dist0){
					        aAvg.D0 += Convert.ToDouble(data[dIndex.D0]);
				        }
				        if(Dist1){
                            aAvg.D1 += Convert.ToDouble(data[dIndex.D1]);
				        }
				        if(Dist2){
                            aAvg.D2 += Convert.ToDouble(data[dIndex.D2]);
				        }
				        if(Dist3){
                            aAvg.D3 += Convert.ToDouble(data[dIndex.D3]);
				        }
			        }
			        aAvg.counter++;
		        }
	        }
        }

        private void processTitlesLine(string titles, bool[] write, StreamWriter filelog){
	        bool firstBot = true;
	        string title = "";

            string[] ss = titles.Split(',');
	        if(write[0]){
		        title += "Time [ms],";
	        }
	        if(write[1]){ 
		        title += "Time [s],";
	        }

	        int dataPos = 0;
	        dIndex.time = dataPos;
            for(int i=1; i<ss.Length; i++){											// Load information of the robots into a vector of structs "robot"...
		        if(write[2] && !allClusters){
			        title += "All Clusters,";
			        allClusters = true;
		        }
		        if(write[9] && !averageMetrics){
			        averageMetrics = true;
		        }
		        if(ss[i].Contains("Cluster")){
			        if(write[3]){ 
				        title += "Biggest Cluster,";
			        }
			        dataPos++;
			        dIndex.BCluster = dataPos;
			        biggestClusters = true;
		        }else if(ss[i].Contains("Secret Area")){
			        if(write[4]){ 
				        title += "Secret Area,";
			        }
			        dataPos++;
			        dIndex.SecArea = dataPos;
			        secretAreas = true;
		        }else if(ss[i].Contains("Distance 0")){
			        if(write[5]){ 
				        title += "Distance 0,";
			        }
			        dataPos++;
			        dIndex.D0 = dataPos;
			        Dist0 = true;
		        }else if(ss[i].Contains("Distance 1")){
			        if(write[6]){ 
				        title += "Distance 1,";
			        }
			        dataPos++;
			        dIndex.D1 = dataPos;
			        Dist1 = true;
		        }else if(ss[i].Contains("Distance 2")){
			        if(write[7]){ 
				        title += "Distance 2,";
			        }
			        dataPos++;
			        dIndex.D2 = dataPos;
			        Dist2 = true;
		        }else if(ss[i].Contains("Distance 3")){
			        if(write[8]){ 
				        title += "Distance 3,";
			        }
			        dataPos++;
			        dIndex.D3 = dataPos;
			        Dist3 = true;
		        }else if(ss[i].Contains("Robot")){
			        if(firstBot){
				        dataPos++;
				        dIndex.Rob0 = dataPos;
				        firstBot = false;
			        }
			        if(write[10]){ 
				        title += robots.ToString() + "Bot X,";
			        }
			        if(write[11]){
                        title += robots.ToString() + "Bot Y,";
			        }
			        if(write[12]){ 
				        title += robots.ToString() + "Bot A,";
			        }
			        robots++;
		        }else{
		
		        }
	        }
	        if(write[13]){
                filelog.WriteLine(title);
	        }
        }

        private void initTitleVars(){
	        biggestClusters = false;
	        secretAreas = false;
	        allClusters = false;
	        averageMetrics = false;
	        Dist0 = false;
	        Dist1 = false;
	        Dist2 = false;
	        Dist3 = false;

	        robots = 0;
        }
    }
    
    class Video_Builder {

        public struct rCoords {
            public int x, y;
            public double a;

            public rCoords(int p1, int p2, double p3) {
                x = p1;
                y = p2;
                a = p3;
            }

            public void updateCoords(int p1, int p2, double p3) {
                x = p1;
                y = p2;
                a = p3;
            }
        }

        public struct oCoords {
            public int x, y, t;

            public oCoords(int xp, int yp, int type) {
                x = xp;
                y = yp;
                t = type;
            }

            public void updateCoords(int xp, int yp, int type) {
                x = xp;
                y = yp;
                t = type;
            }

        }

        public struct wCoords {
            public double xL, yL, xC, yC;
            public int col;

            public wCoords(double xl, double yl, double xc, double yc, int c) {
                xL = xl;
                yL = yl;
                xC = xc;
                yC = yc;
                col = c;
            }

            public void updateCoords(double xl, double yl, double xc, double yc, int c) {
                xL = xl;
                yL = yl;
                xC = xc;
                yC = yc;
                col = c;
            }

        }

        General gen = new General();

        public void process_Video(MainMenu.mainMenu mainForm, string fileLocation, int framesJump, bool saveVideo, bool saveImages) {
            string[] lines = File.ReadAllLines(fileLocation);

            bool checkLines = true;
            bool clearContinue = true;

            int genCount = 0;
            int robotCount = 0;
            int objCount = 0;
            int indexOfRobot = 0;                   // How many data points will be ignored to find the robots...
            int indexOfObjects = 0;                 // How many data points will be ignored to find the objects...
            int W = 0;
            int H = 0;
            string envWalls = "";
            string folderName = "";

            List<wCoords> walls = new List<wCoords>();
            List<Bitmap> videoData = new List<Bitmap>();

            try {
                // ****** Folder Creation if not exists ********************************************************************************
                int fNamePos = fileLocation.LastIndexOf("\\") + 1;
                folderName = "Logger\\Replays\\" + fileLocation.Substring(fNamePos, fileLocation.Length - fNamePos - 4);
                if (!Directory.Exists(folderName)) {
                    Directory.CreateDirectory(folderName);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                clearContinue = false;
            }

            if (clearContinue) {
                try {
                    // ****** Try to get the arena size (Only possible in 2.0 records) *****************************************************
                    W = Convert.ToInt32(gen.getSubStringBetween(lines[0], "Width", ","));
                    H = Convert.ToInt32(gen.getSubStringBetween(lines[0], "Height", ","));
                    envWalls = gen.getSubStringBetween(lines[1], "Environment", ",");
                } catch (Exception ex) {
                    W = Convert.ToInt32(Interaction.InputBox("Provide the environment Width:", "Environment dimention not found!", "400", -1, -1));
                    H = Convert.ToInt32(Interaction.InputBox("Provide the environment Height:", "Environment  dimention not found!", "300", -1, -1));
                    envWalls = Interaction.InputBox("Provide the name of the used environmet:", "Environment name not found!", "None", -1, -1);
                }

                try {
                    // ****** Configure recording ******************************************************************************************
                    while (checkLines) {
                        if (lines[genCount].IndexOf("amount of Robots") != -1) {        // Obtain the amount of robots in record...
                            int chp = lines[genCount].IndexOf(",") + 1;
                            robotCount = Convert.ToInt32(lines[genCount].Substring(chp, lines[genCount].Length - chp));
                        }
                        if (lines[genCount].IndexOf("Time [ms]") != -1) {               // Detect what is saved... (Time, measurments, robots, objects)...
                            string[] recordTitles = lines[genCount].Split(',');
                            indexOfRobot = Array.IndexOf(recordTitles, "Robot 0");
                            indexOfObjects = Array.IndexOf(recordTitles, "Object 0");
                            if (indexOfObjects != -1) {
                                objCount = (recordTitles.Count() - indexOfObjects - 1) / 2;   // Position x + Position y of each object...
                                //MessageBox.Show(indexOfObjects.ToString() + " " + objCount.ToString());
                            }
                        }
                        genCount++;
                        if (genCount > 10) {
                            if (!lines[genCount].Substring(0, 1).Equals("0")) {
                                checkLines = false;
                            }
                        }
                    }

                    // ****** Prepare GUI for image and video creation ********************************************************************
                    int countFramesjump = framesJump + 1;           // So the first frame is processed...
                    mainForm.updateVideoPBStats(genCount, lines.Length, 1);
                    if (!envWalls.Equals("none")) {
                        walls = getWallsofImage(envWalls);
                    } else {
                        walls.Clear();
                    }

                    // ****** Create the images *******************************************************************************************
                    for (int i = genCount; i < lines.Length; i++) {
                        if (countFramesjump >= framesJump) {
                            Bitmap thisImage = CreateBitmapAtRuntime(getRobotsOfImage(lines[i], robotCount, indexOfRobot), getObjectsOfImage(lines[i], objCount, indexOfObjects), walls, W, H);
                            videoData.Add(thisImage);
                            if (saveImages) {
                                string name = folderName + "\\Frame_" + (i - genCount).ToString() + ".jpg";
                                thisImage.Save(name);
                            }
                            //mainForm.updateImageShower(thisImage);
                            countFramesjump = 0;
                        } else {
                            countFramesjump++;
                        }
                        mainForm.updateVideoPBPerformStep();
                    }
                    mainForm.updateVideoPBValue(genCount);

                    // ****** Create video ************************************************************************************************
                    if (saveVideo) {
                        CreateVideoAtRuntime(folderName, videoData);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private Bitmap CreateBitmapAtRuntime(List<rCoords> robCds, List<oCoords> objCds, List<wCoords> wallCds, int w, int h) {
            int BPensize = 1;
            float ObjRad = 4.5f;
            float BotRad = 4f;

            // Origin (0,0) is located at the top left corner...
            Bitmap imageRec = new Bitmap(w, h);
            Pen blackPen = new Pen(Color.Black, BPensize);
            Graphics arena = Graphics.FromImage(imageRec);
            arena.DrawRectangle(blackPen, 0, 0, w - BPensize, h - BPensize);  // Arena overall frame...

            // ****** Add environment painting...
            for (int k = 0; k < wallCds.Count; k++) {
                arena.FillRectangle(setFillColor(wallCds[k].col), (int)wallCds[k].xC, (int)wallCds[k].yC, (int)wallCds[k].xL, (int)wallCds[k].yL);
            }

            // ****** Add objects...
            for (int i = 0; i < objCds.Count; i++) {
                float x = objCds[i].x - ObjRad;
                float y = objCds[i].y - ObjRad;
                float width = 2 * ObjRad;
                float height = 2 * ObjRad;
                arena.FillEllipse(Brushes.Red, x, y, width, height);
            }

            // ****** Add robots...
            for (int i = 0; i < robCds.Count; i++) {
                float x = robCds[i].x - BotRad;
                float y = robCds[i].y - BotRad;
                float width = 2 * BotRad;
                float height = 2 * BotRad;
                arena.FillEllipse(Brushes.Green, x, y, width, height);
                Rectangle rect = new Rectangle(robCds[i].x - 1, robCds[i].y, 2, (int)BotRad);
                RotateRectangle(arena, rect, (float)gen.RadianToDegree(robCds[i].a) + 270.0F, robCds[i].x, robCds[i].y);
            }

            //int fac = 2;
            imageRec.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //Bitmap newImage = new Bitmap(w * fac, h * fac);
            //using (var graphics = Graphics.FromImage(newImage)) {
            //    graphics.DrawImage(imageRec, 0, 0, w + fac, h + fac);
            //}

            return imageRec;
        }

        private void CreateVideoAtRuntime(string location, List<Bitmap> data) {
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(location + "\\VideoRun.avi", data[0].Width, data[0].Height, 25, VideoCodec.MPEG4, 1000000);

            //Bitmap image = new Bitmap(width, height);
            //Graphics g = Graphics.FromImage(image);
            //g.DrawString("Replay: xxx", new System.Drawing.Font("Calibri", 30), Brushes.White, 80, 240);
            //g.Save();
            //g.FillRectangle(Brushes.Black, 0, 0, width, height);
            //for (int i = 0; i < 25; i++) {
            //    writer.WriteVideoFrame(image);
            //}

            for (int i = 0; i < data.Count; i++) {
                System.Windows.Forms.Application.DoEvents();        // Lets the application keep working while the video is been processed...
                writer.WriteVideoFrame(data[i]);
            }

            writer.Close();
        }

        private void RotateRectangle(Graphics g, System.Drawing.Rectangle r, float angle, int x, int y) {
            using (Matrix m = new Matrix()) {
                m.RotateAt(angle, new PointF(x, y));
                g.Transform = m;
                g.FillRectangle(Brushes.Yellow, r);
                g.ResetTransform();
            }
        }

        private SolidBrush setFillColor(int selection) {
            SolidBrush brush;
            switch (selection) {
                case 0:
                    brush = new SolidBrush(Color.White);
                    break;
                case 1:
                    brush = new SolidBrush(Color.LightGray);
                    break;
                case 2:
                    //const Color Color::lightGray (0.8, 0.8, 0.8);
                    //const Color Color::midLightGray(0.65, 0.65, 0.65);
                    //const Color Color::gray(0.5, 0.5, 0.5);
                    //const Color Color::midHeavyGray(0.35, 0.35, 0.35);
                    //const Color Color::heavyGray (0.20, 0.20, 0.20);
                    //Color c = Color.FromArgb(0.65, 0.65, 0.65);
                    brush = new SolidBrush(Color.LightSlateGray);
                    break;
                case 3:
                    brush = new SolidBrush(Color.Gray);
                    break;
                case 4:
                    brush = new SolidBrush(Color.DimGray);
                    break;
                case 5:
                    brush = new SolidBrush(Color.DarkSlateGray);
                    break;
                case 6:
                    brush = new SolidBrush(Color.Black);
                    break;
                case 7:
                    brush = new SolidBrush(Color.Red);
                    break;
                case 8:
                    brush = new SolidBrush(Color.Blue);
                    break;
                case 9:
                    brush = new SolidBrush(Color.Green);
                    break;
                case 10:
                    brush = new SolidBrush(Color.Purple);
                    break;
                case 11:
                    brush = new SolidBrush(Color.Cyan);
                    break;
                case 12:
                    brush = new SolidBrush(Color.Yellow);
                    break;
                default:
                    brush = new SolidBrush(Color.Gray);
                    break;
            }
            return brush;
        }

        private List<wCoords> getWallsofImage(string envFile) {
            List<wCoords> wCds = new List<wCoords>();
            string[] wallsLines = File.ReadAllLines("Data\\Environments\\" + envFile + ".txt");
            for (int i = 0; i < wallsLines.Length; i++) {
                if (wallsLines[i].IndexOf("w") != -1) {
                    string[] param = wallsLines[i].Split(',');
                    double xLenght = Convert.ToDouble(param[1]);
                    double yLenght = Convert.ToDouble(param[2]);
                    double xCenter = Convert.ToDouble(param[4]) - (xLenght / 2);      // x Type: System.Int32 The x-coordinate of the upper-left corner of the rectangle to fill.
                    double yCenter = Convert.ToDouble(param[5]) - (yLenght / 2);      // y Type: System.Int32 The y-coordinate of the upper-left corner of the rectangle to fill.
                    wCoords wCd = new wCoords(xLenght, yLenght, xCenter, yCenter, Convert.ToInt32(param[6]));
                    wCds.Add(wCd);
                }
            }
            return wCds;
        }

        private List<rCoords> getRobotsOfImage(string line, int nRobots, int offset) {
            List<rCoords> rCds = new List<rCoords>();
            string[] robParam = line.Split(',');
            int paramIndex = offset;
            for (int r = 0; r < nRobots; r++) {
                rCoords rC = new rCoords(Convert.ToInt32(robParam[paramIndex++]), Convert.ToInt32(robParam[paramIndex++]), Convert.ToDouble(robParam[paramIndex++]));
                rCds.Add(rC);
                //MessageBox.Show(rxPos.ToString() + " " + ryPos.ToString() + " " + raPos.ToString());
            }
            return rCds;
        }

        private List<oCoords> getObjectsOfImage(string line, int nObjects, int offset) {
            List<oCoords> oCds = new List<oCoords>();
            string[] objParam = line.Split(',');
            int paramIndex = offset;
            for (int r = 0; r < nObjects; r++) {
                oCoords oC = new oCoords(Convert.ToInt32(objParam[paramIndex++]), Convert.ToInt32(objParam[paramIndex++]), 1);
                oCds.Add(oC);
                //MessageBox.Show(rxPos.ToString() + " " + ryPos.ToString() + " " + raPos.ToString());
            }
            return oCds;
        }

    }

}
