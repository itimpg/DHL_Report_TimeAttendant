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
        public IEnumerable<EmployeeResultModel> GetHrSource(string filePath)
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
                        var src = dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(ToString(x, "id_card")));

                        yield return new EmployeeResultModel
                        {
                            DataDate = src
                                .GroupBy(x => ToDateTime(x, "str_date"))
                                .Select(g => g.Key)
                                .FirstOrDefault(x => x.HasValue),
                            Employees = src.Select(x => new EmployeeInfoModel
                            {
                                Id = ToString(x, "id_card"),
                                Name = ToString(x, "fs_name"),
                                Company = ToString(x, "company"),
                                Department = ToString(x, "descript"),
                            }).ToList()
                        };
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
