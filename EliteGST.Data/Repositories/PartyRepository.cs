using System.Collections.Generic;
using System.Linq;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

namespace EliteGST.Data.Repositories
{
    public class PartyRepository : BaseRepository<Party>, IPartyRepository
    {
        public PartyRepository()
            : base ("parties")
        {

        }

        public IEnumerable<Party> GetByPartyType(string name, PartyType type, params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            var sql = string.Format("SELECT {0} FROM {1} WHERE PartyType = @type AND CompanyName LIKE CONCAT('%', @name, '%')", cols, Table);
            return Connection.Query<Party>(sql, new { type = type, name = name });
        }
    }
}
