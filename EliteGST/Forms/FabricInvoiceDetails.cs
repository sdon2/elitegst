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
using EliteGST.Data.Repositories;
using EliteGST.Data.Models;

namespace EliteGST.Forms
{
    public partial class FabricInvoiceDetails : BaseForm
    {
        public int Id;

        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();
        private ProductRepository _prrepo = ServiceContainer.GetInstance<ProductRepository>();
        private InvoiceRepository _irepo = ServiceContainer.GetInstance<InvoiceRepository>();
        private InvoiceFabricProductRepository _iprepo = ServiceContainer.GetInstance<InvoiceFabricProductRepository>();
        private OptionRepository _orepo = ServiceContainer.GetInstance<OptionRepository>();

        private int _maxProducts = 10;
        private bool _productSelected = false;
        private int _productIndex;
        private Invoice _invoice;
        private List<InvoiceFabricProduct> _invoiceProducts;
        private BindingList<FabricProductView> _productView = new BindingList<FabricProductView>();

        private int _billingId;
        private Party _billing;

        private int _shippingId;
        private Party _shipping;

        private int _productId;
        private decimal _defaultCGSTRate;
        private decimal _defaultSGSTRate;
        private decimal _defaultIGSTRate;
        private string _defaultRemarks;
        private decimal _defaultFoldingLossRate;

