using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Database;
using Harmony;

namespace MightyVincent
{
    public static class LimcUtils
    {
        private static bool IsInit { get; set; }
        private static bool IsVanilla { get; set; }

        static LimcUtils()
        {
            IsInit = false;
            Init();
        }

        public static void Init()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            {
                lock (callingAssembly)
                {
                    if (IsInit)
                    {
                        return;
                    }
                    IsInit = true;
                    InitInternal();
                }
            }
        }

        private static void InitInternal()
        {
            var activeDlcId = "vanilla_id";

            var type = AccessTools.TypeByName("DlcManager");
            var method = AccessTools.Method(type, "GetActiveDlcId");
            var instance = AccessTools.CreateInstance(type);
            var result = method?.Invoke(instance, null);
            if (result is string s && s != "")
            {
                activeDlcId = s;
            }
            // var activeDlcId = DlcManager.GetActiveDlcId().ToLower();
            // if (activeDlcId == "")
            // {
            //     activeDlcId = "vanilla_id";
            // }

            IsVanilla = activeDlcId == "vanilla_id";
        }

        public static void AddTech(Db db, string id, string[] itemIDs)
        {
            if (IsVanilla)
            {
                // support for vanilla
                var field = AccessTools.Field(typeof(Techs), "TECH_GROUPING");
                if (field?.GetValue(db.Techs) is Dictionary<string, string[]> techGrouping)
                {
                    var items = techGrouping[id]?.ToList();
                    items?.AddRange(itemIDs);
                    techGrouping[id] = items?.ToArray();
                }
            }
            else
            {
                // support for Space Out!
                // db.Techs.Get(id)?.unlockedItemIDs?.AddRange(itemIDs);
                var field = AccessTools.Field(typeof(Tech), "unlockedItemIDs");
                if (field?.GetValue(db.Techs?.Get(id)) is List<string> unlockedItemIDs)
                {
                    unlockedItemIDs.AddRange(itemIDs);
                }
            }
        }
    }
}