using System;
using Elite.Utilities;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;

namespace EliteGST.Forms
{
    public partial class ProductDetails : BaseForm
    {
        public int Id;
        private Product _product;
        private ProductRepository _prepo = ServiceContainer.GetInstance<ProductRepository>();

        public ProductDetails()
        {
            InitializeComponent();
        }

        private void Product_Load(object sender, EventArgs e)
        {
            try
            {
                _product = _prepo.GetById(Id);
                if (_product == null)
                {
                    _product = new Product { IsActive = true };
                }

                txtProductDescription.DataBindings.Add("Text", _product, "ProductDescription");
                txtHSN.DataBindings.Add("Text", _product, "HSN");
                txtUOM.DataBindings.Add("Text", _product, "UoM");
                txtRate.DataBindings.Add("Text", _product, "Rate");
                chkIsActive.DataBindings.Add("Checked", _product, "IsActive");
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductDescription.Text.Trim() == "") throw new Exception("Please enter valid product description");
                if (Convert.ToDecimal(txtRate.EditValue) <= 0) throw new Exception("Please enter valid product rate");
                if (Id == 0)
                {
                    _prepo.Insert(_product);
                }
                else
                {
                    _prepo.Update(_product, Id);
                }
                Helpers.ShowSuccess("Product details saved successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
