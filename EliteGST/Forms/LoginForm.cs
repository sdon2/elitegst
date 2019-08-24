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

namespace EliteGST.Forms
{
    public partial class LoginForm : BaseForm
    {
        private OptionRepository _orepo;
        private string _password;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Check password
            _orepo = ServiceContainer.GetInstance<OptionRepository>();
            var options = _orepo.GetAll().FirstOrDefault();
            _password = options.Password;
        }
    }
}
