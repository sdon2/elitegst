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
        private OptionRepository _orepo;
        private FinancialYearRepository _frepo;
        private string _password;

        private List<FinancialYear> financialYears = new List<FinancialYear>();

        public LoginForm()
        {
            InitializeComponent();
            _orepo = ServiceContainer.GetInstance<OptionRepository>();
        }

        private bool isPasswordRequired()
        {            
            var options = _orepo.GetAll().FirstOrDefault();
            return options != null && !String.IsNullOrEmpty(options.Password);
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
            _frepo = ServiceContainer.GetInstance<FinancialYearRepository>();
            var options = _orepo.GetAll().FirstOrDefault();
            _password = options.Password;
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
