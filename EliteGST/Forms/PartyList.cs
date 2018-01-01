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
    public partial class PartyList : BaseForm
    {
        public Party Party { get; set; }
        public PartyType PartyType { get; set; }
        public bool SelectMode { get; set; }

        private BindingList<Party> _parties;
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

        public PartyList()
        {
            InitializeComponent();
        }

        private void ProductList_Load(object sender, EventArgs e)
        {
            Text = PartyType == (Data.PartyType.Customer) ? "Customer's List" : "Supplier's List";

            _parties = new BindingList<Party>();
            dataGridView1.DataSource = _parties;
            var cols = new List<int> { 0, 1, 6, 10 };
            cols.ForEach(i => dataGridView1.Columns[i].Visible = false);
            FindParties();
        }

        private void FindParties(string name = "")
        {
            try
            {
                _parties.Clear();
                var px = _prepo.GetByPartyType(name, PartyType);
                if (SelectMode) px = px.Where(p1 => p1.IsActive == true);
                px.ToList().ForEach(pi => _parties.Add(pi));
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            FindParties(textEdit1.Text);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            textEdit1.Text = "";
            FindParties();
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
            using (var pf = new Forms.PartyDetails())
            {
                pf.PartyType = PartyType;
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FindParties(textEdit1.Text);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var pf = new Forms.PartyDetails())
            {
                var Id = _parties[dataGridView1.SelectedRows[0].Index].Id;
                pf.Id = Id;
                if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FindParties(textEdit1.Text);
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
                Party = _parties[dataGridView1.SelectedRows[0].Index];
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
            if (!Helpers.Confirm("Are you sure about deleting this party?")) return;
            try
            {
                _prepo.Delete(_parties[dataGridView1.SelectedRows[0].Index].Id);
                Helpers.ShowSuccess("Party deleted successfully");
                FindParties(textEdit1.Text);
            }
            catch (MySqlException)
            {
                Helpers.ShowError("Cannot delete party. Make sure you have no other transactions related to this party");
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }
    }
}
