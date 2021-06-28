using System.Linq;
using AsLimc.commons;
using HarmonyLib;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using PeterHan.PLib.PatchManager;
using UnityEngine;

namespace AsLimc.SimplerPipPlantRule {
    public sealed class Patches : KMod.UserMod2 {
        private static Settings settings;
        
		public override void OnLoad(Harmony harmony) {
			base.OnLoad(harmony);
            PUtil.InitLibrary();
            settings = new Settings();
			new PPatchManager(harmony).RegisterPatchClass(typeof(Patches));
			new POptions().RegisterOptions(this, typeof(Settings));
            VLib.Init();
            // Settings.Init();
        }

        [HarmonyPatch(typeof(SeedPlantingMonitor.Def), MethodType.Constructor)]
        internal class SeedPlantingMonitor_Def_Constructor {
            public static void Postfix(ref float ___searchMinInterval, ref float ___searchMaxInterval) {
                ___searchMinInterval = settings.SearchMinInterval;
                ___searchMaxInterval = settings.SearchMaxInterval;
            }
        }

        [HarmonyPatch(typeof(PlantableCellQuery), MethodType.Constructor)]
        internal class PlantableCellQuery_Constructor {
            public static void Postfix(ref int ___plantDetectionRadius, ref int ___maxPlantsInRadius) {
                ___plantDetectionRadius = settings.PlantDetectionRadius;
                ___maxPlantsInRadius = settings.MaxPlantsInRadius;
            }
        }

        [HarmonyPatch(typeof(PlantableCellQuery), "CountNearbyPlants")]
        internal class PlantableCellQuery_CountNearbyPlants {
            public static bool Prefix(int cell, int radius, out int __result) {
//                Debug.Log($"CountNearbyPlants Prefix: {cell}, {radius}");
                Grid.CellToXY(cell, out var x, out var y);
                var side = radius * 2 + 1;
                var x_bottomLeft = x - radius;
                var y_bottomLeft = y - radius;
                var pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();

                GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, side, side, GameScenePartitioner.Instance.plants, pooledList);
                var countWithoutTreeBud = pooledList.Count(entry => !(bool) ((Component) entry.obj).GetComponent<TreeBud>());
//                var total = pooledList.Count;
                pooledList.Recycle();
//                Debug.Log($"countWithoutTreeBud: ({x},{y}), {countWithoutTreeBud} / {total}");
                __result = countWithoutTreeBud;
                return false;
            }
        }

		[PLibMethod(RunAt.OnStartGame)]
		internal static void OnStartGame() {
			settings = POptions.ReadSettings<Settings>() ?? new Settings();
		}
    }
}