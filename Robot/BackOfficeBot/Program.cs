using System;
using System.Windows.Forms;

namespace BackOfficeBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "In BackOfficeBot Stage 1 - \n");

            // Use the assembly GUID as the name of the mutex which we use to detect if an application instance is already running
            log4net.Config.XmlConfigurator.Configure();
            bool createdNew = false;
            string mutexName = System.Reflection.Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName, out createdNew))
            {
                if (!createdNew)
                {
                    // Only allow one instance
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    STAApplicationContext context = new STAApplicationContext();
                    Application.Run(context);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error");
                }
            }
        }
    }
}
