using DHL.Report.TimeAttendance.Managers.Interfaces;
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
                    var query = @"
                        SELECT
                            c.f_ConsumerNO, sr.f_CardNO, sr.f_InOut, sr.f_ReadDate
                        FROM t_b_consumer c
                        INNER JOIN t_d_SwipeRecord sr 
                            ON  c.f_ConsumerID = sr.f_ConsumerID 
                            AND c.f_CardNo = sr.f_CardNo
                        WHERE sr.f_ReadDate BETWEEN @from AND @to
                        ORDER BY sr.f_ReadDate";
                    var command = new OleDbCommand(query, connection);
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

                    return data.AsEnumerable().Select(x => new EmployeeSwipeInfoModel
                    {
                        EmployeeId = x.Field<string>("f_ConsumerNO").Trim(),
                        SwipeCode = x.Field<decimal>("f_CardNo").ToString(),
                        IsOut = Convert.ToBoolean(x.Field<byte>("f_InOut")),
                        ReadDate = x.Field<DateTime>("f_ReadDate")
                    });
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
