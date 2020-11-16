using UnityEngine;
using System.Collections;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.Villagers;
using Assets.BehaviourTrees;
using Storage;
using System;

namespace BehaviourTrees
{
    public class CuttingTreeNodes
    {

        public class PickNearestTree : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public PickNearestTree(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

                TreeScript nearestAvailableTree;

                if (!treeSanctuary.CalculateAvailableNearestTree(villagerRef, out nearestAvailableTree))
                {
                    return BTStatus.FAILURE;
                }

                vBB.CurrentNearestAvailableTree = nearestAvailableTree;

                nearestAvailableTree.IsOccupied = true;

                vBB.MoveToLocation = nearestAvailableTree.transform.position;

                return BTStatus.SUCCESS;
            }
        }

        public class ChopTree : BTNode
        {
            private VillagerBB vBB;
            private Villager VillagerRef;

            public ChopTree(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                VillagerRef = villager;
            }

            public override BTStatus Execute()
            {
                vBB.CurrentNearestAvailableTree.Chop(VillagerRef);
                VillagerRef.UpdateAIText("Chopped a Tree");

                StorageContainer Storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

                Storage.AddWoodToStorage(vBB.CurrentNearestAvailableTree.WoodPerChop);

                return BTStatus.SUCCESS;
            }
        }

        public class ChopTreeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;

            public ChopTreeDecorator(BTNode WrappedNode, BaseBlackboard bb) : base(WrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
            }

            public override bool CheckStatus()
            {
                return vBB.CurrentNearestAvailableTree.Health > 0;
            }
        }

        public class FindTreeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;

            public FindTreeDecorator(BTNode WrappedNode, BaseBlackboard bb) : base(WrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
            }

            public override bool CheckStatus()
            {

                TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

                return treeSanctuary.AreAvailableTreesInSanctuary() && !vBB.CurrentNearestAvailableTree;
            }
        }
    }
}