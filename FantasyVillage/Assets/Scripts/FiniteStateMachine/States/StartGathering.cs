using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using Villagers;

namespace States
{

    public sealed class StartGathering : State<Villager>
    {
        public static StartGathering Instance { get; } = new StartGathering();
        static StartGathering() { }
        private StartGathering() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText($"Begun BT: {nameof(v.StartChoppingTreesBt)}");
            v.StartChoppingTreesBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText($"Ended BT: {nameof(v.StartChoppingTreesBt)}");
        }
    }
}