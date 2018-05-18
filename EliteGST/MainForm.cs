using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using EliteGST.Data;
using Elite.Utilities;
using EliteGST.Data.Repositories;
using System.IO;

namespace EliteGST
{
    public partial class MainForm : XtraForm
    {
        private PartyRepository _prepo;
        private bool isPacksRequired;
        private bool isFabricInvoiceRequired;

        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void companyInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var cf = new Forms.CompanyInfo())
            {
                if (cf.ShowDialog() == System.Windows.Forms.DialogResult.OK) UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            var company = _prepo.GetByPartyType(string.Empty, PartyType.Self, "CompanyName").FirstOrDefault();
            if (company != null)
            {
                Text = "Elite GST - Easy Solution for GST Accounting - " + company.CompanyName;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            statusTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");

            try
            {
                var config = new Dictionary<string, string>();
                Bootstrap.Init();

                // Include purchase order facility
                if (Config.config["IncludePurchaseOrder"] == "true")
                {
                    Height = 430;
                    tileItem4.Visible = true;
                    tileItem5.Visible = true;
                }
                else
                {
                    Height = 270;
                    tileControl1.Dock = DockStyle.Fill;
                    tileControl2.Visible = false;
                    tileItem4.Visible = false;
                    tileItem5.Visible = false;
                }

                // Include packs?
                if (Config.config["PacksRequired"] == "true")
                {
                    isPacksRequired = true;
                }

                // Include fabric invoices?
                if (Config.config["FabricInvoiceRequired"] == "true")
                {
                    isFabricInvoiceRequired = true;
                }

                // Initiate before loading
                _prepo = ServiceContainer.GetInstance<PartyRepository>();
                UpdateTitle();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
                Close();
            }
        }

        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            using (var pl = new Forms.ProductList())
            {
                pl.ShowDialog();
            }
        }

        private void tileItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            using (var pl = new Forms.PartyList())
            {
                pl.PartyType = PartyType.Customer;
                pl.ShowDialog();
            }
        }

        private void tileItem3_ItemClick(object sender, TileItemEventArgs e)
        {
            using (var pl = new Forms.InvoiceList())
            {
                pl.IsPacksRequired = isPacksRequired;
                pl.IsFabricInvoiceRequired = isFabricInvoiceRequired;
                pl.ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var abt = new Forms.About())
            {
                abt.ShowDialog();
            }
        }

        private static void StartProcess(string process)
        {
            try
            {
                System.Diagnostics.Process.Start(process);
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProcess("calc.exe");
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProcess("notepad.exe");
        }

        private void mSExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProcess("excel.exe");
        }

        private void mSWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProcess("winword.exe");
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var mysqldumppath = Config.config["MySqlDump Path"];
                if (!File.Exists(mysqldumppath))
                    throw new Exception("MySqlDump.exe not found. Please set it first");

                var database = Database.Name;
                var dt = DateTime.Now;
                var destdir = string.Format("{0}_{1}.sqlbak", database, dt.ToString("dd-MM-yyyy_h-mm-ss"));

                using (var savedlg = new SaveFileDialog())
                {
                    savedlg.Filter = "Database Backup Files|*.sqlbak;";
                    savedlg.FileName = destdir;
                    if (savedlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var filename = savedlg.FileName;
                        this.Cursor = Cursors.WaitCursor;
                        var arguments = string.Format("--user=\"root\" --password=\"root\" --add-drop-table --complete-insert --databases \"{0}\" --tables \"parties\" \"products\" \"invoices\" \"invoiceproducts\" \"invoicefabricproducts\" \"purchaseorders\" \"purchaseorderproducts\" \"options\" --result-file=\"{1}\"", database, filename);
                        RunProcess(mysqldumppath, arguments);
                        Helpers.ShowSuccess("Database backup created successfully");
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private static string RunProcess(string command, string arguments = "")
        {
            var proc = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string output = "";

            while (!proc.StandardOutput.EndOfStream)
            {
                output += proc.StandardOutput.ReadLine();
            }
            proc.WaitForExit();
            return output;
        }

        private void restoreDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var mysqlpath = Config.config["MySql Path"];
                if (!File.Exists(mysqlpath))
                    throw new Exception("MySql.exe not found. Please set it first");

                var database = Database.Name;

                using (var opendlg = new OpenFileDialog())
                {
                    opendlg.Filter = "Database Backup Files|*.sqlbak;";
                    opendlg.FileName = "";
                    opendlg.Multiselect = false;
                    opendlg.CheckFileExists = true;

                    if (opendlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var filename = opendlg.FileName;
                        this.Cursor = Cursors.WaitCursor;
                        var arguments = string.Format("/c {0} --user=root --password=root {1} < \"{2}\"", mysqlpath, database, filename);
                        RunProcess("cmd.exe", arguments);
                        UpdateTitle();
                        Helpers.ShowSuccess("Database restored successfully");
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void tileItem4_ItemClick(object sender, TileItemEventArgs e)
        {
            using (var pl = new Forms.PurchaseOrderList())
            {
                pl.ShowDialog();
            }
        }

        private void tileItem5_ItemClick(object sender, TileItemEventArgs e)
        {
            using (var pl = new Forms.PartyList())
            {
                pl.PartyType = PartyType.Supplier;
                pl.ShowDialog();
            }
        }

        private void defaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Forms.DefaultsForm())
            {
                form.ShowDialog();
            }
        }

        private void setMySqlPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Config.config["MySql Path"]))
            {
                if (!Helpers.Confirm("MySql path already set and verified. Do you want to set it again?")) return;
            }

            using (var opendlg = new OpenFileDialog())
            {
                opendlg.Filter = "MySql executable|mysql.exe;";
                opendlg.FileName = "";
                opendlg.CheckFileExists = true;

                if (opendlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var filename = opendlg.FileName;
                    if (filename.Contains(' '))
                    {
                        Helpers.ShowError("File path must not contain spaces");
                        return;
                    }
                    Config.config["MySql Path"] = filename;
                    Bootstrap.WriteConfig(Config.config);
                    Helpers.ShowSuccess("MySql path set successfully");
                }
            }
        }

        private void setMySqlDumpPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Config.config["MySqlDump Path"]))
            {
                if (!Helpers.Confirm("MySQLDump path already set and verified. Do you want to set it again?")) return;
            }

            using (var opendlg = new OpenFileDialog())
            {
                opendlg.Filter = "MySqlDump executable|mysqldump.exe;";
                opendlg.FileName = "";
                opendlg.CheckFileExists = true;

                if (opendlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var filename = opendlg.FileName;
                    if (filename.Contains(' '))
                    {
                        Helpers.ShowError("File path must not contain spaces");
                        return;
                    }
                    Config.config["MySqlDump Path"] = filename;
                    Bootstrap.WriteConfig(Config.config);
                    Helpers.ShowSuccess("MySqlDump path set successfully");
                }
            }
        }
    }
}
