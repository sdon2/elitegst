private void printToolStripMenuItem_Click(object sender, EventArgs e)
{
	Cursor = Cursors.WaitCursor;

	try
	{
		using (var ctx = new Context())
		{
			var _invoice = _invoices[dataGridView1.SelectedRows[0].Index];
			var invoice = ctx.Invoices.Find(_invoice.Id);
			var taxable = invoice.InvoiceProducts.Aggregate(new Decimal(), (c, n) => { return c += n.TaxableValue; });
			var i = new
			{
				Id = invoice.InvoiceId,
				Date = invoice.InvoiceDate.ToShortDateString(),
				TransportMode = invoice.TransportMode,
				VehicleNumber = invoice.VehicleNumber,
				Quantity = invoice.InvoiceProducts.Aggregate(new Decimal(), (c, n) => { return c += n.Quantity; }).ToString("n0"),
				Discount = invoice.InvoiceProducts.Aggregate(new Decimal(), (c, n) => { return c += n.Discount; }).ToString("n0"),
				Taxable = taxable.ToString("n0"),
				CGST = _invoice.CGST.ToString("n0"),
				SGST = _invoice.SGST.ToString("n0"),
				IGST = _invoice.IGST.ToString("n0"),
				Taxable1 = taxable.ToString("c"),
				TotalTaxes = _invoice.TotalTaxes.ToString("c"),
				LoadingCharges = invoice.LoadingCharges.ToString("c"),
				OtherCharges = invoice.OtherCharges.ToString("c"),
				TotalAmount = _invoice.Amount.ToString("c"),
				AmountInWords = Convert.ToInt32(_invoice.Amount).ConvertToWords(),
				Remarks = invoice.Remarks.Replace(Environment.NewLine, "<br/>")
			};

			var company = ctx.Companies.Find(1);
			var billing = ctx.Customers.Find(invoice.BillingId);
			var shipping = ctx.Customers.Find(invoice.ShippingId);

			var rproducts = new List<InvoiceProductsPrint>();
			var ips = invoice.InvoiceProducts.ToList();
			for (var j = 0; j < ips.Count; j++)
			{
				var ip = ips[j];
				rproducts.Add(new InvoiceProductsPrint
				{
					Id = (j + 1).ToString(),
					Description = ip.Product.ProductDescription,
					HSN = ip.Product.HSN,
					Qty = ip.Quantity.ToString("n0"),
					UOM = ip.Product.UOM,
					Rate = ip.Rate.ToString("n0"),
					Discount = ip.Discount.ToString("n0"),
					Taxable = ip.TaxableValue.ToString("n0"),
					CGSTRate = ip.CGSTRate.ToString("n0"),
					CGST = ip.CGST.ToString("n0"),
					SGSTRate = ip.SGSTRate.ToString("n0"),
					SGST = ip.SGST.ToString("n0"),
					IGSTRate = ip.IGSTRate.ToString("n0"),
					IGST = ip.IGST.ToString("n0")
				});
			}

			var rpc = rproducts.Count;
			if (rpc < 9)
			{
				for (var l = rpc; l <= 9; l++) rproducts.Add(new InvoiceProductsPrint());
			}

			using (var pdfForm = new Forms.ReportViewer())
			{
				using (var pdf = new ReportDocument())
				{
					pdf.PageSize = PageSizes.A4;
					pdf.PageOrientation = PageOrientations.Portrait;
					pdf.Margins = 0.25f;
					pdf.AddPage("reports/invoice.htm", new { Page = "ORIGINAL", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts });
					pdf.AddPage("reports/invoice.htm", new { Page = "DUPLICATE", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts });
					pdf.AddPage("reports/invoice.htm", new { Page = "TRIPLICATE", DIR = Application.StartupPath, company = company, invoice = i, billing = billing, shipping = shipping, products = rproducts });
					pdfForm.ReportDocument = pdf;
					pdfForm.ShowDialog();
				}
			}
		}
	}
	catch (Exception ex)
	{
		Utilities.ShowError(ex.Message);
	}

	Cursor = Cursors.Default;
}
