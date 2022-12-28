using System;
using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        [DisplayName("Financial Year")]
        public int FinancialYearId { get; set; }
        [DisplayName("Purchase Order #")]
        public string PurchaseOrderStringId { get; set; }
        [DisplayName("Date")]
        public DateTime PurchaseOrderDate { get; set; }
        [IgnoreProperty(true)]
        public string Supplier { get; set; }
        [IgnoreProperty(true)]
        public string GSTIN { get; set; }
        [IgnoreProperty(true)]
        public decimal Quantity { get; set; }
        [IgnoreProperty(true)]
        public decimal CGST { get; set; }
        [IgnoreProperty(true)]
        public decimal SGST { get; set; }
        [IgnoreProperty(true)]
        public decimal IGST { get; set; }
        [IgnoreProperty(true)]
        public decimal TotalTaxes
        {
            get
            {
                return CGST + SGST + IGST;
            }
        }
        [IgnoreProperty(true)]
        public decimal Discount { get; set; }
        [IgnoreProperty(true)]
        public decimal Subtotal { get; set; }
        public int BillingId { get; set; }
        public int ShippingId { get; set; }
        [IgnoreProperty(true)]
        public decimal Amount
        {
            get
            {
                return Subtotal + CGST + SGST + IGST - Discount;
            }
        }
        public string Remarks { get; set; }
        public bool IsCancelled { get; set; }
    }
}
