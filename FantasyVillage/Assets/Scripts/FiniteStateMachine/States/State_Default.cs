using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace States
{
    public class State_Default : State<Villager>
    {

        public static State_Default Instance { get; } = new State_Default();
        static State_Default() { }
        private State_Default() { }

        public override void Enter(Villager v) { }

        public override void Execute(Villager v) { }

        public override void Exit(Villager v) { }

    }
}