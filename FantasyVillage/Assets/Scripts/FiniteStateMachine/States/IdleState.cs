using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using LocationThings;
using UnityEngine;

namespace States
{

    public sealed class IdleState : State<Villager>
    {
        public static IdleState Instance { get; } = new IdleState();
        static IdleState() { }
        private IdleState() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Begun BT: " + nameof(v.StartIdleBT));
            v.StartIdleBT();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBT();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText("Ended BT: " + nameof(v.StartIdleBT));
        }
    }
}