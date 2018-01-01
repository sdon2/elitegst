using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteGST.Data;
using Elite.Utilities;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;

namespace EliteGST.Forms
{
    public partial class PartyDetails : BaseForm
    {
        public int Id { get; set; }
        public PartyType PartyType { get; set; }
        private Party _party;
        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();

        public PartyDetails()
        {
            InitializeComponent();
        }

        private void CompanyInfo_Load(object sender, EventArgs e)
        {
            if (PartyType == Data.PartyType.Customer)
            {
                Text = "Customer Details";
            }
            else if (PartyType == Data.PartyType.Supplier)
            {
                Text = "Supplier Details";
            }
            else
            {
                Text = "Company Info";
            }

            try
            {
                _party = _prepo.GetById(Id);
                if (_party == null)
                {
                    _party = new Party
                    {
                        PartyType = PartyType,
                        IsActive = true
                    };
                }

                txtCompanyName.DataBindings.Add("Text", _party, "CompanyName");
                txtAddress.DataBindings.Add("Text", _party, "Address");
                txtCity.DataBindings.Add("Text", _party, "City");
                txtState.DataBindings.Add("Text", _party, "State");
                txtCode.DataBindings.Add("Text", _party, "Code");
                txtPhone.DataBindings.Add("Text", _party, "Phone");
                txtEmail.DataBindings.Add("Text", _party, "Email");
                txtGSTIN.DataBindings.Add("Text", _party, "GSTIN");
                chkIsActive.DataBindings.Add("Checked", _party, "IsActive");
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
                if (Id == 0)
                {
                    _prepo.Insert(_party);
                }
                else
                {
                    _prepo.Update(_party, _party.Id);
                }
                Helpers.ShowSuccess("Details saved successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
