using Assets.Scripts.Villagers;
using UnityEngine;
using static Assets.Scripts.Villagers.Villager;

namespace Assets.Scripts.FiniteStateMachine.States
{

    public sealed class GoHomeAndSleep : State<Villager>
    {
        public static GoHomeAndSleep Instance { get; } = new GoHomeAndSleep();
        static GoHomeAndSleep() { }
        private GoHomeAndSleep() { }




        public override void TravelTo(Villager v)
        {
            v.navMesh.SetDestination(v.farm.transform.position);
        }

        public override void Enter(Villager v)
        {
            if (v.Location != Locations.home)
            {
                Debug.Log(v.name + "Is Entering their home...");
                v.ChangeLocation(Locations.home);
            }

        }

        public override void Execute(Villager v)
        {
            Debug.Log(v.name + " is Chilling At Home");

            Debug.Log(v.name + " is leaving his house in 3");

            Debug.Log(v.name + " is leaving his house in 2");

            Debug.Log(v.name + " is leaving his house in 1");


            v.ChangeState(StartFarming.Instance);

        }

        public override void Exit(Villager v)
        {
            Debug.Log(v.name + " has left their house");

        }
    }
}