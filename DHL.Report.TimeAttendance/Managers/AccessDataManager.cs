using DHL.Report.TimeAttendance.Managers.Interfaces;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace DHL.Report.TimeAttendance.Managers
{
    public class AccessDataManager : IAccessDataManager
    {
        public DataTable ReadData(string filePath, string password)
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

            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Jet OLEDB: Database Password={password};";

            var result = new DataTable();
            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                var query = "Select * From t_b_customer";
                var command = new OleDbCommand(query, connection);
                var reader = command.ExecuteReader();
                result.Load(reader);

                connection.Close();
            }

            return result;
        }
    }
}
