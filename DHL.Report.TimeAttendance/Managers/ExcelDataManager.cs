using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ExcelDataManager : IExcelDataManager
    {
        public IEnumerable<EmployeeInfoModel> GetHrSource(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Excel file path");
            }

            var extension = Path.GetExtension(filePath).ToLower();
            var supportedExtension = new[] { ".xls", ".xlsx" };
            if (!supportedExtension.Contains(extension))
            {
                throw new ArgumentException("Support only excel file.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Cannot found the Excel file: " + filePath);
            }

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = extension == ".xls" ?
                    ExcelReaderFactory.CreateBinaryReader(stream) :
                    ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    reader.IsFirstRowAsColumnNames = true;
                    var result = reader.AsDataSet();

                    foreach (DataTable dt in result.Tables)
                    {
                        if (!dt.Columns.Contains("id_card"))
                        {
                            continue;
                        }

                        var src = dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(ToString(x, "id_card")));
                        var employees = src.Select(x => new EmployeeInfoModel
                        {
                            DataDate = ToDateTime(x, "str_date") ?? DateTime.Today,
                            EmployeeId = ToString(x, "id_card"),
                            ShiftCode = ToString(x, "shift"),
                            Name = ToString(x, "fs_name"),
                            Company = ToString(x, "company"),
                            Department = ToString(x, "descript"),
                        });

                        foreach (var e in employees)
                        {
                            yield return e;
                        }
                    }
                }
            }
        }

        private string ToString(DataRow dr, string columnName)
        {
            return dr[columnName] == DBNull.Value ? string.Empty : dr.Field<string>(columnName);
        }

        private DateTime? ToDateTime(DataRow dr, string columnName)
        {
            return dr[columnName] == DBNull.Value ? null : dr.Field<DateTime?>(columnName);
        }
    }
}
