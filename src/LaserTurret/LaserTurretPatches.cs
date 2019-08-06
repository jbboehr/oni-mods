using System;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using Random = UnityEngine.Random;

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


    /*[HarmonyPatch(typeof(KMonoBehaviour), "Trigger")]
    internal class KMonoBehaviour_Trigger
    {
        public static void Prefix(int hash, object data = null)
        {
            Debug.Log("---------Trigger---------");
//            Debug.Log(hash);
//            Debug.Log(data);
            if (hash == (int) GameHashes.ObjectMovementStateChanged
                || hash == (int) GameHashes.BeginChore)
            {
                Debug.Log(GameHashDict[hash]);
                Debug.Log(JsonUtility.ToJson(data));
                throw new ArithmeticException();
            }
        }

        private static readonly Dictionary<int, string> GameHashDict;

        static KMonoBehaviour_Trigger()
        {
            GameHashDict = new Dictionary<int, string>();
            foreach (var value in Enum.GetValues(typeof(GameHashes)))
            {
                GameHashDict[(int) value] = value.ToString();
            }
        }
    }*/
}