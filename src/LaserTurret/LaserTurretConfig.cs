using System.Collections.Generic;
using AsLimc.commons;
using TUNING;
using UnityEngine;

namespace AsLimc.LaserTurret
{
    public class LaserTurretConfig : VBuildingConfig {
        public const string ID = "LaserTurret";
        public override LocString name => LocStrings.LaserTurret.NAME;
        public override LocString desc => LocStrings.LaserTurret.DESC;
        public override LocString effect => LocStrings.LaserTurret.EFFECT;
        public override string id => ID;
        public override string planName => "Food";
        public override string techId => "AnimalControl";
        protected override string anim => "laser_turret_kanim";
        protected override int width => 1;
        protected override int height => 1;

        protected override Dictionary<string, float> constructionRecipe => new Dictionary<string, float>() {
            {MATERIALS.REFINED_METALS[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]}
        };
        protected override int hitpoints => BUILDINGS.HITPOINTS.TIER0;
        protected override float constructionTime => BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1;
        protected override float meltingPoint => BUILDINGS.MELTING_POINT_KELVIN.TIER1;
        protected override float temperatureModificationMassScale { get; }
        protected override BuildLocationRule buildLocationRule => BuildLocationRule.OnFoundationRotatable;
        protected override EffectorValues decor => BUILDINGS.DECOR.PENALTY.TIER2;
        protected override EffectorValues noise => NOISE_POLLUTION.NOISY.TIER0;
        protected override HashSet<Tag> overlayTags => OverlayScreen.SolidConveyorIDs;
        private int _RANGE = 7;
        private int _VISUALIZER_X = -_RANGE;
        private int _VISUALIZER_Y = 0;
        private int _VISUALIZER_WIDTH = width + _RANGE * 2;
        private int _VISUALIZER_HEIGHT = height + _RANGE;

        protected override void ConfigureBuildingDef(BuildingDef buildingDef) {
            base.ConfigureBuildingDef(buildingDef);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = BUILDINGS.ENERGY_CONSUMPTION_WHEN_ACTIVE.TIER3;
            buildingDef.ExhaustKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER0;
            buildingDef.SelfHeatKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER3;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<MiningSounds>();
            go.AddOrGet<KSelectable>();
            go.AddOrGet<LogicOperationalController>();
            var storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            var filters = new List<Tag>();
            filters.AddRange(STORAGEFILTERS.BAGABLE_CREATURES);
            filters.AddRange(STORAGEFILTERS.SWIMMING_CREATURES);
            storage.storageFilters = filters;
            storage.allowSettingOnlyFetchMarkedItems = false;
            go.AddOrGet<TreeFilterable>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.IncreaseRanchingMedium.Id;
            AddVisualizer(go, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            var turret = go.AddOrGet<LaserTurret>();
            turret.visualizerX = _VISUALIZER_X;
            turret.visualizerY = _VISUALIZER_Y;
            turret.visualizerWidth = _VISUALIZER_WIDTH;
            turret.visualizerHeight = _VISUALIZER_HEIGHT;
            AddVisualizer(go, false);
        }

        private static void AddVisualizer(GameObject go, bool movable)
        {
            var visualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
            visualizer.x = _VISUALIZER_X;
            visualizer.y = _VISUALIZER_Y;
            visualizer.width = _VISUALIZER_WIDTH;
            visualizer.height = _VISUALIZER_HEIGHT;
            visualizer.movable = movable;
            visualizer.blocking_tile_visible = false;
            go.GetComponent<KPrefabID>().instantiateFn += o => o.GetComponent<StationaryChoreRangeVisualizer>().blocking_cb = LaserTurret.AttackBlockingCallback;
        }
    }
}