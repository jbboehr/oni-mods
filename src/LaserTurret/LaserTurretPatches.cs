using Database;
using Harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class LaserTurret_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.NAME", "Laser Turret");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.DESC", "Hey, you! Freeze!");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.EFFECT",
                "Target and attack critters nearby. Can be controlled by signal.");

            ModUtil.AddBuildingToPlanScreen("Food", LaserTurretConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LaserTurret_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Techs.TECH_GROUPING["AnimalControl"] = Techs.TECH_GROUPING["AnimalControl"]
                .Append(LaserTurretConfig.ID);
        }
    }
}