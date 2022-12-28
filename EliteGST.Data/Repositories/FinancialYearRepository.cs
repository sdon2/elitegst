using System.Collections.Generic;
using System.Linq;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class FinancialYearRepository : BaseRepository<FinancialYear>, IFinancialYearRepository
    {
        public FinancialYearRepository()
            : base ("financialyears")
        {

        }

        public IEnumerable<FinancialYear> GetAllDesc(params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            return Connection.Query<FinancialYear>(string.Format("SELECT {0} FROM {1} ORDER BY Id DESC", cols, Table));
        }

        public int GetCount()
        {
            var sql = string.Format("SELECT COUNT(Id) FROM {0}", Table);
            return Connection.ExecuteScalar<int>(sql);
        }
    }
}
