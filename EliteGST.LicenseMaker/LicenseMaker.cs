using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace EliteGST.LicenseMaker
{
    public partial class LicenseMaker : Form
    {
        public LicenseMaker()
        {
            InitializeComponent();
        }

        private void LicenseMaker_Load(object sender, EventArgs e)
        {
            textBox1.Text = EliteLicenseMakerBase.LicenseMaker.GetLicense("{D54A01CA-0224-4E51-87C1-C0F35B2429E9}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
