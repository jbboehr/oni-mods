using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using TUNING;

namespace MightyVincent
{
    public class HyperbaricReservoirSettings
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Always3InterestsSettings
        {
            [Option("Number of interests", "The number of interests.", null)]
            [Limit(0.0, 10.0)]
            [JsonProperty]
            public int numberOfInterests { get; set; }

            [Option("Random number of interests", "Active it to disable the interest modification.", null)]
            [JsonProperty]
            public bool randomNumberOfInterests { get; set; }

            [Option("Points when 1 interest", "", null)]
            [Limit(0.0, 50.0)]
            [JsonProperty]
            public int pointsWhen1Interest { get; set; }

            [Option("Points when 2 interest", "", null)]
            [Limit(0.0, 50.0)]
            [JsonProperty]
            public int pointsWhen2Interest { get; set; }

            [Option("Points when 3 interest", "", null)]
            [Limit(0.0, 50.0)]
            [JsonProperty]
            public int pointsWhen3Interest { get; set; }

            [Option("Points when more than 3 interest", "", null)]
            [Limit(0.0, 50.0)]
            [JsonProperty]
            public int pointsWhenMoreThan3Interest { get; set; }

            [Option("Number of Good traits", "", null)]
            [Limit(0.0, 5.0)]
            [JsonProperty]
            public int numberOfGoodTraits { get; set; }

            [Option("Number of Bad traits", "", null)]
            [Limit(0.0, 5.0)]
            [JsonProperty]
            public int numberOfBadTraits { get; set; }

            [Option("Disable joy trait", "", null)]
            [JsonProperty]
            public bool disableJoyTrait { get; set; }

            [Option("Disable stress trait", "", null)]
            [JsonProperty]
            public bool disableStressTrait { get; set; }

            [Option("Starting level on printing pod", "Set the experience of in game printed dups.", null)]
            [Limit(0.0, 5.0)]
            [JsonProperty]
            public int startingLevelOnPrintingPod { get; set; }

            public Always3InterestsSettings()
            {
                this.pointsWhen1Interest = 7;
                this.pointsWhen2Interest = 3;
                this.pointsWhen3Interest = 1;
                this.pointsWhenMoreThan3Interest = 1;
                this.numberOfInterests = 3;
                this.randomNumberOfInterests = false;
                this.numberOfGoodTraits = 1;
                this.numberOfBadTraits = 1;
                this.disableJoyTrait = false;
                this.disableStressTrait = false;
                this.startingLevelOnPrintingPod = 1;
            }
        }

        internal class TuningConfigPatch
        {
            public static Always3InterestsSettings settings;

            public static void OnLoad()
            {
                PUtil.InitLibrary(true);
                POptions.RegisterOptions(typeof(Always3InterestsSettings));
                settings = new Always3InterestsSettings();
                ReadSettings();
                Traverse.Create<DUPLICANTSTATS>().Field<int[]>("APTITUDE_ATTRIBUTE_BONUSES").Value = new int[11]
                {
                    settings.pointsWhen1Interest,
                    settings.pointsWhen2Interest,
                    settings.pointsWhen3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest,
                    settings.pointsWhenMoreThan3Interest
                };
            }

            public static void ReadSettings()
            {
                Debug.Log((object) "Loading settings");
                settings = POptions.ReadSettings<Always3InterestsSettings>();
                if (settings != null)
                    return;
                settings = new Always3InterestsSettings();
            }
        }
    }
}