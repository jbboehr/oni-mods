using System.IO;
using Klei;
using Newtonsoft.Json;

namespace MightyVincent
{
    internal static class ConfigLoader
    {
        public static void Load(string modId, string filename)
        {
            var mod = Global.Instance.modManager.mods.Find(o => o.label.id.Equals(modId));
            if (mod == null)
            {
                Debug.LogError($"Mod {modId} is not installed");
                return;
            }

            var path = FileSystem.Normalize(Path.Combine(mod.label.install_path, filename));
            if (!File.Exists(path))
            {
                State.Config = new Config();
                File.WriteAllText(path, JsonConvert.SerializeObject(State.Config, Formatting.Indented));
            }
            else
            {
                State.Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            }
        }
    }
}