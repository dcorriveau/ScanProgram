/*!*************************************************************
 *!                        Program.cs                          *
 *!                     Dylan Corriveau                        *
 *!                     March 12, 2012                         *
 *! This program acts as a startup location for the scanning   *
 *!                         program.                           *
 *!*************************************************************/
//!these using statements are used to include the necessary features needed to run
using System;
using System.Windows.Forms;
//! \namespace ScanProgram
//! This namespace is used to link together all of the implementations and functionality found inside of the Scanning Program
namespace ScanProgram
{
    //! \class Program
    //! This class is used to act as a starting point for the scanning program
    internal static class Program
    {
        [STAThread]
        //! This function is used to start enable all of the visuals, set all of the text rendering and run the program
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmScanProgram());
        }
    }
}