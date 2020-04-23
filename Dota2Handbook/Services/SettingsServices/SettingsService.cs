using System;
using Windows.UI.Xaml;
using Template10.Utils;
using Template10.Common;
using Template10.Services.SettingsService;

namespace Dota2Handbook.Services.SettingsServices
{
    using Infrastructure;

    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        private ISettingsHelper _helper;
        private SettingsService() =>
            _helper = new SettingsHelper();

        public bool UseShellBackButton
        {
            get => _helper.Read(nameof(UseShellBackButton), true);
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);

                BootStrapper.Current.NavigationService.GetDispatcherWrapper().Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read(nameof(AppTheme), theme.ToString());
                return Enum.TryParse(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value, true);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get => _helper.Read(nameof(CacheMaxDuration), TimeSpan.MaxValue);
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public bool IsReadingViewEnabled
        {
            get => _helper.Read(nameof(IsReadingViewEnabled), true);
            set => _helper.Write(nameof(IsReadingViewEnabled), value);
        }

        public string LanguageCode
        {
            get => _helper.Read(nameof(LanguageCode), Constants.EnShort2);
            set => _helper.Write(nameof(LanguageCode), value);
        }

        public string Language
        {
            get => _helper.Read(nameof(Language), Constants.English);
            set => _helper.Write(nameof(Language), value);
        }
    }
}