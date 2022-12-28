using EliteGST.Data.Models;

namespace EliteGST.Data.Repositories
{
    public class OptionRepository : BaseRepository<Option>
    {
        public OptionRepository()
            : base ("options")
        {

        }
    }
}
