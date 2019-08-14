using System;
using KSerialization;
using UnityEngine;

#pragma warning disable 649

namespace MightyVincent
{
    public class HyperbaricReservoir : KMonoBehaviour
    {
        private MeterController _meter;
        [MyCmpGet] private Storage _storage;

        [MyCmpReq] public Operational operational;
        private float _startMass;
        private float _endMass;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_fill", "meter_OL");
            Subscribe((int) GameHashes.OnStorageChange, OnStorageChange);
            Subscribe((int) HyperbaricReservoirHashes.OnConduitUpdateStart, OnConduitUpdateStart);
            Subscribe((int) HyperbaricReservoirHashes.OnConduitUpdateEnd, OnConduitUpdateEnd);
            OnStorageChange(null);
        }

        private void OnStorageChange(object data)
        {
            _meter.SetPositionPercent(Mathf.Clamp01(_storage.MassStored() / _storage.capacityKg));
        }

        private void OnConduitUpdateStart(object o)
        {
            if (!(o is Storage storage)) return;
            _startMass = storage.MassStored();
        }

        private void OnConduitUpdateEnd(object o)
        {
            if (!(o is Storage storage)) return;
            _endMass = storage.MassStored();
            operational.SetActive(_startMass < _endMass);
        }
    }
}