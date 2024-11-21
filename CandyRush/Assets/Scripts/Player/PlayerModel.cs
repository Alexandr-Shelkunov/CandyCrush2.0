namespace Alexender.Runer
{
    public class PlayerModel
    {
        public float DistanceScore { get; set; }
        public int CandyCount { get; set; }
        public float Weight { get; set; }

        public PlayerModel()
        {
            ResetStats();
        }

        public void ResetStats()
        {
            DistanceScore = 0f;
            CandyCount = 0;
            Weight = 40f;
        }
    }
}
