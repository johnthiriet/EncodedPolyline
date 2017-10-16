using System;
using System.Globalization;
using Xamarin.Forms;

namespace EncodedPolyline
{
    public class PolylineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;
            PolylineColor polylineColor = (PolylineColor)value;

            switch (polylineColor)
            {
                case PolylineColor.Blue:
                    color = Color.Blue;
                    break;
                case PolylineColor.Red:
                    color = Color.Red;
                    break;
                default:
                    throw new NotImplementedException("Unknown color");
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            PolylineColor polylineColor;

            if (color == Color.Blue)
                polylineColor = PolylineColor.Blue;
            else if (color == Color.Red)
                polylineColor = PolylineColor.Red;
            else
                throw new NotImplementedException("Unknown color");

            return polylineColor;
        }
    }
}
