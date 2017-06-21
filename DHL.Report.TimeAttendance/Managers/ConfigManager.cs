using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models;
using System.IO;
using System;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ConfigManager : IConfigManager
    {
        #region Fields & Properties
        private const string CONFIG_FILE_NAME = "config.xml";
        private string _basePath;
        private readonly IXmlManager _xmlManager;

        private ConfigModel DefaultModel
        {
            get
            {
                return new ConfigModel
                {
                    MorningShift = new Shift { Name = "กะเช้า", From = new TimeSpan(6, 0, 0), To = new TimeSpan(15, 0, 0) },
                    EveningShift = new Shift { Name = "กะบ่าย", From = new TimeSpan(14, 0, 0), To = new TimeSpan(23, 0, 0) },
                    NightShift = new Shift { Name = "กะดึก", From = new TimeSpan(22, 0, 0), To = new TimeSpan(7, 0, 0) },
                    MorningShiftWithOT = new Shift { Name = "กะเช้ามีโอที", From = new TimeSpan(6, 0, 0), To = new TimeSpan(18, 0, 0) },
                    NightShiftWithOT = new Shift { Name = "กะดึกมีโอที", From = new TimeSpan(18, 0, 0), To = new TimeSpan(6, 0, 0) }
                };
            }
        }
        #endregion

        #region Constructor
        public ConfigManager(IXmlManager xmlManager, string basePath)
        {
            _xmlManager = xmlManager;
            _basePath = basePath;
        }
        #endregion

        #region Public Methods
        public async Task<ConfigModel> GetConfigAsync()
        {
            string filePath = Path.Combine(_basePath, CONFIG_FILE_NAME);
            if (File.Exists(filePath))
            {
                string xmlText = File.ReadAllText(filePath);
                return await Task.Run(() =>
                {
                    return _xmlManager.Deserialize<ConfigModel>(xmlText);
                });
            }
            else
            {
                await SaveConfigAsync(DefaultModel);
                return DefaultModel;
            }
        }

        public async Task SaveConfigAsync(ConfigModel model)
        {
            await Task.Run(() =>
            {
                string filePath = Path.Combine(_basePath, CONFIG_FILE_NAME);
                string xmlText = _xmlManager.Serialize(model);
                File.WriteAllText(filePath, xmlText);
            });
        }
        #endregion
    }
}
