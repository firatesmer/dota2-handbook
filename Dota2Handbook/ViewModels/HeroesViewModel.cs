using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.Resources;

using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Data;
    using Enums;
    using Views;
    using Utilities;
    using Infrastructure;
    using static Dota2Handbook.Utilities.SecondaryTilePin;

    public class HeroesViewModel : ViewModelBase
    {
        #region Properties & Constructor
        ObservableCollection<Hero> _heroList;
        public ObservableCollection<Hero> HeroList { get; private set; }
        public ObservableCollection<HeroData> HeroDataList { get; private set; }
        public ObservableCollection<Ability> HeroAbilityList { get; private set; }
        public ObservableCollection<GroupInfoList> GroupedHeroList { get; private set; }

        Hero SelectedHero;
        HeroData SelectedHeroData;

        string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (Set(ref _filter, value))
                {
                    FilterHeroes();
                    GroupHeroes();
                }
            }
        }

        public HeroesViewModel()
        {
            _heroList = new ObservableCollection<Hero>();

            GroupedHeroList = new ObservableCollection<GroupInfoList>();
            HeroList = new ObservableCollection<Hero>();
            HeroDataList = new ObservableCollection<HeroData>();
            HeroAbilityList = new ObservableCollection<Ability>();

            Start().ContinueWith(x => GetHeroesAbilities());
        }
        #endregion

        #region Public Methods
        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrWhiteSpace(sender.Text))
                sender.ItemsSource = HeroList.Select(x => x.Localized_Name);
            else
                sender.ItemsSource = null;
        }

        public void SelectedHeroChanged(object sender, ItemClickEventArgs e)
        {
            Busy.SetBusy(true);

            SelectedHero = (e.ClickedItem) as Hero;

            SelectedHeroData = HeroRepository.GetHeroData(SelectedHero.Name.Replace(Constants.HeroString, string.Empty));
            SelectedHeroData.HeroFullImage = SelectedHero.FullImage;
            SelectedHeroData.AbilityList = HeroRepository.GetHeroAbilityByHeroName(SelectedHero.Name.Replace(Constants.HeroString, string.Empty));

            NavigationService.Navigate(typeof(HeroDetail), SelectedHeroData, new SuppressNavigationTransitionInfo());
        }

        public async void Pin()
        {
            PinTileObject secondaryTile = new PinTileObject()
            {
                TileId = PageEnum.Heroes.ToString(),
                DisplayName = PageEnum.Heroes.ToString(),
                Arguments = PageEnum.Heroes.ToString(),
            };

            await PinTile(secondaryTile);

            await DialogBox.Show(ResourceLoader.GetForCurrentView().GetString("PinnedPage")).ExecuteAsync(null,null);
        }
        #endregion

        #region Private Methods
        private async Task Start()
        {
            if (Connection.HasInternetAccess)
            {
                Busy.SetBusy(true);

                await GetHeroes();
                await GetHeroesData();

                FilterHeroes();
                GroupHeroes();

                Busy.SetBusy(false);
            }

            else
                await DialogBox.Show(Constants.InternetConnectionError, ResourceLoader.GetForCurrentView().GetString("Error")).ExecuteAsync(null, null);
        }

        private async Task GetHeroes() =>
            _heroList = await HeroRepository.GetHeroes();

        private async Task GetHeroesData() =>
            HeroDataList = await HeroRepository.GetHeroesData();

        private async Task GetHeroesAbilities() =>
            HeroAbilityList = await HeroRepository.GetAbilityData();

        private void GroupHeroes()
        {
            GroupedHeroList.Clear();

            var query = from hero in HeroList
                        group hero by hero.Localized_Name[0] into g
                        orderby g.Key
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList();

                info.Key = g.GroupName;

                foreach (var item in g.Items)
                    info.Add(item);

                GroupedHeroList.Add(info);
            }
        }

        private void FilterHeroes()
        {
            if (string.IsNullOrWhiteSpace(_filter))
                _filter = string.Empty;

            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            var result = _heroList.Where(d => d.Localized_Name.ToLowerInvariant()
                         .StartsWith(lowerCaseFilter))
                         .ToList();

            var toRemove = HeroList.Except(result).ToList();

            foreach (var x in toRemove)
                HeroList.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > HeroList.Count || !HeroList[i].Equals(resultItem))
                    HeroList.Insert(i, resultItem);
            }
        }
        #endregion

        #region Navigation
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) =>
            Task.CompletedTask;

        public override Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending) =>
            Task.CompletedTask;

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
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