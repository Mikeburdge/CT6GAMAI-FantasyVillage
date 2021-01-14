using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace States
{

    public sealed class StateIdle : State<Villager>
    {
        public static StateIdle Instance { get; } = new StateIdle();
        static StateIdle() { }
        private StateIdle() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Idle");
            v.StartIdleBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
        }
    }
}