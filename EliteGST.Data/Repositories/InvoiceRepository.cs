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

        public IEnumerable<int> GetYears()
        {
            var sql = string.Format("SELECT DISTINCT YEAR(InvoiceDate) FROM {0}", Table);
            return Connection.Query<int>(sql);
        }

        public IEnumerable<Invoice> GetByPartyName(string name, int limit, int offset, int finyear)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, parties.CompanyName AS Customer, parties.GSTIN AS GSTIN FROM {1} INNER JOIN parties ON parties.Id={1}.BillingId", cols, Table);
            sql += " WHERE parties.CompanyName LIKE CONCAT('%', @name, '%')";
            if (finyear != 0) sql += string.Format(" AND {0}.FinancialYearId = {1}", Table, finyear);
            sql += string.Format(" ORDER BY CAST({0}.InvoiceStringId AS UNSIGNED) DESC LIMIT {1} OFFSET {2}", Table, limit, offset);
            return Connection.Query<Invoice>(sql, new { name = name });
        }

        public decimal GetPreviousByPartyId(int id, DateTime fromDate)
        {
            var sql = string.Format("SELECT * FROM customerreport WHERE PartyId=@id");
            var fromDateString = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
            sql += string.Format(" AND InvoiceDate < '{0}'", fromDateString);
            var data = Connection.Query<dynamic>(sql, new { id = id }).ToList();

            var result = 0.00M;
            data.ForEach(e =>
            {
                result += (e.Subtotal + e.CGST + e.SGST + e.IGST) - e.Discount;
            });
            return result;
        }

        public IEnumerable<dynamic> GetByPartyId(int id, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var sql = string.Format("SELECT * FROM customerreport WHERE PartyId=@id");
            
            if (fromDate != null)
            {
                var _fromDate = (DateTime)fromDate;
                var fromDateString = _fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                sql += string.Format(" AND InvoiceDate >= '{0}'", fromDateString);
            }

            if (toDate != null)
            {
                var _toDate = (DateTime)toDate;
                var toDateString = _toDate.ToString("yyyy-MM-dd HH:mm:ss");
                sql += string.Format(" AND InvoiceDate <= '{0}'", toDateString);
            }

            return Connection.Query<dynamic>(sql, new { id = id });
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
