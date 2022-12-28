using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using EliteGST.Data;
using Elite.Utilities;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;

namespace EliteGST.Forms
{
    public partial class PurchaseOrderDetails : BaseForm
    {
        public int Id;

        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();
        private ProductRepository _prrepo = ServiceContainer.GetInstance<ProductRepository>();
        private PurchaseOrderRepository _irepo = ServiceContainer.GetInstance<PurchaseOrderRepository>();
        private PurchaseOrderProductRepository _iprepo = ServiceContainer.GetInstance<PurchaseOrderProductRepository>();
        private OptionRepository _orepo = ServiceContainer.GetInstance<OptionRepository>();

        private int _maxProducts = 10;
        private bool _productSelected = false;
        private int _productIndex;
        private PurchaseOrder _purchaseOrder;
        private List<PurchaseOrderProduct> _purchaseOrderProducts;
        private BindingList<ProductView> _productView = new BindingList<ProductView>();

        private int _billingId;
        private Party _billing;

        private int _shippingId;
        private Party _shipping;

        private int _productId;
        private decimal _defaultCGSTRate;
        private decimal _defaultSGSTRate;
        private decimal _defaultIGSTRate;
        private decimal _defaultDiscountRate;
        private string _defaultRemarks;

        public PurchaseOrderDetails()
        {
            InitializeComponent();
        }

