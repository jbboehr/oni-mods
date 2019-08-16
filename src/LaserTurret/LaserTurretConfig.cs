using TUNING;
using UnityEngine;

namespace MightyVincent
{
    public class LaserTurretConfig : IBuildingConfig
    {
        public const string ID = "LaserTurret";

//        private const string ANIM = "auto_miner_kanim";
        private const string ANIM = "laser_turret_kanim";
        private const int WIDTH = 1;
        private const int HEIGHT = 1;
        private const int RANGE = 7;
        private const int VISUALIZER_X = -RANGE;
        private const int VISUALIZER_Y = 0;
        private const int VISUALIZER_WIDTH = WIDTH + RANGE * 2;
        private const int VISUALIZER_HEIGHT = HEIGHT + RANGE;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, ANIM, 10, 10f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f,
                BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 2f;
            buildingDef.PermittedRotations = PermittedRotations.R360;
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
            turret.visualizerX = VISUALIZER_X;
            turret.visualizerY = VISUALIZER_Y;
            turret.visualizerWidth = VISUALIZER_WIDTH;
            turret.visualizerHeight = VISUALIZER_HEIGHT;
            AddVisualizer(go, false);
        }

        private static void AddVisualizer(GameObject go, bool movable)
        {
            StationaryChoreRangeVisualizer visualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
            visualizer.x = VISUALIZER_X;
            visualizer.y = VISUALIZER_Y;
            visualizer.width = VISUALIZER_WIDTH;
            visualizer.height = VISUALIZER_HEIGHT;
            visualizer.movable = movable;
            visualizer.blocking_tile_visible = false;
        }
    }
}