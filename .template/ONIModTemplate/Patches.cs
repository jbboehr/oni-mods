using Harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace MightyVincent
{
    internal static class Patches
    {/*caret*/
        public static void OnLoad()
        {
            Debug.Log("Dev Mod");
        }
    }
}