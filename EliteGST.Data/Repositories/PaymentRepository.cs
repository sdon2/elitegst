using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository()
            : base ("payments")
        {

        }

        public new virtual Payment GetById(int id, params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            var sql = string.Format("SELECT {0} FROM paymentsext WHERE Id=@id", cols);
            return Connection.QueryFirstOrDefault<Payment>(sql, new { id = id });
        }

        public IEnumerable<Payment> GetByPartyName(string name, params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            var sql = string.Format("SELECT {0} FROM paymentsext WHERE Customer LIKE CONCAT('%', @name, '%')", cols);
            return Connection.Query<Payment>(sql, new { name = name });
        }

        public IEnumerable<Payment> GetByPartyId(int id, params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            var sql = string.Format("SELECT {0} FROM {1} WHERE CustomerId = @id", cols, Table);
            return Connection.Query<Payment>(sql, new { id = id });
        }
    }
}
