using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xunit;

namespace Dota2Handbook.Tests
{
    using Data;

    public class ItemsTest
    {
        [Fact]
        public void ItemCollectionNotEmptyTest()
        {
            Task<ObservableCollection<Item>> items = ItemRepository.GetItems();

            Assert.True(items.Result.Any());
        }
    }
}