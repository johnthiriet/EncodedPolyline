using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using EncodedPolyline;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(EncodedPolyline.Droid.CustomMapRenderer))]
namespace EncodedPolyline.Droid
{
    public class CustomMapRenderer : MapRenderer, ICustomNativeMap
    {
        private CustomMap CustomMap => (CustomMap)Element;
        private Polyline _polyline;

        public void AddPolyline(PolylineInfo polylineInfo)
        {
            var polylineOptions = new PolylineOptions()
                .InvokeColor(CustomMap.PolylineColor.ToAndroid())
                .InvokeWidth(4);

            var positions = new LatLng[polylineInfo.Points.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                var p = polylineInfo.Points[i];
                positions[i] = new LatLng(p.Latitude, p.Longitude);
            }

            polylineOptions.Add(positions);
            _polyline = NativeMap.AddPolyline(polylineOptions);
        }

        public void ClearPolyline()
        {
            _polyline?.Remove();
            _polyline?.Dispose();
        }

        public void FitToPolyline(PolylineInfo polylineInfo)
        {
            var sw = new LatLng(polylineInfo.SouthWest.Latitude, polylineInfo.SouthWest.Longitude);
            var ne = new LatLng(polylineInfo.NorthEast.Latitude, polylineInfo.NorthEast.Longitude);

            LatLngBounds bounds = new LatLngBounds(sw, ne);

            Control.Post(() => NativeMap.MoveCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 10)));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            if (e == null)
            {
                throw new System.ArgumentNullException(nameof(e));
            }

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ((CustomMap)e.NewElement).NativeMap = null;
            }

            if (e.NewElement != null)
            {
                ((CustomMap)e.NewElement).NativeMap = this;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            CustomMap.RefreshPolyline();
        }
    }
}
