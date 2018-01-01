using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public InvoiceType InvoiceType { get; set; }
        [DisplayName("Invoice #")]
        public string InvoiceStringId { get; set; }
        [DisplayName("Date")]
        public DateTime InvoiceDate { get; set; }
        [IgnoreProperty(true)]
        public string Customer { get; set; }
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
        public string TransportMode { get; set; }
        public string VehicleNumber { get; set; }
        public string Remarks { get; set; }
        public decimal LoadingCharges { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal RoundingOff { get; set; }
        public bool IsCancelled { get; set; }
        [IgnoreProperty(true)]
        public decimal Amount
        {
            get
            {
                return Subtotal + CGST + SGST + IGST + OtherCharges + LoadingCharges - Discount + RoundingOff;
            }
        }
    }
}
