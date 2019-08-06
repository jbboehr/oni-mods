using TUNING;
using UnityEngine;

namespace MightyVincent
{
    public class LaserTurretConfig : IBuildingConfig
    {
        public const string Id = "LaserTurret";
        private const string Anim = "auto_miner_kanim";
        private const int Width = 1;
        private const int Height = 1;
        private const int Range = 7;
        private const int VisualizerX = -Range;
        private const int VisualizerY = 0;
        private const int VisualizerWidth = Width + Range * 2;
        private const int VisualizerHeight = Height + Range;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(Id, Width, Height, Anim, 10, 10f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f,
                BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 2f;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, Id);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<MiningSounds>();
            go.AddOrGet<KSelectable>();
            go.AddOrGet<LogicOperationalController>();
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            AddVisualizer(go, true);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            AddVisualizer(go, false);
            go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.IncreaseRanchingMedium.Id;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
            var turret = go.AddOrGet<LaserTurret>();
            turret.visualizerX = VisualizerX;
            turret.visualizerY = VisualizerY;
            turret.visualizerWidth = VisualizerWidth;
            turret.visualizerHeight = VisualizerHeight;
            AddVisualizer(go, false);
        }

        private static void AddVisualizer(GameObject go, bool movable)
        {
            StationaryChoreRangeVisualizer visualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
            visualizer.x = VisualizerX;
            visualizer.y = VisualizerY;
            visualizer.width = VisualizerWidth;
            visualizer.height = VisualizerHeight;
            visualizer.movable = movable;
            visualizer.blocking_tile_visible = false;
        }
    }
}