using Assets.Scripts.FiniteStateMachine;
using Assets.Scripts.Villagers;
using LocationThings;
using UnityEngine;

namespace States
{

    public sealed class GoHomeAndSleep : State<Villager>
    {
        public static GoHomeAndSleep Instance { get; } = new GoHomeAndSleep();
        static GoHomeAndSleep() { }
        private GoHomeAndSleep() { }


        public override void Enter(Villager v)
        {
            Debug.Log(v.name + " has begun BT: " + nameof(v.StartGoHomeAndSleepBT));
            v.StartGoHomeAndSleepBT();
        }

        public override void Execute(Villager v)
        {
            v.ExecuteBT();
        }

        public override void Exit(Villager v)
        {
            v.GetComponent<Renderer>().enabled = true;
        }
    }
}