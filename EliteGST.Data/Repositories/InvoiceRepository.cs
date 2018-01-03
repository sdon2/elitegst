using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository()
            : base ("invoices")
        {

        }

        public IEnumerable<Invoice> GetByPartyName(string name)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, parties.CompanyName AS Customer, parties.GSTIN AS GSTIN FROM {1} INNER JOIN parties ON parties.Id={1}.BillingId WHERE parties.CompanyName LIKE CONCAT('%', @name, '%') ORDER BY CAST({1}.InvoiceStringId AS UNSIGNED) DESC", cols, Table);
            return Connection.Query<Invoice>(sql, new { name = name });
        }

        public IEnumerable<Invoice> GetByPartyName(string name, int limit, int offset)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, parties.CompanyName AS Customer, parties.GSTIN AS GSTIN FROM {1} INNER JOIN parties ON parties.Id={1}.BillingId WHERE parties.CompanyName LIKE CONCAT('%', @name, '%') ORDER BY CAST({1}.InvoiceStringId AS UNSIGNED) DESC LIMIT {2} OFFSET {3}", cols, Table, limit, offset);
            return Connection.Query<Invoice>(sql, new { name = name });
        }

        public Invoice GetTotalsByInvoiceType(int invoiceId, InvoiceType type)
        {
            var table = (type == InvoiceType.Normal) ? "invoicetotal" : "fabricinvoicetotal";
            var sql = string.Format("SELECT * FROM {0} WHERE Id=@id", table);
            return Connection.QueryFirstOrDefault<Invoice>(sql, new { id = invoiceId });
        }

        public int GetCount()
        {
            var sql = string.Format("SELECT COUNT(Id) FROM {0}", Table);
            return Connection.ExecuteScalar<int>(sql);
        }
    }
}
