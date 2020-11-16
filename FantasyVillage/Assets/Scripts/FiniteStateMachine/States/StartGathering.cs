using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using LocationThings;
using UnityEngine;

namespace States
{

    public sealed class StartGathering : State<Villager>
    {
        public static StartGathering Instance { get; } = new StartGathering();
        static StartGathering() { }
        private StartGathering() { }


        public override void Enter(Villager v){
        v.UpdateAIText("Begun BT: " + nameof(v.StartChoppingTreesBT));
            v.StartChoppingTreesBT();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBT();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText("Ended BT: " + nameof(v.StartChoppingTreesBT));
        }
    }
}