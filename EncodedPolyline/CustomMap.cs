using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncodedPolyline
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty EncodedPolylineProperty =
            BindableProperty.Create("EncodedPolyline", typeof(string), typeof(CustomMap), null, BindingMode.OneWay, null, OnEncodedPolylineChanged);

        public static readonly BindableProperty PolylineColorProperty =
            BindableProperty.Create("PolylineColor", typeof(Color), typeof(CustomMap), Color.Blue, BindingMode.OneWay, null, OnEncodedPolylineColorChanged);

        public ICustomNativeMap NativeMap { get; set; }

        public string EncodedPolyline
        {
            get => (string)GetValue(EncodedPolylineProperty);
            set => SetValue(EncodedPolylineProperty, value);
        }

        public Color PolylineColor
        {
            get => (Color)GetValue(PolylineColorProperty);
            set => SetValue(PolylineColorProperty, value);
        }

        private static void OnEncodedPolylineChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomMap)bindable).RefreshPolyline();
        }

        private static void OnEncodedPolylineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomMap)bindable).RefreshPolyline();
        }

        private void AddPolyline(string encodedPoints)
        {
            var info = new PolylineInfo();
            var points = DecodePolyline(encodedPoints);

            if (points != null && points.Count == 0)
                return;

            info.Points = points.ToArray();

            double minLat = double.MaxValue;
            double maxLat = double.MinValue;
            double minLon = double.MaxValue;
            double maxLon = double.MinValue;

            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                minLat = Math.Min(minLat, p.Latitude);
                maxLat = Math.Max(maxLat, p.Latitude);
                minLon = Math.Min(minLon, p.Longitude);
                maxLon = Math.Max(maxLon, p.Longitude);
                info.Points[i] = p;
            }

            var latCenter = (maxLat - minLat) * 0.5d + minLat;
            var lonCenter = (maxLon - minLon) * 0.5d + minLon;

            info.Center = new Position(latCenter, lonCenter);
            info.SouthWest = new Position(minLat, minLon);
            info.NorthEast = new Position(maxLat, maxLon);

            NativeMap.AddPolyline(info);
            NativeMap.FitToPolyline(info);
        }

        private List<Position> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
            {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<Position>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var mLatLng = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }

        public void RefreshPolyline()
        {
            if (NativeMap == null)
                return;

            NativeMap.ClearPolyline();

            var encodedPolyline = EncodedPolyline;

            if (!string.IsNullOrWhiteSpace(encodedPolyline))
                AddPolyline(encodedPolyline);
        }
    }
}
