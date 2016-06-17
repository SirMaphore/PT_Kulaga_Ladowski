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

        private string currentDesktop = "";

        private void DesktopInitialize(string name)
        {
            if (!Desktop.DesktopExists(name))
            {
                Desktop.DesktopCreate(name);

                Desktop.ProcessCreate(name, Application.ExecutablePath, "start");
            }

            Desktop.DesktopSwitch(name);
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

            currentDesktop = Desktop.DesktopName(Desktop.DesktopOpenInput());

            base.OnLoad(e);
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            DesktopInitialize("Default");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DesktopInitialize("Desktop_New");
        }
    }
}
