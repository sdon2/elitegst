using System;
using System.Collections.Generic;
using System.Linq;
using EliteGST.Data;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;
using Elite.Utilities;

namespace EliteGST.Forms
{
    public partial class PaymentDetails : BaseForm
    {
        public int Id { get; set; }
        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();
        private InvoiceRepository _irepo = ServiceContainer.GetInstance<InvoiceRepository>();
        private PaymentRepository _payrepo = ServiceContainer.GetInstance<PaymentRepository>();
        private Payment _payment;

        public PaymentDetails()
        {
            InitializeComponent();
        }

        private void CompanyInfo_Load(object sender, EventArgs e)
        {
            var parties = _prepo.GetByPartyType(string.Empty, PartyType.Customer, "Id", "CompanyName").ToList();
            comboBox1.Items.Add(new { Id = 0, Customer = "--SELECT--" });
            foreach (var party in parties)
            {
                comboBox1.Items.Add(new { Id = party.Id, Customer = party.CompanyName });
            }
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Customer";

            try
            {
                _payment = _payrepo.GetById(Id);
                if (_payment == null)
                {
                    _payment = new Payment
                    {
                        PaymentDate = DateTime.Now.Date,
                        Remarks = "N/A",
                        Customer = "--SELECT--"
                    };
                }

                paymentDate.DateTime = _payment.PaymentDate.Date;
                txtAmount.DataBindings.Add("Text", _payment, "Amount");
                txtRemarks.DataBindings.Add("Text", _payment, "Remarks");
                comboBox1.SelectedIndex = comboBox1.FindStringExact(_payment.Customer, 0);
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedParty = (dynamic)comboBox1.SelectedItem;
                _payment.CustomerId = selectedParty.Id;
                _payment.PaymentDate = paymentDate.DateTime.Date;
                if (_payment.CustomerId == 0)
                {
                    throw new Exception("Please select customer");
                }

                if (Id == 0)
                {
                    _payrepo.Insert(_payment);
                }
                else
                {
                    _payrepo.Update(_payment, Id);
                }
                Helpers.ShowSuccess("Details saved successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int customerId = ((dynamic)comboBox1.SelectedItem).Id;
                if (customerId == 0)
                {
                    lblBalance.Text = "Current Balance: N/A";
                }
                else
                {
                    var balance = _prepo.GetById(customerId, "OpeningBalance").OpeningBalance + _irepo.GetPreviousByPartyId(customerId, DateTime.Now); ;
                    lblBalance.Text = "Current Balance: " + balance.ToString("n");
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
