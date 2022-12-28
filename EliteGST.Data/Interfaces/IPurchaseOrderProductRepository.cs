using System.Collections.Generic;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IPurchaseOrderProductRepository : IRepository<PurchaseOrderProduct>
    {
        IEnumerable<PurchaseOrderProduct> GetProductsForPurchaseOrder(int purchaseOrderId);
        int DeleteAllPurchaseOrderProducts(int purchaseOrderId);
    }
}
