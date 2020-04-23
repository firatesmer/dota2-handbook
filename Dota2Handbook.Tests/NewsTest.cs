using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xunit;

namespace Dota2Handbook.Tests
{
    using Data;

    public class NewsTest
    {
        [Theory]
        [InlineData(25)]
        [InlineData(50)]
        [InlineData(100)]
        public void NewsCollectionCountTest(int count)
        {
            Task<ObservableCollection<NewsItem>> news = NewsRepository.GetNews(count);

            Assert.Equal(count, news.Result.Count);
        }
    }
}