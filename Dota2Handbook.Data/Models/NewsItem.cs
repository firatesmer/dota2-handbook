using System.Collections.ObjectModel;

namespace Dota2Handbook.Data
{
    public class NewsItem
    {
        public string GId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool Is_External_Url { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }

        public string FeedLabel { get; set; }

        public int Date { get; set; }

        public string Feedname { get; set; }
    }

    public class AppNews
    {
        public int AppId { get; set; }

        public ObservableCollection<NewsItem> NewsItems { get; set; }
    }

    public class RootObjectNewsItem
    {
        public AppNews AppNews { get; set; }
    }
}