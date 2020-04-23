using Windows.ApplicationModel.Resources;
using Template10.Behaviors;

namespace Dota2Handbook.Utilities
{
    public static class DialogBox
    {
        public static MessageDialogAction Show(string message, string title = "") =>
            new MessageDialogAction()
            {
                Content = message,
                Title = title
            };
    }
}