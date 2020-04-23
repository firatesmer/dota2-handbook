using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Dota2Handbook.Data
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public int cost { get; set; }
        public int secret_shop { get; set; }
        public int side_shop { get; set; }
        public int recipe { get; set; }
        public string localized_name { get; set; }
        public BitmapImage Image { get; set; }
    }

    public class ItemResult
    {
        public ObservableCollection<Item> items { get; set; }
        public int status { get; set; }
    }

    public class ItemRootObject
    {
        public ItemResult result { get; set; }
    }
}