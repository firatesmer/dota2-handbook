using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Template10.Services.SerializationService;

namespace Dota2Handbook.Views
{
    public sealed partial class Settings : Page
    {
        ISerializationService _SerializationService;

        public Settings()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            _SerializationService = SerializationService.Json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }
    }
}