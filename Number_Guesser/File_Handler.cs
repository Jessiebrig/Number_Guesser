using System;
using System.IO;

namespace Number_Guesser
{
    public partial class Auto_Guesser
    {
        readonly static string directory = Environment.CurrentDirectory;
        readonly string Generatedtxt = directory + "\\Generated.txt";
        readonly string Configstxt = directory + "\\Configs.txt";
        readonly string TWords = directory + "\\TagalogWords.txt";
        //=====
        string TextValue;
        string[] TextArray;
        string filename;
        //=====
        StreamReader Streamvalue;
        public void Stream(string file)//Store Values to TextArray
        {
            filename = file;
            Streamvalue = new StreamReader(filename);
            TextValue = Streamvalue.ReadToEnd();
            TextArray = TextValue.Split('\n');
        }
        public void StreamClose() { Streamvalue.Close(); }
        //=====
        public void Write(string finame, string string_)
        {
            string[] Lines = string_.Split('\n');
            LOG("Writing " + Lines.Length + " Chars..");
            File.WriteAllLines(finame, Lines);
            LOG("Writing Complete..");
        }
        //=====
        public string Get_Gens()//Get_Generateds_____
        {
            Stream(Generatedtxt);
            TextValue = null;
            foreach (string value in TextArray)
            {
                TextValue += value + Environment.NewLine;
                //LOG("Generateds: " + value);
            }
            StreamClose();
            LOG("Initializing Generated.txt to TextArray[] Complete..");
            return TextValue;
        }
        //=====
        //Get_Config_____
        int Config_Index;
        public int Get_Int(string keyword) { return int.Parse(Get_String(keyword)); }//Get and Return int value from Config.txt
        public string Get_String(string keyword)//Get and Return string from Config.txt
        {
            Stream(Configstxt);
            for (Config_Index = 0; Config_Index < TextArray.Length; Config_Index++)//Get Value of the first keyword
            {
                if (TextArray[Config_Index].Contains(keyword))
                {
                    TextValue = TextArray[Config_Index];
                    TextValue = TextValue.Replace(keyword, "");
                    TextValue = TextValue.Remove(0, 1);
                    break;
                }
            }
            StreamClose();
            return TextValue;
        }
        //=====
        public void Set_Config(string keyword, string NValue)//Update Value
        {
            Stream("Configs.txt");
            for (Config_Index = 0; Config_Index < TextArray.Length; Config_Index++)//Get Value of the first keyword
            {
                if (TextArray[Config_Index].Contains(keyword))
                {
                    TextValue = TextArray[Config_Index];
                    string[] line = TextValue.Split(' ');
                    line[0] = line[0] + " ";
                    line[1] = NValue;
                    TextValue = null;
                    foreach (string value in line) { TextValue += value; }
                    TextArray[Config_Index] = TextValue;
                    LOG(keyword + " new value is " + line[1]);
                    break;
                }
            }
            StreamClose();
            File.WriteAllLines(Configstxt, TextArray);
            LOG("Configs Updated successfully..");
        }
    }
}
