namespace Dota2Handbook.Data
{
    public class Tournament
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string PrizePool { get; set; }
        public string GameVersion { get; set; }
        public string FirstPlace { get; set; }
        public string SecondPlace { get; set; }
        public string ThirdPlace { get; set; }
        public bool AreThereSemiFinalists { get; set; }
    }
}