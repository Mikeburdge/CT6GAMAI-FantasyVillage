using Assets.Scripts.FiniteStateMachine.States;
using Assets.Scripts.Villagers;
using UnityEngine;

namespace Assets.Scripts.UtilityTheory.Desires
{
    public class ReturnHomeDesire : Desire
    {
        public ReturnHomeDesire()
        {
            state = GoHomeAndSleep.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            
            float bias = villager.ReturnHomeBias;

            float factorOne = 1 - (villager.Health / 100);

            float distance = Vector3.Distance(villager.transform.position, villager.home.transform.position);

            desireVal = bias * (factorOne / distance*100);

            Debug.Log(villager + "'s Going Home Desire: " + desireVal);

            return;
        }
    }
}