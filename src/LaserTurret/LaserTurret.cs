using System;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MightyVincent
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LaserTurret : StateMachineComponent<LaserTurret.Instance>, ISim1000ms
    {
        private static readonly HashedString HashRotation = (HashedString) "rotation";

        [EventRef] private string _rotateSound = "AutoMiner_rotate";
        private float _armRot = 45f;
        private float _turnRate = 180f;
        [MyCmpGet] private Rotatable _rotatable;
        [MyCmpReq] private Operational _operational;

        [MyCmpGet] private KSelectable _selectable;

//        [MyCmpReq] private MiningSounds _miningSounds;
        public float range;
        public int visualizerX;
        public int visualizerY;
        public int visualizerWidth;
        public int visualizerHeight;

        private KBatchedAnimController _armAnimCtrl;
        private GameObject _armGo;
        private LoopingSounds _loopingSounds;
        private KAnimLink _link;
        private bool _rotationComplete;
        private bool _rotateSoundPlaying;
        private GameObject _hitEffectPrefab;
        private GameObject _hitEffect;

        public KPrefabID target;
        private int _targetCell;
        private bool HasTarget => target != null;
        private bool RotationComplete => HasTarget && _rotationComplete;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            simRenderLoadBalance = true;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _hitEffectPrefab = Assets.GetPrefab((Tag) "fx_dig_splash");
            var component = GetComponent<KBatchedAnimController>();
            var armName = component.name + ".gun";
            _armGo = new GameObject(armName);
            _armGo.SetActive(false);
            _armGo.transform.parent = component.transform;
            _loopingSounds = _armGo.AddComponent<LoopingSounds>();
            _rotateSound = GlobalAssets.GetSound(_rotateSound);
            _armGo.AddComponent<KPrefabID>().PrefabTag = new Tag(armName);
            _armAnimCtrl = _armGo.AddComponent<KBatchedAnimController>();
            _armAnimCtrl.AnimFiles = new[] {component.AnimFiles[0]};
            _armAnimCtrl.initialAnim = "gun";
            _armAnimCtrl.isMovable = true;
            _armAnimCtrl.sceneLayer = Grid.SceneLayer.TransferArm;
            component.SetSymbolVisiblity((KAnimHashedString) "gun_target", false);
            Vector3 column = component.GetSymbolTransform(new HashedString("gun_target"), out _)
                .GetColumn(3);
            column.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
            _armGo.transform.SetPosition(column);
            _armGo.SetActive(true);
            _link = new KAnimLink(component, _armAnimCtrl);
            RotateArm(_rotatable.GetRotatedOffset(Quaternion.Euler(0.0f, 0.0f, -45f) * Vector3.up), true,
                0.0f);
            StopAttack();
            smi.StartSM();
        }

        public void Sim1000ms(float dt)
        {
            if (!_operational.IsOperational) return;
            RefreshTarget();
            _operational.SetActive(HasTarget);
        }

        private void RefreshTarget()
        {
            if (target != null && IsAttackable(target) && _targetCell == Grid.PosToCell(target.transform.gameObject))
                return;
            ClearTarget();
            var creature = GetClosestAttackableCreature();
            if (creature == null) return;
            target = creature;
            _targetCell = Grid.PosToCell(target.transform.gameObject);
            _rotationComplete = false;
        }

        private void ClearTarget()
        {
            target = null;
            _targetCell = int.MinValue;
            _rotationComplete = true;
        }

        private KPrefabID GetClosestAttackableCreature()
        {
            KPrefabID targetCreature = null;

            var cavityInfo = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this));
            if (cavityInfo != null)
            {
                float targetAge = int.MinValue;
                float targetIncubation = int.MaxValue;

                foreach (var creature in cavityInfo.creatures)
                {
                    if (!IsAttackable(creature))
                    {
                        continue;
                    }

                    var age = Db.Get().Amounts.Age.Lookup(creature).value;
                    if (age > targetAge)
                    {
                        // oldest
                        targetCreature = creature;
                        targetAge = age;
                    }
                    else if (age.Equals(targetAge))
                    {
                        var incubation = Db.Get().Amounts.Incubation.Lookup(creature).value;
                        if (incubation < targetIncubation)
                        {
                            // lowest incubation
                            targetCreature = creature;
                            targetIncubation = incubation;
                        }
                    }
                }
            }

            return targetCreature;
        }

        private bool IsAttackable(KPrefabID creature)
        {
            return creature != null
                   && IsReachable(creature)
                   && !creature.HasTag(GameTags.Creatures.Bagged) && !creature.HasTag(GameTags.Trapped)
                   && (bool) creature.GetComponent<Health>() && !creature.GetComponent<Health>().IsDefeated();
        }

        private bool IsReachable(KPrefabID creature)
        {
            var xy1 = Grid.PosToXY(transform.position);
            var targetMinX = xy1.x + visualizerX;
            var targetMaxX = targetMinX - 1 + visualizerWidth;
            var targetMinY = xy1.y + visualizerY;
            var targetMaxY = targetMinY - 1 + visualizerHeight;
            var xy2 = Grid.PosToXY(creature.transform.position);
//            return Vector2.Distance(transform.position, creature.transform.GetPosition()) <= range;
            return Grid.IsValidCell(Grid.PosToCell(creature.transform.gameObject))
                   && xy2.x >= targetMinX && xy2.x <= targetMaxX
                   && xy2.y >= targetMinY && xy2.y <= targetMaxY
                   && Grid.IsPhysicallyAccessible(xy1.x, xy1.y, xy2.x, xy2.y, true);
        }


        private void AttackCreature(KPrefabID creature)
        {
            var health = creature.GetComponent<Health>();
            if (!(bool) health)
                return;
            var amount = Mathf.RoundToInt(Random.Range(3f, 6f)) *
                         (1f + creature.GetComponent<AttackableBase>().GetDamageMultiplier());
            health.Damage(amount);

            var effects = creature.GetComponent<Effects>();
            if (!(bool) effects)
                return;
            effects.Add("WasAttacked", true);
//                    creature.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
        }


        // --------------------------------- states --------------------------------------

        public void StartAttack()
        {
            Trigger(GameHashes.Attacking.GetHashCode(), target);
            CreateHitEffect();
            _armAnimCtrl.Play((HashedString) "gun_digging", KAnim.PlayMode.Loop);
        }

        public void StopAttack()
        {
            ClearTarget();
            Trigger(GameHashes.StoppedAttacking.GetHashCode());
            DestroyHitEffect();
            _armAnimCtrl.Play((HashedString) "gun", KAnim.PlayMode.Loop);
        }

        public void UpdateAttack(float dt)
        {
            if (!HasTarget || !_rotationComplete)
                return;
            AttackCreature(target);
//            _miningSounds.SetPercentComplete(Grid.Damage[_digCell]);
            var posCcc = Grid.CellToPosCCC(_targetCell, Grid.SceneLayer.FXFront2);
            posCcc.z = 0.0f;
            var position = _armGo.transform.GetPosition();
            position.z = 0.0f;
            var sqrMagnitude = (posCcc - position).sqrMagnitude;
            _armAnimCtrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
            if (IsAttackable(target))
                return;
            target = null;
            _rotationComplete = false;
        }

        private void CreateHitEffect()
        {
            if (_hitEffectPrefab == null)
                return;
            if (_hitEffect != null)
                DestroyHitEffect();
            _hitEffect = GameUtil.KInstantiate(_hitEffectPrefab,
                Grid.CellToPosCCC(_targetCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2);
            _hitEffect.SetActive(true);
            var component = _hitEffect.GetComponent<KBatchedAnimController>();
            component.sceneLayer = Grid.SceneLayer.FXFront2;
            component.initialMode = KAnim.PlayMode.Loop;
            component.enabled = false;
            component.enabled = true;
        }

        private void DestroyHitEffect()
        {
            if (_hitEffectPrefab == null ||
                !(_hitEffect != null))
                return;
            _hitEffect.DeleteObject();
            _hitEffect = null;
        }

        public void UpdateRotation(float dt)
        {
            if (!HasTarget)
                return;
            var posCcc = Grid.CellToPosCCC(_targetCell, Grid.SceneLayer.TileMain);
            posCcc.z = 0.0f;
            var position = _armGo.transform.GetPosition();
            position.z = 0.0f;
            RotateArm(Vector3.Normalize(posCcc - position), false, dt);
        }

        private void RotateArm(Vector3 targetDir, bool warp, float dt)
        {
            if (_rotationComplete)
                return;
            var a = MathUtil.Wrap(-180f, 180f,
                MathUtil.AngleSigned(Vector3.up, targetDir, Vector3.forward) - _armRot);
            _rotationComplete = Mathf.Approximately(a, 0.0f);
            var num = a;
            if (warp)
                _rotationComplete = true;
            else
                num = Mathf.Clamp(num, -_turnRate * dt, _turnRate * dt);
            _armRot += num;
            _armRot = MathUtil.Wrap(-180f, 180f, _armRot);
            _armGo.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _armRot);
            if (!_rotationComplete)
            {
                StartRotateSound();
                _loopingSounds.SetParameter(_rotateSound, HashRotation, _armRot);
            }
            else
                StopRotateSound();
        }

        private void StartRotateSound()
        {
            if (_rotateSoundPlaying)
                return;
            _loopingSounds.StartSound(_rotateSound);
            _rotateSoundPlaying = true;
        }

        private void StopRotateSound()
        {
            if (!_rotateSoundPlaying)
                return;
            _loopingSounds.StopSound(_rotateSound);
            _rotateSoundPlaying = false;
        }


        public class Instance : GameStateMachine<States, Instance, LaserTurret, object>.GameInstance
        {
            public Instance(LaserTurret master)
                : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, Instance, LaserTurret>
        {
            public State Off;
            public ReadyStates On;

            public override void InitializeStates(out BaseState baseState)
            {
                baseState = Off;
                root.DoNothing();
                Off
                    .PlayAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, On,
                        smi => smi.GetComponent<Operational>().IsOperational);
                On.DefaultState(On.Idle)
                    .EventTransition(GameHashes.OperationalChanged, Off,
                        smi => !smi.GetComponent<Operational>().IsOperational);
                On.Idle
                    .PlayAnim("on")
                    .EventTransition(GameHashes.ActiveChanged, On.Moving,
                        smi => smi.GetComponent<Operational>().IsActive);
                On.Moving
                    .Enter(smi => smi.master.StartRotateSound())
                    .Exit(smi => smi.master.StopRotateSound())
                    .PlayAnim("working")
                    .EventTransition(GameHashes.ActiveChanged, On.Idle,
                        smi => !smi.GetComponent<Operational>().IsActive)
                    .Update((smi, dt) => smi.master.UpdateRotation(dt), UpdateRate.SIM_33ms)
                    .Transition(On.Attacking, RotationComplete);
                On.Attacking
                    .Enter(smi => smi.master.StartAttack())
                    .Exit(smi => smi.master.StopAttack())
                    .PlayAnim("working")
                    .EventTransition(GameHashes.ActiveChanged, On.Idle,
                        smi => !smi.GetComponent<Operational>().IsActive)
                    .Update((smi, dt) => smi.master.UpdateAttack(dt))
                    .Transition(On.Moving, Not(RotationComplete));
            }

            public static bool RotationComplete(LaserTurret.Instance smi)
            {
                return smi.master.RotationComplete;
            }

            public class ReadyStates : State
            {
                public State Idle;
                public State Moving;
                public State Attacking;
            }
        }
    }
}