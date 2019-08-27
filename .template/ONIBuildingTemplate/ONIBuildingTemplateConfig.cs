using System;
using TUNING;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable ClassNeverInstantiated.Global

namespace MightyVincent
{
    public class ONIBuildingTemplateConfig : IBuildingConfig
    {
        public new const string ID = "ONIBuildingTemplate";
        private const int WIDTH = 1;
        private const int HEIGHT = 1;

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(ID, WIDTH, HEIGHT, "floor_basic_kanim", 100, 120f,
                TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY, TUNING.MATERIALS.ANY_BUILDABLE, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
            return buildingDef;
        }

    }
}