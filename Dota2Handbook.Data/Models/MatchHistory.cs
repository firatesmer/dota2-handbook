using System.Collections.ObjectModel;

namespace Dota2Handbook.Data
{
    public class Player
    {
        public object account_id { get; set; }
        public int player_slot { get; set; }
        public int hero_id { get; set; }
    }

    public class Match
    {
        public object match_id { get; set; }
        public object match_seq_num { get; set; }
        public int start_time { get; set; }
        public int lobby_type { get; set; }
        public int radiant_team_id { get; set; }
        public int dire_team_id { get; set; }
        public ObservableCollection<Player> players { get; set; }
    }

    public class ResultMatch
    {
        public int status { get; set; }
        public int num_results { get; set; }
        public int total_results { get; set; }
        public int results_remaining { get; set; }
        public ObservableCollection<Match> matches { get; set; }
    }

    public class RootObjectMatch
    {
        public ResultMatch result { get; set; }
    }
}