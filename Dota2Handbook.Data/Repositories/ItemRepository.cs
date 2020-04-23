using System;
using System.Linq;
using System.Net.Http;
using Windows.UI.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2Handbook.Data
{
    using Infrastructure;

    /// <summary>
    /// Repository of Items
    /// </summary>
    public static class ItemRepository
    {
        static ObservableCollection<Item> ItemList;
        static ObservableCollection<ItemData> ItemDataList;

        public static List<Tuple<int, Item>> BuildsIntoList;
        public static List<Tuple<int, Item>> BuildsFromList;

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>List of Items</returns>
        public static async Task<ObservableCollection<Item>> GetItems()
        {
            using (HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.ItemsURL, Constants.LanguageCode, Constants.Stream_APIKey)))
                ItemList = JsonConvert.DeserializeObject<ItemRootObject>(httpResponseMessage.Content.ReadAsStringAsync().Result).result.items;

            // Added for avoiding thread error for unit test
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () =>
                            {
                                ItemList = new ObservableCollection<Item>(ItemList.OrderBy(i => i.localized_name));

                                foreach (Item item in ItemList)
                                {
                                    if (item.recipe == 1)
                                        item.Image = new BitmapImage(new Uri(string.Format(Constants.ItemImageURL, Constants.RecipeString)));
                                    else
                                        item.Image = new BitmapImage(new Uri(string.Format(Constants.ItemImageURL, item.name.Replace(Constants.ItemString, string.Empty))));
                                }

                                ItemList.Where(i => i.name == $"{Constants.ItemString}{Constants.ItemCheese}").FirstOrDefault().cost = 0;
                            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return ItemList;
        }

        /// <summary>
        /// Gets all items' information
        /// </summary>
        /// <returns>List of Items' Information</returns>
        public static async Task<ObservableCollection<ItemData>> GetItemsData()
        {
            if (ItemDataList != null)
                return ItemDataList;

            ItemDataList = new ObservableCollection<ItemData>();

            using (HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.ItemsDataURL, Constants.Language)))
            {
                dynamic jsonString = JsonConvert.DeserializeObject(httpResponseMessage.Content.ReadAsStringAsync().Result);

                foreach (var item in (jsonString as JObject).First.First)
                {
                    ItemData itemData = new ItemData();
                    itemData.id = Convert.ToInt32((item as JProperty).First["id"].ToString());
                    itemData.img = (item as JProperty).First["img"].ToString();
                    itemData.dname = (item as JProperty).First["dname"].ToString();
                    itemData.qual = (item as JProperty).First["qual"].ToString();
                    itemData.cost = Convert.ToInt32((item as JProperty).First["cost"].ToString());
                    itemData.desc = (item as JProperty).First["desc"].ToString();
                    itemData.notes = (item as JProperty).First["notes"].ToString();
                    itemData.attrib = (item as JProperty).First["attrib"].ToString();
                    itemData.mc = (item as JProperty).First["mc"].ToString();
                    itemData.cd = (item as JProperty).First["cd"].ToString();
                    itemData.lore = (item as JProperty).First["lore"].ToString();
                    itemData.components = (item as JProperty).Value["components"].ToObject<List<string>>();
                    itemData.created = Convert.ToBoolean((item as JProperty).Value["created"].ToString());

                    ItemDataList.Add(itemData);
                }

                ItemDataList.Where(i => Regex.Split(i.img, Constants.ItemImageExtension)[0].Equals(Constants.ItemCheese, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().cost = 0;
            }

            return ItemDataList;
        }

        /// <summary>
        /// Gets selected item's information
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>Item's Information</returns>
        public static ItemData GetItemData(int id) =>
             ItemDataList.Where(i => i.id == id).FirstOrDefault();

        public static void FillLists()
        {
            BuildsIntoList = new List<Tuple<int, Item>>();
            BuildsFromList = new List<Tuple<int, Item>>();

            FillBuildsFromList();
            FillBuildsIntoList();
        }

        /// <summary>
        /// Fills "Builds From List" list
        /// </summary>
        private static void FillBuildsFromList()
        {
            foreach (ItemData itemData in ItemDataList)
                if (itemData.components != null)
                    foreach (string component in itemData.components)
                        BuildsFromList.Add(Tuple.Create(itemData.id, ItemList.Where(i => i.name == $"{Constants.ItemString}{component}").FirstOrDefault()));
        }

        /// <summary>
        /// Fills "Builds Into List" list
        /// </summary>
        private static void FillBuildsIntoList()
        {
            ItemDataList.ToList().ForEach(x =>
            {
                if (x.components != null)
                    foreach (string component in x.components.Distinct())
                    {
                        Item itemTo = ItemList.Where(y => y.name == $"{Constants.ItemString}{Regex.Split(x.img, Constants.ItemImageExtension)[0]}").FirstOrDefault();
                        Item itemFrom = ItemList.Where(y => y.name == $"{Constants.ItemString}{component}").FirstOrDefault();

                        BuildsIntoList.Add(Tuple.Create(itemFrom.id, itemTo));
                    }
            });
        }

        /// <summary>
        /// Gets selected item's "Builds From List" components
        /// </summary>
        /// <param name="id">Selected Item's Id</param>
        /// <returns>Item List</returns>
        public static List<Item> GetItemsForBuildsFromList(int id) =>
             BuildsFromList.Where(x => x.Item1 == id).Select(x => x.Item2).ToList();

        /// <summary>
        /// Gets selected item's "Builds Into List" components
        /// </summary>
        /// <param name="id">Selected Item's Id</param>
        /// <returns>Item List</returns>
        public static List<Item> GetItemsForBuildIntoList(int id) =>
             BuildsIntoList.Where(x => x.Item1 == id).Select(x => x.Item2).ToList();
    }
}