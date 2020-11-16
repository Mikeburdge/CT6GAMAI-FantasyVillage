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
                villagerRef.GetComponent<Renderer>().enabled = false;
                return BTStatus.SUCCESS;
            }

        }

        public class ReplenishHealthAndStamina : BTNode
        {
            private VillagerBB vBB;
            private Villager VillagerRef;

            public ReplenishHealthAndStamina(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                VillagerRef = villager;
            }

            public override BTStatus Execute()
            {
                float preCalc = VillagerRef.Health;

                VillagerRef.Health += 20;

                if (VillagerRef.Health > 100)
                {
                    VillagerRef.Health = 100;
                }
                Debug.Log(VillagerRef + " has been healed by " + (VillagerRef.Health - preCalc));

                preCalc = VillagerRef.Stamina;

                VillagerRef.Stamina += 20;

                if (VillagerRef.Stamina > 100)
                {
                    VillagerRef.Stamina = 100;
                }

                Debug.Log(VillagerRef + " has recovered " + (VillagerRef.Stamina - preCalc) + " stamina");

                return BTStatus.SUCCESS;
            }

        }

    }
}