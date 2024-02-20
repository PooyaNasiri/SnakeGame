using System;
using System.Windows.Forms;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Data.Common;

namespace Snake
{


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Screen.PrimaryScreen.Bounds.Width <= 800 || Screen.PrimaryScreen.Bounds.Height <= 600)
                Application.Run(new Error());
            else
                Application.Run(new Form1());
        }
    }
    
}




