using System.Collections.Generic;
using Elite.Utilities;
using EliteGST.Data;
using System.Windows.Forms;
using EliteGST.Data.Repositories;

namespace EliteGST
{
    internal static class Bootstrap
    {
        private static string configFile = Application.StartupPath + "\\Config.xml";

        public static void Init()
        {
            var config = ReadConfig();
            if (config["Host"] == null) config["Host"] = "localhost";
            if (config["Database"] == null) config["Database"] = "elitegst";
            if (config["IncludePurchaseOrder"] == null) config["IncludePurchaseOrder"] = "false";
            if (config["PacksRequired"] == null) config["PacksRequired"] = "false";
            if (config["FabricInvoiceRequired"] == null) config["FabricInvoiceRequired"] = "false";
            if (config["MySqlDump Path"] == null) config["MySqlDump Path"] = "C:\\MySQL\\bin\\mysqldump.exe";
            if (config["MySql Path"] == null) config["MySql Path"] = "C:\\MySQL\\bin\\mysql.exe";
            if (config["Invoice Report"] == null) config["Invoice Report"] = "invoice.htm";
            if (config["Invoice-Pack Report"] == null) config["Invoice-Pack Report"] = "invoice-pack.htm";
            if (config["Fabric Invoice Report"] == null) config["Fabric Invoice Report"] = "fabric-invoice.htm";
            if (config["Purchase Order Report"] == null) config["Purchase Order Report"] = "purchase-order.htm";
            
            Config.config = config;
            WriteConfig(config);
            
            Database.SetHost(config["Host"]);
            Database.SetDatabase(config["Database"]);

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

        public static Dictionary<string, string> ReadConfig()
        {
            var result =  new Dictionary<string, string>();
            result.Add("Host", ConfigManager.ReadConfigFromFile(configFile, "Options", "Host"));
            result.Add("Database", ConfigManager.ReadConfigFromFile(configFile, "Options", "Database"));
            result.Add("IncludePurchaseOrder", ConfigManager.ReadConfigFromFile(configFile, "Options", "IncludePurchaseOrder"));
            result.Add("PacksRequired", ConfigManager.ReadConfigFromFile(configFile, "Options", "PacksRequired"));
            result.Add("FabricInvoiceRequired", ConfigManager.ReadConfigFromFile(configFile, "Options", "FabricInvoiceRequired"));
            result.Add("MySqlDump Path", ConfigManager.ReadConfigFromFile(configFile, "Options", "MySqlDump Path"));
            result.Add("MySql Path", ConfigManager.ReadConfigFromFile(configFile, "Options", "MySql Path"));
            result.Add("Invoice Report", ConfigManager.ReadConfigFromFile(configFile, "Options", "Invoice Report"));
            result.Add("Invoice-Pack Report", ConfigManager.ReadConfigFromFile(configFile, "Options", "Invoice-Pack Report"));
            result.Add("Fabric Invoice Report", ConfigManager.ReadConfigFromFile(configFile, "Options", "Fabric Invoice Report"));
            result.Add("Purchase Order Report", ConfigManager.ReadConfigFromFile(configFile, "Options", "Purchase Order Report"));
            return result;
        }

        public static void WriteConfig(Dictionary<string, string> config)
        {

            foreach (var conf in config)
            {
                ConfigManager.WriteConfigToFile(configFile, "Options", conf.Key, conf.Value);
            }
        }
    }
}
