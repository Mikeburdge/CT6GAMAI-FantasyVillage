using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace States
{
    public class StateDefault : State<Villager>
    {

        public static StateDefault Instance { get; } = new StateDefault();
        static StateDefault() { }
        private StateDefault() { }

        public override void Enter(Villager v) { }

        public override void Execute(Villager v) { }

        public override void Exit(Villager v) { }

    }
}