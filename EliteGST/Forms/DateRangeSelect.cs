using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Elite.Utilities;
using EliteGST.Data.Repositories;
using EliteGST.Data;
using Elite.Reports;

namespace EliteGST.Forms
{
    public partial class DateRangeSelect : BaseForm
    {
        public int CustomerId { get; set; }

        public DateRangeSelect()
        {
            InitializeComponent();
        }

        private void DateRangeSelect_Load(object sender, EventArgs e)
        {
            var today = DateTime.Now.Date;
            var monthAgo = today - new TimeSpan(30, 0, 0, 0);
            toDate.DateTime = today;
            fromDate.DateTime = monthAgo;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (fromDate.DateTime > toDate.DateTime)
                    throw new Exception("From date cannot be older than to date");

                var irepo = ServiceContainer.GetInstance<InvoiceRepository>();
                var prepo = ServiceContainer.GetInstance<PartyRepository>();

                var company = prepo.GetByPartyType(string.Empty, PartyType.Self).FirstOrDefault();
                var customer = prepo.GetById(CustomerId);
                var dentries = irepo.GetByPartyId(CustomerId, fromDate.DateTime, toDate.DateTime).ToList();
                var previous = irepo.GetPreviousByPartyId(CustomerId, fromDate.DateTime);
                previous += customer.OpeningBalance;

                var entries = new List<CustomerReport>();
                var id = 1;
                var rtotal = previous;
                dentries.ForEach(entry =>
                {
                    var total = (entry.Subtotal + entry.CGST + entry.SGST + entry.IGST) - entry.Discount;
                    rtotal += total;
                    var report = new CustomerReport
                    {
                        Id = id,
                        Date = ((DateTime)entry.InvoiceDate).ToShortDateString(),
                        Particulars = (entry.Id == 0) ? entry.CompanyName : string.Format("TO INVOICE #{0}", entry.InvoiceStringId),
                        Subtotal = entry.Subtotal - entry.Discount,
                        CGST = entry.CGST,
                        SGST = entry.SGST,
                        IGST = entry.IGST,
                        Total = total,
                        Balance = rtotal
                    };
                    entries.Add(report);
                    id++;
                });

                using (var pdfForm = new ReportViewer())
                {
                    using (var pdf = new ReportDocument())
                    {
                        pdf.PageSize = PageSizes.A4;
                        pdf.PageOrientation = PageOrientations.Portrait;
                        pdf.Margins = 0.5f;

                        var report = "reports/customer-report.htm";
                        pdf.AddPage(report, new { Date = DateTime.Now.ToShortDateString(), FromDate = fromDate.DateTime.ToShortDateString(), ToDate = toDate.DateTime.ToShortDateString(),  DIR = Application.StartupPath, company = company, customer = customer, outstanding = previous, entries = entries });
                        pdfForm.ReportDocument = pdf;
                        pdfForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }

    internal class CustomerReport
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Particulars { get; set; }
        public decimal Subtotal { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
    }
}
