using Newtonsoft.Json;
using PeterHan.PLib;

namespace MightyVincent
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [Option("Liquid Reservoir Steel Mass (kg)", "The Steel Mass of Liquid Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float LiquidReservoirSteelMassKg { get; set; }

        [Option("Liquid Reservoir Plastic Mass (kg)", "The Plastic Mass of Liquid Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float LiquidReservoirPlasticMassKg { get; set; }

        [Option("Liquid Reservoir Power Consumption (watts)", "The Power Consumption of Liquid Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float LiquidReservoirPowerConsumptionWatts { get; set; }

        [Option("Liquid Reservoir Capacity Multiplier", "The Capacity Multiplier of Liquid Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public int LiquidReservoirCapacityMultiplier { get; set; }

        [Option("Gas Reservoir Steel Mass (kg)", "The Steel Mass of Gas Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float GasReservoirSteelMassKg { get; set; }

        [Option("Gas Reservoir Plastic Mass (kg)", "The Plastic Mass of Gas Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float GasReservoirPlasticMassKg { get; set; }

        [Option("Gas Reservoir Power Consumption (watts)", "The Power Consumption of Gas Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public float GasReservoirPowerConsumptionWatts { get; set; }

        [Option("Gas Reservoir Capacity Multiplier", "The Capacity Multiplier of Gas Reservoir. (Restart Needed)", null)]
        [JsonProperty]
        public int GasReservoirCapacityMultiplier { get; set; }

        public Settings()
        {
            LiquidReservoirSteelMassKg = 800f;
            LiquidReservoirPlasticMassKg = 100f;
            LiquidReservoirPowerConsumptionWatts = 40f;
            LiquidReservoirCapacityMultiplier = 4;
            GasReservoirSteelMassKg = 800f;
            GasReservoirPlasticMassKg = 100f;
            GasReservoirPowerConsumptionWatts = 40f;
            GasReservoirCapacityMultiplier = 4;
        }
    }
}