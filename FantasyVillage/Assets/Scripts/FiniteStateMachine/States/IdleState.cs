using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using Villagers;

namespace States
{

    public sealed class IdleState : State<Villager>
    {
        public static IdleState Instance { get; } = new IdleState();
        static IdleState() { }
        private IdleState() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText($"Begun BT: {nameof(v.StartIdleBt)}");
            v.StartIdleBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText($"Ended BT: {nameof(v.StartIdleBt)}");
        }
    }
}