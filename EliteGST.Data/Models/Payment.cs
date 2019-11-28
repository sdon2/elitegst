using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int FinancialYearId { get; set; }
        [DisplayName("Date")]
        public DateTime PaymentDate { get; set; }
        [IgnoreProperty(true)]
        public string Customer { get; set; }
        public int CustomerId { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
    }
}
