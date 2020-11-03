using Assets.Scripts.Villagers;
using UnityEngine;
using static Assets.Scripts.Villagers.Villager;

namespace Assets.Scripts.FiniteStateMachine.States
{

    public sealed class StartFarming : State<Villager>
    {
        public static StartFarming Instance { get; } = new StartFarming();
        static StartFarming() { }
        private StartFarming() { }

        public override void TravelTo(Villager v)
        {
            v.navMesh.SetDestination(v.farm.transform.position);
            Debug.Log(v.name + "is moving towards the farm...");
        }

        public override void Enter(Villager v)
        {
            if (v.Location != Locations.farm)
            {
                Debug.Log(v.name + " is Entering the farm...");
                v.ChangeLocation(Locations.farm);
            }
        }

        public override void Execute(Villager v)
        {
            Debug.Log(v.name + " farmed for a bit");

        }

        public override void Exit(Villager v)
        {
            Debug.Log(v.name + " is leaving the farm");
            v.shouldExecute = false;
        }
    }
}