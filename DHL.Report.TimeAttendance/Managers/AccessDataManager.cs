using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers
{
    public class AccessDataManager : IAccessDataManager
    {
        public async Task<IEnumerable<EmployeeSwipeInfoModel>> GetEmployeeSwipeInfo(
            string filePath, string password, DateTime dateFrom, DateTime dateTo)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Access file path");
            }

            var extension = Path.GetExtension(filePath).ToLower();
            var supportedExtension = new[] { ".mdb", ".accdb" };
            if (!supportedExtension.Contains(extension))
            {
                throw new ArgumentException("Support only excel file.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Cannot found the Access file: " + filePath);
            }

            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={filePath};Persist Security Info=False; Jet OLEDB:Database Password={password};";
            using (var connection = new OleDbConnection(connectionString))
            {
                dateFrom = dateFrom.AddDays(-1);
                dateTo = dateTo.AddDays(1);

                try
                {
                    await connection.OpenAsync();
                    var queryString = @"
                        SELECT
                            c.f_ConsumerNO, sr.f_InOut, sr.f_ReadDate
                        FROM t_b_consumer c
                        INNER JOIN t_d_SwipeRecord sr 
                            ON  c.f_ConsumerID = sr.f_ConsumerID 
                            AND c.f_CardNo = sr.f_CardNo
                        WHERE sr.f_ReadDate BETWEEN @from AND @to
                        ORDER BY sr.f_ReadDate";
                    var command = new OleDbCommand(queryString, connection);
                    command.Parameters.Add(new OleDbParameter
                    {
                        ParameterName = "@from",
                        Value = dateFrom,
                    });
                    command.Parameters.Add(new OleDbParameter
                    {
                        ParameterName = "@to",
                        Value = dateTo,
                    });
                    var data = new DataTable();
                    var reader = await command.ExecuteReaderAsync();
                    data.Load(reader);

                    var src = data.AsEnumerable().Select(x => new
                    {
                        EmployeeId = x.Field<string>("f_ConsumerNO").Trim(),
                        IsOut = Convert.ToBoolean(x.Field<byte>("f_InOut")),
                        ReadDate = x.Field<DateTime>("f_ReadDate")
                    });

                    var query =
                        from q in (
                                from i in src.Where(x => x.IsOut == false)
                                join o in src.Where(x => x.IsOut == true)
                                  on i.EmployeeId equals o.EmployeeId
                                where o.ReadDate > i.ReadDate
                                group new { i, o } by new { i.EmployeeId, i.ReadDate } into ig
                                select new
                                {
                                    EmployeeId = ig.Key.EmployeeId,
                                    DateIn = ig.Key.ReadDate,
                                    DateOut = ig.Min(x => x.o.ReadDate)
                                }
                            )
                        group q by new { q.EmployeeId, q.DateOut } into g
                        let dateIn = g.Max(x => x.DateIn)
                        select new
                        {
                            EmployeeId = g.Key.EmployeeId,
                            DateIn = dateIn,
                            DateOut = g.Key.DateOut,
                        };

                    var employees = query
                        .OrderBy(x => x.DateIn)
                        .GroupBy(x => x.EmployeeId)
                        .Select(g => new
                        {
                            EmployeeId = g.Key,
                            Items = g.SelectWithPrev((cur, prev, isfirst) => new 
                            {
                                EmployeeId = cur.EmployeeId,
                                DateIn = cur.DateIn,
                                DateOut = cur.DateOut,
                                WorkingTime = cur.DateOut - cur.DateIn,
                                NotWorkingTime = isfirst || (cur.DateIn - prev.DateOut).TotalHours > 4 ? TimeSpan.Zero : (cur.DateIn - prev.DateOut),
                                ReportDate = (isfirst || (cur.DateIn - prev.DateOut).TotalHours > 4 ? TimeSpan.Zero : (cur.DateIn - prev.DateOut)) == TimeSpan.Zero ?
                                    cur.DateIn.Date : prev.DateIn.Date
                            })
                            .SelectWithPrev((cur, prev, isfirst) => new EmployeeSwipeInfoModel
                            {
                                EmployeeId = cur.EmployeeId,
                                DateIn = cur.DateIn,
                                DateOut = cur.DateOut,
                                WorkingTime = cur.DateOut - cur.DateIn,
                                NotWorkingTime = cur.NotWorkingTime,
                                ReportDate = isfirst || cur.NotWorkingTime == TimeSpan.Zero ?
                                    cur.ReportDate : prev.ReportDate
                            })
                            .OrderBy(x => x.DateIn)
                        });

                    return employees
                        .SelectMany(x => x.Items.Where(r => r.ReportDate > dateFrom && r.ReportDate < dateTo))
                        .OrderBy(x => x.EmployeeId)
                        .ThenBy(x => x.DateIn);
                }
                catch (OleDbException ex)
                {
                    if (ex.Message == "Not a valid password.")
                    {
                        throw new Exception("รหัสผ่านสำหรับเข้าถึง Access ไม่ถูกต้อง");
                    }
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
                return new List<EmployeeSwipeInfoModel>();
            }
        }
    }
}
