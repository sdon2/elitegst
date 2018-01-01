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
    public partial class CompanyInfo : BaseForm
    {
        private Party _company;
        private Option _option;
        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();
        private OptionRepository _orepo = ServiceContainer.GetInstance<OptionRepository>();

        public CompanyInfo()
        {
            InitializeComponent();
        }

        private void CompanyInfo_Load(object sender, EventArgs e)
        {
            try
            {
                _company = _prepo.GetByPartyType(string.Empty, PartyType.Self).FirstOrDefault();
                if (_company == null) _company = new Party
                {
                    PartyType = PartyType.Self
                };

                _option = _orepo.GetAll().FirstOrDefault();
                if (_option == null) _option = new Option();

                txtCompanyName.DataBindings.Add("Text", _company, "CompanyName");
                txtAddress.DataBindings.Add("Text", _company, "Address");
                txtCity.DataBindings.Add("Text", _company, "City");
                txtState.DataBindings.Add("Text", _company, "State");
                txtCode.DataBindings.Add("Text", _company, "Code");
                txtPhone.DataBindings.Add("Text", _company, "Phone");
                txtEmail.DataBindings.Add("Text", _company, "Email");
                txtGSTIN.DataBindings.Add("Text", _company, "GSTIN");
                txtAccNo.DataBindings.Add("Text", _option, "BankAccNo");
                txtAccName.DataBindings.Add("Text", _option, "BankAccName");
                txtBankBranch.DataBindings.Add("Text", _option, "BankBranch");
                txtIFSC.DataBindings.Add("Text", _option, "BankIFSC");
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
                // Save company
                if (_company.Id == 0)
                {
                    _prepo.Insert(_company);
                }
                else
                {
                    _prepo.Update(_company, _company.Id);
                }

                // Save bank details
                if (_option.Id == 0)
                {
                    _orepo.Insert(_option);
                }
                else
                {
                    _orepo.Update(_option, _option.Id);
                }
                Helpers.ShowSuccess("Company information saved successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
