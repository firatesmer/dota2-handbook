using System;
using Windows.UI.Xaml.Data;
using Windows.ApplicationModel.Resources;

namespace Dota2Handbook.Converters
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return ResourceLoader.GetForCurrentView().GetString("SemiFinalists/Text");
            else
                return ResourceLoader.GetForCurrentView().GetString("ThirdPlace/Text");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            null;
    }
}