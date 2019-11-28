using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IFinancialYearRepository : IRepository<FinancialYear>
    {
        int GetCount();
    }
}
