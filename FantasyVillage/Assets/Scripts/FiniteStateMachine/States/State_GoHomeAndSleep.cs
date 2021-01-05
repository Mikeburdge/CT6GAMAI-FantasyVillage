using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class State_GoHomeAndSleep : State<Villager>
    {
        public static State_GoHomeAndSleep Instance { get; } = new State_GoHomeAndSleep();
        static State_GoHomeAndSleep() { }
        private State_GoHomeAndSleep() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Go Home and Rest");
            v.StartGoHomeAndSleepBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            foreach (var renderer in v.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
    }
}