using System;
using System.Globalization;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Platforms.Common.Converters
{
    /// <summary>Use this converter together with the <see cref="Behaviors.Event2CommandBehavior">Event2CommandBehavior</see> to pass the selected <see cref="Xamarin.Forms.ListView">ListView</see> item.</summary>
    public class SelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as SelectedItemChangedEventArgs;
            return eventArgs?.SelectedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
