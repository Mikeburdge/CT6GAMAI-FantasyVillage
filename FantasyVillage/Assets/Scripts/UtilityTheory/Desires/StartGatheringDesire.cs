using Assets.Scripts.Villagers;
using LocationThings;
using States;
using Storage;
using UnityEngine;
using UtilityTheory;

namespace Desires
{
    public class StartGatheringDesire : Desire
    {
        public StartGatheringDesire()
        {
            state = StartGathering.Instance;
        }

        public override void CalculateDesireValue(Villager villager)
        {

            StorageContainer Storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

            float bias = villager.StartGatheringBias;

            float factorOne = 1 - (Storage.WoodInStorage / Storage.MaxStorageCapacity);

            Vector3 forestPosition = LocationPositions.GetPositionFromLocation(LocationNames.forest);

            float distance = Vector3.Distance(villager.transform.position, forestPosition);

            desireVal = bias * (factorOne / distance * 100);
        }
    }
}