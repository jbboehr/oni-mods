using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using STRINGS;

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
    }

    internal class PREFABS
    {
        public class HYPERBARICLIQUIDRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyper Liquid Reservoir", nameof(HYPERBARICLIQUIDRESERVOIR));
            public static LocString DESC = "Hyper Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID} resources piped into it.";
        }

        public class HYPERBARICGASRESERVOIR
        {
            public static LocString NAME = UI.FormatAsLink("Hyper Gas Reservoir", nameof(HYPERBARICGASRESERVOIR));
            public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSGAS} resources piped into it.";
        }
    }

    [HarmonyPatch(typeof(LegacyModMain), "LoadBuildings")]
    internal class GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            LocString.CreateLocStringKeys(typeof(PREFABS), "STRINGS.BUILDINGS.");

            ModUtil.AddBuildingToPlanScreen("Base", HyperLiquidReservoirConfig.ID);
            ModUtil.AddBuildingToPlanScreen("Base", HyperGasReservoirConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class Db_Initialize
    {
        private static void Postfix(Db __instance)
        {
            LimcUtils.AddTech(__instance, "ValveMiniaturization", new[] {HyperLiquidReservoirConfig.ID, HyperGasReservoirConfig.ID});
        }
    }

    [HarmonyPatch(typeof(ConduitConsumer), "ConduitUpdate")]
    internal class ConduitConsumer_ConduitUpdate
    {
        private static void Prefix(ConduitConsumer __instance)
        {
            __instance.Trigger((int) Hashes.OnConduitConsumerUpdateStart, __instance.storage);
        }

        private static void Postfix(ConduitConsumer __instance)
        {
            __instance.Trigger((int) Hashes.OnConduitConsumerUpdateEnd, __instance.storage);
        }
    }

    [HarmonyPatch(typeof(ConduitDispenser), "ConduitUpdate")]
    internal class ConduitDispenser_ConduitUpdate
    {
        private static void Prefix(ConduitDispenser __instance)
        {
            __instance.Trigger((int) Hashes.OnConduitDispenserUpdateStart, __instance.storage);
        }

        private static void Postfix(ConduitDispenser __instance)
        {
            __instance.Trigger((int) Hashes.OnConduitDispenserUpdateEnd, __instance.storage);
        }
    }
}