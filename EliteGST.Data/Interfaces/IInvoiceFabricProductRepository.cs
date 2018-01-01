using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IInvoiceFabricProductRepository : IRepository<InvoiceFabricProduct>
    {
        IEnumerable<InvoiceFabricProduct> GetProductsForInvoice(int invoiceId);
        int DeleteAllInvoiceProducts(int invoiceId);
    }
}
