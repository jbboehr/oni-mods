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
    internal class PREFABS
    {
        public class HYPERBARICLIQUIDRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyperbaric Liquid Reservoir", nameof(HYPERBARICLIQUIDRESERVOIR));
            public static LocString DESC = "Hyperbaric Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID} resources piped into it.";
        }

        public class HYPERBARICGASRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyperbaric Gas Reservoir", nameof(HYPERBARICGASRESERVOIR));
            public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSGAS} resources piped into it.";
        }
    }

    [HarmonyPatch(typeof(LegacyModMain), "LoadBuildings")]
    internal class GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix(List<Type> types)
        {
            LocString.CreateLocStringKeys(typeof(PREFABS), "STRINGS.BUILDINGS.");

            ModUtil.AddBuildingToPlanScreen("Base", HyperbaricLiquidReservoirConfig.ID);
            ModUtil.AddBuildingToPlanScreen("Base", HyperbaricGasReservoirConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Techs.TECH_GROUPING["ValveMiniaturization"] = Techs.TECH_GROUPING["ValveMiniaturization"]
                .Append(HyperbaricLiquidReservoirConfig.ID);
            Techs.TECH_GROUPING["ValveMiniaturization"] = Techs.TECH_GROUPING["ValveMiniaturization"]
                .Append(HyperbaricGasReservoirConfig.ID);
        }
    }

    [HarmonyPatch(typeof(ConduitConsumer), "ConduitUpdate")]
    internal class ConduitConsumer_ConduitUpdate
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