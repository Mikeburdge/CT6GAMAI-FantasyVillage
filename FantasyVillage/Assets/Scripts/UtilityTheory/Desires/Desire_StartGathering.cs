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
            StorageContainer storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

            float bias = villager.StartGatheringBias;

            float factorOne = 1 - (storage.WoodInStorage / storage.maxStorageCapacity);

            Vector3 forestPosition = LocationPositions.GetPositionFromLocation(LocationNames.Forest);

            float distance = Vector3.Distance(villager.transform.position, forestPosition);

            DesireVal = bias * (factorOne / distance * 100);
        }
    }
}