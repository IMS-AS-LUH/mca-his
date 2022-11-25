using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace HisDemo.Aufnahmestation
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] cliArgs)
        {
            if (cliArgs.Length > 0)
            {
                ExitWithHelp();
                return;
            }

            // Prevent Multi-Start
            // Derived from: https://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
            string mutexId = "Global\\{6ab27f3b-1876-457d-bca9-c551d5bbc696}";
            bool mutexCreatedNew;
            var mutex = new System.Threading.Mutex(false, mutexId, out mutexCreatedNew);
            bool mutexAquired = false;
            try
            {
                mutexAquired = mutex.WaitOne(100, false);
            } catch (System.Threading.AbandonedMutexException)
            {
                mutexAquired = true;
            }

            if (!mutexAquired)
            {
                MessageBox.Show("Die Aufnahmestation wird auf diesem Rechner bereits ausgeführt!", "HIS-Demo Startfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#if DEBUG
            // Put alternate path here if you want to separate
            StoragePath = @"C:\his-demo-data";
            SecondaryStoragePath = @"C:\his-demo-data-secondary";
#else
            // Put the correct paths here, i.e. mounted secure network share
            StoragePath = @"C:\his-demo-data";
            SecondaryStoragePath = @"C:\his-demo-data-secondary";
#endif

            if (!System.IO.Directory.Exists(StoragePath))
            {
                MessageBox.Show($"Datenspeicher nicht verbunden. BITTE IT KONTAKTIEREN!\n\n SW:\n{System.Reflection.Assembly.GetExecutingAssembly().FullName}", "HIS-Demo Startfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto ExitAndCleanup;
            }

            try
            {
                Logger = new FileLogger(StoragePath, "Aufnahmestation");
            } catch (Exception x)
            {
                MessageBox.Show($"Fehler bei Log-Erstellung. BITTE IT KONTAKTIEREN!\n\n{x.Message}\n\n SW:\n{System.Reflection.Assembly.GetExecutingAssembly().FullName}", "HIS-Demo Startfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto ExitAndCleanup;
            }

            if (!System.IO.Directory.Exists(SecondaryStoragePath))
            {
                Logger.Warning("SecondaryStoragePath does not Exist");
                SecondaryStoragePath = null;
            }
            try
            {
                Logger.Diagnostic("Reading SambaConfigFile ...");
                Config = new SambaFileConfig(System.IO.Path.Combine(StoragePath, @"system\mca_config.ini"), Logger);
            } catch (Exception x)
            {
                try
                {
                    Logger.Fatal($"Top-Level Exception (reading SambaFileConfig): {x}");
                }
                catch { }
                MessageBox.Show($"Konfigurationsdatei nicht lesbar. BITTE IT KONTAKTIEREN!\n\n{x.GetType()}: {x.Message}\n\n SW:\n{System.Reflection.Assembly.GetExecutingAssembly().FullName}", "HIS-Demo Softwarefehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto ExitAndCleanup;
            }

            
            try
            {
                Logger.Diagnostic("Starting UI ...");
                Application.ThreadException += ApplicationOnThreadException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Aufnahme aufnahme = new Aufnahme();
                Application.Run(aufnahme);

                Logger.Information("Application terminated.");
            } catch (Exception x)
            {
                try
                {
                    Logger.Fatal($"Top-Level Exception (Program try/catch): {x}");
                }
                catch { }
                MessageBox.Show($"Unbekannter Anwendungsfehler (Top T/C). BITTE IT KONTAKTIEREN!\n\n{x.GetType()}: {x.Message}\n\n SW:\n{System.Reflection.Assembly.GetExecutingAssembly().FullName}", "HIS-Demo Softwarefehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ExitAndCleanup:
            if (mutexAquired)
                mutex.ReleaseMutex();
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                Logger.Fatal($"Top-Level Exception (ApplicationOnThreadException): {e.Exception}");
            }
            catch { }
            MessageBox.Show($"Unbekannter Anwendungsfehler (App TE). BITTE IT KONTAKTIEREN!\n\n{e.Exception.GetType()}: {e.Exception.Message}\n\n SW:\n{System.Reflection.Assembly.GetExecutingAssembly().FullName}", "HIS-Demo Softwarefehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }


        public static Logger Logger;
        public static string StoragePath;
        public static string SecondaryStoragePath;
        public static SambaFileConfig Config;

        private static void ExitWithHelp()
        {
            string info = "Start mit unbekannten Parametern / Debugaufruf\r\n";
            info += $"Machine: {Environment.MachineName}, User: {Environment.UserDomainName}/{Environment.UserName}\n";
            info += $"SW: {System.Reflection.Assembly.GetExecutingAssembly().FullName}, CLR: {Environment.Version}\n";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                try
                {
                    //if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                info += $"Interface \"{item.Name}\":\n  IP {ip.Address} [{item.GetPhysicalAddress()}] {item.NetworkInterfaceType}\n";
                            }
                        }
                    }
                } catch { }
            }

            MessageBox.Show(info, "HIS-Demo : CLI-Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
