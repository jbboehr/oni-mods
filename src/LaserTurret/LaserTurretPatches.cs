using Harmony;
using STRINGS;

namespace MightyVincent
{
    internal class PREFABS
    {
        public class LASERTURRET
        {
            public static LocString NAME = UI.FormatAsLink("Laser Turret", nameof(LASERTURRET));
            public static LocString DESC = "Hey, you! Freeze!";
            public static LocString EFFECT = $"Target and attack {UI.CODEX.CATEGORYNAMES.CREATURES} nearby. Can be controlled by signal.";
        }
    }

    [HarmonyPatch(typeof(LegacyModMain), "LoadBuildings")]
    internal class LegacyModMain_LoadBuildings
    {
        private static void Prefix()
        {
            LocString.CreateLocStringKeys(typeof(PREFABS), "STRINGS.BUILDINGS.");

            ModUtil.AddBuildingToPlanScreen("Food", LaserTurretConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class Db_Initialize
    {
        private static void Postfix(Db __instance)
        {
            LimcUtils.AddTech(__instance, "AnimalControl", new[] {LaserTurretConfig.ID});
        }
    }
}