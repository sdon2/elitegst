using System.Collections.Generic;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class InvoiceFabricProductRepository : BaseRepository<InvoiceFabricProduct>, IInvoiceFabricProductRepository
    {
        public InvoiceFabricProductRepository()
            : base ("invoicefabricproducts")
        {

        }

        public IEnumerable<InvoiceFabricProduct> GetProductsForInvoice(int invoiceId)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, products.ProductDescription AS ProductDescription, products.HSN AS HSN, products.UoM AS UoM FROM {1} INNER JOIN products ON products.Id={1}.ProductId WHERE {1}.InvoiceId=@id", cols, Table);
            return Connection.Query<InvoiceFabricProduct>(sql, new { id = invoiceId });
        }

        public int DeleteAllInvoiceProducts(int invoiceId)
        {
            var sql = string.Format("DELETE FROM {0} WHERE {0}.InvoiceId=@id", Table);
            return Connection.Execute(sql);
        }
    }
}
