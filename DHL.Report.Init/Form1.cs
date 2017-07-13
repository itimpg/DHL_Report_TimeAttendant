﻿using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Data.OleDb;

namespace DHL.Report.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load += (s, e) =>
            {
#if DEBUG
                txtAccess.Text = @"‪D:\DHL\iCCard3000.mdb";
                txtPassword.Text = "168168";
                txtExcel.Text = @"D:\DHL\attendance_tops.xlsx";
#endif
            };
        }

        private void btnBroseAccess_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Access Files|*.mdb;*.accdb",
            };
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtAccess.Text = dlg.FileName;
            }
        }

        private void btnBrowseExcel_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel Files|*.xls;*.xlsx",
            };
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtExcel.Text = dlg.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExcel.Text) || string.IsNullOrEmpty(txtAccess.Text))
            {
                MessageBox.Show("กรุณาเลือกไฟล์");
                return;
            }

            string accessFile = txtAccess.Text;
            string accessPassword = txtPassword.Text;
            string excelFile = txtExcel.Text;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                List<ExcelItem> excelItems;
                // read excel
                var extension = Path.GetExtension(excelFile).ToLower();
                using (var stream = File.Open(excelFile, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = extension == ".xls" ?
                        ExcelReaderFactory.CreateBinaryReader(stream) :
                        ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        reader.IsFirstRowAsColumnNames = true;
                        var dt = reader.AsDataSet().Tables[0];

                        // todo: specific column name
                        string employeeNoColumn = "EmpNo";
                        string scanIdColumn = "ScanId";
                        if (!dt.Columns.Contains(employeeNoColumn) && !dt.Columns.Contains(scanIdColumn))
                        {
                            MessageBox.Show("Excel ผิดฟอร์แมท");
                        }

                        excelItems = dt.AsEnumerable().Select(x => new ExcelItem
                        {
                            EmployeeNo = x.Field<string>(employeeNoColumn),
                            ScanId = x.Field<string>(scanIdColumn)
                        }).ToList();
                    }
                }

                // execute command into access
                string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={accessFile};Persist Security Info=False; Jet OLEDB:Database Password={accessPassword};";
                using (var connection = new OleDbConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var queryString = @"
                        UPDATE t_b_consumer
                        SET f_ConsumerNO = @employee_no
                        WHERE f_ConsumerNO = @scan_id";

                        foreach (var item in excelItems)
                        {
                            using (var command = new OleDbCommand(queryString, connection))
                            {
                                command.Parameters.Add(new OleDbParameter
                                {
                                    ParameterName = "@employee_no",
                                    Value = item.EmployeeNo,
                                });
                                command.Parameters.Add(new OleDbParameter
                                {
                                    ParameterName = "@scan_id",
                                    Value = item.ScanId,
                                });
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }

    public class ExcelItem
    {
        public string EmployeeNo { get; set; }
        public string ScanId { get; set; }
    }
}
