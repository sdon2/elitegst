namespace EliteGST.Forms
{
    partial class PaymentDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtAmount = new DevExpress.XtraEditors.TextEdit();
            this.txtRemarks = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.paymentDate = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.paymentDate);
            this.layoutControl1.Controls.Add(this.comboBox1);
            this.layoutControl1.Controls.Add(this.simpleButton3);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Controls.Add(this.txtAmount);
            this.layoutControl1.Controls.Add(this.txtRemarks);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(351, 200);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(61, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(278, 21);
            this.comboBox1.TabIndex = 17;
            // 
            // simpleButton3
            // 
            this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton3.Location = new System.Drawing.Point(174, 156);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(165, 32);
            this.simpleButton3.StyleController = this.layoutControl1;
            this.simpleButton3.TabIndex = 15;
            this.simpleButton3.Text = "&Close";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 156);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(158, 32);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "&Save";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(61, 61);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Properties.Mask.EditMask = "n";
            this.txtAmount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAmount.Size = new System.Drawing.Size(109, 20);
            this.txtAmount.StyleController = this.layoutControl1;
            this.txtAmount.TabIndex = 5;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(63, 101);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(276, 51);
            this.txtRemarks.StyleController = this.layoutControl1;
            this.txtRemarks.TabIndex = 6;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(351, 200);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.Control = this.txtAmount;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 49);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(162, 24);
            this.layoutControlItem2.Text = "Amount";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(46, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtRemarks;
            this.layoutControlItem3.Location = new System.Drawing.Point(51, 73);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(63, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(280, 71);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "Remarks";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(46, 13);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.simpleButton2;
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItem11.MinSize = new System.Drawing.Size(37, 26);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(162, 36);
            this.layoutControlItem11.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.simpleButton3;
            this.layoutControlItem12.Location = new System.Drawing.Point(162, 144);
            this.layoutControlItem12.MinSize = new System.Drawing.Size(39, 26);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(169, 36);
            this.layoutControlItem12.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem4.Control = this.comboBox1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(331, 25);
            this.layoutControlItem4.Text = "Customer";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(46, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 73);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(51, 71);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(162, 25);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(169, 48);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // paymentDate
            // 
            this.paymentDate.EditValue = null;
            this.paymentDate.Location = new System.Drawing.Point(61, 37);
            this.paymentDate.Name = "paymentDate";
            this.paymentDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.paymentDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.paymentDate.Size = new System.Drawing.Size(109, 20);
            this.paymentDate.StyleController = this.layoutControl1;
            this.paymentDate.TabIndex = 18;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.paymentDate;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(162, 24);
            this.layoutControlItem1.Text = "Date";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(46, 13);
            // 
            // PaymentDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.simpleButton3;
            this.ClientSize = new System.Drawing.Size(351, 200);
            this.Controls.Add(this.layoutControl1);
            this.Name = "PaymentDetails";
            this.Text = "Customer Details";
            this.Load += new System.EventHandler(this.CompanyInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit txtAmount;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private System.Windows.Forms.ComboBox comboBox1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.MemoEdit txtRemarks;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.DateEdit paymentDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}