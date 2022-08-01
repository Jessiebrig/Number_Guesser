using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Number_Guesser
{
    public partial class Auto_Guesser : Form
    {
        public static TextBox[] Number;
        public static TextBox[] Letter;
        public delegate void DELETEGATE();
        //for (int ctr = 0; ctr<chars.Length; ctr++){Console.WriteLine("   {0}: {1}", ctr, chars[ctr]);}
        //================================================================
        public Auto_Guesser()
        {
            InitializeComponent();
            Number = new TextBox[] { Digit0, Digit1, Digit2, Digit3, Digit4};
            Letter = new TextBox[] { L0, L1, L2, L3, L4, L5, L6, L7, L8};
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            thiswindow = FindWindow(null, "Auto_Guesser");
            RegisterHotKey(thiswindow, 1, (uint)FsModifiers.Control, (uint)Keys.Space);//Auto_Type
            RegisterHotKey(thiswindow, 2, (uint)FsModifiers.Control, (uint)Keys.Enter);//Start_Auto_Type
            LastSend = Get_Int("Last_Send:");
            SetText(Start_INDEX, LastSend.ToString());
            SetMinMax(60, 100);
        }
        //=====================================================================
        //Generate_____
        int Sets;
        bool start = false;
        Thread Char_Gen;
        private void Gen_Num_Click(object sender, EventArgs e)
    {
            if (start) { Stop_Thread(Char_Gen, Gen_Num, "Generate"); }
            else  //Generate
            {
                LastSend = 0;
                string num = LastSend.ToString();
                Set_Config("Last_Send:", num);
                SetText(Start_INDEX, num);
                Sets = ParseTB(Set);
                Gen_Num.Text = "Stop";
                start = true;
                Gen_Num.BackColor = Color.Red;
                this.BackColor = Color.LightGreen;
                Char_Gen = new Thread(() => {
                    Genenerate_Num(Sets);
                });
                Char_Gen.Start();
            }
        }
        Thread Word_Gen;
        string H;//Store Hints from TextBox if Letter_() is called
        private void Gen_Words_Click(object sender, EventArgs e)
        {
            if (start) { Stop_Thread(Word_Gen, Gen_Words, "Generate"); }
            else  //Generate
            {
                Gen_Words.Text = "Stop";
                start = true;
                Gen_Words.BackColor = Color.Red;
                this.BackColor = Color.LightGreen;
                Word_Gen = new Thread(() => {
                    GenWords();
                });
                Word_Gen.Start();
            }
        }
        private void Clear_Click(object sender, EventArgs e) { CLEAR_S(); }
        private void Auto_Guesser_FormClosed(object sender, FormClosedEventArgs e)
        { UnregisterHotKey(thiswindow, 1); Set_Config("Last_Send:", LastSend.ToString()); }
        private void Final_TextChanged(object sender, EventArgs e) { Words = GetText(Final); }
        private void Stop_Click(object sender, EventArgs e)
        {
            if (autotype)
            {
                autotype = false;
                start_auto_type = false;
                Stop_Thread(Typer, Stop, "Stop");
            }
        }
        private void TEST_Click(object sender, EventArgs e) { LOG("For Debugging Only.."); }
        int LastSend;
        private void Start_INDEX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    LastSend = ParseTB(Start_INDEX);
                    string number = LastSend.ToString();
                    LOG("First Index Set to: " + number);
                    SetText(Start_INDEX, number);
                }
                catch (Exception ex) { LOGEX("Exception at Start_INDEX_KeyDown()", ex); }
            }
        }
        private void Minimum_TextChanged(object sender, EventArgs e) { Min = ParseTB(Minimum); }
        private void Maximum_TextChanged(object sender, EventArgs e) { Max = ParseTB(Maximum); }
    }
}
