using System;
using Windows.UI.Xaml;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.DataTransfer;
using Template10.Mvvm;

namespace Dota2Handbook.ViewModels
{
    using Data;
    using Enums;
    using Utilities;
    using Infrastructure;
    using static Utilities.SecondaryTilePin;

    public class SettingsViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public PinPartViewModel PinPartViewModel { get; } = new PinPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
        public VersionHistoryPartViewModel VersionHistoryPartViewModel { get; } = new VersionHistoryPartViewModel();

        public SettingsViewModel()
        { }
        #endregion
    }

    #region SettingsPartViewModel
    public class SettingsPartViewModel : ViewModelBase
    {
        #region Properties & Constructor
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;

            ShellDrawnBackButtonVisibility = ApiInformation.IsTypePresent(Constants.WINDOWS_API__HardwareButtons) ? Visibility.Collapsed : Visibility.Visible;
        }

        public bool IsReadModeEnabled
        {
            get { return _settings.IsReadingViewEnabled; }
            set
            {
                _settings.IsReadingViewEnabled = value;
                base.RaisePropertyChanged();
            }
        }

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        public IList<string> LanguageList
        {
            get { return SetLanguagesToComboBox(Constants.LanguageCode); }
        }

        string _selectedComboBoxItem = SetLanguageToSelectedItem(Constants.LanguageCode);
        public string SelectedComboBoxItem
        {
            get { return _selectedComboBoxItem; }
            set
            {
                Set(ref _selectedComboBoxItem, value);

                ChangeSelectedLanguage(SelectedComboBoxItem);

                IsRestartButtonEnabled = true;
            }
        }

        bool _isRestartButtonEnabled;
        public bool IsRestartButtonEnabled
        {
            get { return _isRestartButtonEnabled; }
            set => Set(ref _isRestartButtonEnabled, value);
        }

        Visibility _shellDrawnBackButtonVisibility;
        public Visibility ShellDrawnBackButtonVisibility
        {
            get { return _shellDrawnBackButtonVisibility; }
            set => Set(ref _shellDrawnBackButtonVisibility, value);
        }
        #endregion

        #region Private Methods
        private IList<string> SetLanguagesToComboBox(string languageCode)
        {
            IList<string> languages;

            switch (languageCode)
            {
                case Constants.TrShort:
                    languages = new List<string>() { Constants.TrEN, Constants.TrTR };
                    break;
                default:
                    languages = new List<string>() { Constants.EnEN, Constants.EnTR };
                    break;
            }

            return languages;
        }

        private static string SetLanguageToSelectedItem(string languageCode)
        {
            string language;

            switch (languageCode)
            {
                case Constants.TrShort:
                    language = Constants.TrTR;
                    break;
                default:
                    language = Constants.EnEN;
                    break;
            }

            return language;
        }

        private void ChangeSelectedLanguage(string language)
        {
            if (Constants.TurkishLanguageList.Contains(language))
            {
                _settings.Language = Constants.Turkish;
                _settings.LanguageCode = Constants.TrShort;
            }
            else if (Constants.EnglishLanguageList.Contains(language))
            {
                _settings.Language = Constants.English;
                _settings.LanguageCode = Constants.EnShort;
            }
        }
        #endregion
    }
    #endregion

    #region PinPartViewModel
    public class PinPartViewModel : ViewModelBase
    {
        #region Properties & Constructor
        string _isNewsPinned;
        public string IsNewsPinned
        {
            get { return _isNewsPinned; }
            set => Set(ref _isNewsPinned, value);
        }

        string _isHeroesPinned;
        public string IsHeroesPinned
        {
            get { return _isHeroesPinned; }
            set => Set(ref _isHeroesPinned, value);
        }

        string _isItemsPinned;
        public string IsItemsPinned
        {
            get { return _isItemsPinned; }
            set => Set(ref _isItemsPinned, value);
        }

        string _isLeaderboardsPinned;
        public string IsLeaderboardsPinned
        {
            get { return _isLeaderboardsPinned; }
            set => Set(ref _isLeaderboardsPinned, value);
        }

        public PinPartViewModel() =>
            CheckPinnedPages();
        #endregion

        #region Public Methods
        public async void PinTile(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();

            PinTileObject secondaryTile = new PinTileObject()
            {
                TileId = tag,
                DisplayName = tag,
                Arguments = tag,
            };

            await SecondaryTilePin.PinTile(secondaryTile);

            CheckPinnedPages();
        }

        public void CheckPinnedPages()
        {
            string pinned = ResourceLoader.GetForCurrentView().GetString("PinnedPage");
            string notPinned = ResourceLoader.GetForCurrentView().GetString("NotPinnedPage");

            IsNewsPinned = IsPinTileExists(PageEnum.News.ToString()) ? pinned : notPinned;
            IsHeroesPinned = IsPinTileExists(PageEnum.Heroes.ToString()) ? pinned : notPinned;
            IsItemsPinned = IsPinTileExists(PageEnum.Items.ToString()) ? pinned : notPinned;
            IsLeaderboardsPinned = IsPinTileExists(PageEnum.Leaderboards.ToString()) ? pinned : notPinned;
        }

        #endregion
    }
    #endregion

    #region AboutPartViewModel
    public class AboutPartViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public AboutPartViewModel()
        {
            var v = Package.Current.Id.Version;
            Version = $"{ResourceLoader.GetForCurrentView().GetString("Version")} {v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
        }

        public Uri Logo => Package.Current.Logo;

        public string DisplayName => Package.Current.DisplayName;

        public string Publisher => Package.Current.PublisherDisplayName;

        public string Version { get; set; }
        #endregion

        #region Public Methods
        public void ShareApplication()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

            Share share = new Share(dataTransferManager, DisplayName, Constants.ApplicationStoreLink, Share.ShareType.Application);
            share.ShareContent();
        }
        #endregion
    }
    #endregion

    #region VersionHistoryPartViewModel
    public class VersionHistoryPartViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public ObservableCollection<VersionHistory> VersionHistoryList { get; private set; }

        public VersionHistoryPartViewModel()
        {
            SeedVersionHistoryList();
        }
        #endregion

        #region Private Methods
        private void SeedVersionHistoryList()
        {
            VersionHistoryList = new ObservableCollection<VersionHistory>()
            {
                new VersionHistory()
                {
                    Title = $"v1.2.4 ({string.Format("{0:D}", new DateTime(2017, 11, 10))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v2")}" }
                },

                new VersionHistory()
                {
                    Title = $"v1.2.3 ({string.Format("{0:D}", new DateTime(2017, 11, 9))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v15")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v14")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v0")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.2.2 ({string.Format("{0:D}", new DateTime(2017, 8, 31))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v2")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v13")}" }
                },

                new VersionHistory()
                {
                    Title = $"v1.2.1 ({string.Format("{0:D}", new DateTime(2017, 8, 5))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v12")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.2.0 ({string.Format("{0:D}", new DateTime(2017, 7, 5))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.9 ({string.Format("{0:D}", new DateTime(2017, 5, 15))})",
                    VersionDescriptions = new List<string>(){$@"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.8 ({string.Format("{0:D}", new DateTime(2017, 3, 11))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v11")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v10")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v8")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v9")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.7 ({string.Format("{0:D}", new DateTime(2017, 3, 11))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v8")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v9")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.6 ({string.Format("{0:D}", new DateTime(2017, 2, 13))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.5 ({string.Format("{0:D}", new DateTime(2017, 2, 1))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v7")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.4 ({string.Format("{0:D}", new DateTime(2017, 1, 29))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.3 ({string.Format("{0:D}", new DateTime(2017, 1, 24))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.2 ({string.Format("{0:D}", new DateTime(2017, 1, 21))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v6")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v5")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v4")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v3")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.1 ({string.Format("{0:D}", new DateTime(2016, 12, 17))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"v0")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v1")}",
                                                             $"- {ResourceLoader.GetForCurrentView().GetString($"v2")}"}
                },

                new VersionHistory()
                {
                    Title = $"v1.1.0 ({string.Format("{0:D}", new DateTime(2016, 12, 7))})",
                    VersionDescriptions = new List<string>(){$"- {ResourceLoader.GetForCurrentView().GetString($"FirstRelease")}",
                                                                 Environment.NewLine }
                }
            };
        }
        #endregion
    }
    #endregion
}