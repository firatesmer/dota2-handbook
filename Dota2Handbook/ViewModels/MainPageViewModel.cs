using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.Resources;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Views;
    using Enums;
    using Data;
    using Utilities;
    using Infrastructure;
    using Services.SettingsServices;
    using static Dota2Handbook.Utilities.SecondaryTilePin;

    public class MainPageViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public IList<string> ComboBoxItemList = Constants.NewsCountList;

        ObservableCollection<NewsItem> _news;
        ObservableCollection<NewsItem> _updates;
        public ObservableCollection<NewsItem> News { get; private set; }
        public ObservableCollection<NewsItem> Updates { get; private set; }

        SettingsService _settings;
  
        NewsItem SelectedNewsItem { get; set; }

        string _newsCount = Constants.DefaultNewsCount;
        public string NewsCount
        {
            get { return _newsCount; }
            set => Set(ref _newsCount, value);

        }

        string _updatesCount = Constants.DefaultUpdateCount;
        public string UpdatesCount
        {
            get { return _updatesCount; }
            set => Set(ref _updatesCount, value);
            
        }

        string _filterNews;
        public string FilterNews
        {
            get { return _filterNews; }
            set
            {
                if (Set(ref _filterNews, value))
                    PerformFilteringForNews();
            }
        }

        string _filterUpdates;
        public string FilterUpdates
        {
            get { return _filterUpdates; }
            set
            {
                if (Set(ref _filterUpdates, value))
                    PerformFilteringForUpdates();
            }
        }

        public MainPageViewModel()
        {
            News = new ObservableCollection<NewsItem>();
            Updates = new ObservableCollection<NewsItem>();

            _settings = SettingsService.Instance;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
        #endregion

        #region Public Methods
        public void NewsItemTapped(object sender, object e)
        {
            SelectedNewsItem = ((ListView)sender).SelectedItem as NewsItem;

            if (SelectedNewsItem.Feedname.Equals(Constants.NewsFeedStringForUpdates, StringComparison.OrdinalIgnoreCase) &&
                _settings.IsReadingViewEnabled)
                NavigationService.Navigate(typeof(NewsDetailHTML), SelectedNewsItem, new SuppressNavigationTransitionInfo());
            else
                NavigationService.Navigate(typeof(NewsDetail), SelectedNewsItem, new SuppressNavigationTransitionInfo());
        }

        public async void Pin()
        {
            PinTileObject secondaryTile = new PinTileObject()
            {
                TileId = PageEnum.News.ToString(),
                DisplayName = PageEnum.News.ToString(),
                Arguments = PageEnum.News.ToString(),
            };

            await PinTile(secondaryTile);

            await DialogBox.Show(ResourceLoader.GetForCurrentView().GetString("PinnedPage")).ExecuteAsync(null, null);
        }

        public async Task Start()
        {
            if (Connection.HasInternetAccess)
            {
                Busy.SetBusy(true);

                await GetNews();
                await GetUpdates();

                Busy.SetBusy(false);
            }
            else
                await DialogBox.Show(Constants.InternetConnectionError, ResourceLoader.GetForCurrentView().GetString("Error")).ExecuteAsync(null, null);
        }

        public async Task GetNews()
        {
            _news = await NewsRepository.GetNews(Convert.ToInt32(NewsCount));
            PerformFilteringForNews();
        }

        public async Task GetUpdates()
        {
            _updates = await NewsRepository.GetUpdates(Convert.ToInt32(UpdatesCount));
            PerformFilteringForUpdates();
        }
        #endregion

        #region Private Methods
        private void PerformFilteringForNews()
        {
            if (string.IsNullOrWhiteSpace(_filterNews))
                _filterNews = string.Empty;

            var lowerCaseFilter = FilterNews.ToLowerInvariant().Trim();

            var result =
                _news.Where(d => d.Title.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            var toRemove = News.Except(result).ToList();

            foreach (var x in toRemove)
                News.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > News.Count || !News[i].Equals(resultItem))
                    News.Insert(i, resultItem);
            }
        }

        private void PerformFilteringForUpdates()
        {
            if (string.IsNullOrWhiteSpace(_filterUpdates))
                _filterUpdates = string.Empty;

            var lowerCaseFilter = FilterUpdates.ToLowerInvariant().Trim();

            var result =
                _updates.Where(d => d.Title.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            var toRemove = Updates.Except(result).ToList();

            foreach (var x in toRemove)
                Updates.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Updates.Count || !Updates[i].Equals(resultItem))
                    Updates.Insert(i, resultItem);
            }
        }
        #endregion

        #region Navigation
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) => 
            await Task.CompletedTask;

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending) => 
            await Task.CompletedTask;

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
        #endregion

        #region Settings
        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Settings), 0);

        public void GotoPin() =>
            NavigationService.Navigate(typeof(Settings), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Settings), 2);

        public void GoToVersionHistory() =>
            NavigationService.Navigate(typeof(Settings), 3);
        #endregion
    }
}