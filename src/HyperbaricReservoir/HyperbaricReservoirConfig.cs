using TUNING;
using UnityEngine;

// ReSharper disable ClassNeverInstantiated.Global

namespace MightyVincent
{
    public class HyperbaricLiquidReservoirConfig : LiquidReservoirConfig
    {
        public new const string ID = "HyperbaricLiquidReservoir";
        private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
        private const int WIDTH = 2;
        private const int HEIGHT = 3;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "liquidreservoir_kanim", 500, 240f,
                new[]
                {
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0],
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
                },
                new[]
                {
                    MATERIALS.ALL_METALS[0],
                    MATERIALS.PLASTICS[0]
                }, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            buildingDef.InputConduitType = CONDUIT_TYPE;
            buildingDef.OutputConduitType = CONDUIT_TYPE;
            buildingDef.Floodable = false;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 60f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            base.ConfigureBuildingTemplate(go, prefabTag);
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperbaricReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= 5;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = true;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }
    }

    public class HyperbaricGasReservoirConfig : GasReservoirConfig
    {
        public new const string ID = "HyperbaricGasReservoir";
        private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
        private const int WIDTH = 5;
        private const int HEIGHT = 3;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "gasstorage_kanim", 500, 240f,
                new[]
                {
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0],
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
                },
                new[]
                {
                    MATERIALS.ALL_METALS[0],
                    MATERIALS.PLASTICS[0]
                }, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            buildingDef.InputConduitType = CONDUIT_TYPE;
            buildingDef.OutputConduitType = CONDUIT_TYPE;
            buildingDef.Floodable = false;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 60f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            base.ConfigureBuildingTemplate(go, prefabTag);
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperbaricReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= 5;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = true;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }
    }
}