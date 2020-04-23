using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Dota2Handbook.Utilities
{
    public static class SecondaryTilePin
    {
        public class PinTileObject
        {
            public string TileId { get; set; }
            public string DisplayName { get; set; }
            public string Arguments { get; set; }
            public Uri Square44x44Logo { get; set; } = new Uri("ms-appx:///Assets/Square44x44Logo.scale-100.png");
            public Uri Square71x71Logo { get; set; } = new Uri("ms-appx:///Assets/Square71x71Logo.scale-100.png");
            public Uri Square150x150Logo { get; set; } = new Uri("ms-appx:///Assets/Square150x150Logo.scale-100.png");
            public Uri Wide310x150Logo { get; set; } = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png");
            public Uri Square310x310Logo { get; set; } = new Uri("ms-appx:///Assets/Square310x310Logo.scale-100.png");
            public bool RoamingEnabled { get; set; }
            public bool ShowNameOnSquare150x150Logo { get; set; } = true;
            public bool ShowNameOnWide310x150Logo { get; set; } = true;
            public bool ShowNameOnSquare310x310Logo { get; set; } = true;
        }

        public static async Task PinTile(PinTileObject secondaryTileObject)
        {
            SecondaryTile tile = new SecondaryTile(secondaryTileObject.TileId,
                                                   secondaryTileObject.DisplayName,
                                                   secondaryTileObject.Arguments,
                                                   secondaryTileObject.Square150x150Logo,
                                                   TileSize.Default);

            tile.RoamingEnabled = secondaryTileObject.RoamingEnabled;

            tile.VisualElements.Square44x44Logo = secondaryTileObject.Square44x44Logo;
            tile.VisualElements.Square71x71Logo = secondaryTileObject.Square71x71Logo;
            tile.VisualElements.Wide310x150Logo = secondaryTileObject.Wide310x150Logo;
            tile.VisualElements.Square310x310Logo = secondaryTileObject.Square310x310Logo;

            tile.VisualElements.ShowNameOnSquare150x150Logo = secondaryTileObject.ShowNameOnSquare150x150Logo;
            tile.VisualElements.ShowNameOnWide310x150Logo = secondaryTileObject.ShowNameOnWide310x150Logo;
            tile.VisualElements.ShowNameOnSquare310x310Logo = secondaryTileObject.ShowNameOnSquare310x310Logo;

            await tile.RequestCreateAsync();
        }

        public static bool IsPinTileExists(string tileId) =>
            SecondaryTile.Exists(tileId);
    }
}
