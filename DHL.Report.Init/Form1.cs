using System;
using System.Windows.Forms;

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
        }
    }
}
