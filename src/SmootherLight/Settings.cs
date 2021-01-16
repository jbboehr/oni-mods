using Newtonsoft.Json;
using PeterHan.PLib;

namespace MightyVincent
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [Option("Light through Mesh Tiles", "Can light go through Mesh Tiles. (Restart Needed)", null)]
        [JsonProperty]
        public bool LightThroughMeshTiles { get; set; }

        public Settings()
        {
            LightThroughMeshTiles = false;
        }
    }
}