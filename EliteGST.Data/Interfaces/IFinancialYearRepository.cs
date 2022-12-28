using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IFinancialYearRepository : IRepository<FinancialYear>
    {
        int GetCount();
    }
}
