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
using MySql.Data.MySqlClient;

namespace EliteGST.Forms
{
    public partial class ProductList : BaseForm
    {
        public Product Product;
        public bool SelectMode;
        private BindingList<Product> _products;
        private ProductRepository _prepo = ServiceContainer.GetInstance<ProductRepository>();

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
                case Keys.Delete:
                    if (dataGridView1.SelectedRows.Count == 1) deleteToolStripMenuItem.PerformClick();
                    return true;
                case Keys.F2:
                    if (dataGridView1.SelectedRows.Count == 1) editToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.N:
                    newToolStripMenuItem.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public ProductList()
        {
            InitializeComponent();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            _products = new BindingList<Product>();
            dataGridView1.DataSource = _products;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            FindProducts();
        }

        private void FindProducts(string description = "")
        {
            try
            {
                _products.Clear();
                var px = _prepo.GetAll().Where(p => p.ProductDescription.Contains(description));
                if (SelectMode) px = px.Where(p1 => p1.IsActive == true);
                px.ToList().ForEach(pi => _products.Add(pi));
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            FindProducts(textEdit1.Text);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            textEdit1.Text = "";
            FindProducts();
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
            deleteToolStripMenuItem.Visible = singleRowSelected;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.ProductDetails())
            {
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textEdit1.Text = "";
                    FindProducts();
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.ProductDetails())
            {
                var Id = _products[dataGridView1.SelectedRows[0].Index].Id;
                pf.Id = Id;
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FindProducts(textEdit1.Text);
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
                Product = _products[dataGridView1.SelectedRows[0].Index];
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                editToolStripMenuItem.PerformClick();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            if (!Helpers.Confirm("Are you sure about deleting this product?")) return;
            try
            {
                _prepo.Delete(_products[dataGridView1.SelectedRows[0].Index].Id);
                Helpers.ShowSuccess("Product deleted successfully");
                FindProducts(textEdit1.Text);
            }
            catch (MySqlException)
            {
                Helpers.ShowError("Cannot delete product. Make sure you have no other transactions related to this product");
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
