using System;
using System.Collections.Generic;
using System.Linq;
using EliteGST.Data;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;
using Elite.Utilities;

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
            var states = "Jammu & Kashmir|01,Himachal Pradesh|02,Punjab|03,Chandigarh|04,Uttarakhand|05,Haryana|06,Delhi|07,Rajasthan|08,Uttar Pradesh|09,Bihar|10,Sikkim|11,Arunachal Pradesh|12,Nagaland|13,Manipur|14,Mizoram|15,Tripura|16,Meghalaya|17,Assam|18,West Bengal|19,Jharkhand|20,Orissa|21,Chhattisgarh|22,Madhya Pradesh|23,Gujarat|24,Daman & Diu|25,Dadra & Nagar Haveli|26,Maharashtra|27,Andhra Pradesh|28,Karnataka|29,Goa|30,Lakshadweep|31,Kerala|32,Tamil Nadu|33,Puducherry|34,Andaman & Nicobar Islands|35,Telengana|36,Andrapradesh(New)|37".Split(',').OrderBy(i => i).ToArray();
            comboBox1.Items.Add(new { Code = "", State = "Unknown" });
            foreach (var state in states)
            {
                var stateParts = state.Split('|');
                comboBox1.Items.Add(new { Code = stateParts[1], State = string.Format("{0} - {1}", stateParts[0], stateParts[1]) });
            }
            comboBox1.ValueMember = "Code";
            comboBox1.DisplayMember = "State";

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
                txtPhone.DataBindings.Add("Text", _party, "Phone");
                txtEmail.DataBindings.Add("Text", _party, "Email");
                txtGSTIN.DataBindings.Add("Text", _party, "GSTIN");
                txtOpeningBalance.DataBindings.Add("Text", _party, "OpeningBalance");
                chkIsActive.DataBindings.Add("Checked", _party, "IsActive");
                if (!string.IsNullOrEmpty(_party.Code))
                    comboBox1.SelectedIndex = comboBox1.FindStringExact(string.Format("{0} - {1}", _party.State, _party.Code), 0);
                else
                    comboBox1.SelectedIndex = 0;

                PartyType = _party.PartyType;

                // Set form title
                if (PartyType == Data.PartyType.Customer)
                {
                    Text = "Customer Details";
                }
                else if (PartyType == Data.PartyType.Supplier)
                {
                    Text = "Supplier Details";
                    txtOpeningBalance.Enabled = false;
                }
                else
                {
                    Text = "Company Info";
                }
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
                var selectedState = (dynamic)comboBox1.SelectedItem;
                _party.Code = selectedState.Code;
                if (selectedState.State == "Unknown")
                {
                    _party.State = "";
                }
                else
                {
                    var stateText = (string)selectedState.State;
                    var state = stateText.Substring(0, stateText.IndexOf(" - "));
                    _party.State = state;
                }

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
