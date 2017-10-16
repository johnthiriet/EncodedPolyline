using CoreLocation;
using EncodedPolyline;
using MapKit;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(EncodedPolyline.iOS.CustomMapRenderer))]
namespace EncodedPolyline.iOS
{
    public class CustomMapRenderer : MapRenderer, ICustomNativeMap
    {
        private MKPolylineRenderer _polylineRenderer;
        private MKPolyline _polyline;
        private CustomMap CustomMap => (CustomMap)Element;
        private MKMapView NativeMap => (MKMapView)Control;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var nativeMap = (MKMapView)Control;
                var customMap = (CustomMap)e.NewElement;
                customMap.NativeMap = this;
                nativeMap.OverlayRenderer = GetOverlayRenderer;

                customMap.RefreshPolyline();
            }
            else if (e.OldElement != null)
            {
                var customMap = (CustomMap)e.NewElement;
                customMap.NativeMap = null;
            }
        }

        private MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
        {
            if (_polylineRenderer == null && !Equals(overlayWrapper, null))
            {
                var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
                _polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline)
                {
                    FillColor = CustomMap.PolylineColor.ToUIColor(),
                    StrokeColor = CustomMap.PolylineColor.ToUIColor(),
                    LineWidth = 3,
                    Alpha = 0.5f
                };
            }
            return _polylineRenderer;
        }

        public void ClearPolyline()
        {
            _polylineRenderer?.Dispose();
            _polylineRenderer = null;

            if (_polyline != null)
            {
                NativeMap.RemoveOverlay(_polyline);
                _polyline.Dispose();
                _polyline = null;
            }
        }

        public void AddPolyline(PolylineInfo polylineInfo)
        {
            var positions = new CLLocationCoordinate2D[polylineInfo.Points.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                var p = polylineInfo.Points[i];
                positions[i] = new CLLocationCoordinate2D(p.Latitude, p.Longitude);
            }
              
            _polyline = MKPolyline.FromCoordinates(positions);
            NativeMap.AddOverlay(_polyline, MKOverlayLevel.AboveRoads);
        }

        public void FitToPolyline(PolylineInfo polylineInfo)
        {
            Control.BeginInvokeOnMainThread(() =>
            {
                NativeMap.VisibleMapRect = NativeMap.MapRectThatFits(_polyline.BoundingMapRect, new UIEdgeInsets(10, 10, 10, 10));
            });
        }
    }
}
