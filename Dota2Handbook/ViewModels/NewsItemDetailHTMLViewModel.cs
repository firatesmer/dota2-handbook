using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Data;
    using Views;
    using Utilities;

    public class NewsItemDetailHTMLViewModel : ViewModelBase
    {
        #region Properties & Constructor
        string _HTMLData;
        public string HTMLData
        {
            get { return _HTMLData; }
            set => Set(ref _HTMLData, value);
        }

        string _pageHeaderTitle;
        public string PageHeaderTitle
        {
            get { return _pageHeaderTitle; }
            set => Set(ref _pageHeaderTitle, value);
        }

        string _contentTitle;
        public string ContentTitle
        {
            get { return _contentTitle; }
            set => Set(ref _contentTitle, value);
        }

        NewsItem NewsItem;

        public NewsItemDetailHTMLViewModel()
        {

        }
        #endregion

        #region Public Methods
        public void Share()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

            Share share = new Share(dataTransferManager, NewsItem.Title, NewsItem.Url);
            share.ShareContent();
        } 
        #endregion

        #region Navigation
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            NewsItem = (NewsItem)parameter;

            PageHeaderTitle = NewsItem.Title;
            HTMLData = NewsItem.Contents;
            ContentTitle = NewsItem.Title;

            await Task.CompletedTask;
        }

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