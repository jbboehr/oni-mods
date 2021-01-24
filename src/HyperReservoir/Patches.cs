using AsLimc.commons;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;
using PeterHan.PLib.Options;

namespace AsLimc.HyperReservoir {
    internal class Patches {
        public static void OnLoad() {
            PUtil.InitLibrary();
            PLocalization.Register();
            VLib.Init();
            POptions.RegisterOptions(typeof(HyperReservoirSettings));
            HyperReservoirSettings.Init();
            // ModUtil.RegisterForTranslation(typeof(LocStrings));
        }
    }
}