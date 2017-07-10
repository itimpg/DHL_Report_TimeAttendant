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
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"ประจำวัน {d.Date.ToString("d MMM yyyy")}");
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
                            cells[row, 8].Value = employee.TotalWorkingTime.ToString(@"hh\:mm");
                            cells[row, 9].Value = employee.TotalNotWorkingTime.ToString(@"hh\:mm");
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
                                cells[row, 6].Value = work.In.ToString("dd/MM/yyyy HH:mm");
                                cells[row, 7].Value = work.Out.HasValue ? work.Out.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                                cells[row, 8].Value = work.WorkingTime.ToString(@"hh\:mm");
                                cells[row, 9].Value = work.NotWorkingTime.ToString(@"hh\:mm");
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
                            cells[row, 8].Value = employee.TotalWorkingTimeOT.ToString(@"hh\:mm");
                            cells[row, 9].Value = employee.TotalNotWorkingTimeOT.ToString(@"hh\:mm");
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
                                cells[row, 6].Value = work.In.ToString("dd/MM/yyyy HH:mm");
                                cells[row, 7].Value = work.Out.HasValue ? work.Out.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                                cells[row, 8].Value = work.WorkingTime.ToString(@"hh\:mm");
                                cells[row, 9].Value = work.NotWorkingTime.ToString(@"hh\:mm");
                                row++;
                            }
                        }
                        var allCell = cells[1, 1, row - 1, 9];
                        allCell.AutoFitColumns();
                        allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
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
                        .Select(x => new
                        {
                            Day = x.ReportDate.Day,
                            Id = x.EmployeeId,
                            Name = x.Name,
                            Dept = x.Department,
                            Shift = $"{x.ShiftCode} {x.ShiftName}",
                            StartWork = x.WorkingSwipes.Min(s => s.In),
                            EndWork = x.WorkingSwipes.Max(s => s.Out),
                            TotalIn = x.TotalWorkingTime,
                            TotalOut = x.TotalNotWorkingTime,
                            Sum = x.TotalWorkingTime + x.TotalNotWorkingTime,
                            StartWorkOT = x.OtSwipes.Min(s => s.In),
                            EndWorkOT = x.OtSwipes.Max(s => s.Out),
                            TotalInOT = x.TotalWorkingTimeOT,
                            TotalOutOT = x.TotalNotWorkingTimeOT,
                            SumOT = x.TotalWorkingTimeOT + x.TotalNotWorkingTimeOT
                        }).OrderBy(x => x.Id).ThenBy(x => x.Day).ToList();

                    int row = 3;
                    foreach (var emp in data)
                    {
                        cells[row, 1].Value = emp.Id;
                        cells[row, 2].Value = emp.Name;
                        cells[row, 3].Value = emp.Dept;
                        cells[row, 4].Value = emp.Shift;
                        cells[row, 5].Value = emp.StartWork.ToString("dd/MM/yyyy HH:mm");
                        cells[row, 6].Value = emp.EndWork.HasValue ? emp.EndWork.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                        cells[row, 7].Value = emp.TotalIn.ToString(@"hh\:mm");
                        cells[row, 8].Value = emp.TotalOut.ToString(@"hh\:mm");
                        cells[row, 9].Value = emp.Sum.ToString(@"hh\:mm");
                        cells[row, 10].Value = emp.StartWorkOT.ToString("dd/MM/yyyy HH:mm");
                        cells[row, 11].Value = emp.EndWorkOT.HasValue ? emp.EndWorkOT.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                        cells[row, 12].Value = emp.TotalInOT.ToString(@"hh\:mm");
                        cells[row, 13].Value = emp.TotalOutOT.ToString(@"hh\:mm");
                        cells[row, 14].Value = emp.SumOT.ToString(@"hh\:mm");

                        row++;
                    }
                    var allCell = cells[1, 1, row - 1, 14];
                    allCell.AutoFitColumns();
                    allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

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
                    cells[2, 3].Value = "IN";
                    cells[2, 4].Value = "OUT";
                    cells[2, 5].Value = "SUM";
                    cells[2, 6].Value = "IN";
                    cells[2, 7].Value = "OUT";
                    cells[2, 8].Value = "SUM";
                    cells[1, 1, 2, 8].Style.Font.Bold = true;
                    cells[1, 1, 2, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cells[1, 1, 2, 8].Style.Fill.BackgroundColor.SetColor(Color.Silver);
                    cells[1, 1, 2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cells[1, 1, 2, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var data = company.Employees
                        .GroupBy(x => new { x.Department, x.ShiftCode, x.ShiftName })
                        .Select(g => new
                        {
                            Dept = g.Key.Department,
                            Shift = $"{g.Key.ShiftCode} {g.Key.ShiftName}",
                            AvgIn = g.Select(x => x.TotalWorkingTime).Average(),
                            AvgOut = g.Select(x => x.TotalNotWorkingTime).Average(),
                            TotalSum = g.Select(x => x.TotalWorkingTime + x.TotalNotWorkingTime).Average(),
                            AvgInOT = g.Select(x => x.TotalWorkingTimeOT).Average(),
                            AvgOutOT = g.Select(x => x.TotalNotWorkingTimeOT).Average(),
                            TotalSumOT = g.Select(x => x.TotalWorkingTimeOT + x.TotalNotWorkingTimeOT).Average(),
                        });

                    int row = 3;
                    foreach (var d in data)
                    {
                        cells[row, 1].Value = d.Dept;
                        cells[row, 2].Value = d.Shift;
                        cells[row, 3].Value = d.AvgIn.ToString(@"hh\:mm");
                        cells[row, 4].Value = d.AvgOut.ToString(@"hh\:mm");
                        cells[row, 5].Value = d.TotalSum.ToString(@"hh\:mm");
                        cells[row, 6].Value = d.AvgInOT.ToString(@"hh\:mm");
                        cells[row, 7].Value = d.AvgOutOT.ToString(@"hh\:mm");
                        cells[row, 8].Value = d.TotalSumOT.ToString(@"hh\:mm");

                        row++;
                    }

                    var allCell = cells[1, 1, row - 1, 8];
                    allCell.AutoFitColumns();
                    allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

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
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"ประจำวัน {d.Date.ToString("d MMM yyyy")} รวม");

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
                            cells[row, 5].Value = emp.Work.ToString(@"hh\:mm");
                            cells[row, 6].Value = emp.NoWork.ToString(@"hh\:mm");
                            cells[row, 7].Value = emp.WorkOT.ToString(@"hh\:mm");
                            cells[row, 8].Value = emp.NoWorkOT.ToString(@"hh\:mm");

                            row++;
                        }

                        var allCell = cells[1, 1, row - 1, 8];
                        allCell.AutoFitColumns();
                        allCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        string fileName = $"{reportNameTemplate} ({company.Company}).xlsx";
                        string filePath = Path.Combine(dirPath, fileName);
                        package.SaveAs(new FileInfo(filePath));
                    }
                }
            }
        }
    }
}