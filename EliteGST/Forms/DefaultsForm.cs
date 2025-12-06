using System;
using System.Linq;
using EliteGST.Data.Repositories;
using Elite.Utilities;
using EliteGST.Data.Models;

namespace EliteGST.Forms
{
    public partial class DefaultsForm : BaseForm
    {
        private OptionRepository _repo = ServiceContainer.GetInstance<OptionRepository>();
        private Option option;

        public DefaultsForm()
        {
            InitializeComponent();
        }

        private void DefaultsForm_Load(object sender, EventArgs e)
        {
            var config = Config.config;
            if (!(bool)config["include_purchase_order"])
            {
                txtPurchaseOrderRemarks.Enabled = false;
            }
            if (!(bool) config["fabric_invoice_required"])
            {
                txtFabricInvoiceRemarks.Enabled = false;
                txtFoldingLossRate.Enabled = false;
            }

            option = _repo.GetAll().FirstOrDefault();
            if (option == null) option = new Option();

            txtCGSTRate.DataBindings.Add("Text", option, "DefaultCGSTRate");
            txtSGSTRate.DataBindings.Add("Text", option, "DefaultSGSTRate");
            txtIGSTRate.DataBindings.Add("Text", option, "DefaultIGSTRate");
            txtDiscountRate.DataBindings.Add("Text", option, "DefaultDiscountRate");
            txtFoldingLossRate.DataBindings.Add("Text", option, "DefaultFoldingLossRate");
            txtInvoiceRemarks.DataBindings.Add("Text", option, "DefaultInvoiceRemarks");
            txtPurchaseOrderRemarks.DataBindings.Add("Text", option, "DefaultPurchaseOrderRemarks");
            txtFabricInvoiceRemarks.DataBindings.Add("Text", option, "DefaultFabricInvoiceRemarks");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((Convert.ToDecimal(txtCGSTRate.EditValue) > 0 || Convert.ToDecimal(txtSGSTRate.EditValue) > 0) && Convert.ToDecimal(txtIGSTRate.EditValue) > 0)
                    throw new Exception("You can set either CGST, SGST (or) IGST. Do not update both of them");

                if (option.Id == 0)
                {
                    _repo.Insert(option);
                }
                else
                {
                    _repo.Update(option, option.Id);
                }
                Helpers.ShowSuccess("Defaults updated successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
