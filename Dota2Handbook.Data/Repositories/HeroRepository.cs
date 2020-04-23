using System;
using System.Linq;
using System.Net.Http;
using Windows.UI.Core;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2Handbook.Data
{
    using Infrastructure;

    /// <summary>
    /// Repository of Heroes
    /// </summary>
    public static class HeroRepository
    {
        static ObservableCollection<HeroData> HeroDataList;
        static ObservableCollection<Ability> AbilityList;

        /// <summary>
        /// Gets all heroes
        /// </summary>
        /// <returns>List of Heroes</returns>
        public static async Task<ObservableCollection<Hero>> GetHeroes()
        {
            ObservableCollection<Hero> heroes = new ObservableCollection<Hero>();

            using (HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.HeroesURL, Constants.LanguageCode, Constants.Stream_APIKey)))
                heroes = JsonConvert.DeserializeObject<RootObjectHero>(httpResponseMessage.Content.ReadAsStringAsync().Result).Result.Heroes;

            heroes = new ObservableCollection<Hero>(heroes.OrderBy(h => h.Localized_Name));

            // Added for avoiding thread error for unit test
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () =>
                            {
                                foreach (Hero item in heroes)
                                {
                                    item.LargeImage = new BitmapImage(new Uri(string.Format(Constants.HeroLargeImageURL, item.Name.Replace(Constants.HeroString, string.Empty))));
                                    item.FullImage = new BitmapImage(new Uri(string.Format(Constants.HeroFullImageURL, item.Name.Replace(Constants.HeroString, string.Empty))));
                                }
                            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return heroes;
        }

        /// <summary>
        /// Gets all heroes' information
        /// </summary>
        /// <returns>List of Heroes' Information</returns>
        public static async Task<ObservableCollection<HeroData>> GetHeroesData()
        {
            if (HeroDataList != null)
                return HeroDataList;

            HeroDataList = new ObservableCollection<HeroData>();

            using (HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.HeroesDataURL, Constants.Language)))
            {
                dynamic jsonString = JsonConvert.DeserializeObject(httpResponseMessage.Content.ReadAsStringAsync().Result);

                foreach (var item in jsonString)
                {
                    HeroData heroData = new HeroData();
                    heroData.id = (item as JProperty).Name;
                    heroData.name = (item as JProperty).Value["name"].ToString();
                    heroData.Bio = (item as JProperty).Value["bio"].ToString();
                    heroData.atk = (item as JProperty).Value["atk"].ToString();
                    heroData.atk_l = (item as JProperty).Value["atk_l"].ToString();
                    heroData.roles = (item as JProperty).Value["roles"].ToString();
                    heroData.Roles_L = (item as JProperty).Value["roles_l"].ToString();

                    HeroDataList.Add(heroData);
                }
            }

            return HeroDataList;
        }

        /// <summary>
        /// Gets all heroes' abilities
        /// </summary>
        /// <returns>List of Heroes' Abilities</returns>
        public static async Task<ObservableCollection<Ability>> GetAbilityData()
        {
            AbilityList = new ObservableCollection<Ability>();

            using (HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.HeroAbilityDataURL, Constants.Language)))
            {
                dynamic jsonString = JsonConvert.DeserializeObject(httpResponseMessage.Content.ReadAsStringAsync().Result);

                foreach (var item in jsonString)
                {
                    IEnumerable abilityList = (item as JProperty).First.Values();

                    foreach (var item2 in abilityList)
                    {
                        Ability ability = new Ability();

                        ability.desc = StringMethods.TrimEnd((item2 as JObject)["desc"].ToString().Replace("\r\n\r\n", "\r\n"), "\r\n");

                        if (ability.desc.StartsWith(Constants.HeroSpecialBonusString))
                            continue;

                        ability.SkillName = ((item2 as JObject).Parent as JProperty).Name;
                        ability.dname = (item2 as JObject)["dname"].ToString();
                        ability.affects = Regex.Replace(StringMethods.TrimEnd((item2 as JObject)["affects"].ToString(), @"<br />"), @"(\</?font(.*?)/?\>)", string.Empty);
                        ability.notes = (item2 as JObject)["notes"].ToString();
                        ability.dmg = (item2 as JObject)["dmg"].ToString();
                        ability.attrib = (item2 as JObject)["attrib"].ToString();
                        ability.cmb = Regex.Replace((item2 as JObject)["cmb"].ToString(), "<.*?>", string.Empty).Trim();
                        ability.lore = (item2 as JObject)["lore"].ToString();
                        ability.hurl = (item2 as JObject)["hurl"].ToString();

                        AbilityList.Add(ability);
                    }

                    break;
                }
            }

            return AbilityList;
        }

        /// <summary>
        /// Gets selected hero's information
        /// </summary>
        /// <param name="heroName">Selected Hero's Name</param>
        /// <returns>HeroData</returns>
        public static HeroData GetHeroData(string heroName) =>
            HeroDataList.Where(h => h.id.Equals(heroName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        /// <summary>
        /// Gets selected hero's abilities
        /// </summary>
        /// <param name="heroName">Selected Hero's Abilities</param>
        /// <returns>Hero Ability List</returns>
        public static List<Ability> GetHeroAbilityByHeroName(string heroName)
        {
            List<Ability> selectedHeroAbilityList = AbilityList.Where(a => a.hurl.Equals(heroName, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (Ability ability in selectedHeroAbilityList)
                ability.Image = new BitmapImage(new Uri(string.Format(Constants.HeroAbilityImageURL, ability.SkillName)));

            return selectedHeroAbilityList;
        }
    }
}