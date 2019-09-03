using System.Collections.Generic;
using Harmony;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal class SmootherLightPatches
    {
        private const string _MOD_ID = "1839645620";
        private const string _CONFIG_FILE = "config.json";

        public static void OnLoad()
        {
            ConfigLoader.Load(_MOD_ID, _CONFIG_FILE);
        }

        [HarmonyPatch(typeof(DiscreteShadowCaster), "GetVisibleCells")]
        internal class DiscreteShadowCaster_GetVisibleCells
        {
            public static bool Prefix(int cell, List<int> visiblePoints, int range, LightShape shape)
            {
                LightGridTool.GetVisibleCells(cell, visiblePoints, range, shape);
                return false;
            }
        }

        [HarmonyPatch(typeof(MeshTileConfig), "CreateBuildingDef")]
        internal class MeshTileConfig_CreateBuildingDef
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.BlockTileIsTransparent = State.Config.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(MeshTileConfig), "ConfigureBuildingTemplate")]
        internal class MeshTileConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = State.Config.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "CreateBuildingDef")]
        internal class GasPermeableMembraneConfig_CreateBuildingDef
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.BlockTileIsTransparent = State.Config.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "ConfigureBuildingTemplate")]
        internal class GasPermeableMembraneConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = State.Config.LightThroughMeshTiles;
            }
        }
    }
}