using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }
        public string HSN { get; set; }
        public string UoM { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }
    }
}
