using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace EliteGST.DatabaseTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCreateDatabase_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = null;
            try
            {
                using (connection = Database.getConnection(txtHost.Text))
                {
                    var sql = string.Format("CREATE DATABASE IF NOT EXISTS `{0}` DEFAULT CHARACTER SET = utf8 DEFAULT COLLATE = utf8_general_ci", txtDatabase.Text);
                    var command = new MySqlCommand(sql, connection);
                    var result = command.ExecuteNonQuery();
                    MessageBox.Show("Database created successfully\nResult: " + result, "Alert");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                }
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDropDatabase_Click(object sender, EventArgs e)
        {
            var message = string.Format("Are you sure want to drop the database '{0}'?\nImportant: This cannot be undone!!!", txtDatabase.Text);
            if (MessageBox.Show(message, "Confirm Dropping Database", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No) return;

            MySqlConnection connection = null;
            try
            {
                using (connection = Database.getConnection(txtHost.Text))
                {
                    var sql = string.Format("DROP DATABASE IF EXISTS `{0}`", txtDatabase.Text);
                    var command = new MySqlCommand(sql, connection);
                    var result = command.ExecuteNonQuery();
                    MessageBox.Show("Database dropped successfully\nResult: " + result, "Alert");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                }
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSqlExecute_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = null;
            try
            {
                using (connection = Database.getConnection(txtHost.Text))
                {
                    var sql = string.Format("USE `{0}`", txtDatabase.Text);
                    var command = new MySqlCommand(sql, connection);
                    var result = command.ExecuteNonQuery();
                    if (!string.IsNullOrWhiteSpace(txtSqlText.Text))
                    {
                        command.CommandText = txtSqlText.Text;
                        result = command.ExecuteNonQuery();
                    }
                    MessageBox.Show("SQL executed successfully\nResult: " + result, "Alert");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                }
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string sqlFile = Application.StartupPath + "\\db.sql";
            if (File.Exists(sqlFile))
            {
                txtSqlText.Text = File.ReadAllText(sqlFile);
            }
        }
    }
}

