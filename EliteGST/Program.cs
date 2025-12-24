using System;
using System.Windows.Forms;
using System.Threading;
using DevExpress.Skins;
using Elite.Utilities;
using CommandLine;

namespace EliteGST
{
    public class Options
    {
        [Option('i', "install", Default = false, Required = false, HelpText = "Install database.")]
        public bool Install { get; set; }

        [Option('d', "database", Default = "elitegst", Required = false, HelpText = "Set database name for installing.")]
        public string Database { get; set; }

        [Option('y', "year", Default = "2025-2026", Required = false, HelpText = "Create financial year for use.")]
        public string FinancialYear { get; set; }
    }

    static class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{D18B21BC-BC48-4b3e-84CC-378239740B1F}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                using (new ConsoleScope())
                {
                    Parser.Default.ParseArguments<Options>(args)
                        .WithParsed<Options>(o =>
                        {
                            new DatabaseInstaller(o);
                        });
                }

            }
            else
            {
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Office 2016 Colorful";
                SkinManager.EnableFormSkins();

                if (!Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Helpers.ShowError("Application is already running");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Initializer.Initialize(new MainForm(), "{D54A01CA-0224-4E51-87C1-C0F35B2429E9}");

                Application.Exit();
            }
        }
    }
}
