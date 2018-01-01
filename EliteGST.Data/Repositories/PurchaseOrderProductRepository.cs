using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class PurchaseOrderProductRepository : BaseRepository<PurchaseOrderProduct>, IPurchaseOrderProductRepository
    {
        public PurchaseOrderProductRepository()
            : base ("purchaseorderproducts")
        {

        }

        public IEnumerable<PurchaseOrderProduct> GetProductsForPurchaseOrder(int purchaseOrderId)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, products.ProductDescription AS ProductDescription, products.HSN AS HSN, products.UoM AS UoM FROM {1} INNER JOIN products ON products.Id={1}.ProductId WHERE {1}.PurchaseOrderId=@id", cols, Table);
            return Connection.Query<PurchaseOrderProduct>(sql, new { id = purchaseOrderId });
        }

        public int DeleteAllPurchaseOrderProducts(int purchaseOrderId)
        {
            var sql = string.Format("DELETE FROM {0} WHERE {0}.PurchaseOrderId=@id", Table);
            return Connection.Execute(sql, new { id = purchaseOrderId });
        }
    }
}
