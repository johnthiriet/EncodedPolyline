using Xamarin.Forms.Maps;

namespace EncodedPolyline
{
    public class PolylineInfo
    {
        public Position Center { get; set; }
        public Position[] Points { get; set; }
        public Position SouthWest { get; set; }
        public Position NorthEast { get; set; }
    }
}
