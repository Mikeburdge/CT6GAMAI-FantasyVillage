using Assets.Scripts.FiniteStateMachine;
using UnityEngine;
using Villagers;

namespace States
{

    public sealed class GoHomeAndSleep : State<Villager>
    {
        public static GoHomeAndSleep Instance { get; } = new GoHomeAndSleep();
        static GoHomeAndSleep() { }
        private GoHomeAndSleep() { }


        public override void Enter(Villager v)
        {
            v.UpdateAIText("Begun BT: " + nameof(v.StartGoHomeAndSleepBt));
            v.StartGoHomeAndSleepBt();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBt();
        }

        public override void Exit(Villager v)
        {
            v.UpdateAIText("Ended BT: " + nameof(v.StartGoHomeAndSleepBt));
            v.GetComponent<Renderer>().enabled = true;
        }
    }
}