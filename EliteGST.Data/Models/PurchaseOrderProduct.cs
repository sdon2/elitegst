using System;
using System.Linq;
using System.Text;

namespace EliteGST.Data.Models
{
    public class PurchaseOrderProduct
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        [IgnoreProperty(true)]
        public string ProductDescription { get; set; }
        [IgnoreProperty(true)]
        public string HSN { get; set; }
        [IgnoreProperty(true)]
        public string UoM { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        [IgnoreProperty(true)]
        public decimal Subtotal { get { return Quantity * Rate; } }
        public decimal Discount { get; set; }
        [IgnoreProperty(true)]
        public decimal TaxableValue { get { return Subtotal - Discount; } }
        public decimal CGSTRate { get; set; }
        public decimal CGST { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal SGST { get; set; }
        public decimal IGSTRate { get; set; }
        public decimal IGST { get; set; }
    }
}
