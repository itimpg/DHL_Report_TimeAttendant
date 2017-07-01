using DHL.Report.TimeAttendance.Data.Entities;
using DHL.Report.TimeAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;

namespace DHL.Report.TimeAttendance.Data
{
    public class MyContext : DbContext, IMyContext
    {
        public MyContext(string connectionString)
            : base(new SQLiteConnection() { ConnectionString = "data source=" + connectionString }, true)
        {
        }

        public DbSet<Shift> Shifts { get; set; }
    }
}
