using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace EncodedPolyline
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int i = 0;

        public MainViewModel()
        {
            SwitchPolylineCommand = new Command(SwitchPolyline);
            SwitchPolylineColorCommand = new Command(SwitchPolylineColor);
        }

        public ICommand SwitchPolylineCommand { get; }
        public ICommand SwitchPolylineColorCommand { get; }

        private string _encodedPolyline;
        public string EncodedPolyline
        {
            get => _encodedPolyline;
            set => Set(ref _encodedPolyline, value);
        }

        private PolylineColor _polylineColor;
        public PolylineColor PolylineColor
        {
            get => _polylineColor;
            set => Set(ref _polylineColor, value);
        }

        private Position _position;
        public Position Position
        {
            get => _position;
            set => Set(ref _position, value);
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public async void Start()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await StartAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async Task StartAsync()
        {
            var position = await Plugin.Geolocator.CrossGeolocator.Current.GetLastKnownLocationAsync();
            if (position == null)
                position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
            Position = position;
        }

        private void SwitchPolyline()
        {
            if (i % 2 == 1)
            {
                // Paris
                EncodedPolyline = "kegiH}{dMv`@mhBhKzDcJtx@xFfFmRj_AsJiGg@wB}FsDoAv@eLmH~DoQvGhFu@jE";
            }
            else
            {
                // Beijing
                EncodedPolyline = "utqrFwzleUuxAfBtBv{Ap{AsH?q|Bmm@tAu@{cEdj@mE_A{|Aqv@dAf@xw@oh@fBSftEHtQjCfFl`BgC^hb@iCH";
            }

            i++;
        }

        private void SwitchPolylineColor()
        {
            if (PolylineColor == PolylineColor.Red)
                PolylineColor = PolylineColor.Blue;
            else
                PolylineColor = PolylineColor.Red;
        }

        #region Infrastructure
        private void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
