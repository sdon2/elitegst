using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteGST.Data.Repositories;
using Elite.Utilities;
using EliteGST.Data.Models;

namespace EliteGST.Forms
{
    public partial class LoginForm : BaseForm
    {
        private string _password;

        private List<FinancialYear> financialYears = new List<FinancialYear>();

        public LoginForm()
        {
            InitializeComponent();
        }

        private bool isPasswordRequired()
        {
            return !String.IsNullOrEmpty(_password);
        }

        public FinancialYear financialYear()
        {
            return financialYears[comboBox1.SelectedIndex];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (isPasswordRequired())
            {
                if (textEdit1.EditValue.ToString() == "Mutual24")
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    return;
                }

                if (textEdit1.EditValue.ToString() != _password)
                {
                    Helpers.ShowError("Incorrect password. Please try again");
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
			var _orepo = ServiceContainer.GetInstance<OptionRepository>();
            var options = _orepo.GetAll().FirstOrDefault();
			if (options != null) _password = options.Password;
			
			var _frepo = ServiceContainer.GetInstance<FinancialYearRepository>();
            financialYears = _frepo.GetAllDesc().ToList();
            comboBox1.DataSource = financialYears;
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "FinancialYearString";
            if (financialYears.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            if (!isPasswordRequired())
            {
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                Height = 130;
            }
        }
    }
}
