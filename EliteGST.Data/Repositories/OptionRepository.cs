using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EliteGST.Data.Models;
using EliteGST.Data.Interfaces;
using Dapper;

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
