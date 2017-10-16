namespace EncodedPolyline
{
    public interface ICustomNativeMap
    {
        void AddPolyline(PolylineInfo info);
        void FitToPolyline(PolylineInfo info);
        void ClearPolyline();
    }
}
