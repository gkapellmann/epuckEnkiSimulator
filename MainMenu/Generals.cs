using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generals {

    class General {

        public struct robot {
            public int id;
            public int x;
            public int y;
            public double a;
        };

        public bool checkForOnlyNumbers(KeyPressEventArgs e) {
            bool correct = false;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                correct = true;
            }
            return correct;
        }

        public bool IsBitSet(int b, int pos) {
            return (b & (1 << pos)) != 0;
        }

        public bool GetBit(int data, int bitNumber) {
            BitArray ba = new BitArray(new int[] { data });
            return ba.Get(bitNumber);
        }

        public bool timeConversionTXT(string line, ref string dataWrite){
	        bool wToFile = true;
            if (line.Contains("[ms]")) {
                int time_ms = returnTime_mS_TXT(line);
                int inSec = time_ms / 1000;
                dataWrite = time_ms.ToString() + "," + inSec.ToString() + ",";
            } else {
                wToFile = false;
                dataWrite = " ";
            }
	        return wToFile;
        }

        public bool timeConversionCSV(string[] data, ref string dataWrite, ref int tForProg, int tRef, bool miliSec, bool Sec){
	        bool wToFile = false;
	        try{
		        string time_ms_s = data[0];
		        int time_ms = Convert.ToInt32(time_ms_s);
		        if(time_ms > tForProg){							
			        int inSec = time_ms / 1000;
			        tForProg += tRef;
			        if(miliSec){
				        dataWrite = time_ms.ToString() + ",";
			        }
			        if(Sec){
                        dataWrite = inSec.ToString() + ",";
			        }
			        wToFile = true;
		        }
	        }catch(Exception ex){
		        wToFile = false;
	        }
	        return wToFile;
        }

        public bool checkFileExist(List<string> files, string mark) {
            bool test = false;
            for (int i = 0; i < files.Count; i++) {
                if (textAfterMark(files[i], "\\").Contains(mark)) {
                    test = true;
                    break;
                }
            }
            return test;
        }

        public bool IsDivisble(int x, int n) {
            return (x % n) == 0;
        }

        public int returnTime_mS_TXT(string line){
	        int ms = 0;
	        try{
                ms = getTimeMs(line);
	        }catch(Exception ex){
		        ms = 0;
	        }
	        return ms;
        }

        public int returnTime_S_TXT(string line){
	        int s = 0;
	        try{
		        s = returnTime_mS_TXT(line);
		        s /= 1000;
	        }catch(Exception ex){
		        s = 0;
	        }
	        return s;
        }

        public int getTimeMs(string line) {
            string[] parts = line.Split(' ');
            int pos = parts[1].IndexOf(":") + 1;
            return Convert.ToInt32(parts[1].Substring(pos, parts[1].Length - pos));
        }

        public int countClusters(string[] posData, int nRobots){									// Counts the number of clusters in an instant, based on the robots positions...
	        int p=0, t=0, biggestGroup=0, iter_turn = 0;
	        List<int> countedBot = new List<int>();
            List<robot> allBots = new List<robot>(); ;

	        int spaces = posData.Length - 1;
	        for(int b = nRobots-1; b >= 0; b--){
		        countedBot.Add(-1);
		        robot bot;
		        bot.id = b;
		        bot.a = Convert.ToDouble(posData[spaces--]);
                bot.y = Convert.ToInt32(posData[spaces--]);
		        bot.x = Convert.ToInt32(posData[spaces--]);
		        allBots.Add(bot);
		        //cout << "ID:" << bot.id << " X:" << bot.x << " Y:" << bot.y << " A:" << bot.a << endl;
	        }
	        countedBot[p] = 0;

	        do{
		        int j=1;
		        do{
			        for(int i=0; i<allBots.Count; i++){														    // Check within all initialized robots...
				        // Get coordinates...
				        if(distBetweenPoints(allBots[countedBot[p]], allBots[i]) <= MyConstants.clusterDistance){	// Check for close robots within a range...
                            if(!countedBot.Contains(allBots[i].id)){	// If doens't exist in focused array...
						        countedBot[j+t] = allBots[i].id;												// Add for further focus...				
						        j++;
					        }
				        }
				
			        }
			        p++;																						// Next focused robot...
		        }while(p!=j+t);

		        t+=j;
		        if(j > 1){
			        iter_turn++;
		        }
		        if(j > biggestGroup){														                    // j = how many robots were counted...
			        biggestGroup = j;														
		        }
		        for(int i=0; i<nRobots; i++){
			        if(!countedBot.Contains(i)){		                                                        // If doens't exist in focused array...
				        countedBot[p] = i;
				        break;
			        }
		        }
		        //cout << "p=" << to_string(p) << " t=" << to_string(t) << " j=" << to_string(j) << endl;
	        }while(p < nRobots);

	        //cout << "Biggest group: " << biggestGroup << endl;
	        return iter_turn;
        }

        public double distBetweenPoints(robot e1, robot e2){
	        return Math.Sqrt((e2.x - e1.x)*(e2.x - e1.x) + (e2.y - e1.y)*(e2.y - e1.y));
        }

        public double distBetweenPoints(Point e1, Point e2) {
            return Math.Sqrt((e2.X - e1.X) * (e2.X - e1.X) + (e2.Y - e1.Y) * (e2.Y - e1.Y));
        }

        public double angleOfLine(Point p1, Point p2, bool axis, double dist) {     // If axis = true -> X Axis, If axis = false -> Y Axis...
            int co;
            if (axis) {
                co = Math.Abs(p1.X - p2.X);
            } else {
                co = Math.Abs(p1.Y - p2.Y);
            }
            return Math.Asin(co/dist);
        }

        public double averageOrSimple(bool fValue, string data, double result, ref double addition, int counter){
	        if(fValue){
		        result = Convert.ToDouble(data);
	        }else{
		        result = addition / counter;
	        }
	        addition = 0;
            return result;
        }

        public double RadianToDegree(double angle) {
            return angle * (180.0 / Math.PI);
        }

        public double DegreesToRadians(double degrees) {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        public string textAfterMark(string text, string mark) {
            int pos = text.LastIndexOf(mark) + 1;
            string onlyFileName = text.Substring(pos, text.Length - pos);
            return onlyFileName;
        }

        public string textBeforeMark(string text, string mark) {
            int pos = text.LastIndexOf(mark);
            string onlyFileName = text.Substring(0, pos);
            return onlyFileName;
        }

        public string translateBool(bool value) {
            if (value) {
                return "1";
            } else {
                return "0";
            }
        }

        public string getSubStringBetween(string input, string startRef, string mRef) {
            int stPos = input.IndexOf(startRef);
            int sLen = input.IndexOf(mRef, stPos) + 1;
            int sLen2 = input.IndexOf(mRef, sLen);
            return input.Substring(sLen, sLen2 - sLen);
        }

        public List<String> DirSearch(string sDir) {
            List<String> files = new List<String>();
            try {
                foreach (string f in Directory.GetFiles(sDir)) {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir)) {
                    files.AddRange(DirSearch(d));
                }
            } catch (System.Exception excpt) {
                MessageBox.Show(excpt.Message);
            }
            return files;
        }

    }

    class MyConstants {
        public const int bitswanted = 14;
        public const int clusterDistance = 15;
    }

}
