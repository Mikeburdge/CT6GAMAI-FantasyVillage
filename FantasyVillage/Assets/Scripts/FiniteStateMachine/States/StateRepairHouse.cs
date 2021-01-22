using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class StateRepairHouse : State<Villager>
    {
        public static StateRepairHouse Instance { get; } = new StateRepairHouse();
        static StateRepairHouse() { }
        private StateRepairHouse() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Repair House");
            v.StartRepairHouse();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText("No Longer Repairing House");
        }
    }
}