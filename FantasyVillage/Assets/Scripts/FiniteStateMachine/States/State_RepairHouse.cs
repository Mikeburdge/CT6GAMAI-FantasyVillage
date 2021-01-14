using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class State_RepairHouse : State<Villager>
    {
        public static State_RepairHouse Instance { get; } = new State_RepairHouse();
        static State_RepairHouse() { }
        private State_RepairHouse() { }


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