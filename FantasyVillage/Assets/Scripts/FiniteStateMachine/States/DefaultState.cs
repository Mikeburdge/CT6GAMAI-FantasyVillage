using UnityEngine;
using System.Collections;
using Assets.Scripts.Villagers;

namespace Assets.Scripts.FiniteStateMachine.States
{
    public class DefaultState : State<Villager>
    {

        public static DefaultState Instance { get; } = new DefaultState();
        static DefaultState() { }
        private DefaultState() { }


        public override void TravelTo(Villager v)
        {
            v.navMesh.SetDestination(v.home.transform.position);
            Debug.Log(v.name + "is moving towards their home...");
        }

        public override void Enter(Villager v) { }

        public override void Execute(Villager v) { }

        public override void Exit(Villager v) { }

    }
}