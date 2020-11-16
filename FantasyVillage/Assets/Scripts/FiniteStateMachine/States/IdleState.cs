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
            Debug.Log(v.name + " has begun BT: " + nameof(v.StartIdleBT));
            v.StartIdleBT();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBT();
        }

        public override void Exit(Villager v)
        {
            Debug.Log(v.name + " no longer idling", v);
        }
    }
}