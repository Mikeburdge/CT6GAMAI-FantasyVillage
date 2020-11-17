using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.Villagers;
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
                villagerRef.GetComponent<Renderer>().enabled = false;
                return BtStatus.Success;
            }

        }

        public class ReplenishHealthAndStamina : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public ReplenishHealthAndStamina(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                float preCalc = villagerRef.Health;

                villagerRef.Health += 20;

                if (villagerRef.Health > 100)
                {
                    villagerRef.Health = 100;
                }
                villagerRef.UpdateAIText($"healed {villagerRef.Health - preCalc} health");

                preCalc = villagerRef.Stamina;

                villagerRef.Stamina += 20;

                if (villagerRef.Stamina > 100)
                {
                    villagerRef.Stamina = 100;
                }

                villagerRef.UpdateAIText($" has recovered {villagerRef.Stamina - preCalc} stamina");

                return BtStatus.Success;
            }
        }
    }
}