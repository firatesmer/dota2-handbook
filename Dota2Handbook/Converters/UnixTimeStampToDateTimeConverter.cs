using System;
using Windows.UI.Xaml.Data;

namespace Dota2Handbook.Converters
{
    public class UnixTimeStampToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double ticks = double.Parse(value.ToString());

            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(ticks).ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            null;
    }
}