using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Harmony;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal class SmootherLightPatches
    {
        [HarmonyPatch(typeof(DiscreteShadowCaster), "GetVisibleCells")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
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