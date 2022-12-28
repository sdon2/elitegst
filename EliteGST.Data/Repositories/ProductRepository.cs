using EliteGST.Data.Models;

namespace EliteGST.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository()
            : base ("products")
        {

        }
    }
}
