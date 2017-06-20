using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System;

namespace DHL.Report.TimeAttendance.Managers
{
    public class AboutManager : IAboutManager
    {
        #region Fields & Properties
        private string _basePath;
        #endregion

        #region Constructor
        public AboutManager(string basePath)
        {
            _basePath = basePath;
        }
        #endregion

        #region Public Method
        public async Task<AboutModel> GetAboutAsyc()
        {
            return await Task.Run(() =>
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
            });
        }
        #endregion
    }
}
