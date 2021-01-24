using AsLimc.commons;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;

namespace AsLimc.LaserTurret {
    internal class Patches {
        public static void OnLoad() {
            PUtil.InitLibrary();
            PLocalization.Register();
            VLib.Init();
            //ModUtil.RegisterForTranslation(typeof(LocStrings));
        }
    }
}