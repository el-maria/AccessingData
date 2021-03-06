﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.IO;

namespace AccessingData
{
    public partial class WorkWithData : Form
    {
        public WorkWithData()
        {
            InitializeComponent();
        }

        private void PrepareDemo(bool ShowGrid)
        {
            PrepareDemo(ShowGrid, pgeListBox);
        }

        private void PrepareDemo(bool ShowGrid, TabPage SelectedPage)
        {
            if (demoList.DataSource == null)
            {
                demoList.Items.Clear();
            }
            else
            {
                demoList.DataSource = null;
                demoList.DisplayMember = "";
            }

            demoGrid.Visible = ShowGrid;
            tabDemo.SelectedTab = SelectedPage;
        }


        private void sqlDataReaderButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(false);
            DataReaderFromOleDB();
        }
        private void DataReaderFromOleDB()
        {
            string strSQL = "SELECT * FROM Customers";

            try
            {

                using (OleDbConnection cnn = new OleDbConnection(Properties.Settings.Default.OleDbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(strSQL, cnn))
                    {
                        cnn.Open();

                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            // Loop through all the rows, retrieving the 
                            // columns you need. Also look into the GetString
                            // method (and other Get... methods) for a faster 
                            // way to retrieve individual columns.
                           
                            while (dr.Read())
                            {
                                demoList.Items.Add(string.Format("Customer: {0}, Company name: {1}, Contact name: {2}, Contact title: {3} ", dr["CustomerID"], dr["CompanyName"], dr["ContactName"], dr["ContactTitle"] ));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void oleDbDataSetButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true);
            DataSetFromOleDb(); 
        }
        private void DataSetFromOleDb()
        {
            string strSQL = "SELECT * FROM Products WHERE CategoryID=1";

            //new for 2
            string strSQL1 = "SELECT ProductName,SupplierID FROM Products WHERE CategoryID=1";

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, Properties.Settings.Default.OleDbConnectionString))
                {

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "ProductInfo");

                    //new for 2
                    OleDbDataAdapter adapter1 = new OleDbDataAdapter(strSQL1, Properties.Settings.Default.OleDbConnectionString);
                    adapter1.Fill(ds,"product");

                    demoList.DataSource = ds.Tables["ProductInfo"];
                    demoList.DisplayMember = "ProductID";

                    // Εναλλάκτικά θα μπορούσαμε να γεμίσουμε το Listbox
                    // παίρνοντας μία μία τις γραμμές του πίνακα και προσθέτοντάς τες

                    //For Each dr As DataRow In _
                    // ds.Tables("ProductInfo").Rows
                    //    demoList.Items.Add(dr("ProductName").ToString)
                    //Next dr

                    //demoGrid.DataSource = ds.Tables["ProductInfo"];
                    
                    //new for 2
                    demoGrid.DataSource = ds.Tables["product"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //erwtima 3
        private void sqlDataSetButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true);
            DataTableFromSQL();
        }
        private void DataTableFromSQL()
        {
            string strSQL = "SELECT * FROM Products WHERE CategoryID = 1";

            //new for 3
            string strSQL1 = "SELECT * FROM Employees WHERE City = 'London'";
            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, Properties.Settings.Default.OleDbConnectionString))
                {
                    DataTable dt1 = new DataTable();
                    adapter.Fill(dt1);

                    //new for 3
                    OleDbDataAdapter adapter1 = new OleDbDataAdapter(strSQL1, Properties.Settings.Default.OleDbConnectionString);
                    DataTable dt2 = new DataTable();
                    adapter1.Fill(dt2); 
                   

                    foreach (DataColumn dc in dt1.Columns)
                    {
                        demoList.Items.Add(string.Format("{0} ({1})", dc.ColumnName, dc.DataType));
                    }

                    //new for 3
                    foreach (DataColumn dc in dt2.Columns)
                    {
                        demoList.Items.Add(string.Format("{0} ({1})", dc.ColumnName, dc.AllowDBNull));
                    }

                    //new for 3
                    demoGrid.DataSource = dt2;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void dataTableButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true, pgeGrid);
            CreateDataTable();
        }
        private void CreateDataTable()
        {
            // Create a DataTable filled with information
            // about files in the current folder.
            // 
            // Note the use of the FileInfo and 
            // DirectoryInfo objects, provided by the 
            // .NET framework, in the System.IO namespace.

            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(System.String));
            dt.Columns.Add("Size", typeof(System.Int64));
            //new for 4
            dt.Columns.Add("ReadOnly File", typeof(System.String));

            DataRow dr = default(DataRow);
            DirectoryInfo dir = new DirectoryInfo("C:\\");
            foreach (FileInfo fi in dir.GetFiles())
            {
                dr = dt.NewRow();
                dr[0] = fi.Name;
                dr[1] = fi.Length;
                //new for 4
                dr[2] = fi.IsReadOnly;
                dt.Rows.Add(dr);
            }

            // Bind the DataGridView to this DataTable.
            demoGrid.DataSource = dt;
        }

        //new for 5 in excersise 2
        private void WorkWithData_Load(object sender, EventArgs e)
        {
            PrepareDemo(true);
            LoadFromSQL();
        }
        private void LoadFromSQL()
        {
            string strSQL = "SELECT EmployeeID, LastName FROM Employees";

            try
            {
               
                    using (OleDbConnection cnn = new OleDbConnection(Properties.Settings.Default.OleDbConnectionString))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(strSQL, cnn))
                        {
                            cnn.Open();

                            using (OleDbDataReader dr = cmd.ExecuteReader())
                            {
                            DataTable dt = new DataTable();
                            // dt.Columns.Add("EmployeeID", typeof(int));
                            dt.Columns.Add("LastName", typeof(string));
                            //dt.Columns.Add("FirstName", typeof(string));

                            
                            dt.Load(dr);
                           comboBox1.ValueMember = "EmployeeID";
                            comboBox1.DisplayMember = "LastName";
                            comboBox1.DataSource = dt;
                           
                            }
                        }
                    }
             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = comboBox1.SelectedValue.ToString();

            //new for 6
            //string strSQL1 = "SELECT * FROM Orders";
            //if (comboBox1.SelectedValue == )

        }
    }
}
