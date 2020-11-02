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

            float factorOne = 1 - (100 / villager.Health);

            float distance = Vector3.Distance(villager.transform.position, villager.home.transform.position);

            desireVal = bias * (factorOne / distance);

            Debug.Log(villager + "'s Going Home Desire: " + desireVal);

            return;
        }
    }
}