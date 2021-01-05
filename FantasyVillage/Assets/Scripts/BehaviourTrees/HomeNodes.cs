using Assets.BehaviourTrees;
using BehaviourTrees.VillagerBlackboards;
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
    }
}