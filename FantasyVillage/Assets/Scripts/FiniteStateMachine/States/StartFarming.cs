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
            v.navMesh.SetDestination(v.home.transform.position);
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
            Debug.Log(v.name + " is stuck in the farm...");
            //Debug.Log("Feeding The System with MY gold... " +
            //  v.MoneyInBank);
            //v.AddToMoneyInBank(v.GoldCarried);
            //v.ChangeState(EnterMineAndDigForNuggets.Instance);
        }

        public override void Exit(Villager v)
        {
            Debug.Log("Leaving the bank...");
        }
    }
}