using System;
using Windows.Storage.Streams;
using Windows.ApplicationModel.DataTransfer;

namespace Dota2Handbook.Utilities
{
    public class Share
    {
       string _uri;
       string _title;
       ShareType _shareType;
       DataTransferManager _dataTransferManager;

        public Share(DataTransferManager dataTransferManager, string title, string uri, ShareType shareType = ShareType.WebLink)
        {
            _dataTransferManager = dataTransferManager;
            _title = title;
            _uri = uri;
            _shareType = shareType;
        }

        public void ShareContent()
        {
            if (DataTransferManager.IsSupported())
            {
                _dataTransferManager.DataRequested += DataTransferManager_DataRequested;

                DataTransferManager.ShowShareUI();
            }
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage requestData = args.Request.Data;
            requestData.Properties.Title = _title;
            requestData.SetWebLink(new Uri(_uri));

            switch (_shareType)
            {
                case ShareType.Application:
                    requestData.Properties.Square30x30Logo = RandomAccessStreamReference.CreateFromUri(Windows.ApplicationModel.Package.Current.Logo);
                    break;
            }

            _dataTransferManager.DataRequested -= DataTransferManager_DataRequested;
        }

        public enum ShareType
        {
            WebLink,
            Application
        }
    }
}