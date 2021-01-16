using System.Collections.Generic;
using TUNING;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyVincent
{
    public class HyperLiquidReservoirConfig : LiquidReservoirConfig
    {
        public new const string ID = "HyperbaricLiquidReservoir";
        private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
        private const int WIDTH = 2;
        private const int HEIGHT = 3;

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "liquidreservoir_kanim", 500, 240f,
                new[]
                {
//                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
                    Patches.settings.LiquidReservoirSteelMassKg,
//                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
                    Patches.settings.LiquidReservoirPlasticMassKg,
                },
                new[]
                {
                    SimHashes.Steel.ToString(),
                    MATERIALS.PLASTICS[0]
                }, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
            buildingDef.InputConduitType = CONDUIT_TYPE;
            buildingDef.OutputConduitType = CONDUIT_TYPE;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
            {
                LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT,
                    STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_INACTIVE, false, false)
            };
            // modding
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = Patches.settings.LiquidReservoirPowerConsumptionWatts;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));

            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidReservoir");
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            base.ConfigureBuildingTemplate(go, prefabTag);
            go.AddOrGet<LogicOperationalController>();
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= Patches.settings.LiquidReservoirCapacityMultiplier;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = false;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }
    }

    public class HyperGasReservoirConfig : GasReservoirConfig
    {
        public new const string ID = "HyperbaricGasReservoir";
        private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
        private const int WIDTH = 5;
        private const int HEIGHT = 3;

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "gasstorage_kanim", 500, 240f,
                new[]
                {
//                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
                    Patches.settings.GasReservoirSteelMassKg,
//                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
                    Patches.settings.GasReservoirPlasticMassKg,
                },
                new[]
                {
                    SimHashes.Steel.ToString(),
                    MATERIALS.PLASTICS[0]
                }, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
            buildingDef.InputConduitType = CONDUIT_TYPE;
            buildingDef.OutputConduitType = CONDUIT_TYPE;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
            {
                LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT,
                    STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_INACTIVE, false, false)
            };
            // modding
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = Patches.settings.GasReservoirPowerConsumptionWatts;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));

            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasReservoir");
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            base.ConfigureBuildingTemplate(go, prefabTag);
            go.AddOrGet<LogicOperationalController>();
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= Patches.settings.GasReservoirCapacityMultiplier;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = false;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }
    }
}