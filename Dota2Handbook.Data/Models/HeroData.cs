using System.Collections;
using Windows.UI.Xaml.Media.Imaging;

namespace Dota2Handbook.Data
{
    public class HeroData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string atk { get; set; }
        public string atk_l { get; set; }
        public string roles { get; set; }
        public BitmapImage VerticalImage { get; set; }
        public BitmapImage HeroFullImage { get; set; }
        public IList AbilityList { get; set; }

        private string bio;
        public string Bio
        {
            get
            {
                return bio;
            }

            set
            {
                bio = value.Replace("\t", string.Empty);
            }
        }

        private string roles_l;
        public string Roles_L
        {
            get
            {
                return roles_l;
            }

            set
            {
                roles_l = string.Join(", ", value.Replace("[", string.Empty)
                                                 .Replace("]", string.Empty)
                                                 .Replace("\"", string.Empty)
                                                 .Replace("\r", string.Empty)
                                                 .Replace("\n", string.Empty)
                                                 .Trim());
            }
        }
    }
}