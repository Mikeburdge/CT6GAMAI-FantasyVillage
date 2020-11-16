using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using Assets.Scripts.Villagers;
using UnityEngine;

namespace BehaviourTrees
{
    public class HomeNodes
    {
        public class GoHomeDecorator : ConditionalDecorator
        {
            VillagerBB vBB;
            Villager VillagerRef;

            public GoHomeDecorator(BTNode WrappedNode, BaseBlackboard bb, Villager villager) : base(WrappedNode, bb)
            {
                vBB = (VillagerBB)bb;
                VillagerRef = villager;
            }

            public override bool CheckStatus()
            {
                VillagerRef.UpdateAIText("Checking if can go home");
                return VillagerRef.Stamina < 100;
            }
        }

        public class EnterHome : BTNode
        {
            private VillagerBB vBB;
            private Villager VillagerRef;

            public EnterHome(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                VillagerRef = villager;
            }

            public override BTStatus Execute()
            {
                VillagerRef.UpdateAIText("Entered House");
                   VillagerRef.GetComponent<Renderer>().enabled = false;
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
                VillagerRef.UpdateAIText("healed " + (VillagerRef.Health - preCalc));

                preCalc = VillagerRef.Stamina;

                VillagerRef.Stamina += 20;

                if (VillagerRef.Stamina > 100)
                {
                    VillagerRef.Stamina = 100;
                }

                VillagerRef.UpdateAIText(VillagerRef + " has recovered " + (VillagerRef.Stamina - preCalc) + " stamina");

                return BTStatus.SUCCESS;
            }

        }

    }
}