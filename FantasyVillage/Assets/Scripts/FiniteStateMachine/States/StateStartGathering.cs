using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class StateStartGathering : State<Villager>
    {
        public static StateStartGathering Instance { get; } = new StateStartGathering();
        static StateStartGathering() { }
        private StateStartGathering() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Chopping Trees");
            v.StartChoppingTreesBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            Debug.Log($"{v} is not longer chopping trees");
            if (v.bb.CurrentNearestAvailableTree)
            {
                Debug.Log($"{v} has left a tree half chopped");
                v.bb.CurrentNearestAvailableTree.isOccupied = false;
            }

            v.bb.AStarPath = null;
            v.bb.CurrentNearestAvailableTree = null;
            v.UpdateAIText("No Longer Chopping Trees");
        }
    }
}