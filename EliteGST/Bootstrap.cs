using System.Collections.Generic;
using Elite.Utilities;
using EliteGST.Data;
using System.Windows.Forms;
using EliteGST.Data.Repositories;

namespace EliteGST
{
    internal static class Bootstrap
    {
        public static void Init()
        {
            var config = ReadConfig();
            
            Config.config = config;
            
            Database.SetCredentials(config["host"].ToString(), config["database"].ToString(), config["user"].ToString(), config["password"].ToString());

            InitDatabase();

            ServiceContainer.Register<PartyRepository>(() => new PartyRepository(), true);
            ServiceContainer.Register<ProductRepository>(() => new ProductRepository(), true);
            ServiceContainer.Register<InvoiceRepository>(() => new InvoiceRepository(), true);
            ServiceContainer.Register<PaymentRepository>(() => new PaymentRepository(), true);
            ServiceContainer.Register<InvoiceProductRepository>(() => new InvoiceProductRepository(), true);
            ServiceContainer.Register<InvoiceFabricProductRepository>(() => new InvoiceFabricProductRepository(), true);
            ServiceContainer.Register<PurchaseOrderRepository>(() => new PurchaseOrderRepository(), true);
            ServiceContainer.Register<PurchaseOrderProductRepository>(() => new PurchaseOrderProductRepository(), true);
            ServiceContainer.Register<OptionRepository>(() => new OptionRepository(), true);
            ServiceContainer.Register<FinancialYearRepository>(() => new FinancialYearRepository(), true);
        }

        private static void InitDatabase()
        {
            if (!Database.CreateDatabaseIfNotExists())
            {
                Utils.ShowError("Unable to create Database.\n Try creating yourself!");
                Application.Exit();
                return;
            }

            if (ConfigManager.GetValue("database_imported", "false") == "false")
            {
                if (!Database.ImportMySQL())
                {
                    Utils.ShowError("Unable to import Database.\n Try importing yourself!");
                    Application.Exit();
                    return;
                }

                ConfigManager.SaveValue("database_imported", "true");
            }
        }

        public static Dictionary<string, object> ReadConfig()
        {
            var result =  new Dictionary<string, object>();
            result.Add("host", Config.GetStringValue("host", string.Empty));
            result.Add("database", Config.GetStringValue("database", string.Empty));
            result.Add("user", Config.GetStringValue("user", string.Empty));
            result.Add("password", Config.GetStringValue("password", string.Empty));
            result.Add("include_purchase_order", Config.GetBoolValue("include_purchase_order", false));
            result.Add("packs_required", Config.GetBoolValue("packs_required", false));
            result.Add("fabric_invoice_required", Config.GetBoolValue("fabric_invoice_required", false));
            result.Add("mysql_dump_path", Config.GetStringValue("mysql_dump_path", string.Empty));
            result.Add("mysql_path", Config.GetStringValue("mysql_path", string.Empty));
            result.Add("invoice_a4", Config.GetStringValue("invoice_a4", string.Empty));
            result.Add("a5_invoice", Config.GetBoolValue("a5_invoice", false));
            result.Add("invoice_a5", Config.GetStringValue("invoice_a5", string.Empty));
            result.Add("invoice_pack", Config.GetStringValue("invoice_pack", string.Empty));
            result.Add("fabric_invoice", Config.GetStringValue("fabric_invoice", string.Empty));
            result.Add("purchase_order", Config.GetStringValue("purchase_order", string.Empty));
            return result;
        }
    }
}
