using System;
using TUNING;
using UnityEngine;
using Object = UnityEngine.Object;

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
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "liquidreservoir_kanim", 500, 240f,
                new[]
                {
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER6[0],
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
                },
                new[]
                {
                    SimHashes.Steel.ToString(),
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
            go.AddOrGet<LogicOperationalController>();
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperbaricReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= 6;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = false;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigurePreview(def, go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigureUnderConstruction(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigureComplete(go);
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
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "gasstorage_kanim", 500, 240f,
                new[]
                {
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER6[0],
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
                },
                new[]
                {
                    SimHashes.Steel.ToString(),
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
            go.AddOrGet<LogicOperationalController>();
            Object.DestroyImmediate(go.GetComponent<Reservoir>());
            go.AddOrGet<HyperbaricReservoir>();
            var storage = go.GetComponent<Storage>();
            storage.capacityKg *= 6;
            var consumer = go.GetComponent<ConduitConsumer>();
            consumer.alwaysConsume = false;
            consumer.capacityKG = storage.capacityKg;
            var dispenser = go.GetComponent<ConduitDispenser>();
            dispenser.alwaysDispense = true;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigurePreview(def, go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigureUnderConstruction(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            base.DoPostConfigureComplete(go);
        }
    }
}