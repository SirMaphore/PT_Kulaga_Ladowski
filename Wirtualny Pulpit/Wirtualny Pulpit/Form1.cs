using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using System.Drawing.Imaging;
using System.IO;

namespace Wirtualny_Pulpit
{
    ////////////////////////////////
    ///////////  FORM1  ////////////
    ////////////////////////////////
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Program.Start == "start")
            {
                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
                Thread.Sleep(500);

                this.Opacity = 0;
                this.WindowState = FormWindowState.Normal;
                Thread.Sleep(500);
                this.Opacity = 100;
            }

            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nazwa = "Default";

            if (!Desktop.CzyIstnieje(nazwa))
            {
                Desktop.Stworz(nazwa);

                Desktop.StworzProces(nazwa, Application.ExecutablePath, "start");
            }

            Desktop.Przelacz(nazwa);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nazwa = "Nowy_Pulpit_Drugi";
            
            if (!Desktop.CzyIstnieje(nazwa))
            {
                Desktop.Stworz(nazwa);

                Desktop.StworzProces(nazwa, Application.ExecutablePath, "start");
            }

            Desktop.Przelacz(nazwa);
        }
    }
}