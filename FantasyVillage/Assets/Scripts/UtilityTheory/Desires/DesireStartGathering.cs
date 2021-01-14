using States;
using Storage;
using UnityEngine;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class DesireStartGathering : Desire
    {
        public DesireStartGathering()
        {
            State = State_StartGathering.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            var storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

            var bias = villager.StartGatheringWoodBias;

            var factorOne = 1 - (storage.WoodInStorage / storage.maxStorageCapacity);

            DesireVal = bias * factorOne;

            villager.DisplayStartGatheringDesire = DesireVal;
        }
    }
}