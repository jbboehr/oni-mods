using System.Collections.Generic;
using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal static class Patches
    {
        public static Settings settings;

        public static void OnLoad()
        {
            PUtil.InitLibrary();
            POptions.RegisterOptions(typeof(Settings));
            Debug.Log("Loading settings");
            settings = POptions.ReadSettings<Settings>() ?? new Settings();
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
                __result.BlockTileIsTransparent = Patches.settings.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(MeshTileConfig), "ConfigureBuildingTemplate")]
        internal class MeshTileConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = Patches.settings.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "CreateBuildingDef")]
        internal class GasPermeableMembraneConfig_CreateBuildingDef
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.BlockTileIsTransparent = Patches.settings.LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "ConfigureBuildingTemplate")]
        internal class GasPermeableMembraneConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = Patches.settings.LightThroughMeshTiles;
            }
        }
    }
}