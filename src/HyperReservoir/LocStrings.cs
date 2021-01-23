using STRINGS;

// ReSharper disable UnusedType.Global
#pragma warning disable 414

namespace AsLimc.HyperReservoir {
    internal static class LocStrings {
        public static class Settings {
            public static readonly LocString MOD_NAME = "Hyper Reservoir";

            public static class LiquidReservoirSteelMassKg {
                public static readonly LocString NAME = "Liquid Reservoir steel mass (kg)";
                public static readonly LocString TOOLTIP = "The steel mass of building a Liquid Reservoir";
            }

            public static class LiquidReservoirPlasticMassKg {
                public static readonly LocString NAME = "Liquid Reservoir plastic mass (kg)";
                public static readonly LocString TOOLTIP = "The plastic mass of building a Liquid Reservoir";
            }

            public static class LiquidReservoirPowerConsumptionWatts {
                public static readonly LocString NAME = "Liquid Reservoir power consumption (watts)";
                public static readonly LocString TOOLTIP = "The power consumption of Liquid Reservoir";
            }

            public static class LiquidReservoirCapacityMultiplier {
                public static readonly LocString NAME = "Liquid Reservoir capacity multiplier";
                public static readonly LocString TOOLTIP = "The capacity multiplier of Liquid Reservoir";
            }

            public static class GasReservoirSteelMassKg {
                public static readonly LocString NAME = "Gas Reservoir steel mass (kg)";
                public static readonly LocString TOOLTIP = "The steel mass of building a Gas Reservoir";
            }

            public static class GasReservoirPlasticMassKg {
                public static readonly LocString NAME = "Gas Reservoir plastic mass (kg)";
                public static readonly LocString TOOLTIP = "The plastic mass of building a Gas Reservoir";
            }

            public static class GasReservoirPowerConsumptionWatts {
                public static readonly LocString NAME = "Gas Reservoir power consumption (watts)";
                public static readonly LocString TOOLTIP = "The power consumption of Gas Reservoir";
            }

            public static class GasReservoirCapacityMultiplier {
                public static readonly LocString NAME = "Gas Reservoir capacity multiplier";
                public static readonly LocString TOOLTIP = "The capacity multiplier of Gas Reservoir";
            }
        }

        public static class HyperLiquidReservoir {
            public static readonly LocString NAME = UI.FormatAsLink("Hyper Liquid Reservoir", HyperLiquidReservoirConfig.ID);

            public static readonly LocString DESC =
                $"Hyper Liquid Reservoir's input needs {UI.FormatAsLink("Power", "POWER")} to contain more resource and its' output can be controlled by <link=\"LOGIC\">Automation</link>." +
                " It cannot receive manually delivered resources.";

            public static readonly LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID} resources piped into it.";
        }

        public static class HyperGasReservoir {
            public static readonly LocString NAME = UI.FormatAsLink("Hyper Gas Reservoir", HyperGasReservoirConfig.ID);

            public static readonly LocString DESC =
                $"Hyper Gas Reservoir's input needs {UI.FormatAsLink("Power", "POWER")} to contain more resource and its' output can be controlled by <link=\"LOGIC\">Automation</link>." +
                " It cannot receive manually delivered resources.";

            public static readonly LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSGAS} resources piped into it.";
        }
    }
}