using System.Collections.Generic;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        IEnumerable<Payment> GetByPartyId(int id, params string[] columns);
        IEnumerable<Payment> GetByPartyName(string name, int year, params string[] columns);
    }
}
