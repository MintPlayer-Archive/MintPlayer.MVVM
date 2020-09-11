using System;
using System.Globalization;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.Converters
{
    public class InvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }
            else
            {
                throw new Exception($"Source value for the {nameof(InvertConverter)} is not a bool.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }
            else
            {
                throw new Exception($"Target value for the {nameof(InvertConverter)} is not a bool.");
            }
        }
    }
}
