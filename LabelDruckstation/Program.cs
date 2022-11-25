using System;
using System.Windows.Forms;
using HisDemo;
using System.IO;
using System.Reflection;

namespace LabelDruckstation
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {   
            try
            {
                string assyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Logger = new FileLogger(assyDir, "LabelDruckstation");
                LogsPath = assyDir;
            } catch
            {
                try
                {
                    string userDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
                    string logDir = Path.Combine(userDir, "MCA", "Logs");
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);
                    Logger = new FileLogger(logDir, "LabelDruckstation");
                    LogsPath = logDir;
                } catch
                {
                    MessageBox.Show("Hinweis: Log-Datei konnte nicht erstellt werden!", "LabelDruckstation");
                }
            }

            Logger.LevelFilter = Logger.Level.All;

            Logger.Diagnostic("Starting UI ...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainFrame mainFrame = new MainFrame();
            Application.Run(mainFrame);

            Logger.Information("Application terminated.");

        }

        public static Logger Logger;
        public static string LogsPath;
    }
}
