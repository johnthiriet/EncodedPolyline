using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncodedPolyline
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private MainViewModel _vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _vm = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _vm.PropertyChanged += OnViewModelPropertyChanged;

            _vm.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _vm.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Position):
                    var position = new Position(_vm.Position.Latitude, _vm.Position.Longitude);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(10)));
                    break;
            }
        }
    }
}