        private void InvoiceDetails_Load(object sender, EventArgs e)
        {
            try
            {
                simpleButton8.Enabled = _productSelected;
                simpleButton4.Enabled = !_productSelected;
                simpleButton5.Enabled = _productSelected;
                dataGridView1.DataSource = _productView;
                var cols = new List<int> { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                cols.ForEach(col => dataGridView1.Columns[col].Visible = false);

                var options = _orepo.GetAll().FirstOrDefault();
                if (options == null) options = new Option();
                _defaultCGSTRate = options.DefaultCGSTRate;
                _defaultSGSTRate = options.DefaultSGSTRate;
                _defaultIGSTRate = options.DefaultIGSTRate;
                _defaultDiscountRate = options.DefaultDiscountRate;
                _defaultRemarks = options.DefaultPurchaseOrderRemarks;

                _purchaseOrder = _irepo.GetById(Id);
                if (_purchaseOrder == null)
                {
                    _purchaseOrder = new PurchaseOrder();
                    _purchaseOrder.PurchaseOrderDate = DateTime.Now;
                    _purchaseOrderProducts = new List<PurchaseOrderProduct>();
                    _purchaseOrder.PurchaseOrderStringId = IdGenerators.GeneratePurchaseOrderId();
                    _purchaseOrder.Remarks = _defaultRemarks;
                }
                else
                {
                    _purchaseOrderProducts = _iprepo.GetProductsForPurchaseOrder(Id).ToList();
                }

                txtInvoiceId.Text = _purchaseOrder.PurchaseOrderStringId;
                txtDate.DateTime = _purchaseOrder.PurchaseOrderDate;
                txtRemarks.Text = _purchaseOrder.Remarks;

                _billingId = _purchaseOrder.BillingId;
                _billing = _prepo.GetById(_billingId);
                if (_billing == null)
                {
                    _billing = new Party { PartyType = PartyType.Supplier };
                }

                txtBillCompanyName.Text = _billing.CompanyName;
                txtBillingAddress.Text = _billing.Address;
                txtBillingGSTIN.Text = _billing.GSTIN;

                _shippingId = _purchaseOrder.ShippingId;
                _shipping = _prepo.GetById(_shippingId);
                if (_shipping == null)
                {
                    _shipping = new Party { PartyType = PartyType.Supplier };
                }
                txtShippingCompanyName.Text = _shipping.CompanyName;
                txtShippingAddress.Text = _shipping.Address;
                txtShippingGSTIN.Text = _shipping.GSTIN;

                var _firstRound = true;
                foreach (var _ip in _purchaseOrderProducts)
                {
                    var ip = _prrepo.GetById(_ip.ProductId);
                    if (ip != null)
                    {
                        var pv = new ProductView { Id = _ip.Id, ProductDescription = ip.ProductDescription, Qty = _ip.Quantity, Rate = _ip.Rate, Discount = _ip.Discount, HSN = ip.HSN, CGST = _ip.CGST, CGSTRate = _ip.CGSTRate, SGST = _ip.SGST, SGSTRate = _ip.SGSTRate, IGST = _ip.IGST, IGSTRate = _ip.IGSTRate, ProductId = ip.Id };
                        _productView.Add(pv);
                        _defaultDiscountRate = (pv.DiscountRate > _defaultDiscountRate) ? pv.DiscountRate : _defaultDiscountRate;
                        if (_firstRound)
                        {
                            _defaultCGSTRate = pv.CGSTRate;
                            _defaultSGSTRate = pv.SGSTRate;
                            _defaultIGSTRate = pv.IGSTRate;
                            _firstRound = false;
                        }
                    }
                }

                CalculateTotal();

            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void CalculateTotal()
        {
            var total = 0.00M;
            foreach (var p in _productView)
            {
                total += (p.Qty * p.Rate - p.Discount) + p.CGST + p.SGST + p.IGST;
            }

            lblInvoiceTotal.Text = total.ToString("f2");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (var cus = new Forms.PartyList())
            {
                cus.PartyType = PartyType.Supplier;
                cus.SelectMode = true;
                if (cus.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var customer = cus.Party;
                    _billingId = customer.Id;
                    txtBillCompanyName.Text = customer.CompanyName;
                    txtBillingAddress.Text = customer.Address;
                    txtBillingGSTIN.Text = customer.GSTIN;
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            using (var cus = new Forms.PartyList())
            {
                cus.PartyType = PartyType.Supplier;
                cus.SelectMode = true;
                if (cus.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var customer = cus.Party;
                    _shippingId = customer.Id;
                    txtShippingCompanyName.Text = customer.CompanyName;
                    txtShippingAddress.Text = customer.Address;
                    txtShippingGSTIN.Text = customer.GSTIN;
                }
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            using (var p = new Forms.ProductList())
            {
                p.SelectMode = true;
                if (p.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var pro = p.Product;
                    _productId = pro.Id;
                    txtProductName.Text = pro.ProductDescription;
                    txtProductHSN.Text = pro.HSN;
                    txtProductQty.EditValue = 1.00m;
                    txtProductRate.EditValue = pro.Rate;
                    txtProductDiscount.EditValue = 0.00m;
                    txtDiscountRate.EditValue = _defaultDiscountRate;
                    txtCGSTRate.EditValue = _defaultCGSTRate;
                    txtSGSTRate.EditValue = _defaultSGSTRate;
                    txtIGSTRate.EditValue = _defaultIGSTRate;
                    CalculateProductTotal();
                    txtProductRate.Focus();
                }
            }
        }

        private void CalculateProductTotal()
        {

            var qty = Convert.ToDecimal(txtProductQty.EditValue);
            var rate = Convert.ToDecimal(txtProductRate.EditValue);
            var drate = Convert.ToDecimal(txtDiscountRate.EditValue);
            
            var cgstRate = Convert.ToDecimal(txtCGSTRate.EditValue);
            var sgstRate = Convert.ToDecimal(txtSGSTRate.EditValue);
            var igstRate = Convert.ToDecimal(txtIGSTRate.EditValue);

            txtProductDiscount.Text = (qty > 0 && rate > 0 && drate > 0) ? (qty * rate * (drate / 100)).ToString("f2") : "0.00";
            var discount = Convert.ToDecimal(txtProductDiscount.EditValue);

            var subtotal = (qty * rate) - discount;
            txtCGST.Text = (subtotal * (cgstRate / 100)).ToString("f2");
            txtSGST.Text = (subtotal * (sgstRate / 100)).ToString("f2");
            txtIGST.Text = (subtotal * (igstRate / 100)).ToString("f2");

            var cgst = Convert.ToDecimal(txtCGST.EditValue);
            var sgst = Convert.ToDecimal(txtSGST.EditValue);
            var igst = Convert.ToDecimal(txtIGST.EditValue);
            
            
            txtProductTotal.Text = (subtotal + cgst + sgst + igst).ToString("f2");
        }

        private void txtCGSTRate_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtSGSTRate_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtIGSTRate_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            _productId = 0;
            txtProductName.Text = "";
            txtProductHSN.Text = "";
            txtProductQty.EditValue = 0.00m;
            txtProductRate.EditValue = 0.00m;
            txtProductDiscount.EditValue = 0.00m;
            txtDiscountRate.EditValue = 0.00m;
            txtCGSTRate.EditValue = 0.00m;
            txtSGSTRate.EditValue = 0.00m;
            txtIGST.EditValue = 0.00m;
            txtIGSTRate.EditValue = 0.00m;
            _productSelected = false;
            simpleButton8.Enabled = _productSelected;
            simpleButton4.Enabled = !_productSelected;
            simpleButton5.Enabled = _productSelected;
            CalculateProductTotal();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (_productId == 0) throw new Exception("Please select a product by clicking select button");
                if (_productView.Count > _maxProducts - 1) throw new Exception(string.Format("No of products cannot exceed {0}.", _maxProducts));
                if (Convert.ToDecimal(txtProductTotal.EditValue) <= 0) throw new Exception("Product quantity and rate must be greater than 0");
                var product = new ProductView
                {
                    Id = 0,
                    ProductId = _productId,
                    ProductDescription = txtProductName.Text,
                    HSN = txtProductHSN.Text,
                    Rate = Convert.ToDecimal(txtProductRate.EditValue),
                    Qty = Convert.ToDecimal(txtProductQty.EditValue),
                    CGST = Convert.ToDecimal(txtCGST.EditValue),
                    CGSTRate = Convert.ToDecimal(txtCGSTRate.EditValue),
                    SGST = Convert.ToDecimal(txtSGST.EditValue),
                    SGSTRate = Convert.ToDecimal(txtSGSTRate.EditValue),
                    IGST = Convert.ToDecimal(txtIGST.EditValue),
                    IGSTRate = Convert.ToDecimal(txtIGSTRate.EditValue),
                    Discount = Convert.ToDecimal(txtProductDiscount.EditValue)
                };
                _productView.Add(product);
                _defaultDiscountRate = product.DiscountRate;
                _defaultCGSTRate = product.CGSTRate;
                _defaultSGSTRate = product.SGSTRate;
                _defaultIGSTRate = product.IGSTRate;
                CalculateTotal();
                simpleButton3.PerformClick();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void txtProductQty_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtProductRate_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtProductDiscount_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtCGST_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtSGST_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void txtIGST_EditValueChanged(object sender, EventArgs e)
        {
            CalculateProductTotal();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (_productSelected && dataGridView1.SelectedRows.Count == 1)
            {
                _productView.RemoveAt(dataGridView1.SelectedRows[0].Index);
                simpleButton3.PerformClick();
                CalculateTotal();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                _productSelected = true;
                _productIndex = dataGridView1.SelectedRows[0].Index;
                var p = _productView[_productIndex];
                _productId = p.ProductId;
                txtProductName.Text = p.ProductDescription;
                txtProductHSN.Text = p.HSN;
                txtProductQty.EditValue = p.Qty;
                txtProductRate.EditValue = p.Rate;
                txtProductDiscount.EditValue = p.Discount;
                txtDiscountRate.EditValue = p.DiscountRate;
                txtCGST.EditValue = p.CGST;
                txtCGSTRate.EditValue = p.CGSTRate;
                txtSGST.EditValue = p.SGST;
                txtSGSTRate.EditValue = p.SGSTRate;
                txtIGST.EditValue = p.IGST;
                txtIGSTRate.EditValue = p.IGSTRate;
                simpleButton8.Enabled = _productSelected;
                simpleButton4.Enabled = !_productSelected;
                simpleButton5.Enabled = _productSelected;
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int _tempid;
                if (!int.TryParse(txtInvoiceId.Text, out _tempid)) throw new Exception("Purchase order no must be a number");
                if (_billingId == 0) throw new Exception("Please select billing address");
                if (_shippingId == 0) throw new Exception("Please select shipping address");
                if (dataGridView1.Rows.Count < 1) throw new Exception("Please add some products before proceeding");

                var purchaseOrder = _irepo.GetById(Id);
                if (purchaseOrder == null)
                {
                    purchaseOrder = new PurchaseOrder();
                }

                purchaseOrder.FinancialYearId = MainForm.financialYear.Id;
                purchaseOrder.PurchaseOrderDate = txtDate.DateTime.Date;
                purchaseOrder.PurchaseOrderStringId = txtInvoiceId.Text;
                purchaseOrder.BillingId = _billingId;
                purchaseOrder.ShippingId = _shippingId;
                purchaseOrder.Remarks = txtRemarks.Text;

                if (Id == 0)
                {
                    int id = _irepo.Insert(purchaseOrder) ?? 0;
                    if (id == 0)
                    {
                        throw new Exception("Unable to add purchase order");
                    }
                    Id = id;
                }
                else
                {
                    _irepo.Update(purchaseOrder, Id);
                    _iprepo.DeleteAllPurchaseOrderProducts(Id);
                }

                foreach (var ip in _productView)
                {
                    var p = new PurchaseOrderProduct { PurchaseOrderId = Id, ProductId = ip.ProductId, Quantity = ip.Qty, Rate = ip.Rate, Discount = ip.Discount, CGST = ip.CGST, CGSTRate = ip.CGSTRate, SGST = ip.SGST, SGSTRate = ip.SGSTRate, IGST = ip.IGST, IGSTRate = ip.IGSTRate };
                    _iprepo.Insert(p);
                }

                Helpers.ShowSuccess("Purchase Order saved successfully");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void txtLoading_EditValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void txtOtherCharges_EditValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (_productId == 0) throw new Exception("Please select a product by clicking select button");
                if (Convert.ToDecimal(txtProductTotal.EditValue) <= 0) throw new Exception("Product quantity and rate must be greater than 0");
                var product = new ProductView
                {
                    Id = 0,
                    ProductId = _productId,
                    ProductDescription = txtProductName.Text,
                    HSN = txtProductHSN.Text,
                    Rate = Convert.ToDecimal(txtProductRate.EditValue),
                    Qty = Convert.ToDecimal(txtProductQty.EditValue),
                    CGST = Convert.ToDecimal(txtCGST.EditValue),
                    CGSTRate = Convert.ToDecimal(txtCGSTRate.EditValue),
                    SGST = Convert.ToDecimal(txtSGST.EditValue),
                    SGSTRate = Convert.ToDecimal(txtSGSTRate.EditValue),
                    IGST = Convert.ToDecimal(txtIGST.EditValue),
                    IGSTRate = Convert.ToDecimal(txtIGSTRate.EditValue),
                    Discount = Convert.ToDecimal(txtProductDiscount.EditValue)
                };
                _productView.RemoveAt(_productIndex);
                _productView.Insert(_productIndex, product);
                CalculateTotal();
                simpleButton3.PerformClick();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void txtDiscountRate_EditValueChanged(object sender, EventArgs e)
        {
            var quantity = Convert.ToDecimal(txtProductQty.EditValue);
            var rate = Convert.ToDecimal(txtProductRate.EditValue);
            var discountRate = Convert.ToDecimal(txtDiscountRate.EditValue);
            var discount = (quantity * rate) * (discountRate / 100);
            txtProductDiscount.EditValue = discount.ToString("f2");
        }
    }
}
