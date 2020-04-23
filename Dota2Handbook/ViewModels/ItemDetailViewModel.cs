using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using Template10.Mvvm;
using Template10.Services.NavigationService;


namespace Dota2Handbook.ViewModels
{
    using Data;
    using Views;

    public class ItemDetailViewModel : ViewModelBase
    {
        #region Properties & Constructor
        Item SelectedItem;
        public ItemData SelectedItemData { get; private set; }

        Visibility _showPanelBuildsFrom;
        public Visibility ShowPanelBuildsFrom
        {
            get { return _showPanelBuildsFrom; }
            set => Set(ref _showPanelBuildsFrom, value);
        }

        Visibility _showPanelBuildsInto;
        public Visibility ShowPanelBuildsInto
        {
            get { return _showPanelBuildsInto; }
            set => Set(ref _showPanelBuildsInto, value);
        }

        public ItemDetailViewModel()
        {

        }
        #endregion

        #region Public Methods
        public void ItemTapped(object sender, object e)
        {
            SelectedItem = ((ListView)sender).SelectedItem as Item;

            if (SelectedItem.recipe == 1)
                return;

            SelectedItemData = ItemRepository.GetItemData(SelectedItem.id);
            SelectedItemData.Image = SelectedItem.Image;
            SelectedItemData.buildsIntoList = ItemRepository.GetItemsForBuildIntoList(SelectedItem.id);
            SelectedItemData.buildsFromList = ItemRepository.GetItemsForBuildsFromList(SelectedItem.id);
            SelectedItemData.requiresSecretShop = SelectedItem.secret_shop == 0 ? false : true;
            SelectedItemData.AvailableAtSideShop = SelectedItem.side_shop == 0 ? false : true;

            NavigationService.Navigate(typeof(ItemDetail), SelectedItemData, new SuppressNavigationTransitionInfo());
        }
        #endregion

        #region Navigation
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            SelectedItemData = (ItemData)parameter;
            ShowPanelBuildsFrom = SelectedItemData.buildsFromList.Count <= 0 ? Visibility.Collapsed : Visibility.Visible;
            ShowPanelBuildsInto = SelectedItemData.buildsIntoList.Count <= 0 ? Visibility.Collapsed : Visibility.Visible;

            Busy.SetBusy(false);

            return Task.CompletedTask;
        }

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