#pragma warning disable 649

namespace AsLimc.HyperReservoir {
    public enum Hashes {
        // "AsLimc.HyperReservoir.Hashes.OnConnectionStatusUpdated".GetHashCode()
        OnConnectionStatusUpdated = -1603832120,
    }

    internal class EnergyConsumerEx : EnergyConsumer {
        public override void SetConnectionStatus(CircuitManager.ConnectionStatus connectionStatus) {
            base.SetConnectionStatus(connectionStatus);
            Trigger((int) Hashes.OnConnectionStatusUpdated, new ConnectionStatusUpdated(IsPowered));
        }

        internal class ConnectionStatusUpdated {
            public readonly bool isPowered;

            public ConnectionStatusUpdated(bool isPowered) {
                this.isPowered = isPowered;
            }
        }
    }

    public class HyperReservoir : Reservoir {
        [MyCmpGet] public BuildingEnabledButton buildingEnabledButton;
        [MyCmpReq] public Operational operational;
        [MyCmpGet] public EnergyConsumer energyConsumer;
        [MyCmpGet] public ConduitConsumer conduitConsumer;
        [MyCmpGet] public ConduitDispenser conduitDispenser;

        protected override void OnSpawn() {
            base.OnSpawn();
            Subscribe((int) Hashes.OnConnectionStatusUpdated, OnConnectionStatusUpdated);
            Subscribe((int) GameHashes.PowerStatusChanged, OnPowerStatusChanged);
            Subscribe((int) GameHashes.LogicEvent, OnLogicValueChanged);
        }

        private void OnConnectionStatusUpdated(object data) {
            if (data is EnergyConsumerEx.ConnectionStatusUpdated connectionStatusUpdated) {
                conduitConsumer.alwaysConsume = buildingEnabledButton.IsEnabled && connectionStatusUpdated.isPowered;
            }
        }

        private void OnPowerStatusChanged(object data) {
            if (!(data is bool buildingEnabled))
                return;
            conduitConsumer.alwaysConsume = buildingEnabled && energyConsumer.IsPowered;
            conduitDispenser.alwaysDispense = buildingEnabled && operational.GetFlag(LogicOperationalController.LogicOperationalFlag);
        }

        private void OnLogicValueChanged(object data) {
            if (data is LogicValueChanged logicValueChanged && logicValueChanged.portID == LogicOperationalController.PORT_ID) {
                conduitDispenser.alwaysDispense = buildingEnabledButton.IsEnabled && operational.GetFlag(LogicOperationalController.LogicOperationalFlag);
            }
        }
    }
}