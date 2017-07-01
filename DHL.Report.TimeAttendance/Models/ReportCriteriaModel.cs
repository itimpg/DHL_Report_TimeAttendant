using GalaSoft.MvvmLight;

namespace DHL.Report.TimeAttendance.Models
{
    public class ReportCriteriaModel : ObservableObject
    {
        private string _accessFilePath;
        public string AccessFilePath
        {
            get { return _accessFilePath; }
            set { Set(() => AccessFilePath, ref _accessFilePath, value); }
        }

        private string _excelFilePath;
        public string ExcelFilePath
        {
            get { return _excelFilePath; }
            set { Set(() => ExcelFilePath, ref _excelFilePath, value); }
        }

        private string _outputDir;
        public string OutputDir
        {
            get { return _outputDir; }
            set { Set(() => OutputDir, ref _outputDir, value); }
        }

        private bool _isOption1;
        public bool IsOption1
        {
            get { return _isOption1; }
            set { Set(() => IsOption1, ref _isOption1, value); }
        }

        private bool _isOption2;
        public bool IsOption2
        {
            get { return _isOption2; }
            set { Set(() => IsOption2, ref _isOption2, value); }
        }

        private bool _isOption3;
        public bool IsOption3
        {
            get { return _isOption3; }
            set { Set(() => IsOption3, ref _isOption3, value); }
        }

        private bool _isOption4;
        public bool IsOption4
        {
            get { return _isOption4; }
            set { Set(() => IsOption4, ref _isOption4, value); }
        }
    }
}
