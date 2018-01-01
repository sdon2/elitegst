using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data;
using System.Data;
using Elite.Utilities;
using EliteGST.Data.Repositories;

namespace EliteGST
{
    public static class IdGenerators
    {
        public static string GenerateInvoiceId()
        {
            var value = 0;

            var _irepo = ServiceContainer.GetInstance<InvoiceRepository>();

            if (_irepo.GetCount() > 0)
            {
                var val = 0;
                var prevRec = _irepo.GetAll("Id", "InvoiceStringId").OrderByDescending(j => j.Id).First();
                if (Int32.TryParse(prevRec.InvoiceStringId, out val))
                {
                    value = val;
                }
                else
                {
                    value = prevRec.Id;
                }
            }
            return (++value).ToString();
        }

        public static string GeneratePurchaseOrderId()
        {
            var value = 0;

            var _irepo = ServiceContainer.GetInstance<PurchaseOrderRepository>();

            if (_irepo.GetCount() > 0)
            {
                var val = 0;
                var prevRec = _irepo.GetAll("Id", "PurchaseOrderStringId").OrderByDescending(j => j.Id).First();
                if (Int32.TryParse(prevRec.PurchaseOrderStringId, out val))
                {
                    value = val;
                }
                else
                {
                    value = prevRec.Id;
                }
            }
            return (++value).ToString();
        }
    }
}