        public FabricInvoiceDetails()
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
                var cols = new List<int> { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                cols.ForEach(col => dataGridView1.Columns[col].Visible = false);

                var options = _orepo.GetAll().FirstOrDefault();
                if (options == null) options = new Option();
                _defaultCGSTRate = options.DefaultCGSTRate;
                _defaultSGSTRate = options.DefaultSGSTRate;
                _defaultIGSTRate = options.DefaultIGSTRate;
                _defaultFoldingLossRate = options.DefaultFoldingLossRate;
                _defaultRemarks = options.DefaultFabricInvoiceRemarks;

                _invoice = _irepo.GetById(Id);
                if (_invoice == null)
                {
                    chkCancelled.Enabled = false;
                    _invoice = new Invoice();
                    _invoice.InvoiceType = InvoiceType.Fabric;
                    _invoice.InvoiceDate = DateTime.Now;
                    _invoice.TransportMode = "By Road";
                    _invoiceProducts = new List<InvoiceFabricProduct>();
                    _invoice.InvoiceStringId = IdGenerators.GenerateInvoiceId();
                    _invoice.Remarks = _defaultRemarks;
                }
                else
                {
                    _invoiceProducts = _iprepo.GetProductsForInvoice(Id).ToList();
                }

                txtInvoiceId.Text = _invoice.InvoiceStringId;
                txtDate.DateTime = _invoice.InvoiceDate;
                txtTransportMode.Text = _invoice.TransportMode;
                txtVehicleNumber.Text = _invoice.VehicleNumber;
                txtRemarks.Text = _invoice.Remarks;
                txtLoading.EditValue = _invoice.LoadingCharges;
                txtOtherCharges.EditValue = _invoice.OtherCharges;
                txtRoundingOff.EditValue = _invoice.RoundingOff;
                chkCancelled.Checked = _invoice.IsCancelled;

                _billingId = _invoice.BillingId;
                _billing = _prepo.GetById(_billingId);
                if (_billing == null)
                {
                    _billing = new Party { PartyType = PartyType.Customer };
                }

                txtBillCompanyName.Text = _billing.CompanyName;
                txtBillingAddress.Text = _billing.Address;
                txtBillingGSTIN.Text = _billing.GSTIN;

                _shippingId = _invoice.ShippingId;
                _shipping = _prepo.GetById(_shippingId);
                if (_shipping == null)
                {
                    _shipping = new Party { PartyType = PartyType.Customer };
                }
                txtShippingCompanyName.Text = _shipping.CompanyName;
                txtShippingAddress.Text = _shipping.Address;
                txtShippingGSTIN.Text = _shipping.GSTIN;

                var _firstRound = true;
                foreach (var _ip in _invoiceProducts)
                {
                    var ip = _prrepo.GetById(_ip.ProductId);
                    if (ip != null)
                    {
                        var pv = new FabricProductView { Id = _ip.Id, ProductDescription = ip.ProductDescription, Meters = _ip.Meters, Rate = _ip.Rate, FoldingLoss = _ip.FoldingLoss, FoldingLossRate = _ip.FoldingLossRate, HSN = ip.HSN, CGST = _ip.CGST, CGSTRate = _ip.CGSTRate, SGST = _ip.SGST, SGSTRate = _ip.SGSTRate, IGST = _ip.IGST, IGSTRate = _ip.IGSTRate, ProductId = ip.Id, Bales = _ip.Bales, Pieces = _ip.Pieces };
                        _productView.Add(pv);
                        _defaultFoldingLossRate = (pv.FoldingLossRate > _defaultFoldingLossRate) ? pv.FoldingLossRate : _defaultFoldingLossRate;
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
            var loading = Convert.ToDecimal(txtLoading.EditValue);
            var other = Convert.ToDecimal(txtOtherCharges.EditValue);
            var roff = Convert.ToDecimal(txtRoundingOff.EditValue);
            foreach (var p in _productView)
            {
                total += ((p.Meters - p.FoldingLoss) * p.Rate) + p.CGST + p.SGST + p.IGST;
            }

            lblInvoiceTotal.Text = (total + loading + other + roff).ToString("f2");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (var cus = new Forms.PartyList())
            {
                cus.PartyType = PartyType.Customer;
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
                cus.PartyType = PartyType.Customer;
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
                    txtProductMeters.EditValue = 1.00m;
                    txtProductRate.EditValue = pro.Rate;
                    txtFoldingLoss.EditValue = 0.00m;
                    txtFoldingLossRate.EditValue = _defaultFoldingLossRate;
                    txtCGSTRate.EditValue = _defaultCGSTRate;
                    txtSGSTRate.EditValue = _defaultSGSTRate;
                    txtIGSTRate.EditValue = _defaultIGSTRate;
                    txtBales.EditValue = 1;
                    txtPieces.EditValue = 1;
                    CalculateProductTotal();
                    txtProductRate.Focus();
                }
            }
        }

        private void CalculateProductTotal()
        {

            var meters = Convert.ToDecimal(txtProductMeters.EditValue);
            var rate = Convert.ToDecimal(txtProductRate.EditValue);
            var frate = Convert.ToDecimal(txtFoldingLossRate.EditValue);
            
            var cgstRate = Convert.ToDecimal(txtCGSTRate.EditValue);
            var sgstRate = Convert.ToDecimal(txtSGSTRate.EditValue);
            var igstRate = Convert.ToDecimal(txtIGSTRate.EditValue);

            txtFoldingLoss.Text = (meters > 0 && rate > 0 && frate > 0) ? (meters * (frate / 100)).ToString("f2") : "0.00";
            var floss = Convert.ToDecimal(txtFoldingLoss.EditValue);

            var amount = (meters - floss) * rate;
            txtCGST.Text = (amount * (cgstRate / 100)).ToString("f2");
            txtSGST.Text = (amount * (sgstRate / 100)).ToString("f2");
            txtIGST.Text = (amount * (igstRate / 100)).ToString("f2");

            var cgst = Convert.ToDecimal(txtCGST.EditValue);
            var sgst = Convert.ToDecimal(txtSGST.EditValue);
            var igst = Convert.ToDecimal(txtIGST.EditValue);
            
            
            txtProductTotal.Text = (amount + cgst + sgst + igst).ToString("f2");
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
            txtProductMeters.EditValue = 0.00m;
            txtProductRate.EditValue = 0.00m;
            txtFoldingLoss.EditValue = 0.00m;
            txtFoldingLossRate.EditValue = 0.00m;
            txtCGSTRate.EditValue = 0.00m;
            txtSGSTRate.EditValue = 0.00m;
            txtIGST.EditValue = 0.00m;
            txtIGSTRate.EditValue = 0.00m;
            txtBales.EditValue = 1;
            txtPieces.EditValue = 1;
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
                if (Convert.ToDecimal(txtProductTotal.EditValue) <= 0) throw new Exception("Product meters and rate must be greater than 0");
                var product = new FabricProductView
                {
                    Id = 0,
                    ProductId = _productId,
                    ProductDescription = txtProductName.Text,
                    HSN = txtProductHSN.Text,
                    Rate = Convert.ToDecimal(txtProductRate.EditValue),
                    Meters = Convert.ToDecimal(txtProductMeters.EditValue),
                    CGST = Convert.ToDecimal(txtCGST.EditValue),
                    CGSTRate = Convert.ToDecimal(txtCGSTRate.EditValue),
                    SGST = Convert.ToDecimal(txtSGST.EditValue),
                    SGSTRate = Convert.ToDecimal(txtSGSTRate.EditValue),
                    IGST = Convert.ToDecimal(txtIGST.EditValue),
                    IGSTRate = Convert.ToDecimal(txtIGSTRate.EditValue),
                    FoldingLoss = Convert.ToDecimal(txtFoldingLoss.EditValue),
                    FoldingLossRate = Convert.ToDecimal(txtFoldingLossRate.EditValue),
                    Bales = Convert.ToInt32(txtBales.EditValue),
                    Pieces = Convert.ToInt32(txtPieces.EditValue)
                };
                _productView.Add(product);
                _defaultFoldingLossRate = product.FoldingLossRate;
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
                txtProductMeters.EditValue = p.Meters;
                txtProductRate.EditValue = p.Rate;
                txtFoldingLoss.EditValue = p.FoldingLoss;
                txtFoldingLossRate.EditValue = p.FoldingLossRate;
                txtCGST.EditValue = p.CGST;
                txtCGSTRate.EditValue = p.CGSTRate;
                txtSGST.EditValue = p.SGST;
                txtSGSTRate.EditValue = p.SGSTRate;
                txtIGST.EditValue = p.IGST;
                txtIGSTRate.EditValue = p.IGSTRate;
                txtBales.EditValue = p.Bales;
                txtPieces.EditValue = p.Pieces;
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
                if (!int.TryParse(txtInvoiceId.Text, out _tempid)) throw new Exception("Invoice no must be a number");
                if (_billingId == 0) throw new Exception("Please select billing address");
                if (_shippingId == 0) throw new Exception("Please select shipping address");
                if (dataGridView1.Rows.Count < 1) throw new Exception("Please add some products before proceeding");

                var invoice = _irepo.GetById(Id);
                if (invoice == null)
                {
                    invoice = new Invoice();
                }
                invoice.InvoiceType = InvoiceType.Fabric;
                invoice.InvoiceDate = txtDate.DateTime.Date;
                invoice.InvoiceStringId = txtInvoiceId.Text;
                invoice.BillingId = _billingId;
                invoice.ShippingId = _shippingId;
                invoice.TransportMode = txtTransportMode.Text;
                invoice.VehicleNumber = txtVehicleNumber.Text;
                invoice.Remarks = txtRemarks.Text;
                invoice.LoadingCharges = Convert.ToDecimal(txtLoading.EditValue);
                invoice.OtherCharges = Convert.ToDecimal(txtOtherCharges.EditValue);
                invoice.RoundingOff = Convert.ToDecimal(txtRoundingOff.EditValue);
                invoice.IsCancelled = chkCancelled.Checked;

                if (Id == 0)
                {
                    int id = _irepo.Insert(invoice) ?? 0;
                    if (id == 0)
                    {
                        throw new Exception("Unable to add invoice");
                    }
                    Id = id;
                }
                else
                {
                    _iprepo.DeleteAllInvoiceProducts(Id);
                }

                foreach (var ip in _productView)
                {
                    var p = new InvoiceFabricProduct { InvoiceId = Id, ProductId = ip.ProductId, Meters = ip.Meters, Rate = ip.Rate, FoldingLoss = ip.FoldingLoss, FoldingLossRate = ip.FoldingLossRate, CGST = ip.CGST, CGSTRate = ip.CGSTRate, SGST = ip.SGST, SGSTRate = ip.SGSTRate, IGST = ip.IGST, IGSTRate = ip.IGSTRate, Bales = ip.Bales, Pieces = ip.Pieces };
                    _iprepo.Insert(p);
                }
                Helpers.ShowSuccess("Invoice saved successfully");
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
                if (Convert.ToDecimal(txtProductTotal.EditValue) <= 0) throw new Exception("Product meters and rate must be greater than 0");
                var product = new FabricProductView
                {
                    Id = 0,
                    ProductId = _productId,
                    ProductDescription = txtProductName.Text,
                    HSN = txtProductHSN.Text,
                    Rate = Convert.ToDecimal(txtProductRate.EditValue),
                    Meters = Convert.ToDecimal(txtProductMeters.EditValue),
                    Bales = Convert.ToInt32(txtBales.EditValue),
                    Pieces = Convert.ToInt32(txtPieces.EditValue),
                    FoldingLoss = Convert.ToDecimal(txtFoldingLoss.EditValue),
                    FoldingLossRate = Convert.ToDecimal(txtFoldingLossRate.EditValue),
                    CGST = Convert.ToDecimal(txtCGST.EditValue),
                    CGSTRate = Convert.ToDecimal(txtCGSTRate.EditValue),
                    SGST = Convert.ToDecimal(txtSGST.EditValue),
                    SGSTRate = Convert.ToDecimal(txtSGSTRate.EditValue),
                    IGST = Convert.ToDecimal(txtIGST.EditValue),
                    IGSTRate = Convert.ToDecimal(txtIGSTRate.EditValue)
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
            var quantity = Convert.ToDecimal(txtProductMeters.EditValue);
            var rate = Convert.ToDecimal(txtProductRate.EditValue);
            var foldingLessRate = Convert.ToDecimal(txtFoldingLossRate.EditValue);
            var foldingLess = quantity -  (foldingLessRate / 100);
            txtFoldingLoss.EditValue = foldingLess.ToString("f2");
        }
    }

    internal class FabricProductView
    {
        public int Id { get; set; }
        [DisplayName("Product")]
        public string ProductDescription { get; set; }
        [DisplayName("Meters")]
        public decimal Meters { get; set; }
        public decimal Rate { get; set; }
        public int Bales { get; set; }
        public int Pieces { get; set; }
        public int ProductId { get; set; }
        public string HSN { get; set; }
        public decimal FoldingLoss { get; set; }
        public decimal FoldingLossRate { get; set; }
        public decimal CGST { get; set; }
        public decimal CGSTRate { get; set; }
        public decimal SGST { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTRate { get; set; }
    }
}
