using Windows.Networking.Connectivity;
using Microsoft.Toolkit.Uwp.Connectivity;

namespace Dota2Handbook.Utilities
{
    public class Connection
    {
        public static bool HasInternetAccess { get; private set; }

        public Connection()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformationOnNetworkStatusChanged;

            CheckInternetAccess();
        }

        private void NetworkInformationOnNetworkStatusChanged(object sender) => 
            CheckInternetAccess();

        private void CheckInternetAccess() => 
            HasInternetAccess = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
    }
}