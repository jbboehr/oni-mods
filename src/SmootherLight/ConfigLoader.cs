using System.IO;
using Klei;
using Newtonsoft.Json;

namespace MightyVincent
{
    internal class ConfigLoader
    {
        private const string _MOD_ID = "1839645620";
        private const string _CONFIG_FILE = "Config.json";

        public static void OnLoad()
        {
            var mod = Global.Instance.modManager.mods.Find(o => o.label.id.Equals(_MOD_ID));
            if (mod == null)
            {
                Debug.LogError($"Mod {_MOD_ID} is not installed");
                return;
            }

            var path = FileSystem.Normalize(Path.Combine(mod.label.install_path, _CONFIG_FILE));
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