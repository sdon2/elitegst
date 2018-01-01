using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (config["Database"] == null) config["Database"] = "elitegst";
            if (config["IncludePurchaseOrder"] == null) config["IncludePurchaseOrder"] = "false";
            if (config["PacksRequired"] == null) config["PacksRequired"] = "false";
            if (config["FabricInvoiceRequired"] == null) config["FabricInvoiceRequired"] = "false";
            if (config["MySqlDump Path"] == null) config["MySqlDump Path"] = "C:\\MySQL\\bin\\mysqldump.exe";
            if (config["MySql Path"] == null) config["MySql Path"] = "C:\\MySQL\\bin\\mysql.exe";
            Config.config = config;
            WriteConfig(config);
            Database.Name = config["Database"];
            ServiceContainer.Register<PartyRepository>(() => new PartyRepository(), true);
            ServiceContainer.Register<ProductRepository>(() => new ProductRepository(), true);
            ServiceContainer.Register<InvoiceRepository>(() => new InvoiceRepository(), true);
            ServiceContainer.Register<InvoiceProductRepository>(() => new InvoiceProductRepository(), true);
            ServiceContainer.Register<InvoiceFabricProductRepository>(() => new InvoiceFabricProductRepository(), true);
            ServiceContainer.Register<PurchaseOrderRepository>(() => new PurchaseOrderRepository(), true);
            ServiceContainer.Register<PurchaseOrderProductRepository>(() => new PurchaseOrderProductRepository(), true);
            ServiceContainer.Register<OptionRepository>(() => new OptionRepository(), true);
        }

        public static Dictionary<string, string> ReadConfig()
        {
            var result =  new Dictionary<string, string>();
            result.Add("Database", ConfigManager.ReadConfigFromFile(configFile, "Options", "Database"));
            result.Add("IncludePurchaseOrder", ConfigManager.ReadConfigFromFile(configFile, "Options", "IncludePurchaseOrder"));
            result.Add("PacksRequired", ConfigManager.ReadConfigFromFile(configFile, "Options", "PacksRequired"));
            result.Add("FabricInvoiceRequired", ConfigManager.ReadConfigFromFile(configFile, "Options", "FabricInvoiceRequired"));
            result.Add("MySqlDump Path", ConfigManager.ReadConfigFromFile(configFile, "Options", "MySqlDump Path"));
            result.Add("MySql Path", ConfigManager.ReadConfigFromFile(configFile, "Options", "MySql Path"));
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
