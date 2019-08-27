using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Harmony;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal class SmootherLightPatches
    {
        private const string _MOD_ID = "1839645620";
        private const string _CONFIG_FILE = "config.json";

        public static void OnLoad()
        {
            ConfigLoader.Load(_MOD_ID, _CONFIG_FILE);
        }

        [HarmonyPatch(typeof(DiscreteShadowCaster), "GetVisibleCells")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal class DiscreteShadowCaster_GetVisibleCells
        {
            private static bool Prefix(int cell, List<int> visiblePoints, int range, LightShape shape)
            {
                LightGridTool.GetVisibleCells(cell, visiblePoints, range, shape);
                return false;
            }
        }
    }
}