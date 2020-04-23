using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Enums;
    using Views;
    using Utilities;
    using Infrastructure;
    using static Dota2Handbook.Utilities.SecondaryTilePin;

    public class LeaderboardsViewModel : ViewModelBase
    {
        #region Properties & Constructor
        string _itemUrl;
        public string ItemUrl
        {
            get { return _itemUrl; }
            set => Set(ref _itemUrl, value);
        }

        public LeaderboardsViewModel() =>
            ItemUrl = Constants.WorldLeaderboardsURL;
        #endregion

        #region Public Methods
        public async void Pin()
        {
            PinTileObject secondaryTile = new PinTileObject()
            {
                TileId = PageEnum.Leaderboards.ToString(),
                DisplayName = PageEnum.Leaderboards.ToString(),
                Arguments = PageEnum.Leaderboards.ToString(),
            };

            await PinTile(secondaryTile);

            await DialogBox.Show(ResourceLoader.GetForCurrentView().GetString("PinnedPage")).ExecuteAsync(null, null);
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