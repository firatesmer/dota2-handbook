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
    using Data;
    using Enums;
    using Views;
    using Utilities;
    using Infrastructure;
    using static Dota2Handbook.Utilities.SecondaryTilePin;

    public class ItemsViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public ObservableCollection<GroupInfoList> GroupedItemList { get; private set; }

        ObservableCollection<Item> _itemList;

        public ObservableCollection<Item> ItemList { get; private set; }

        public ObservableCollection<ItemData> ItemDataList { get; private set; }

        Item SelectedItem { get; set; }
        ItemData SelectedItemData { get; set; }

        string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (Set(ref _filter, value))
                {
                    FilterItems();
                    GroupItems();
                }
            }
        }

        public ItemsViewModel()
        {
            GroupedItemList = new ObservableCollection<GroupInfoList>();
            ItemList = new ObservableCollection<Item>();

            Start().ContinueWith(x => ItemRepository.FillLists());
        }
        #endregion

        #region Public Methods
        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrWhiteSpace(sender.Text))
                sender.ItemsSource = ItemList.Where(item => item.recipe == 0
                                                 && item.localized_name.StartsWith(Constants.ItemFilter1) == false
                                                 && item.localized_name.Contains(Constants.ItemFilter2) == false
                                                 && item.localized_name.Contains(Constants.ItemFilter3) == false)
                                             .OrderBy(item => item.localized_name)
                                             .Select(item => item.localized_name);
            else
                sender.ItemsSource = null;
        }

        public void SelectedItemChanged(object sender, ItemClickEventArgs e)
        {
            Busy.SetBusy(true);

            SelectedItem = (e.ClickedItem) as Item;

            SelectedItemData = ItemRepository.GetItemData(SelectedItem.id);
            SelectedItemData.Image = SelectedItem.Image;
            SelectedItemData.buildsIntoList = ItemRepository.GetItemsForBuildIntoList(SelectedItem.id);
            SelectedItemData.buildsFromList = ItemRepository.GetItemsForBuildsFromList(SelectedItem.id);
            SelectedItemData.requiresSecretShop = SelectedItem.secret_shop == 0 ? false : true;
            SelectedItemData.AvailableAtSideShop = SelectedItem.side_shop == 0 ? false : true;

            NavigationService.Navigate(typeof(ItemDetail), SelectedItemData, new SuppressNavigationTransitionInfo());
        }

        public async void Pin()
        {
            PinTileObject secondaryTile = new PinTileObject()
            {
                TileId = PageEnum.Items.ToString(),
                DisplayName = PageEnum.Items.ToString(),
                Arguments = PageEnum.Items.ToString(),
            };

            await PinTile(secondaryTile);

            await DialogBox.Show(ResourceLoader.GetForCurrentView().GetString("PinnedPage")).ExecuteAsync(null, null);
        }
        #endregion

        #region Private Methods
        private async Task Start()
        {
            if (Connection.HasInternetAccess)
            {
                Busy.SetBusy(true);

                await GetItems();
                await GetItemsData();

                FilterItems();
                GroupItems();

                Busy.SetBusy(false);
            }

            else
                await DialogBox.Show(Constants.InternetConnectionError, ResourceLoader.GetForCurrentView().GetString("Error")).ExecuteAsync(null, null);
        }

        private async Task GetItems() => 
            _itemList = await ItemRepository.GetItems();

        private async Task GetItemsData() => 
            ItemDataList = await ItemRepository.GetItemsData();

        private void GroupItems()
        {
            GroupedItemList.Clear();

            var query = from
                            item in ItemList
                        where
                            (item.cost > 0 || item.name == Constants.ItemAegis || item.name == $"{Constants.ItemString}{Constants.ItemCheese}")
                            && item.recipe == 0
                            && item.localized_name.StartsWith(Constants.ItemFilter1) == false
                            && item.localized_name.Contains(Constants.ItemFilter2) == false
                            && item.localized_name.Contains(Constants.ItemFilter3) == false
                        group
                            item by item.localized_name[0] into g
                        orderby
                            g.Key
                        select new
                        { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList()
                {
                    Key = g.GroupName
                };

                foreach (var item in g.Items)
                    info.Add(item);

                GroupedItemList.Add(info);
            }
        }

        private void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(_filter))
                _filter = string.Empty;

            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            var result = _itemList.Where(d => d.localized_name.ToLowerInvariant()
                         .Contains(lowerCaseFilter))
                        .ToList();

            var toRemove = ItemList.Except(result).ToList();

            foreach (var x in toRemove)
                ItemList.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > ItemList.Count || !ItemList[i].Equals(resultItem))
                    ItemList.Insert(i, resultItem);
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