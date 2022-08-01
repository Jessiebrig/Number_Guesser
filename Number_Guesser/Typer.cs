using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AutoItX3Lib;

namespace Number_Guesser
{
    public partial class Auto_Guesser
    {
        public AutoItX3 Auto = new AutoItX3();
        private IntPtr thiswindow;
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string sClassname, string sAppName);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public enum FsModifiers { Alt = 0x0001, Control = 0x0002, Shift = 0x0004, Window = 0x0008, }
        protected override void WndProc(ref Message keyPressed)
        {
            if (keyPressed.Msg == 0x0312)
            {
                int id = keyPressed.WParam.ToInt32();
                switch (id)
                {
                    case 1://Auto_Type one Set Only
                        if (!autotype)
                        {
                            autotype = true;
                            Stop.BackColor = Color.Red;
                            LOG("Typing..");
                            Auto_Type();
                        }
                        else
                        {
                            autotype = false;
                            Stop.BackColor = Color.LightSkyBlue;
                            LOG("Stopped..");
                            Stop_Thread(Typer, Stop, "Stop");
                        }
                        break;
                    case 2://Start_Auto_Type Continously
                        if (!start_auto_type)
                        {
                            start_auto_type = true;
                            autotype = true;
                            Stop.BackColor = Color.Red;
                            LOG("Auto Typing..");
                            Start_Auto_Type();
                        }
                        else
                        {
                            autotype = false;
                            start_auto_type = false;
                            Stop_Thread(Typer, Stop, "Stop");
                        }
                        break;
                }
            }
            base.WndProc(ref keyPressed);
        }
        bool start_auto_type = false;
        public void Start_Auto_Type()
        {
            Get_Gens();//Store Generateds to Generated_array
            if (start_auto_type)
            {
                Typer = new Thread(() => {
                    for (int i = LastSend; i <= TextArray.Length - 1; i++)
                    {
                        SetText(GENS, TextArray[i]);
                        SetText(Final, GetText(Before) + GetText(GENS) + GetText(After));
                        CLEAR(GENS);
                        LOG("Sending: " + TextArray[i] + " from Index: " + i.ToString());
                        Type();
                        Auto.Send("{ENTER}");
                        if (i >= TextArray.Length - 1) { Stop_Thread(Typer, Stop, "Stop"); }//Stop if all is Typed
                        LastSend = i;
                        SetText(Start_INDEX, LastSend);
                    }
                });
                Typer.Start();
            }
        }
        Thread Typer;
        public void Auto_Type()
        {
            SetText(Final, GetText(Before) + GetText(GENS) + GetText(After));
            CLEAR(GENS);
            Typer = new Thread(() => { Type(); });
            Typer.Start();
        }
        bool autotype = false;
        public void Type()
        {
        again:
            if (autotype)
            {
                if (Final.Text != "")
                {
                    Sendkeys();
                    Pause();
                    goto again;
                }
                LOG("Sended: " + SENDED);
                SENDED = null;
            }
        }
        string CHARS;
        string SENDED;
        public String Words;
        public void Sendkeys()
        {
            try
            {
                char[] array = new char[1];
                Words.CopyTo(0, array, 0, 1);//copy index_0 of Words to char[] array
                CHARS = SetText(SEND, array[0].ToString().Trim());
                Auto.Send(CHARS);//Send CHARS
                SENDED += CHARS;//Append Sended to SENDED
                Remove0();
            }
            catch (ArgumentOutOfRangeException) { LOG("Index Out Of Range Exception or TextBox is Empty."); }
            catch (Exception ex) { LOGEX("Null Reference Exception", ex); }
        }
    }
}
