using System.Collections.Generic;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IPartyRepository : IRepository<Party>
    {
        IEnumerable<Party> GetByPartyType(string name, PartyType type, params string[] columns);
    }
}
