using System.Collections.Generic;
using Assets.BehaviourTrees;
using BehaviourTrees.VillagerBlackboards;
using PathfindingSection;
using Storage;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class CuttingTreeNodes
    {

        public class GetPathToNearestTree : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public GetPathToNearestTree(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                var treeSanctuary = Object.FindObjectOfType<TreeGenerator>();

                if (!treeSanctuary.CalculateAvailableNearestTree(villagerRef, out var nearestTree)) return BtStatus.Failure;


                vBB.CurrentNearestAvailableTree = nearestTree;
                nearestTree.isOccupied = true;

                var targetPosition = nearestTree.transform.position;
                //basically a null check
                if (targetPosition == Vector3.zero)
                {
                    return BtStatus.Failure;
                }

                Pathfinding.GetPlayerPath(villagerRef, targetPosition, out var path);

                if (path == null) return BtStatus.Failure;

                vBB.AStarPath = path;

                return BtStatus.Success;
            }
        }


        public class GetDirectPathToTree : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public GetDirectPathToTree(BaseBlackboard bb, Villager Villager) : base(bb)
            {
                villagerRef = Villager;
                vBB = (VillagerBB)bb;
            }

            public override BtStatus Execute()
            {
                var treeSanctuary = vBB.TreeGenerator;

                if (!treeSanctuary.CalculateAvailableNearestTree(villagerRef, out var nearestTree)) return BtStatus.Failure;

                var targetPosition = nearestTree.transform.position;
                //basically a null check
                if (targetPosition == Vector3.zero)
                {
                    return BtStatus.Failure;
                }

                if (vBB.CurrentNearestAvailableTree)
                {
                    vBB.CurrentNearestAvailableTree.isOccupied = false;
                }

                //set the nearest tree
                vBB.CurrentNearestAvailableTree = nearestTree;
                //set nearest tree to occupied
                nearestTree.isOccupied = true;

                targetPosition.y = villagerRef.transform.position.y;

                //HAHAHAHAHACKED THE FUCK OUT OF THIS CODE AND BASICALLY MADE THE PATHFINDING NULL FOR THE TREE PART AHAHAHAHAHAH I HATE MYSELF
                List<Vector3> path = new List<Vector3>() { targetPosition };

                vBB.AStarPath = path;

                //return successful
                return BtStatus.Success;
            }
        }

        public class ChopTree : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public ChopTree(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                vBB.CurrentNearestAvailableTree.Chop(villagerRef);
                villagerRef.UpdateAIText("Chopped a Tree");

                var storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

                storage.AddWoodToStorage(vBB.CurrentNearestAvailableTree.woodPerChop);

                return BtStatus.Success;
            }
        }

        public class ChopTreeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;

            public ChopTreeDecorator(BtNode wrappedNode, BaseBlackboard bb) : base(wrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
            }

            public override bool CheckStatus()
            {
                return vBB.CurrentNearestAvailableTree;
            }
        }

        public class FindTreeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;

            public FindTreeDecorator(BtNode wrappedNode, BaseBlackboard bb) : base(wrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
            }

            public override bool CheckStatus()
            {
                return vBB.TreeGenerator.AreAvailableTreesInSanctuary() && !vBB.CurrentNearestAvailableTree;
            }
        }
    }
}