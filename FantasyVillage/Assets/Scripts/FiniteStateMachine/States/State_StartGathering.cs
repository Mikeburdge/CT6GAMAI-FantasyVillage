using Assets.Scripts.FiniteStateMachine;
using Villagers;

namespace States
{

    public sealed class State_StartGathering : State<Villager>
    {
        public static State_StartGathering Instance { get; } = new State_StartGathering();
        static State_StartGathering() { }
        private State_StartGathering() { }


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