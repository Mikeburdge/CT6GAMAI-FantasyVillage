using UnityEngine;
using System.Collections;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.Villagers;
using Assets.BehaviourTrees;
using Storage;
using System;

namespace BehaviourTrees
{
    public class HomeNodes
    {
        public class GoHomeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;
            Villager villagerRef;

            public GoHomeDecorator(BTNode WrappedNode, BaseBlackboard bb, Villager villager) : base(WrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override bool CheckStatus()
            {
                villagerRef.UpdateAIText("Checking if can go home");
                return villagerRef.Stamina < 100;
            }
        }

        public class EnterHome : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public EnterHome(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                villagerRef.UpdateAIText("Entered House");
                   villagerRef.GetComponent<Renderer>().enabled = false;
                return BTStatus.SUCCESS;
            }

        }

        public class ReplenishHealthAndStamina : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public ReplenishHealthAndStamina(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                float preCalc = villagerRef.Health;

                villagerRef.Health += 20;

                if (villagerRef.Health > 100)
                {
                    villagerRef.Health = 100;
                }
                villagerRef.UpdateAIText("healed " + (villagerRef.Health - preCalc));

                preCalc = villagerRef.Stamina;

                villagerRef.Stamina += 20;

                if (villagerRef.Stamina > 100)
                {
                    villagerRef.Stamina = 100;
                }

                villagerRef.AppendAIText(villagerRef + " has recovered " + (villagerRef.Stamina - preCalc) + " stamina");

                return BTStatus.SUCCESS;
            }

        }

    }
}