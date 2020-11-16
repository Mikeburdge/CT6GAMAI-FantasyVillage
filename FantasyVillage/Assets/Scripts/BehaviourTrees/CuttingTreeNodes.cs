using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.Villagers;
using Storage;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class CuttingTreeNodes
    {

        public class PickNearestTree : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public PickNearestTree(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

                TreeScript nearestAvailableTree;

                if (!treeSanctuary.CalculateAvailableNearestTree(villagerRef, out nearestAvailableTree))
                {
                    return BtStatus.Failure;
                }

                vBB.CurrentNearestAvailableTree = nearestAvailableTree;

                nearestAvailableTree.isOccupied = true;

                vBB.MoveToLocation = nearestAvailableTree.transform.position;

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
                //villagerRef.UpdateAIText("Chopped a Tree");

                StorageContainer storage = GameObject.Find("Observer").GetComponent<StorageContainer>();

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

                TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

                return treeSanctuary.AreAvailableTreesInSanctuary() && !vBB.CurrentNearestAvailableTree;
            }
        }
    }
}