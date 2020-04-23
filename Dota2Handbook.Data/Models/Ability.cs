using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Resources;

namespace Dota2Handbook.Data
{
    public class Ability
    {
        public string dname { get; set; }
        public string affects { get; set; }
        public string desc { get; set; }
        public string notes { get; set; }
        public string dmg { get; set; }
        public string attrib { get; set; }
        public string cmb { get; set; }
        public string lore { get; set; }
        public string hurl { get; set; }
        public string SkillName { get; set; }
        public BitmapImage Image { get; set; }
        public string Cooldown { get; set; }
        public string ManaCost { get; set; }
        public string Cmb
        {
            get
            {
                return cmb;
            }

            set
            {
                string[] valueList = cmb.Split(' ');

                if (string.IsNullOrWhiteSpace(valueList[0]))
                    return;

                if (valueList.Length == 1)
                    Cooldown = $"{ResourceLoader.GetForCurrentView().GetString("Cooldown")} :{valueList[0]}";
                else if (valueList.Length == 2)
                {
                    ManaCost = $"{ResourceLoader.GetForCurrentView().GetString("ManaCost")}: {valueList[0]}";
                    Cooldown = $"{ResourceLoader.GetForCurrentView().GetString("Cooldown")}: {valueList[1]}";
                }
            }
        }
    }
}