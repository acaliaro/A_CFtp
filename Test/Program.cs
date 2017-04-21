using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Test
{
    static class Program
    {
        /// <summary>
        /// Il punto di ingresso principale dell'applicazione.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new FormTestFtp());
        }
    }
}