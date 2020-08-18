using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteGST.Data;
using System.ComponentModel.DataAnnotations;
using Elite.Reports;
using Elite.Utilities;
using EliteGST.Data.Models;
using EliteGST.Data.Repositories;
using MySql.Data.MySqlClient;
using System.Dynamic;

namespace EliteGST.Forms
{
    public partial class PurchaseOrderList : BaseForm
    {
        public PurchaseOrder PurchaseOrder;
        public bool SelectMode;
        private BindingList<PurchaseOrder> _purchaseOrders;
        private PurchaseOrderRepository _porepo = ServiceContainer.GetInstance<PurchaseOrderRepository>();
        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();

        // Paging helpers
        private int pageIndex = 0;
        private int pageSize = 20;
        private bool morePages = true;
        private bool customerChanged;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return true;
                case Keys.Enter:
                    PerformSelect();
                    return true;
                case Keys.F2:
                    if (dataGridView1.SelectedRows.Count == 1) editToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Delete:
                    if (dataGridView1.SelectedRows.Count == 1) deleteToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.N:
                    newToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.P:
                    if (dataGridView1.SelectedRows.Count == 1) printToolStripMenuItem.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public PurchaseOrderList()
        {
            InitializeComponent();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            _purchaseOrders = new BindingList<PurchaseOrder>();
            dataGridView1.DataSource = _purchaseOrders;
            var cols = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17 };
            cols.ForEach(i => dataGridView1.Columns[i].Visible = false);
            dataGridView1.Columns[15].DefaultCellStyle.Format = "c";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
            FindPurchaseOrders();
        }

