using UnityEngine;
using UnityEditor;

namespace Storage
{
    public class StorageContainer : MonoBehaviour
    {
        [SerializeField]
        public float MaxStorageCapacity = 200;

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
            float AfterCalculation = WoodInStorage + wood;

            if (AfterCalculation > MaxStorageCapacity)
            {
                WoodInStorage = MaxStorageCapacity;
                return false;
            }
            WoodInStorage += wood;
            return true;
        }
        public bool TakeWoodFromStorage(float wood)
        {
            float AfterCalculation = WoodInStorage - wood;

            if (AfterCalculation < 0)
            {
                return false;
            }
            WoodInStorage -= wood;
            return true;
        }
        public bool AddRocksToStorage(float rocks)
        {
            float AfterCalculation = RocksInStorage + rocks;

            if (AfterCalculation > MaxStorageCapacity)
            {
                FoodInStorage = MaxStorageCapacity;
                return false;
            }
            RocksInStorage += rocks;
            return true;
        }
        public bool TakeRocksFromStorage(float rocks)
        {
            float AfterCalculation = RocksInStorage - rocks;

            if (AfterCalculation < 0)
            {
                return false;
            }
            RocksInStorage -= rocks;
            return true;
        }
        public bool AddFoodToStorage(float food)
        {
            float AfterCalculation = FoodInStorage + food;

            if (AfterCalculation > MaxStorageCapacity)
            {
                FoodInStorage = MaxStorageCapacity;
                return false;
            }
            FoodInStorage += food;
            return true;
        }
        public bool TakeFoodFromStorage(float food)
        {
            float AfterCalculation = FoodInStorage - food;

            if (AfterCalculation < 0)
            {
                return false;
            }

            FoodInStorage -= food;
            return true;
        }



    }
}