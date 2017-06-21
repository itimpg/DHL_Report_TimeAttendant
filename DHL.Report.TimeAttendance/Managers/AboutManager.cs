using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DHL.Report.TimeAttendance.Managers
{
    public class AboutManager : IAboutManager
    {
        #region Constructor
        public AboutManager() { }
        #endregion

        #region Public Method
        public AboutModel GetAbout()
        {
            var assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            DateTime lastedUpdatedDate = File.GetCreationTime(assembly.Location);

            return new AboutModel()
            {
                Version = version,
                LatestUpdatedDate = lastedUpdatedDate
            };
        }
        #endregion
    }
}
