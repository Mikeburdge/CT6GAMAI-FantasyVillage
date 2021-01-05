using System.Collections.Generic;
using Assets.BehaviourTrees;
using BehaviourTrees.VillagerBlackboards;
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
                //get the tree generator component
                var treeSanctuary = vBB.TreeGenerator;

                //check to see if there is a tree available
                if (!treeSanctuary.CalculateAvailableNearestTree(villagerRef, out var nearestAvailableTree)) return BtStatus.Failure;

                //set the nearest tree
                vBB.CurrentNearestAvailableTree = nearestAvailableTree;

                var transform = nearestAvailableTree.transform.position;
                transform.y = villagerRef.transform.position.y;

                //get path to nearest tree
                //Pathfinding.GetPlayerPath(villagerRef, transform, out var path);

                List<Vector3> path = new List<Vector3>(){transform};

                //null check
                if (path == null) return BtStatus.Failure;

                //adds the location of the tree to the path. todo: check to see if it needs to be added to the front or back of the path 
                //HAHAHAHAHACKED THE FUCK OUT OF THIS CODE AND BASICALLY MADE THE PATHFINDING NULL FOR THE TREE PART AHAHAHAHAHAH I HATE MYSELF
                //path.Add(nearestAvailableTree.gameObject.transform.position);
                //path.Insert(0, nearestAvailableTree.gameObject.transform.position);

                //set a* path
                vBB.AStarPath = path;

                //set nearest tree to occupied
                nearestAvailableTree.isOccupied = true;

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
                if (!vBB.CurrentNearestAvailableTree) return false;

                return vBB.CurrentNearestAvailableTree.health > 0;
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