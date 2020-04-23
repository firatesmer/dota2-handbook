using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Dota2Handbook.Data
{
    public partial class Hero
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Localized_Name { get; set; }
        public BitmapImage LargeImage { get; set; }
        public BitmapImage FullImage { get; set; }
    }

    public class ResultHeroes
    {
        public ObservableCollection<Hero> Heroes { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
    }

    public class RootObjectHero
    {
        public ResultHeroes Result { get; set; }
    }
}