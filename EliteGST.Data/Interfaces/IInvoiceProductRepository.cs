using System.Collections.Generic;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IInvoiceProductRepository : IRepository<InvoiceProduct>
    {
        IEnumerable<InvoiceProduct> GetProductsForInvoice(int invoiceId);
        int DeleteAllInvoiceProducts(int invoiceId);
    }
}
