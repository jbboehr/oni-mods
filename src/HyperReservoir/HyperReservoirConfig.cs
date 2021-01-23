using System.Collections.Generic;
using AsLimc.commons;
using TUNING;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR;

namespace AsLimc.HyperReservoir {
    public abstract class HyperReservoirConfig : VBuildingConfig {
        public override string techId => "ValveMiniaturization";
        public override string planName => "Base";
        protected abstract List<Tag> storageFilters { get; }
        protected abstract float capacityKg { get; }
        protected abstract ConduitType conduitType { get; }
        protected abstract float energyConsumptionWhenActive { get; }

        protected override void ConfigureBuildingDef(BuildingDef buildingDef) {
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "HollowMetal";

            // 资源
            buildingDef.InputConduitType = conduitType;
            buildingDef.OutputConduitType = conduitType;
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);

            // 电力
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = energyConsumptionWhenActive;
            buildingDef.ExhaustKilowattsWhenActive = 0f;
            buildingDef.SelfHeatKilowattsWhenActive = 4f;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);

            // 信号
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port> {
                LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), LOGIC_PORT, LOGIC_PORT_ACTIVE, LOGIC_PORT_INACTIVE)
            };
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag) {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<SmartReservoir>();
            go.AddOrGet<HyperReservoir>();
            go.AddOrGet<EnergyConsumerEx>();

            var storage = BuildingTemplates.CreateDefaultStorage(go);
            storage.showDescriptor = true;
            storage.allowItemRemoval = false;
            storage.storageFilters = storageFilters;
            storage.capacityKg = capacityKg;
            storage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
            // storage.showCapacityStatusItem = true;
            VUtils.TrySetField(typeof(Storage), "showCapacityStatusItem", storage, true);
            // storage.showCapacityAsMainStatus = true;
            VUtils.TrySetField(typeof(Storage), "showCapacityAsMainStatus", storage, true);

            var conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = conduitType;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.alwaysConsume = false;
            conduitConsumer.capacityKG = storage.capacityKg;

            var conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.conduitType = conduitType;
            conduitDispenser.alwaysDispense = true;
        }

        public override void DoPostConfigureComplete(GameObject go) {
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
            go.AddOrGetDef<StorageController.Def>();
        }
    }

    public class HyperLiquidReservoirConfig : HyperReservoirConfig {
        public const string ID = "HyperbaricLiquidReservoir";
        public override LocString name => LocStrings.HyperLiquidReservoir.NAME;
        public override LocString desc => LocStrings.HyperLiquidReservoir.DESC;
        public override LocString effect => LocStrings.HyperLiquidReservoir.EFFECT;
        public override string id => ID;
        protected override string anim => "liquidreservoir_kanim";
        protected override int width => 2;
        protected override int height => 3;

        protected override Dictionary<string, float> constructionRecipe => new Dictionary<string, float> {
            {SimHashes.Steel.ToString(), HyperReservoirSettings.Get().LiquidReservoirSteelMassKg},
            {MATERIALS.PLASTICS[0], HyperReservoirSettings.Get().LiquidReservoirPlasticMassKg}
        };

        protected override List<Tag> storageFilters => STORAGEFILTERS.LIQUIDS;
        protected override float capacityKg => 5000f * HyperReservoirSettings.Get().LiquidReservoirCapacityMultiplier;
        protected override HashSet<Tag> overlayTags => OverlayScreen.LiquidVentIDs;
        protected override HashedString viewMode => OverlayModes.LiquidConduits.ID;
        protected override ConduitType conduitType => ConduitType.Liquid;
        protected override float energyConsumptionWhenActive => HyperReservoirSettings.Get().LiquidReservoirPowerConsumptionWatts;
    }

    public class HyperGasReservoirConfig : HyperReservoirConfig {
        public const string ID = "HyperbaricGasReservoir";
        public override LocString name => LocStrings.HyperGasReservoir.NAME;
        public override LocString desc => LocStrings.HyperGasReservoir.DESC;
        public override LocString effect => LocStrings.HyperGasReservoir.EFFECT;
        public override string id => ID;
        protected override string anim => "gasstorage_kanim";
        protected override int width => 5;
        protected override int height => 3;

        protected override Dictionary<string, float> constructionRecipe => new Dictionary<string, float> {
            {SimHashes.Steel.ToString(), HyperReservoirSettings.Get().GasReservoirSteelMassKg},
            {MATERIALS.PLASTICS[0], HyperReservoirSettings.Get().GasReservoirPlasticMassKg}
        };

        protected override List<Tag> storageFilters => STORAGEFILTERS.GASES;
        protected override float capacityKg => 150f * HyperReservoirSettings.Get().GasReservoirCapacityMultiplier;
        protected override HashSet<Tag> overlayTags => OverlayScreen.GasVentIDs;
        protected override HashedString viewMode => OverlayModes.GasConduits.ID;
        protected override ConduitType conduitType => ConduitType.Gas;
        protected override float energyConsumptionWhenActive => HyperReservoirSettings.Get().GasReservoirPowerConsumptionWatts;
    }
}