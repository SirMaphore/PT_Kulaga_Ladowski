using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Wirtualny_Pulpit
{
    ////////////////////////////////
    /////////// PROGRAM ////////////
    ////////////////////////////////
    static class Program
    {
        public static string Start = string.Empty;

        [STAThread]
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                Start = arg.Trim();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}