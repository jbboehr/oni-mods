using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
#pragma warning disable 414

namespace MightyVincent
{
    [HarmonyPatch(typeof(LegacyModMain), "LoadBuildings")]
    internal class HyperbaricReservoir_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix(List<Type> types)
        {
            LocString.CreateLocStringKeys(typeof(PREFABS), "STRINGS.BUILDINGS.");

            ModUtil.AddBuildingToPlanScreen("Base", HyperbaricLiquidReservoirConfig.ID);
            ModUtil.AddBuildingToPlanScreen("Base", HyperbaricGasReservoirConfig.ID);
        }
    }

    internal class PREFABS
    {
        public class HYPERBARICLIQUIDRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyperbaric Liquid Reservoir", nameof(HYPERBARICLIQUIDRESERVOIR));
            public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.";
        }

        public class HYPERBARICGASRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyperbaric Gas Reservoir", nameof(HYPERBARICGASRESERVOIR));
            public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.";
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class HyperbaricReservoir_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Database.Techs.TECH_GROUPING["ValveMiniaturization"] = Database.Techs.TECH_GROUPING["ValveMiniaturization"]
                .Append(HyperbaricLiquidReservoirConfig.ID);
            Database.Techs.TECH_GROUPING["ValveMiniaturization"] = Database.Techs.TECH_GROUPING["ValveMiniaturization"]
                .Append(HyperbaricGasReservoirConfig.ID);
        }
    }

    [HarmonyPatch(typeof(ConduitConsumer), "ConduitUpdate")]
    internal class HyperbaricReservoir_ConduitConsumer_ConduitUpdate
    {
        private static void Prefix(ConduitConsumer __instance)
        {
            __instance.Trigger((int) HyperbaricReservoirHashes.OnConduitUpdateStart, __instance.storage);
        }

        private static void Postfix(ConduitConsumer __instance)
        {
            __instance.Trigger((int) HyperbaricReservoirHashes.OnConduitUpdateEnd, __instance.storage);
        }
    }
}