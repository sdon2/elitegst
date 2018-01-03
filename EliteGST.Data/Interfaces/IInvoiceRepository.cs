﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;

namespace EliteGST.Data.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        IEnumerable<Invoice> GetByPartyName(string name);
        IEnumerable<Invoice> GetByPartyName(string name, int limit, int offset);
        Invoice GetTotalsByInvoiceType(int invoiceId, InvoiceType type);
        int GetCount();
    }
}