        private void FindPurchaseOrders(string customer = "")
        {
            try
            {
                if (customerChanged)
                {
                    _purchaseOrders.Clear();
                }

                if (!morePages) return;

                var px = _porepo.GetByPartyName(customer, pageSize, pageIndex * pageSize, MainForm.financialYear.Id).ToList();

                if (px.Count == pageSize)
                {
                    morePages = true;
                }
                else morePages = false;
                customerChanged = false;

                if (SelectMode) px = px.Where(p => p.IsCancelled == false).ToList();
                foreach (var pi in px)
                {
                    var totals = _porepo.GetTotals(pi.Id);
                    pi.Subtotal = totals.Subtotal;
                    pi.Quantity = totals.Quantity;
                    pi.CGST = totals.CGST;
                    pi.SGST = totals.SGST;
                    pi.IGST = totals.IGST;
                    pi.Discount = totals.Discount;
                    _purchaseOrders.Add(pi);
                }
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            customerChanged = true;
            morePages = true;
            pageIndex = 0;
            FindPurchaseOrders(textEdit1.Text);
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control c in dataGridView1.Controls)
            {
                if (c is VScrollBar)
                {
                    var bar = c as VScrollBar;

                    if (bar.Value + bar.LargeChange > (bar.Maximum + 10))
                    {
                        if (e.NewValue > e.OldValue)
                        {
                            pageIndex++;
                            FindPurchaseOrders(textEdit1.Text);
                        }
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            customerChanged = true;
            morePages = true;
            pageIndex = 0;
            textEdit1.Text = "";
            FindPurchaseOrders();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    Helpers.ExportToExcel(dataGridView1);
                }
                else
                {
                    throw new Exception("Nothing to export");
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var singleRowSelected = dataGridView1.SelectedRows.Count == 1;
            editToolStripMenuItem.Visible = singleRowSelected;
            printToolStripMenuItem.Visible = singleRowSelected;
            printSinglePageToolStripMenuItem.Visible = singleRowSelected;
            deleteToolStripMenuItem.Visible = singleRowSelected;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.PurchaseOrderDetails())
            {
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    customerChanged = true;
                    morePages = true;
                    pageIndex = 0;
                    textEdit1.Text = "";
                    FindPurchaseOrders();
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.PurchaseOrderDetails())
            {
                var Id = _purchaseOrders[dataGridView1.SelectedRows[0].Index].Id;
                pf.Id = Id;
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    customerChanged = true;
                    morePages = true;
                    pageIndex = 0;
                    FindPurchaseOrders(textEdit1.Text);
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PerformSelect();
        }

        private void PerformSelect()
        {
            if (dataGridView1.SelectedRows.Count != 1) return;

            if (SelectMode)
            {
                PurchaseOrder =_purchaseOrders[dataGridView1.SelectedRows[0].Index];
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                editToolStripMenuItem.PerformClick();
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintInvoice(true);
        }

        private void PrintInvoice(bool allPages)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var _purchaseOrder = _purchaseOrders[dataGridView1.SelectedRows[0].Index];
                var taxable = _purchaseOrder.Subtotal - _purchaseOrder.Discount;
                var i = new
                {
                    Id = _purchaseOrder.PurchaseOrderStringId,
                    Date = _purchaseOrder.PurchaseOrderDate.ToShortDateString(),
                    Quantity = _purchaseOrder.Quantity.ToString("f2"),
                    Discount = _purchaseOrder.Discount.ToString("f2"),
                    Taxable = taxable.ToString("f2"),
                    CGST = _purchaseOrder.CGST.ToString("f2"),
                    SGST = _purchaseOrder.SGST.ToString("f2"),
                    IGST = _purchaseOrder.IGST.ToString("f2"),
                    Taxable1 = taxable.ToString("c"),
                    TotalTaxes = _purchaseOrder.TotalTaxes.ToString("c"),
                    TotalAmount = _purchaseOrder.Amount.ToString("c"),
                    AmountInWords = Convert.ToInt32(_purchaseOrder.Amount).ConvertToWords(),
                    Remarks = _purchaseOrder.Remarks.Replace(Environment.NewLine, "<br/>")
                };

                var company = _prepo.GetByPartyType(string.Empty, PartyType.Self).FirstOrDefault();
                if (company == null) company = new Party();
                var billing = _prepo.GetById(_purchaseOrder.BillingId);
                var shipping = _prepo.GetById(_purchaseOrder.ShippingId);

                var rproducts = new List<PurchaseOrderProductsPrint>();
                var iprepo = ServiceContainer.GetInstance<PurchaseOrderProductRepository>();
                var ips = iprepo.GetProductsForPurchaseOrder(_purchaseOrder.Id).ToList();
                for (var j = 0; j < ips.Count; j++)
                {
                    var ip = ips[j];
                    rproducts.Add(new PurchaseOrderProductsPrint
                    {
                        Id = (j + 1).ToString(),
                        Description = ip.ProductDescription,
                        HSN = ip.HSN,
                        Qty = ip.Quantity.ToString("f2"),
                        UOM = ip.UoM,
                        Rate = ip.Rate.ToString("f2"),
                        Discount = ip.Discount.ToString("f2"),
                        Taxable = ip.TaxableValue.ToString("f2"),
                        CGSTRate = ip.CGSTRate.ToString("f2"),
                        CGST = ip.CGST.ToString("f2"),
                        SGSTRate = ip.SGSTRate.ToString("f2"),
                        SGST = ip.SGST.ToString("f2"),
                        IGSTRate = ip.IGSTRate.ToString("f2"),
                        IGST = ip.IGST.ToString("f2")
                    });
                }

                var rpc = rproducts.Count;
                if (rpc < 9)
                {
                    for (var l = rpc; l <= 9; l++) rproducts.Add(new PurchaseOrderProductsPrint());
                }

                using (var pdfForm = new ReportViewer())
                {
                    using (var pdf = new ReportDocument())
                    {
                        pdf.Margins = 0.25f;
                        pdf.PageSize = PageSizes.A4;
                        pdf.PageOrientation = PageOrientations.Portrait;
                        pdf.AddCSS("reports/css/purchase-order-style.css");
                        
                        var report = "reports/" + Config.config["Purchase Order Report"];
                        
                        dynamic data = new ExpandoObject();
                        data.Page = "";
                        data.DIR = Application.StartupPath;
                        data.company = company;
                        data.purchaseOrder = i;
                        data.billing = billing;
                        data.shipping = shipping;
                        data.products = rproducts;

                        if (allPages)
                        {
                            data.Page = "(ORIGINAL)";
                            pdf.AddPage(report, data);
                            data.Page = "(DUPLICATE)";
                            pdf.AddPage(report, data);
                            data.Page = "(TRIPLICATE)";
                            pdf.AddPage(report, data);
                        }
                        else
                        {
                            data.Page = "";
                            pdf.AddPage(report, data);
                        }

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

        private void printSinglePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintInvoice(false);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            if (!Helpers.Confirm("Are you sure about deleting this purchase order?")) return;
            try
            {
                _porepo.Delete(_purchaseOrders[dataGridView1.SelectedRows[0].Index].Id);
                Helpers.ShowSuccess("Purchase order deleted successfully");
                customerChanged = true;
                morePages = true;
                pageIndex = 0;
                FindPurchaseOrders(textEdit1.Text);
            }
            catch (MySqlException)
            {
                Helpers.ShowError("Cannot delete purchase order. Make sure you have no other transactions related to this entry");
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }

    internal class PurchaseOrderProductsPrint
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Rate { get; set; }
        public string Discount { get; set; }
        public string Taxable { get; set; }
        public string CGSTRate { get; set; }
        public string CGST { get; set; }
        public string SGSTRate { get; set; }
        public string SGST { get; set; }
        public string IGSTRate { get; set; }
        public string IGST { get; set; }
        public string Taxes
        {
            get
            {
                if (CGST == "&nbsp;" || SGST == "&nbsp;" || IGST == "&nbsp;") return "&nbsp;";
                return (Convert.ToDecimal(CGST) + Convert.ToDecimal(SGST) + Convert.ToDecimal(IGST)).ToString("f2");
            }
        }
        public string Total
        {
            get
            {
                if (CGST == "&nbsp;" || SGST == "&nbsp;" || IGST == "&nbsp;" || Taxable == "&nbsp;") return "&nbsp;";
                return (Convert.ToDecimal(CGST) + Convert.ToDecimal(SGST) + Convert.ToDecimal(IGST) + Convert.ToDecimal(Taxable)).ToString("f2");
            }
        }

        public PurchaseOrderProductsPrint()
        {
            Id = "&nbsp;";
            Description = "&nbsp;";
            HSN = "&nbsp;";
            Qty = "&nbsp;";
            UOM = "&nbsp;";
            Rate = "&nbsp;";
            Discount = "&nbsp;";
            Taxable = "&nbsp;";
            CGSTRate = "&nbsp;";
            CGST = "&nbsp;";
            SGSTRate = "&nbsp;";
            SGST = "&nbsp;";
            IGSTRate = "&nbsp;";
            IGST = "&nbsp;";
        }
    }
}
