using UnityEngine;
using UnityEngine.Serialization;

namespace Storage
{
    public class StorageContainer : MonoBehaviour
    {
        [FormerlySerializedAs("MaxStorageCapacity")] [SerializeField]
        public float maxStorageCapacity = 200;

        [SerializeField]
        private float woodInStorage;

        [SerializeField]
        private float rocksInStorage;

        [SerializeField]
        private float foodInStorage;

        public float WoodInStorage { get => woodInStorage; set => woodInStorage = value; }
        public float RocksInStorage { get => rocksInStorage; set => rocksInStorage = value; }
        public float FoodInStorage { get => foodInStorage; set => foodInStorage = value; }

        public bool AddWoodToStorage(float wood)
        {
            var afterCalculation = WoodInStorage + wood;

            if (afterCalculation > maxStorageCapacity)
            {
                WoodInStorage = maxStorageCapacity;
                return false;
            }
            WoodInStorage += wood;
            return true;
        }
        public bool TakeWoodFromStorage(float wood)
        {
            var afterCalculation = WoodInStorage - wood;

            if (afterCalculation < 0)
            {
                return false;
            }
            WoodInStorage -= wood;
            return true;
        }
        public bool AddRocksToStorage(float rocks)
        {
            var afterCalculation = RocksInStorage + rocks;

            if (afterCalculation > maxStorageCapacity)
            {
                FoodInStorage = maxStorageCapacity;
                return false;
            }
            RocksInStorage += rocks;
            return true;
        }
        public bool TakeRocksFromStorage(float rocks)
        {
            var afterCalculation = RocksInStorage - rocks;

            if (afterCalculation < 0)
            {
                return false;
            }
            RocksInStorage -= rocks;
            return true;
        }
        public bool AddFoodToStorage(float food)
        {
            var afterCalculation = FoodInStorage + food;

            if (afterCalculation > maxStorageCapacity)
            {
                FoodInStorage = maxStorageCapacity;
                return false;
            }
            FoodInStorage += food;
            return true;
        }
        public bool TakeFoodFromStorage(float food)
        {
            var afterCalculation = FoodInStorage - food;

            if (afterCalculation < 0)
            {
                return false;
            }

            FoodInStorage -= food;
            return true;
        }



    }
}