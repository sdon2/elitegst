using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IInvoiceProductRepository : IRepository<InvoiceProduct>
    {
        IEnumerable<InvoiceProduct> GetProductsForInvoice(int invoiceId);
        int DeleteAllInvoiceProducts(int invoiceId);
    }
}
