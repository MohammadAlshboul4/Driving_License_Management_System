using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Users;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true)
            {
                using (frmLoginScreen login = new frmLoginScreen())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        using (frmMainScreen mainScreen = new frmMainScreen())
                        {
                            mainScreen.ShowDialog();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
