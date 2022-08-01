using System;
using System.Linq;
using System.IO;
using System.Drawing;

namespace Number_Guesser
{
    public partial class Auto_Guesser
    {
        //Word_Generator_____
        string MATCHS;//all Matched
        string[] HINT;
        public void GenWords()
        {
            HINT = new string[Letter.Length];
            Letter_();
            for (int i = 0; i <= Letter.Length - 1; i++) {
                if (HINT[i] == "")
                {
                    BoxColor(Letter[i], Color.Red);
                }
            }
            MATCHS = null;
            Stream(TWords);//Store all Tagalog words to TextArray
            for (int line = 0; line <= TextArray.Length - 1; line++)
            {
                char[] letters = TextArray[line].ToCharArray();
                for (int Li = 0; Li <= HINT.Length - 1; Li++)
                {
                    if (HINT[Li] != "")
                    {
                        //LOG("Hint: " + HINT[Li] + " Checking at Index: " + Li + " of " + TextArray[line]);
                        if (letters[Li].ToString() != HINT[Li]) { break; }//Proceed to next TextArray if current letters[index] doesnt match
                    }
                    if (Li >= HINT.Length - 1) { MATCHS += TextArray[line] + Environment.NewLine; }
                } 
            }
            StreamClose();
            SetText(Gen_List, MATCHS);
            Write(Generatedtxt, MATCHS);
            LOG("Writing Matchs Words to Generated.text Complete..");
            Stop_Thread(Word_Gen, Gen_Words, "Generate");
        }
        //Setter_____
        string[] NUM;
        char[] Chars;
        public string Setter(string newchar)
        {
            Newchar = null;
            //LOG("Generated: " + newchar);
            NUM = new string[Digits];
            Chars = newchar.ToCharArray();
            for (int i = 0; i <= Digits - 1; i++)
            {
                if (Number[i].Text == "")                   //Store Generated Chars
                {
                    NUM[i] = Chars[0].ToString();
                    Newchar = new string(Chars);
                    Newchar = Newchar.Remove(0, 1);
                    Chars = Newchar.ToCharArray();
                }
                else { NUM[i] = Number[i].Text; }           //Store Hint Chars
            }
            foreach (string chars in NUM) { Newchar += chars; }
            LOG("NewChars: " + Newchar);
            return Newchar;
        }
        //Number_Generator_____
        int Digits;
        int Hints;
        int Counter;
        public string CharSet;
        public string Newchar;
        public void Genenerate_Num(int length)
        {
            Digits = ParseTB(digits);
            if (Digits <= 5)
            {
                Hints = 0;
                Number_();
                for (int i = 0; i < Digits; i++) if (Number[i].Text != "") { Hints++; }
                LOG("Hints: " + Hints.ToString());
            again:
                Counter += 1;
                Newchar = Radomize(Digits - Hints);
                CharSet += Setter(Newchar) + Environment.NewLine;
                if (Counter >= length)
                {
                    CharSet = CharSet.Remove(CharSet.Length - 1, 1);
                    Write(Generatedtxt, CharSet);
                }
                else { goto again; }
                Counter = 0;
                CharSet = null;
            }
            else { LOG("Input must be > 0 and <= 5"); }
            Stop_Thread(Char_Gen, Gen_Num, "Generate");
        }
        //Randomizer
        private static Random random = new Random();
        public static string Radomize(int length)
        {
            //const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
