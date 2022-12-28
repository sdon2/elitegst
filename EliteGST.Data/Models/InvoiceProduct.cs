namespace EliteGST.Data.Models
{
    public class InvoiceProduct
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        [IgnoreProperty(true)]
        public string ProductDescription { get; set; }
        [IgnoreProperty(true)]
        public string HSN { get; set; }
        [IgnoreProperty(true)]
        public string UoM { get; set; }
        public decimal Quantity { get; set; }
        public int Packs { get; set; }
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
