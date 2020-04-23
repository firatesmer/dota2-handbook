using Windows.UI.Xaml.Controls;
using Template10.Controls;
using Template10.Services.NavigationService;

namespace Dota2Handbook.Views
{
    using Services.SettingsServices;

    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        private SettingsService _settings;

        public Shell()
        {
            Instance = this;

            InitializeComponent();

            _settings = SettingsService.Instance;
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
        }
    }
}