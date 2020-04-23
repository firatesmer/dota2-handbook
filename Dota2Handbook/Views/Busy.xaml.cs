using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Common;
using Template10.Controls;

namespace Dota2Handbook.Views
{
    public sealed partial class Busy : UserControl
    {
        public Busy() =>
            InitializeComponent();

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(Busy), new PropertyMetadata(false));


        public static void SetBusy(bool busy) =>
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as Busy;
                if (view == null)
                    modal.ModalContent = view = new Busy();
                modal.IsModal = view.IsBusy = busy;
            });
    }
}