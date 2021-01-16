using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace MightyVincent
{
    public class LaserTurretConfig : IBuildingConfig
    {
        public const string ID = "LaserTurret";

        private const string _ANIM = "laser_turret_kanim";
        private const int _WIDTH = 1;
        private const int _HEIGHT = 1;
        private const int _RANGE = 7;
        private const int _VISUALIZER_X = -_RANGE;
        private const int _VISUALIZER_Y = 0;
        private const int _VISUALIZER_WIDTH = _WIDTH + _RANGE * 2;
        private const int _VISUALIZER_HEIGHT = _HEIGHT + _RANGE;

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, _WIDTH, _HEIGHT, _ANIM, 10, 10f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f,
                BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 2f;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, ID);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<MiningSounds>();
            go.AddOrGet<KSelectable>();
            go.AddOrGet<LogicOperationalController>();
            Storage storage = go.AddOrGet<Storage>();
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
            AddVisualizer(go, false);
            go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.IncreaseRanchingMedium.Id;
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