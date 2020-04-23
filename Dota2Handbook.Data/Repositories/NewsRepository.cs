using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Dota2Handbook.Data
{
    using Infrastructure;

    /// <summary>
    /// Repository of News
    /// </summary>
    public static class NewsRepository
    {
        /// <summary>
        /// Gets News
        /// </summary>
        /// <param name="count">Count of News</param>
        /// <returns>List of News</returns>
        public static async Task<ObservableCollection<NewsItem>> GetNews(int count)
        {
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.NewsURL, Constants.AppId, Constants.Stream_APIKey, count, Constants.NewsFeedString)))
                return JsonConvert.DeserializeObject<RootObjectNewsItem>(await httpResponseMessage.Content.ReadAsStringAsync()).AppNews.NewsItems;
        }

        /// <summary>
        /// Gets Updates
        /// </summary>
        /// <param name="count">Count of Updates</param>
        /// <returns>List of Updates</returns>
        public static async Task<ObservableCollection<NewsItem>> GetUpdates(int count)
        {
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(Constants.HttpTimeoutDuration) })
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Format(Constants.NewsURL, Constants.AppId, Constants.Stream_APIKey, count, Constants.NewsFeedStringForUpdates)))
                return JsonConvert.DeserializeObject<RootObjectNewsItem>(await httpResponseMessage.Content.ReadAsStringAsync()).AppNews.NewsItems;
        }
    }
}