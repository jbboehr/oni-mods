using System.Collections.Generic;
using Harmony;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal class SmootherLightPatches
    {
        [HarmonyPatch(typeof(DiscreteShadowCaster), "GetVisibleCells")]
        internal class DiscreteShadowCaster_GetVisibleCells
        {
            private static bool Prefix(int cell, List<int> visiblePoints, int range, LightShape shape)
            {
                LightGridUtil.GetVisibleCells(cell, visiblePoints, range, shape);
                return false;
            }
        }
    }
}