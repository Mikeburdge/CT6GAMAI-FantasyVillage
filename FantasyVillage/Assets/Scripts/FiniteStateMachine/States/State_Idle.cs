using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace States
{

    public sealed class State_Idle : State<Villager>
    {
        public static State_Idle Instance { get; } = new State_Idle();
        static State_Idle() { }
        private State_Idle() { }


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