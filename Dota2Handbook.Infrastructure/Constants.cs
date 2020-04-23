using System.Collections.Generic;

namespace Dota2Handbook.Infrastructure
{
    public static class Constants
    {
        #region API Keys
        public const string Stream_APIKey = "";
        public const string Azure_APIKey = "";
        #endregion

        #region WINDOWS APIs
        public const string WINDOWS_API_StatusBar = "Windows.UI.ViewManagement.StatusBar";
        public const string WINDOWS_API__HardwareButtons = "Windows.Phone.UI.Input.HardwareButtons";
        #endregion

        #region Application
        public static string InternetConnectionError = string.Empty;

        public const string AppId = "570";

        #region News
        public const string NewsURL = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?language=en_us&appid={0}&key={1}&count={2}&feeds={3}";
        public const string NewsFeedStringForUpdates = "steam_updates";
        public const string NewsFeedString = "pcgamer,rps,steam_community_announcements,steam_announce";
        public readonly static IList<string> NewsCountList = new List<string>() { "25", "50", "100" };
        public const string DefaultNewsCount = "25";
        public const string DefaultUpdateCount = "25";
        #endregion

        #region Heroes
        public const string HeroesURL = "https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?language={0}&key={1}";
        public const string HeroLargeImageURL = "http://cdn.dota2.com/apps/dota2/images/heroes/{0}_lg.png";
        public const string HeroFullImageURL = "http://cdn.dota2.com/apps/dota2/images/heroes/{0}_full.png";
        public const string HeroesDataURL = "http://www.dota2.com/jsfeed/heropickerdata/?l={0}";
        public const string HeroAbilityDataURL = "http://www.dota2.com/jsfeed/abilitydata/?l={0}";
        public const string HeroAbilityImageURL = "http://media.steampowered.com/apps/dota2/images/abilities/{0}_hp1.png";
        public const string HeroString = "npc_dota_hero_";
        public const string HeroSpecialBonusString = "DOTA_Tooltip_ability_special_bonus_";
        #endregion

        #region Items
        public const string ItemsURL = "https://api.steampowered.com/IEconDOTA2_570/GetGameItems/V001/?language={0}&key={1}";
        public const string ItemsDataURL = "http://www.dota2.com/jsfeed/itemdata/?l={0}";
        public const string ItemImageURL = "http://cdn.dota2.com/apps/dota2/images/items/{0}_lg.png";
        public const string ItemString = "item_";
        public const string ItemFilter1 = "DOTA_Tooltip_";
        public const string ItemFilter2 = "(Shared)";
        public const string ItemFilter3 = "River Vial";
        public const string ItemAegis = "item_aegis";
        public const string ItemCheese = "cheese";
        public const string ItemImageExtension = "_lg";
        public const string RecipeString = "recipe"; 
        #endregion

        public const string WorldLeaderboardsURL = "http://www.dota2.com/leaderboards";
        #endregion

        #region Contact
        public const string EmailAddress = "firatesmer@hotmail.com.tr";
        public const string ApplicationStoreLink = "https://www.microsoft.com/store/apps/9pp00qq232d5";
        #endregion

        #region Language
        public const string TrTR = "Türkçe";
        public const string TrEN = "İngilizce";
        public const string TrShort = "tr-TR";
        public const string EnTR = "Turkish";
        public const string EnEN = "English";
        public const string EnShort = "en-US";
        public const string EnShort2 = "en_US";

        public const string Turkish = "turkish";
        public const string English = "english";

        public static string Language = string.Empty;
        public static string LanguageCode = string.Empty;

        public readonly static IList<string> TurkishLanguageList = new List<string>() { "Türkçe", "Turkish" };
        public readonly static IList<string> EnglishLanguageList = new List<string>() { "İngilizce", "English" };
        #endregion

        #region Settings
        public const int HttpTimeoutDuration = 15;
        #endregion
    }
}