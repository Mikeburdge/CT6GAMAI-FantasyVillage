using UnityEngine;
using System.Collections;
using Assets.Scripts.Villagers;
using Assets.Scripts.FiniteStateMachine;

namespace States
{
    public class DefaultState : State<Villager>
    {

        public static DefaultState Instance { get; } = new DefaultState();
        static DefaultState() { }
        private DefaultState() { }

        public override void Enter(Villager v) { }

        public override void Execute(Villager v) { }

        public override void Exit(Villager v) { }

    }
}