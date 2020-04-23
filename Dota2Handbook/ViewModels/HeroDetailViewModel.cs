using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Data;
    using Views;

    public class HeroDetailViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public HeroData SelectedHeroData;

        public HeroDetailViewModel()
        { }
        #endregion

        #region Navigation
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            SelectedHeroData = (HeroData)parameter;

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