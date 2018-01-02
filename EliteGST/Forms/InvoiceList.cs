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

namespace EliteGST.Forms
{
    public partial class InvoiceList : BaseForm
    {
        public Invoice Invoice { get; set; }
        public bool SelectMode { get; set; }
        public bool IsPacksRequired { get; set; }
        public bool IsFabricInvoiceRequired { get; set; }
        private BindingList<Invoice> _invoices;
        private InvoiceRepository _irepo = ServiceContainer.GetInstance<InvoiceRepository>();
        private PartyRepository _prepo = ServiceContainer.GetInstance<PartyRepository>();

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
                    if (dataGridView1.SelectedRows.Count == 1) deleteInvoiceToolStripMenuItem.PerformClick();
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

        public InvoiceList()
        {
            InitializeComponent();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            _invoices = new BindingList<Invoice>();
            dataGridView1.DataSource = _invoices;
            var cols = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };
            cols.ForEach(i => dataGridView1.Columns[i].Visible = false);
            dataGridView1.Columns[22].DefaultCellStyle.Format = "c";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
            FindInvoices();
        }

        private void FindInvoices(string customer = "")
        {
            try
            {
                _invoices.Clear();
                var px = _irepo.GetByPartyName(customer).OrderByDescending(i => Convert.ToInt32(i.InvoiceStringId)).ToList();
                if (!checkEdit1.Checked) px = px.Where(p => p.IsCancelled == false).ToList();
                foreach (var pi in px)
                {
                    var totals = _irepo.GetTotalsByInvoiceType(pi.Id, pi.InvoiceType);
                    if (totals == null) totals = new Data.Models.Invoice { InvoiceType = pi.InvoiceType };
                    pi.Quantity = totals.Quantity;
                    pi.Subtotal = totals.Subtotal;
                    pi.CGST = totals.CGST;
                    pi.SGST = totals.SGST;
                    pi.IGST = totals.IGST;
                    pi.Discount = totals.Discount;
                    _invoices.Add(pi);
                }
                SetColumnsColor();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void SetColumnsColor()
        {
            if (_invoices.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var style = new DataGridViewCellStyle();
                        if (_invoices[row.Index].IsCancelled)
                        {
                            style.BackColor = Color.OrangeRed;
                            style.ForeColor = Color.White;
                            if (row.Index == dataGridView1.SelectedRows[0].Index)
                            {
                                style.SelectionBackColor = Color.Red;
                                style.SelectionForeColor = Color.White;
                            }
                        }
                        else
                        {
                            style.BackColor = Color.White;
                            style.ForeColor = Color.Black;
                        }
                        cell.Style = style;
                    }
                }
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            FindInvoices(textEdit1.Text);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            textEdit1.Text = "";
            FindInvoices();
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
            newFabricInvoiceToolStripMenuItem.Visible = IsFabricInvoiceRequired;
            deleteInvoiceToolStripMenuItem.Visible = singleRowSelected;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.InvoiceDetails())
            {
                pf.IsPacksRequired = IsPacksRequired;
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textEdit1.Text = "";
                    FindInvoices();
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Id = _invoices[dataGridView1.SelectedRows[0].Index].Id;

            if (_invoices[dataGridView1.SelectedRows[0].Index].InvoiceType == InvoiceType.Normal)
            {
                using (var pf = new Forms.InvoiceDetails())
                {
                    pf.IsPacksRequired = IsPacksRequired;
                    pf.Id = Id;
                    if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FindInvoices(textEdit1.Text);
                    }
                }
            }
            else
            {
                using (var pf = new Forms.FabricInvoiceDetails())
                {
                    pf.Id = Id;
                    if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FindInvoices(textEdit1.Text);
                    }
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
                Invoice = _invoices[dataGridView1.SelectedRows[0].Index];
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
                var _invoice = _invoices[dataGridView1.SelectedRows[0].Index];
                var taxable = _invoice.Subtotal - _invoice.Discount;

                dynamic i = new
                {
                    Id = _invoice.InvoiceStringId,
                    Date = _invoice.InvoiceDate.ToShortDateString(),
                    TransportMode = _invoice.TransportMode,
                    VehicleNumber = _invoice.VehicleNumber,
                    Quantity = _invoice.Quantity.ToString("f2"),
                    Discount = _invoice.Discount.ToString("f2"),
                    Taxable = taxable.ToString("f2"),
                    CGST = _invoice.CGST.ToString("f2"),
                    SGST = _invoice.SGST.ToString("f2"),
                    IGST = _invoice.IGST.ToString("f2"),
                    Taxable1 = taxable.ToString("c"),
                    TotalTaxes = _invoice.TotalTaxes.ToString("c"),
                    LoadingCharges = _invoice.LoadingCharges.ToString("c"),
                    OtherCharges = _invoice.OtherCharges.ToString("c"),
                    TotalAmount = _invoice.Amount.ToString("c"),
                    AmountInWords = Convert.ToInt32(_invoice.Amount).ConvertToWords(),
                    Remarks = _invoice.Remarks.Replace(Environment.NewLine, "<br/>")
                };

                var company = _prepo.GetByPartyType(string.Empty, PartyType.Self).FirstOrDefault();
                if (company == null) company = new Party();
                var billing = _prepo.GetById(_invoice.BillingId);
                var shipping = _prepo.GetById(_invoice.ShippingId);
                dynamic rproducts;

                if (_invoice.InvoiceType == InvoiceType.Normal)
                {
                    rproducts = new List<InvoiceProductsPrint>();
                    var iprepo = ServiceContainer.GetInstance<InvoiceProductRepository>();
                    var ips = iprepo.GetProductsForInvoice(_invoice.Id).ToList();

                    for (var j = 0; j < ips.Count; j++)
                    {
                        var ip = ips[j];
                        rproducts.Add(new InvoiceProductsPrint
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
                            IGST = ip.IGST.ToString("f2"),
                            Packs = ip.Packs.ToString("n0")
                        });
                    }

                    var rpc = rproducts.Count;
                    var ttp = 8;
                    //if (IsPacksRequired) ttp = 7;
                    if (rpc < ttp)
                    {
                        for (var l = rpc; l <= ttp; l++) rproducts.Add(new InvoiceProductsPrint());
                    }
                }
                else
                {
                    rproducts = new List<InvoiceFabricProductsPrint>();
                    var iprepo = ServiceContainer.GetInstance<InvoiceFabricProductRepository>();
                    var ips = iprepo.GetProductsForInvoice(_invoice.Id).ToList();

                    for (var j = 0; j < ips.Count; j++)
                    {
                        var ip = ips[j];
                        rproducts.Add(new InvoiceFabricProductsPrint
                        {
                            Id = (j + 1).ToString(),
                            Description = ip.ProductDescription,
                            HSN = ip.HSN,
                            Meters = ip.Meters.ToString("f2"),
                            FoldingLoss = ip.FoldingLoss.ToString("f2"),
                            FoldingLossRate = ip.FoldingLossRate.ToString("f2"),
                            MetersAfterFoldingLoss = (ip.Meters - ip.FoldingLoss).ToString("f2"),
                            Bales = ip.Bales.ToString(),
                            Pieces = ip.Pieces.ToString(),
                            Rate = ip.Rate.ToString("f2"),
                            Amount = ip.Amount.ToString("f2"),
                            CGSTRate = ip.CGSTRate.ToString("f2"),
                            CGST = ip.CGST.ToString("f2"),
                            SGSTRate = ip.SGSTRate.ToString("f2"),
                            SGST = ip.SGST.ToString("f2"),
                            IGSTRate = ip.IGSTRate.ToString("f2"),
                            IGST = ip.IGST.ToString("f2")
                        });
                    }

                    var rpc = rproducts.Count;
                    var ttp = 8;
                    if (IsPacksRequired) ttp = 7;
                    if (rpc < ttp)
                    {
                        for (var l = rpc; l <= ttp; l++) rproducts.Add(new InvoiceFabricProductsPrint());
                    }
                }

                var oprepo = ServiceContainer.GetInstance<OptionRepository>();
                var option = oprepo.GetAll().FirstOrDefault();
                if (option == null) option = new Option();
                dynamic bank = new
                {
                    BankAccName = option.BankAccName,
                    BankAccNo = option.BankAccNo,
                    BankBranch = option.BankBranch,
                    BankIFSC = option.BankIFSC
                };

                using (var pdfForm = new ReportViewer())
                {
                    using (var pdf = new ReportDocument())
                    {
                        pdf.PageSize = PageSizes.A4;
                        pdf.PageOrientation = PageOrientations.Portrait;
                        if (_invoice.InvoiceType == InvoiceType.Normal)
                        {
                            pdf.Margins = 0.25f;
                            var report = (IsPacksRequired) ? "reports/invoice-1.htm" : "reports/invoice.htm";
                            if (allPages)
                            {
                                pdf.AddPage(report, new { Page = "(ORIGINAL)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                                pdf.AddPage(report, new { Page = "(DUPLICATE)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                                pdf.AddPage(report, new { Page = "(TRIPLICATE)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                            }
                            else
                            {
                                pdf.AddPage(report, new { Page = "", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                            }
                        }
                        else
                        {
                            pdf.Margins = 0.25f;
                            var report = "reports/fabric-invoice.htm";
                            if (allPages)
                            {
                                pdf.AddPage(report, new { Page = "(ORIGINAL)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                                pdf.AddPage(report, new { Page = "(DUPLICATE)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                                pdf.AddPage(report, new { Page = "(TRIPLICATE)", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                            }
                            else
                            {
                                pdf.AddPage(report, new { Page = "", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts, bank = bank });
                            }
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

            Cursor = Cursors.Default;
        }

        private void printSinglePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintInvoice(false);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            FindInvoices(textEdit1.Text); 
        }

        private void newFabricInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.FabricInvoiceDetails())
            {
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textEdit1.Text = "";
                    FindInvoices();
                }
            }
        }

        private void deleteInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            if (!Helpers.Confirm("Are you sure about deleting this invoice?")) return;
            try
            {
                _irepo.Delete(_invoices[dataGridView1.SelectedRows[0].Index].Id);
                Helpers.ShowSuccess("Invoice deleted successfully");
                FindInvoices(textEdit1.Text);
            }
            catch (MySqlException)
            {
                Helpers.ShowError("Cannot delete invoice. Make sure you have no other transactions related to this entry");
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }

    internal class InvoiceProductsPrint
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
        public string Packs { get; set; }

        public InvoiceProductsPrint()
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
            Packs = "&nbsp;";
        }
    }

    internal class InvoiceFabricProductsPrint
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Bales { get; set; }
        public string Pieces { get; set; }
        public string Meters { get; set; }
        public string FoldingLoss { get; set; }
        public string FoldingLossRate { get; set; }
        public string MetersAfterFoldingLoss { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string CGSTRate { get; set; }
        public string CGST { get; set; }
        public string SGSTRate { get; set; }
        public string SGST { get; set; }
        public string IGSTRate { get; set; }
        public string IGST { get; set; }

        public InvoiceFabricProductsPrint()
        {
            Id = "&nbsp;";
            Description = "&nbsp;";
            HSN = "&nbsp;";
            Meters = "&nbsp;";
            Bales = "&nbsp;";
            Rate = "&nbsp;";
            FoldingLoss = "&nbsp;";
            FoldingLossRate = "&nbsp;";
            MetersAfterFoldingLoss = "&nbsp;";
            Amount = "&nbsp;";
            CGSTRate = "&nbsp;";
            CGST = "&nbsp;";
            SGSTRate = "&nbsp;";
            SGST = "&nbsp;";
            IGSTRate = "&nbsp;";
            IGST = "&nbsp;";
            Pieces = "&nbsp;";
        }
    }
}
