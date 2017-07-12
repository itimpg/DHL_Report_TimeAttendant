using DHL.Report.TimeAttendance.Helpers;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ExcelReportManager : IExcelReportManager
    {
        public void ValidateParameters(string dirPath, EmployeeReportResultModel report)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                throw new ArgumentNullException("Output directory path.");
            }

            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath + " dose not exists.");
            }

            if (report == null)
            {
                throw new ArgumentNullException("report");
            }
        }

        public void CreateDailyReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
            string reportNameTemplate = $"รายงานประจำวัน {report.ReportMonth.ToString("yyyy MMM")}";

            foreach (var company in report.Companies)
            {
                using (var package = new ExcelPackage())
                {
                    var data = company.Employees.GroupBy(x => x.ReportDate)
                        .Select(g => new { Date = g.Key, Employees = g.Select(x => x).OrderBy(x => x.EmployeeId) })
                        .OrderBy(x => x.Date);

                    foreach (var d in data)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(d.Date.Day.ToString("00"));
                        int row = 1;
                        var cells = worksheet.Cells;

                        foreach (var employee in d.Employees)
                        {
                            cells[row, 1].Value = "ID";
                            cells[row, 2].Value = "Name";
                            cells[row, 3].Value = "Department";
                            cells[row, 4].Value = "Shift";
                            cells[row, 5].Value = "ในเวลางาน";
                            cells[row, 5, row, 7].Merge = true;
                            cells[row, 8].Value = "Work";
                            cells[row, 9].Value = "No Work";
                            var work1Style = cells[row, 1, row, 9].Style;
                            work1Style.Fill.PatternType = ExcelFillStyle.Solid;
                            work1Style.Fill.BackgroundColor.SetColor(Color.Black);
                            work1Style.Font.Color.SetColor(Color.White);
                            work1Style.Font.Bold = true;
                            cells[row, 1, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            row++;
                            cells[row, 1].Value = employee.EmployeeId;
                            cells[row, 2].Value = employee.Name;
                            cells[row, 3].Value = employee.Department;
                            cells[row, 4].Value = employee.ShiftName;
                            cells[row, 5].Value = "No";
                            cells[row, 6].Value = "IN";
                            cells[row, 7].Value = "OUT";
                            cells[row, 8].Value = employee.TotalWorkingTime.ToData();
                            cells[row, 9].Value = employee.TotalNotWorkingTime.ToData();
                            cells[row, 1, row, 9].Style.Font.Bold = true;
                            cells[row, 1, row, 4].Style.Font.Color.SetColor(Color.Silver);
                            cells[row, 1, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                            cells[row, 8, row, 9].Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                            cells[row, 1, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            row++;
                            foreach (var work in employee.WorkingSwipes)
                            {
                                cells[row, 1].Value = employee.EmployeeId;
                                cells[row, 2].Value = employee.Name;
                                cells[row, 3].Value = employee.Department;
                                cells[row, 4].Value = $"{employee.ShiftCode} {employee.ShiftName}";
                                cells[row, 5].Value = work.No;
                                cells[row, 6].Value = work.In.ToData();
                                cells[row, 7].Value = work.Out.ToData();
                                cells[row, 8].Value = work.WorkingTime.ToData();
                                cells[row, 9].Value = work.NotWorkingTime.ToData();
                                row++;
                            }
                            cells[row, 1].Value = employee.EmployeeId;
                            cells[row, 2].Value = employee.Name;
                            cells[row, 3].Value = employee.Department;
                            cells[row, 4].Value = employee.ShiftName;
                            cells[row, 5].Value = "OT";
                            cells[row, 5, row, 7].Merge = true;
                            cells[row, 8].Value = "Work";
                            cells[row, 9].Value = "No Work";
                            cells[row, 1, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cells[row, 1, row, 9].Style.Fill.BackgroundColor.SetColor(Color.Black);
                            cells[row, 1, row, 4].Style.Font.Color.SetColor(Color.Black);
                            cells[row, 5, row, 9].Style.Font.Color.SetColor(Color.White);
                            cells[row, 5, row, 9].Style.Font.Bold = true;
                            cells[row, 1, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            row++;
                            cells[row, 1].Value = employee.EmployeeId;
                            cells[row, 2].Value = employee.Name;
                            cells[row, 3].Value = employee.Department;
                            cells[row, 4].Value = employee.ShiftName;
                            cells[row, 5].Value = "No";
                            cells[row, 6].Value = "IN";
                            cells[row, 7].Value = "OUT";
                            cells[row, 8].Value = employee.TotalWorkingTimeOT.ToData();
                            cells[row, 9].Value = employee.TotalNotWorkingTimeOT.ToData();
                            cells[row, 1, row, 9].Style.Font.Bold = true;
                            cells[row, 1, row, 4].Style.Font.Color.SetColor(Color.Silver);
                            cells[row, 1, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                            cells[row, 8, row, 9].Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                            cells[row, 1, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            row++;
                            foreach (var work in employee.OtSwipes)
                            {
                                cells[row, 1].Value = employee.EmployeeId;
                                cells[row, 2].Value = employee.Name;
                                cells[row, 3].Value = employee.Department;
                                cells[row, 4].Value = $"{employee.ShiftCode} {employee.ShiftName}";
                                cells[row, 5].Value = work.No;
                                cells[row, 6].Value = work.In.ToData();
                                cells[row, 7].Value = work.Out.ToData();
                                cells[row, 8].Value = work.WorkingTime.ToData();
                                cells[row, 9].Value = work.NotWorkingTime.ToData();
                                row++;
                            }
                        }
                        var allCell = cells[1, 1, row - 1, 9];
                        allCell.AutoFitColumns();
                        allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        
                        cells[1, 5, row - 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // set employeeId align center
                        cells[1, 1, row - 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    string fileName = $"{reportNameTemplate} ({company.Company}).xlsx";
                    string filePath = Path.Combine(dirPath, fileName);
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        public void CreateMonthlyReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
            string reportNameTemplate = $"รายงานประจำเดือน {report.ReportMonth.ToString("yyyy MMM")}";

            foreach (var company in report.Companies)
            {
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"รายงานประจำเดือน {report.ReportMonth.ToString("MMM yyyy")}");
                    var cells = worksheet.Cells;

                    cells[1, 1].Value = "ID";
                    cells[1, 1, 2, 1].Merge = true;
                    cells[1, 2].Value = "Name";
                    cells[1, 2, 2, 2].Merge = true;
                    cells[1, 3].Value = "Department";
                    cells[1, 3, 2, 3].Merge = true;
                    cells[1, 4].Value = "Shift";
                    cells[1, 4, 2, 4].Merge = true;
                    cells[1, 5].Value = "Shift";
                    cells[1, 5, 1, 9].Merge = true;
                    cells[1, 10].Value = "OT";
                    cells[1, 10, 1, 14].Merge = true;
                    cells[2, 5].Value = "Start";
                    cells[2, 6].Value = "End";
                    cells[2, 7].Value = "IN";
                    cells[2, 8].Value = "OUT";
                    cells[2, 9].Value = "SUM";
                    cells[2, 10].Value = "Start";
                    cells[2, 11].Value = "End";
                    cells[2, 12].Value = "IN";
                    cells[2, 13].Value = "OUT";
                    cells[2, 14].Value = "SUM";
                    cells[1, 1, 2, 14].Style.Font.Bold = true;
                    cells[1, 1, 2, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cells[1, 1, 2, 14].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                    cells[1, 1, 2, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cells[1, 1, 2, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var data = company.Employees
                        .GroupBy(x => new
                        {
                            Day = x.ReportDate,
                            Id = x.EmployeeId,
                            Name = x.Name,
                            Dept = x.Department,
                            Shift = $"{x.ShiftCode} {x.ShiftName}",
                        })
                        .Select(g => new
                        {
                            Day = g.Key.Day,
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            Dept = g.Key.Dept,
                            Shift = g.Key.Shift,
                            StartWork = g.SelectMany(x => x.WorkingSwipes).Min(x => x.In),
                            EndWork = g.SelectMany(x => x.WorkingSwipes).Max(x => x.Out),
                            TotalIn = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalWorkingTime),
                            TotalOut = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalNotWorkingTime),
                            Sum = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalWorkingTime + x.TotalNotWorkingTime),
                            StartWorkOT = g.SelectMany(x => x.OtSwipes).Min(x => x.In),
                            EndWorkOT = g.SelectMany(x => x.OtSwipes).Max(x => x.Out),
                            TotalInOT = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalWorkingTimeOT),
                            TotalOutOT = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalNotWorkingTimeOT),
                            SumOT = g.Aggregate(TimeSpan.Zero, (time, x) => time + x.TotalWorkingTimeOT + x.TotalNotWorkingTimeOT)
                        }).OrderBy(x => x.Id).ThenBy(x => x.Day).ToList();

                    int row = 3;
                    foreach (var emp in data)
                    {
                        cells[row, 1].Value = emp.Id;
                        cells[row, 2].Value = emp.Name;
                        cells[row, 3].Value = emp.Dept;
                        cells[row, 4].Value = emp.Shift;
                        cells[row, 5].Value = emp.StartWork.ToData();
                        cells[row, 6].Value = emp.EndWork.ToData();
                        cells[row, 7].Value = emp.TotalIn.ToData();
                        cells[row, 8].Value = emp.TotalOut.ToData();
                        cells[row, 9].Value = emp.Sum.ToData();
                        cells[row, 10].Value = emp.StartWorkOT.ToData();
                        cells[row, 11].Value = emp.EndWorkOT.ToData();
                        cells[row, 12].Value = emp.TotalInOT.ToData();
                        cells[row, 13].Value = emp.TotalOutOT.ToData();
                        cells[row, 14].Value = emp.SumOT.ToData();

                        row++;
                    }
                    var allCell = cells[1, 1, row - 1, 14];
                    allCell.AutoFitColumns();
                    allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    cells[1, 5, row - 1, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    string fileName = $"{reportNameTemplate} ({company.Company}).xlsx";
                    string filePath = Path.Combine(dirPath, fileName);
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        public void CreateAverageReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
            string reportNameTemplate = $"รายงานค่าเฉลี่ย {report.ReportMonth.ToString("yyyy MMM")}";

            foreach (var company in report.Companies)
            {
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"รายงานค่าเฉลี่ยเดือน {report.ReportMonth.ToString("MMM yyyy")}");
                    var cells = worksheet.Cells;

                    cells[1, 1].Value = company.Company;
                    cells[1, 1, 2, 1].Merge = true;
                    cells[1, 2].Value = "Shift";
                    cells[1, 2, 2, 2].Merge = true;
                    cells[1, 3].Value = "AVG Shift";
                    cells[1, 3, 1, 5].Merge = true;
                    cells[1, 6].Value = "AVG OT";
                    cells[1, 6, 1, 8].Merge = true;
                    cells[2, 3].Value = "Start";
                    cells[2, 4].Value = "End";
                    cells[2, 5].Value = "SUM";
                    cells[2, 6].Value = "Start";
                    cells[2, 7].Value = "End";
                    cells[2, 8].Value = "SUM";
                    cells[1, 1, 2, 8].Style.Font.Bold = true;
                    cells[1, 1, 2, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cells[1, 1, 2, 8].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                    cells[1, 1, 2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cells[1, 1, 2, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var xx = company.Employees
                        .GroupBy(x => new { x.Department, x.ShiftCode, x.ShiftName })
                        .Select(g => new
                        {
                            Dept = g.Key.Department,
                            Shift = $"{g.Key.ShiftCode} {g.Key.ShiftName}",
                            StartWork = g
                                .GroupBy(x => new { x.ReportDate, x.EmployeeId })
                                .Select(x => x.SelectMany(a => a.WorkingSwipes.Select(b => b.In)).Min())
                                .Where(x => x.HasValue)
                                .Select(x => x.Value.TimeOfDay),
                            EndWork = g
                                .GroupBy(x => new { x.ReportDate, x.EmployeeId })
                                .Select(x => x.SelectMany(a => a.WorkingSwipes.Select(b => b.Out)).Max())
                                .Where(x => x.HasValue)
                                .Select(x => x.Value.TimeOfDay),
                            TotalSum = g.Select(x => x.TotalWorkingTime + x.TotalNotWorkingTime).Average(),
                            StartWorkOT = g
                                .GroupBy(x => new { x.ReportDate, x.EmployeeId })
                                .Select(x => x.SelectMany(a => a.OtSwipes.Select(b => b.In)).Min())
                                .Where(x => x.HasValue)
                                .Select(x => x.Value.TimeOfDay),
                            EndWorkOT = g
                                .GroupBy(x => new { x.ReportDate, x.EmployeeId })
                                .Select(x => x.SelectMany(a => a.OtSwipes.Select(b => b.Out)).Max())
                                .Where(x => x.HasValue)
                                .Select(x => x.Value.TimeOfDay),
                            TotalSumOT = g.Select(x => x.TotalWorkingTimeOT + x.TotalNotWorkingTimeOT).Average(),
                        });

                    var gg = xx.ToList();
                    var data = gg
                        .Select(x => new
                        {
                            x.Dept,
                            x.Shift,
                            x.TotalSum,
                            x.TotalSumOT,
                            AvgIn = x.StartWork.Count() > 0 ? x.StartWork.Average() : TimeSpan.Zero,
                            AvgOut = x.EndWork.Count() > 0 ? x.EndWork.Average() : TimeSpan.Zero,
                            AvgInOT = x.StartWorkOT.Count() > 0 ? x.StartWorkOT.Average() : TimeSpan.Zero,
                            AvgOutOT = x.EndWorkOT.Count() > 0 ? x.EndWorkOT.Average() : TimeSpan.Zero,
                        });

                    int row = 3;
                    foreach (var d in data)
                    {
                        cells[row, 1].Value = d.Dept;
                        cells[row, 2].Value = d.Shift;
                        cells[row, 3].Value = d.AvgIn.ToData();
                        cells[row, 4].Value = d.AvgOut.ToData();
                        cells[row, 5].Value = d.TotalSum.ToData();
                        cells[row, 6].Value = d.AvgInOT.ToData();
                        cells[row, 7].Value = d.AvgOutOT.ToData();
                        cells[row, 8].Value = d.TotalSumOT.ToData();

                        row++;
                    }

                    var allCell = cells[1, 1, row - 1, 8];
                    allCell.AutoFitColumns();
                    allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    cells[1, 3, row - 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    string fileName = $"{reportNameTemplate} ({company.Company}).xlsx";
                    string filePath = Path.Combine(dirPath, fileName);
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        public void CreateDailySummaryReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
            string reportNameTemplate = $"รายงานประจำวันรวม {report.ReportMonth.ToString("yyyy MMM")}";

            foreach (var company in report.Companies)
            {
                using (var package = new ExcelPackage())
                {
                    var data = company.Employees.GroupBy(x => x.ReportDate)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Employees = g.GroupBy(x => new { x.EmployeeId, x.Name, x.Department, x.ShiftCode, x.ShiftName })
                                .Select(gx => new
                                {
                                    Id = gx.Key.EmployeeId,
                                    Name = gx.Key.Name,
                                    Dept = gx.Key.Department,
                                    Shift = $"{gx.Key.ShiftCode} {gx.Key.ShiftName}",
                                    Work = gx.Aggregate(TimeSpan.Zero, (res, x) => res + x.TotalWorkingTime),
                                    NoWork = gx.Aggregate(TimeSpan.Zero, (res, x) => res + x.TotalNotWorkingTime),
                                    WorkOT = gx.Aggregate(TimeSpan.Zero, (res, x) => res + x.TotalWorkingTimeOT),
                                    NoWorkOT = gx.Aggregate(TimeSpan.Zero, (res, x) => res + x.TotalNotWorkingTimeOT),
                                }).OrderBy(x => x.Id)
                        })
                        .OrderBy(x => x.Date);

                    foreach (var d in data)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(d.Date.Day.ToString("00"));

                        var cells = worksheet.Cells;

                        cells[1, 1].Value = "รหัสพนักงาน";
                        cells[1, 1, 2, 1].Merge = true;
                        cells[1, 2].Value = "ชื่อ - นามสกุล";
                        cells[1, 2, 2, 2].Merge = true;
                        cells[1, 3].Value = "แผนก";
                        cells[1, 3, 2, 3].Merge = true;
                        cells[1, 4].Value = "Shift";
                        cells[1, 4, 2, 4].Merge = true;
                        cells[1, 5].Value = "ในเวลางาน";
                        cells[1, 5, 1, 6].Merge = true;
                        cells[1, 7].Value = "OT";
                        cells[1, 7, 1, 8].Merge = true;
                        cells[2, 5].Value = "Work";
                        cells[2, 6].Value = "No Work";
                        cells[2, 7].Value = "Work";
                        cells[2, 8].Value = "No Work";
                        cells[1, 1, 2, 8].Style.Font.Bold = true;
                        cells[1, 1, 2, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cells[1, 1, 2, 8].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                        cells[1, 1, 2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cells[1, 1, 2, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        int row = 3;
                        foreach (var emp in d.Employees)
                        {
                            cells[row, 1].Value = emp.Id;
                            cells[row, 2].Value = emp.Name;
                            cells[row, 3].Value = emp.Dept;
                            cells[row, 4].Value = emp.Shift;
                            cells[row, 5].Value = emp.Work.ToData();
                            cells[row, 6].Value = emp.NoWork.ToData();
                            cells[row, 7].Value = emp.WorkOT.ToData();
                            cells[row, 8].Value = emp.NoWorkOT.ToData();

                            row++;
                        }

                        var allCell = cells[1, 1, row - 1, 8];
                        allCell.AutoFitColumns();
                        allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        cells[1, 5, row - 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        string fileName = $"{reportNameTemplate} ({company.Company}).xlsx";
                        string filePath = Path.Combine(dirPath, fileName);
                        package.SaveAs(new FileInfo(filePath));
                    }
                }
            }
        }
    }
}