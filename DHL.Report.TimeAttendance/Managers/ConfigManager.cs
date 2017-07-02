using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models;
using System.IO;

namespace DHL.Report.TimeAttendance.Managers
{
    // unused
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
                    // TODO: init for default config
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
