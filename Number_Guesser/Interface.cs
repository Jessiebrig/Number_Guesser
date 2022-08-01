using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Number_Guesser
{
    public partial class Auto_Guesser
    {
        public int Min, Max;
        public void SetMinMax(int min, int max) { SetText(Minimum, min); SetText(Maximum, max); }
        public void LOG(string log) { this.Invoke(new Action(() => { textScripts.Text += (Time() + " " + log); textScripts.AppendText(Environment.NewLine); })); }
        public void LOGEX(string log, Exception ex) { LOG(log + Environment.NewLine + ex.ToString()); }//Log with Exception Error
        public string Time() { string DOR = DateTime.Now.ToString(); string[] DT = DOR.Split(' '); DOR = DT[1]; return DOR; }
        //=====
        public void Stop_Thread(Thread thread)
        {
            this.Invoke(new Action(() =>{
                start = false;
                this.BackColor = Color.LightSkyBlue;
                try { thread.Abort(); LOG("Generator Stopped"); }
                catch (Exception ex) { LOGEX("Exception at Num_Gen_Stop_()", ex); }
            }));}
        public void Stop_Thread(Thread thread, Button button, string Btext)
        { 
            this.Invoke(new Action(() => {
                button.Text = Btext;
                button.BackColor = Color.DeepSkyBlue;
                Stop_Thread(thread);
            }));}
        //=====
        public void Stop_()
        {
            this.Invoke(new Action(() => {
                autotype = false;
                start_auto_type = false;
                Stop.BackColor = Color.White;
                LOG("Auto Typing Done..");
            }));}
        //=====
        public void Number_()
        {
            this.Invoke(new Action(() => {
                DefaultBoxColor(Number);
                for (int i = Digits; i <= Number.Length - 1; i++) { BoxColor(Number[i], Color.Red); }//change backcolor to RED if null
            }));}
        //=====
        public void Letter_()
        {
            this.Invoke(new Action(() => {
                H = null;
                DefaultBoxColor(Letter);
                for (int i = 0; i <= Letter.Length - 1; i++)
                {
                    HINT[i] = Letter[i].Text;
                    if (HINT[i] != "") { H += HINT[i]; }
                    else { H += "+"; }
                }
                LOG("Hints: " + H);
            }));}
        //=====
        public void Remove0()//Remove 1st letter of the Final.TextBox
        {
            this.Invoke(new Action(() => { Final.Text = Final.Text.Remove(0, 1); }));
        }
        //=====
        public void Pause()
        {
            Random time = new Random();
            int num = time.Next(Min, Max);
            Thread.Sleep(num);
        }
        //=====
        string text;
        int number_;
        public int ParseTB(TextBox textBox)//Convert TextBox.Text to int and return
        {
            this.Invoke(new Action(() =>
            {
                text = textBox.Text;
                Parse(text);
            }));
            return number_;
        }
        public int Parse(string string_)//Convert string to int and return
        {
            try
            {
                LOG("Parsing string: " + string_);
                string[] num = new string[] { string_ };
                text = num.Last();
                text = text.Trim();
                number_ = int.Parse(text);
            }
            catch (FormatException) { LOG("Error while while parsing string: " + text); }
            catch (Exception ex) { LOGEX("Exception at Parse", ex); }
            return number_;
        }
        public string GetText(TextBox textbox)//Store TextBox.Text to string and return
        {
            this.Invoke(new Action(() => { text = textbox.Text; }));
            return text;
        }
        public int SetText(TextBox textbox, int _int)//Send int to TextBox and return the new TextBox.Text
        {
            this.Invoke(new Action(() => { textbox.Text = _int.ToString(); }));
            return _int;
        }
        public string SetText(TextBox textbox, string _string)//Send string to TextBox and return the new TextBox.Text
        {
            this.Invoke(new Action(() => { textbox.Text = _string; }));
            return _string;
        }
        //=====
        public void CLEAR_S() { CLEAR(textScripts); CLEAR(Before); CLEAR(After); }
        public void CLEAR(TextBox textbox) { this.Invoke(new Action(() => { textbox.Clear(); })); }
        //=====
        public void BoxColor(TextBox textbox, Color color) { this.Invoke(new Action(() => { textbox.BackColor = color; })); }
        public void DefaultBoxColor(TextBox[] textbox)
        {
            foreach (TextBox box in textbox)
            { BoxColor(box, Color.DarkTurquoise); }
        }
    }
}
