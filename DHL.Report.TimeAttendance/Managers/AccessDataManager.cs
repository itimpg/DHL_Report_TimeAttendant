﻿using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
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
        public async Task<IEnumerable<EmployeeSwipeInfoModel>> GetEmployeeSwipeInfo(string filePath, string password, DateTime dateFrom, DateTime dateTo)
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
                        Value = dateFrom
                    });
                    command.Parameters.Add(new OleDbParameter
                    {
                        ParameterName = "@to",
                        Value = dateTo
                    });
                    var reader = await command.ExecuteReaderAsync();

                    var data = new DataTable();
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
                                && o.EmployeeId == "60009"
                            group new { i, o } by new { i.EmployeeId, i.ReadDate } into ig
                            select new
                            {
                                EmployeeId = ig.Key.EmployeeId,
                                DateIn = ig.Key.ReadDate,
                                DateOut = ig.Min(x => x.o.ReadDate)
                            }
                        )
                        group q by new { q.EmployeeId, q.DateOut } into g
                        select new EmployeeSwipeInfoModel
                        {
                            EmployeeId = g.Key.EmployeeId,
                            DateIn = g.Max(x => x.DateIn),
                            DateOut = g.Key.DateOut,
                            WorkingTime = g.Key.DateOut - g.Max(x => x.DateIn)
                        };

                    // var qq = query.OrderByDescending(x => x.DateIn).ToList();

                    return query.OrderBy(x => x.DateIn);
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
