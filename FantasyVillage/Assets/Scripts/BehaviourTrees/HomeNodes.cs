using System.Collections.Generic;
using System.Linq;
using Assets.BehaviourTrees;
using BehaviourTrees.VillagerBlackboards;
using PathfindingSection;
using Storage;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class HomeNodes
    {
        public class GoHomeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;
            Villager villagerRef;

            public GoHomeDecorator(BtNode wrappedNode, BaseBlackboard bb, Villager villager) : base(wrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override bool CheckStatus()
            {
                return villagerRef.Stamina < 100;
            }
        }

        public class CanRepairHomeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;
            Villager villagerRef;

            public CanRepairHomeDecorator(BtNode wrappedNode, BaseBlackboard bb, Villager villager) : base(wrappedNode,
                bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override bool CheckStatus()
            {
                List<HouseScript> availableHouses = villagerRef.homes.Where(house => house.needsRepairing).ToList();

                vBB.AvailableHouses = availableHouses;

                return availableHouses.Count > 0;
            }
        }

        public class HouseRangeDecorator : ConditionalDecorator
        {
            private bool WithinRange;
            VillagerBB vBB;
            Villager villagerRef;

            public HouseRangeDecorator(BtNode wrappedNode, BaseBlackboard bb, Villager villager, bool withinRange) : base(
                wrappedNode, bb)
            {
                WithinRange = withinRange;
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override bool CheckStatus()
            {
                var distance = Vector3.Distance(vBB.DoorTransform.position,
                    villagerRef.transform.position);
                var range = 1;

                if (distance < range && WithinRange)
                {
                    return true;
                }

                return distance > range && !WithinRange;
            }
        }

        public class EnterHome : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public EnterHome(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                villagerRef.ChangeVisibility(false);

                foreach (var renderer in villagerRef.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }

                return BtStatus.Success;
            }

        }

        public class ReplenishHealthAndStamina : BtNode
        {
            private Villager villagerRef;

            public ReplenishHealthAndStamina(BaseBlackboard bb, Villager villager) : base(bb)
            {
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                villagerRef.Health += 20;

                if (villagerRef.Health > 100)
                {
                    villagerRef.Health = 100;
                }

                villagerRef.Stamina += 20;

                if (villagerRef.Stamina > 100)
                {
                    villagerRef.Stamina = 100;
                }


                return BtStatus.Success;
            }
        }

        public class GetPathToHouseForRepair : BtNode
        {
            private Villager villagerRef;
            private VillagerBB vBB;

            public GetPathToHouseForRepair(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                if (!vBB.HouseToRepair) return BtStatus.Failure;

                Pathfinding.GetPlayerPath(villagerRef, vBB.DoorTransform.position, out var path, false);
                vBB.AStarPath = path;

                return BtStatus.Success;
            }
        }

        public class GetHouseOnLowestHealth : BtNode
        {
            private VillagerBB vBB;

            public GetHouseOnLowestHealth(BaseBlackboard bb) : base(bb)
            {
                vBB = (VillagerBB)bb;
            }

            public override BtStatus Execute()
            {
                if (vBB.AvailableHouses.Count == 0) return BtStatus.Failure;

                var lowestHealthHouse = vBB.AvailableHouses[0];

                foreach (var house in vBB.AvailableHouses.Where(house => house.HouseHealth < lowestHealthHouse.HouseHealth))
                {
                    lowestHealthHouse = house;
                }

                vBB.DoorTransform = lowestHealthHouse.door.transform;
                vBB.HouseToRepair = lowestHealthHouse;

                return BtStatus.Success;
            }
        }

        public class SlapWoodOnHouse : BtNode
        {
            private Villager villagerRef;
            private VillagerBB vBB;

            public SlapWoodOnHouse(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                if (vBB.AvailableHouses.Count == 0) return BtStatus.Failure;

                var storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

                float woodForRepair = 30;

                if (storage.WoodInStorage < woodForRepair) return BtStatus.Failure;

                storage.TakeWoodFromStorage(woodForRepair);

                vBB.HouseToRepair.RepairHouseBy(woodForRepair);

                return BtStatus.Success;
            }
        }
    }
}
