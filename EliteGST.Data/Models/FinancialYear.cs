using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class FinancialYear
    {
        public int Id { get; set; }
        [DisplayName("Financial Year")]
        public string FinancialYearString { get; set; }
    }
}
