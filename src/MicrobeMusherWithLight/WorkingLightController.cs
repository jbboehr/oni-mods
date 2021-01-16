namespace MightyVincent
{
    public class WorkingLightController : GameStateMachine<WorkingLightController, WorkingLightController.Instance>
    {
        public State Off;
        public State On;

        public override void InitializeStates(out BaseState state)
        {
            state = Off;
            Off.PlayAnim("off")
                .EventTransition(GameHashes.ActiveChanged, On, smi => smi.GetComponent<Operational>().IsActive)
                .Enter(smi => smi.SwitchLight(false))
                ;
            On.PlayAnim("on")
                .EventTransition(GameHashes.ActiveChanged, Off, smi => !smi.GetComponent<Operational>().IsActive)
                .ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight)
                .Enter(smi => smi.SwitchLight(true))
                ;
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            private readonly Light2D _light;

            public Instance(IStateMachineTarget master, Def def)
                : base(master, def)
            {
                _light = master.GetComponent<Light2D>();
            }

            public void SwitchLight(bool on)
            {
                _light.enabled = on;
            }
        }
    }
}