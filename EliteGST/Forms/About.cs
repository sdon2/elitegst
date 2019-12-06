using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace EliteGST.Forms
{
    public partial class About : DevExpress.XtraEditors.XtraForm
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            labelControl2.Text = "Warning: Please do not distribute this computer program without permission of SVP Infotech. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted under the maximum extent possible under law.";
        }
    }
}