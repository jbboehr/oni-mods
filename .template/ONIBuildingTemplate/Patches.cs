using System;
using System.Collections.Generic;
using Database;
using Harmony;
using STRINGS;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
#pragma warning disable 414

namespace MightyVincent
{
    internal static class Patches
    {/*caret*/
        public static void OnLoad()
        {
            Debug.Log("Dev Mod");
        }
    }

    internal class PREFABS
    {
        public class ONIBuildingTemplate
        {
            public static LocString NAME = UI.FormatAsLink("ONIBuildingTemplate", nameof(ONIBuildingTemplate));
            public static LocString DESC = "Description";
            public static LocString EFFECT = "Effect";
        }
    }

    [HarmonyPatch(typeof(LegacyModMain), "LoadBuildings")]
    internal class GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            LocString.CreateLocStringKeys(typeof(PREFABS), "STRINGS.BUILDINGS.");

            ModUtil.AddBuildingToPlanScreen("Base", ONIBuildingTemplateConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Techs.TECH_GROUPING["MedicineI"] = Techs.TECH_GROUPING["MedicineI"]
                .Append(ONIBuildingTemplateConfig.ID);
        }
    }
}