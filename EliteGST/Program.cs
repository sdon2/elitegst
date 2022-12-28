using System;
using System.Windows.Forms;
using System.Threading;
using DevExpress.Skins;
using Elite.Utilities;

namespace EliteGST
{
    static class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{D18B21BC-BC48-4b3e-84CC-378239740B1F}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
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
