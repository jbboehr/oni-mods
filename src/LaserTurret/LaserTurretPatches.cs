using System.Collections.Generic;
using Harmony;

namespace MightyVincent
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class LaserTurret_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.NAME", "Laser Turret");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.DESC", "Hey, you! Freeze!");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERTURRET.EFFECT", "Target and attack critters nearby. Can be controlled by signal.");

            ModUtil.AddBuildingToPlanScreen("Food", LaserTurretConfig.Id);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LaserTurret_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Database.Techs.TECH_GROUPING["AnimalControl"] = Database.Techs.TECH_GROUPING["AnimalControl"]
                .Append(LaserTurretConfig.Id);
        }
    }
}