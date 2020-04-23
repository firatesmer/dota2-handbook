using System.Collections.Generic;

namespace Dota2Handbook.Data
{
    public class VersionHistory
    {
        public string Title { get; set; }

        public IList<string> VersionDescriptions { get; set; }
    }
}