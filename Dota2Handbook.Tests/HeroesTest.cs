using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xunit;

namespace Dota2Handbook.Tests
{
    using Data;

    public class HeroesTest
    {
        [Fact]
        public void HeroCollectionNotEmptyTest()
        {
            Task<ObservableCollection<Hero>> heroes = HeroRepository.GetHeroes();

            Assert.True(heroes.Result.Any());
        }
    }
}