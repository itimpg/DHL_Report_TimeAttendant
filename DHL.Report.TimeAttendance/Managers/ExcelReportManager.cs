using System;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.IO;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ExcelReportManager : IExcelReportManager
    {
        public void CreateReport(string dirPath, object reportObjects)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                throw new ArgumentNullException("Output directory path.");
            }

            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath + " dose not exists.");
            }


        }
    }
}
