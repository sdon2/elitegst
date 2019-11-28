using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        IEnumerable<PurchaseOrder> GetByPartyName(string name);
        IEnumerable<PurchaseOrder> GetByPartyName(string name, int limit, int offset, int finyear);
        PurchaseOrder GetTotals(int purchaseOrderId);
        int GetCount();
    }
}
