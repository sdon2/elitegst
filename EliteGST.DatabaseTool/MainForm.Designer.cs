namespace EliteGST.DatabaseTool
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDropDatabase = new System.Windows.Forms.Button();
            this.btnCreateDatabase = new System.Windows.Forms.Button();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSqlExecute = new System.Windows.Forms.Button();
            this.txtSqlText = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtDatabase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDropDatabase);
            this.groupBox1.Controls.Add(this.btnCreateDatabase);
            this.groupBox1.Controls.Add(this.txtHost);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Commands";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Database:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(155, 38);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(130, 20);
            this.txtDatabase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Host:";
            // 
            // btnDropDatabase
            // 
            this.btnDropDatabase.Location = new System.Drawing.Point(422, 36);
            this.btnDropDatabase.Name = "btnDropDatabase";
            this.btnDropDatabase.Size = new System.Drawing.Size(118, 23);
            this.btnDropDatabase.TabIndex = 3;
            this.btnDropDatabase.Text = "Drop";
            this.btnDropDatabase.UseVisualStyleBackColor = true;
            this.btnDropDatabase.Click += new System.EventHandler(this.btnDropDatabase_Click);
            // 
            // btnCreateDatabase
            // 
            this.btnCreateDatabase.Location = new System.Drawing.Point(295, 36);
            this.btnCreateDatabase.Name = "btnCreateDatabase";
            this.btnCreateDatabase.Size = new System.Drawing.Size(118, 23);
            this.btnCreateDatabase.TabIndex = 2;
            this.btnCreateDatabase.Text = "Create";
            this.btnCreateDatabase.UseVisualStyleBackColor = true;
            this.btnCreateDatabase.Click += new System.EventHandler(this.btnCreateDatabase_Click);
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(15, 38);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(130, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.Text = "localhost";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSqlExecute);
            this.groupBox2.Controls.Add(this.txtSqlText);
            this.groupBox2.Location = new System.Drawing.Point(12, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 357);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sql Command Palette";
            // 
            // btnSqlExecute
            // 
            this.btnSqlExecute.Location = new System.Drawing.Point(15, 321);
            this.btnSqlExecute.Name = "btnSqlExecute";
            this.btnSqlExecute.Size = new System.Drawing.Size(147, 23);
            this.btnSqlExecute.TabIndex = 5;
            this.btnSqlExecute.Text = "Execute";
            this.btnSqlExecute.UseVisualStyleBackColor = true;
            this.btnSqlExecute.Click += new System.EventHandler(this.btnSqlExecute_Click);
            // 
            // txtSqlText
            // 
            this.txtSqlText.Location = new System.Drawing.Point(15, 19);
            this.txtSqlText.Multiline = true;
            this.txtSqlText.Name = "txtSqlText";
            this.txtSqlText.Size = new System.Drawing.Size(525, 293);
            this.txtSqlText.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 469);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EliteGST Database Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDropDatabase;
        private System.Windows.Forms.Button btnCreateDatabase;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSqlExecute;
        private System.Windows.Forms.TextBox txtSqlText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDatabase;
    }
}

