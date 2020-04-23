using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace Dota2Handbook.Data
{
    public class SteamAccount
    {
        public string steamId { get; set; }
        public int communityVisibilityState { get; set; }
        public int profileState { get; set; }
        public string personaname { get; set; }
        public int lastlogoff { get; set; }
        public int pommentpermission { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public int personastate { get; set; }
        public string realname { get; set; }
        public string primaryclanId { get; set; }
        public int timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
        public string locstatecode { get; set; }

        public BitmapImage image { get; set; }
        public string customState { get; set; }
        public string customVisibilityState { get; set; }
    }

    public class Response
    {
        public List<SteamAccount> Players { get; set; }
    }

    public class RootObjectPlayer
    {
        public Response Response { get; set; }
    }
}