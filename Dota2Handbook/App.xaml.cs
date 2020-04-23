using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.Globalization;
using Microsoft.Azure.Mobile;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Microsoft.Services.Store.Engagement;
using Microsoft.Azure.Mobile.Analytics;
using Template10.Common;
using Template10.Controls;

namespace Dota2Handbook
{
    using Utilities;
    using Infrastructure;
    using Services.SettingsServices;
    using Windows.UI.Xaml.Controls;

    [Bindable]
    sealed partial class App : BootStrapper
    {
        #region Properties & Constructor
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings
            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;

            Constants.LanguageCode = settings.LanguageCode;
            Constants.Language = settings.Language;
            #endregion

            MobileCenter.Start(Constants.Azure_APIKey, typeof(Analytics));

            ApplicationLanguages.PrimaryLanguageOverride = Constants.LanguageCode.Split('_')[0];
        }
        #endregion

        #region Public Methods
        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            Constants.InternetConnectionError = ResourceLoader.GetForCurrentView().GetString("InternetConnectionError");
            Connection connection = new Connection();

            StatusBarCustomization();
            RegisterToastNotification(args);
            RegisterEngagementNotification();

            LaunchActivatedEventArgs eventArgs = (LaunchActivatedEventArgs)args;

            // TODO: add your long-running task here
            if (eventArgs != null)
                if (!string.IsNullOrWhiteSpace(eventArgs.Arguments))
                {
                    string pageName = $"Dota2Handbook.Views.{eventArgs.Arguments}";
                    await NavigationService.NavigateAsync(Type.GetType(pageName));
                }
                else
                    await NavigationService.NavigateAsync(typeof(Views.News));
        }
        #endregion

        #region Private Methods
        private async void RegisterEngagementNotification()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        private void RegisterToastNotification(IActivatedEventArgs args)
        {
            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

                StoreServicesEngagementManager engagementManagerForLaunch = StoreServicesEngagementManager.GetDefault();
                string originalArgs = engagementManagerForLaunch.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);
            }
        }

        private void StatusBarCustomization()
        {
            if (ApiInformation.IsTypePresent(Constants.WINDOWS_API_StatusBar))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
        }
        #endregion
    }
}