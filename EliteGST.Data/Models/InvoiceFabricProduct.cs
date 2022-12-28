namespace EliteGST.Data.Models
{
    public class InvoiceFabricProduct
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
        public int Bales { get; set; }
        public int Pieces { get; set; }
        public decimal Meters { get; set; }
        public decimal FoldingLoss { get; set; }
        public decimal FoldingLossRate { get; set; }
        public decimal Rate { get; set; }
        [IgnoreProperty(true)]
        public decimal Amount { get { return (Meters - FoldingLoss) * Rate; } }
        public decimal CGSTRate { get; set; }
        public decimal CGST { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal SGST { get; set; }
        public decimal IGSTRate { get; set; }
        public decimal IGST { get; set; }
    }
}
