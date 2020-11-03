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

            float factorOne = 1 - (villager.Health / 100);

            float distance = Vector3.Distance(villager.transform.position, villager.farm.transform.position);

            desireVal = bias * (factorOne / distance * 100);

            Debug.Log(villager + "'s Farming Desire: " + desireVal);
            return;
        }
    }
}