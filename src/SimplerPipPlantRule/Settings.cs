using Newtonsoft.Json;
using PeterHan.PLib;

namespace MightyVincent
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [Option("Search Min Interval (seconds)", "min interval for seed search. (Restart Needed)", null)]
        [JsonProperty]
        public float SearchMinInterval { get; set; }

        [Option("Search Max Interval (seconds)", "max interval for seed search. (Restart Needed)", null)]
        [JsonProperty]
        public float SearchMaxInterval { get; set; }

        [Option("Plant Detection Radius (tiles)", "detection area is a square area with 'plantDetectionRadius * 2 + 1' length of the sides and centered at the target tile. (Restart Needed)", null)]
        [JsonProperty]
        public int PlantDetectionRadius { get; set; }

        [Option("Max Plants In Radius (tiles)", "max allowed plants in detection area of the target tile is 'maxPlantsInRadius + 1'. (Restart Needed)", null)]
        [JsonProperty]
        public int MaxPlantsInRadius { get; set; }

        public Settings()
        {
            SearchMinInterval = 60f;
            SearchMaxInterval = 300f;
            PlantDetectionRadius = 1;
            MaxPlantsInRadius = 0;
        }
    }
}