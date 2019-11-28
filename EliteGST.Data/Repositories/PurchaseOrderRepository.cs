using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class PurchaseOrderRepository : BaseRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository()
            : base ("purchaseorders")
        {

        }

        public IEnumerable<PurchaseOrder> GetByPartyName(string name)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, parties.CompanyName AS Supplier, parties.GSTIN AS GSTIN FROM {1} INNER JOIN parties ON parties.Id={1}.BillingId WHERE parties.CompanyName LIKE CONCAT('%', @name, '%') ORDER BY CAST({1}.PurchaseOrderStringId AS UNSIGNED) DESC", cols, Table);
            return Connection.Query<PurchaseOrder>(sql, new { name = name });
        }

        public IEnumerable<PurchaseOrder> GetByPartyName(string name, int limit, int offset, int finyear)
        {
            var cols = Table + ".*";
            var sql = string.Format("SELECT {0}, parties.CompanyName AS Supplier, parties.GSTIN AS GSTIN FROM {1} INNER JOIN parties ON parties.Id={1}.BillingId WHERE parties.CompanyName LIKE CONCAT('%', @name, '%') AND {1}.FinancialYearId = {4} ORDER BY CAST({1}.PurchaseOrderStringId AS UNSIGNED) DESC LIMIT {2} OFFSET {3}", cols, Table, limit, offset, finyear);
            return Connection.Query<PurchaseOrder>(sql, new { name = name });
        }

        public PurchaseOrder GetTotals(int purchaseOrderId)
        {
            return Connection.QueryFirstOrDefault<PurchaseOrder>("SELECT * FROM purchaseordertotal WHERE Id=@id", new { id = purchaseOrderId });
        }

        public int GetCount()
        {
            var sql = string.Format("SELECT COUNT(Id) FROM {0}", Table);
            return Connection.ExecuteScalar<int>(sql);
        }
    }
}
