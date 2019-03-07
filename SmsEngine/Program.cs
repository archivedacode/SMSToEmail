using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SmsEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main(string[] args)
        {
            Application.Run(new Engine(args));
        }
    }
}