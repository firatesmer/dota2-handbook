using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Resources;

namespace Dota2Handbook.Data
{
    public class ItemData
    {
        public int id { get; set; }
        public string img { get; set; }
        public string dname { get; set; }
        public string qual { get; set; }
        public int cost { get; set; }
        public string desc { get; set; }
        public string notes { get; set; }
        public string attrib { get; set; }
        public string mc { get; set; }
        public string cd { get; set; }
        public string lore { get; set; }
        public List<string> components { get; set; }
        public bool created { get; set; }
        public BitmapImage Image { get; set; }
        public List<Item> buildsFromList { get; set; }
        public List<Item> buildsIntoList { get; set; }
        // Custom property for secret shop
        public bool requiresSecretShop { get; set; }
        public bool AvailableAtSideShop { get; set; }

        public string Cd
        {
            get
            {
                return cd;
            }

            set
            {
                if (cd.Equals("false", StringComparison.OrdinalIgnoreCase))
                    cd = string.Empty;
                else
                    cd = $"{ResourceLoader.GetForCurrentView().GetString("Cooldown")}: {cd}";
            }
        }

        public string Mc
        {
            get
            {
                return mc;
            }

            set
            {
                if (mc.Equals("false", StringComparison.OrdinalIgnoreCase))
                    mc = string.Empty;
                else
                    mc = $"{ResourceLoader.GetForCurrentView().GetString("ManaCost")}: {mc}";
            }
        }
    }
}