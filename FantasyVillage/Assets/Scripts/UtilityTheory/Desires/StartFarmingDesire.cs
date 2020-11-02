using Assets.Scripts.FiniteStateMachine.States;
using Assets.Scripts.Villagers;
using UnityEngine;

namespace Assets.Scripts.UtilityTheory.Desires
{
    public class StartFarmingDesire : Desire
    {
        public StartFarmingDesire()
        {
            state = StartFarming.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            
            float bias = villager.StartFarmingBias;

            float factorOne = 1 - (100 / villager.Health);

            float distance = Vector3.Distance(villager.transform.position, villager.farm.transform.position);

            desireVal = bias * (factorOne / distance);

            Debug.Log(villager + "'s Farming Desire: " + desireVal);
            return;
        }
    }
}