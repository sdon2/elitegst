using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EliteGST.Data.Models
{
    public class Party
    {
        public int Id { get; set; }
        public PartyType PartyType { get; set; }
        [DisplayName("Company")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string GSTIN { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsActive { get; set; }
        [IgnoreProperty(true)]
        [DisplayName("Current Balance")]
        public decimal Balance { get; set; }
    }
}
