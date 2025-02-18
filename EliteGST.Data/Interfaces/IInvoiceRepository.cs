﻿using System.Collections.Generic;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        IEnumerable<Invoice> GetByPartyName(string name);
        IEnumerable<Invoice> GetByPartyName(string name, int limit, int offset, int finyear);
        Invoice GetTotalsByInvoiceType(int invoiceId, InvoiceType type);
        int GetCount();
    }
}
