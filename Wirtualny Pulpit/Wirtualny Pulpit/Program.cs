using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Wirtualny_Pulpit
{
    static class Program
    {
        // If explorer.exe should start on new desktop
        public static string Start = string.Empty;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Get argument "start", check if should start explorer.exe on new desktop
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