using System;
using System.Text;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Resources;
using Microsoft.Services.Store.Engagement;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Dota2Handbook.ViewModels
{
    using Utilities;
    using Infrastructure;

    public class FeedbackViewModel : ViewModelBase
    {
        #region Properties & Constructor
        public string Body { get; set; }

        bool? _addInformation;
        public bool? AddInformation
        {
            get { return _addInformation; }
            set => Set(ref _addInformation, value);
        }

        Visibility _feedBackHubVisibility;
        public Visibility FeedBackHubVisibility
        {
            get { return _feedBackHubVisibility; }
            set => Set(ref _feedBackHubVisibility, value);
        }

        public FeedbackViewModel()
        {
            AddInformation = true;

            CheckFeedbachHubAvailability();
        }
        #endregion

        #region Public Methods
        public async void SendFeedback()
        {
            if (Connection.HasInternetAccess)
            {
                if (string.IsNullOrWhiteSpace(Body))
                    return;

                EmailMessage emailMessage = new EmailMessage();

                emailMessage.To.Add(new EmailRecipient(Constants.EmailAddress));
                emailMessage.Subject = $"{Information.ApplicationName} - {Information.ApplicationVersion}";
                emailMessage.Body = Body;

                if (AddInformation.HasValue && AddInformation.Value)
                    emailMessage.Body = $"{emailMessage.Body} {GetSystemInformation()}";

                await EmailManager.ShowComposeNewEmailAsync(emailMessage);
            }

            else
                await DialogBox.Show(Constants.InternetConnectionError, ResourceLoader.GetForCurrentView().GetString("Error")).ExecuteAsync(null, null);
        }

        public async void LaunchFeedbackHub() =>
            await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
        #endregion

        #region Private Methods
        private void CheckFeedbachHubAvailability() =>
            FeedBackHubVisibility = StoreServicesFeedbackLauncher.IsSupported() ? Visibility.Visible : Visibility.Collapsed;

        private string GetSystemInformation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Environment.NewLine);
            stringBuilder.AppendLine("------------------");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_Culture")}: {Information.Culture.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_OperatingSystem")}: {Information.OperatingSystem.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_OperatingSystemArchitecture")}: {Information.OperatingSystemArchitecture.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_OperatingSystemVersion")}: {Information.OperatingSystemVersion.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_DeviceFamily")}: {Information.DeviceFamily.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_DeviceModel")}: {Information.DeviceModel.ToString()}");
            stringBuilder.AppendLine($"{ResourceLoader.GetForCurrentView().GetString("Information_DeviceManufacturer")}: {Information.DeviceManufacturer.ToString()}");
            stringBuilder.AppendLine(Environment.NewLine);

            return stringBuilder.ToString();
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
            NavigationService.Navigate(typeof(Views.Settings), 0);

        public void GotoPin() =>
            NavigationService.Navigate(typeof(Views.Settings), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.Settings), 2);

        public void GoToVersionHistory() =>
            NavigationService.Navigate(typeof(Views.Settings), 3);
        #endregion
    }
}