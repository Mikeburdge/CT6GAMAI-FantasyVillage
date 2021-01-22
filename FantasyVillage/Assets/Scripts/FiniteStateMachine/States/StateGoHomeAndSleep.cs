using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class StateGoHomeAndSleep : State<Villager>
    {
        public static StateGoHomeAndSleep Instance { get; } = new StateGoHomeAndSleep();
        static StateGoHomeAndSleep() { }
        private StateGoHomeAndSleep() { }


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
            v.ChangeVisibility(true);
        }
    }
}