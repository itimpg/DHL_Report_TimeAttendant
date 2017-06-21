using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DHL.Report.TimeAttendance.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isVisible = value as bool?;
            return isVisible.HasValue && isVisible.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return ((Visibility)value) == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }
}
