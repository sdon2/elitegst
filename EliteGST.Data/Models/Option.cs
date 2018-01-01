using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteGST.Data.Models
{
    public class Option
    {
        public int Id { get; set; }
        public decimal DefaultCGSTRate { get; set; }
        public decimal DefaultSGSTRate { get; set; }
        public decimal DefaultIGSTRate { get; set; }
        public decimal DefaultDiscountRate { get; set; }
        public decimal DefaultFoldingLossRate { get; set; }
        public string DefaultInvoiceRemarks { get; set; }
        public string DefaultFabricInvoiceRemarks { get; set; }
        public string DefaultPurchaseOrderRemarks { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankBranch { get; set; }
        public string BankIFSC { get; set; }
    }
}
