using LocationThings;
using States;
using Storage;
using UnityEngine;
using UtilityTheory;
using Villagers;

namespace Desires
{
    public class Desire_StartGathering : Desire
    {
        public Desire_StartGathering()
        {
            State = State_StartGathering.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {
            var storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

            var bias = villager.StartGatheringBias;

            var factorOne = 1 - (storage.WoodInStorage / storage.maxStorageCapacity);

            DesireVal = bias * factorOne;

            villager.IdleDesireValue = DesireVal;
        }
    }
}