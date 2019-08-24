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
using System.Text.RegularExpressions;

namespace EliteGST.Forms
{
    public partial class SetPasswordForm : BaseForm
    {
        private OptionRepository _orepo;
        private Option _options;

        public SetPasswordForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var password = textEdit1.EditValue.ToString();
                var cpassword = textEdit2.EditValue.ToString();

                if (_options == null) _options = new Option();

                if (String.IsNullOrEmpty(password) && String.IsNullOrEmpty(cpassword))
                {
                    if (String.IsNullOrEmpty(_options.Password))
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        return;
                    }
                    else
                    {
                        _options.Password = String.Empty;
                    }
                }
                else
                {
                    if (password.Length < 4)
                        throw new Exception("Password must be atleast 4 characters");

                    var regex = new Regex("^([0-9]|[a-zA-Z])+.*$");
                    if (!regex.IsMatch(password))
                    {
                        throw new Exception("Invalid password. Please follow the password guidelines below");
                    }


                    if (password != cpassword)
                        throw new Exception("Password & Confirm Password must be same");

                    _options.Password = password;
                }

                if (_options.Id == 0)
                {
                    _orepo.Insert(_options);
                }
                else
                {
                    _orepo.Update(_options, _options.Id);
                }

                Helpers.ShowSuccess("Password updated successfully. Please restart application");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ShowError(ex.Message);
            }
        }

        private void SetPasswordForm_Load(object sender, EventArgs e)
        {
            _orepo = ServiceContainer.GetInstance<OptionRepository>();
            _options = _orepo.GetAll().FirstOrDefault();
        }
    }
}
