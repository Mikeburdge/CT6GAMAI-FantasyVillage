using UnityEngine;
using System.Collections;
using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using LocationThings;
using Assets.Scripts.Villagers;

namespace BehaviourTrees
{
    public class GenericNodes
    {


        public class GetMoveToLocation : BTNode
        {
            private VillagerBB vBB;

            private LocationNames targetLocation;


            public GetMoveToLocation(BaseBlackboard bb, LocationNames inLocations) : base(bb)
            {
                vBB = (VillagerBB)bb;
                targetLocation = inLocations;
            }

            public override BTStatus Execute()
            {
                Vector3 TargetPosition = LocationPositions.GetPositionFromLocation(targetLocation);

                if (TargetPosition == Vector3.zero)
                {
                    return BTStatus.FAILURE;
                }

                vBB.MoveToLocation = TargetPosition;


                return BTStatus.SUCCESS;
            }
        }
        
        public class SetMoveToHome : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public SetMoveToHome(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                villagerRef.UpdateAIText(this);
                if (!villagerRef.Home)
                {
                    return BTStatus.FAILURE;
                }

                vBB.MoveToLocation = villagerRef.Home.transform.position;


                return BTStatus.SUCCESS;
            }
        }

        public class VillagerMoveTo : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public VillagerMoveTo(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                villagerRef.VillagerMoveTo(vBB.MoveToLocation);
                return BTStatus.SUCCESS;
            }
        }

        public class VillagerWaitTillAtLocation : BTNode
        {
            private VillagerBB vBB;
            private Villager VillagerRef;

            public VillagerWaitTillAtLocation(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                VillagerRef = villager;
            }

            public override BTStatus Execute()
            {
                BTStatus rv = BTStatus.RUNNING;
                if ((VillagerRef.transform.position - vBB.MoveToLocation).magnitude <= 1.0f)
                {
                    VillagerRef.navMesh.isStopped = true;
                    VillagerRef.UpdateAIText(this);
                    rv = BTStatus.SUCCESS;
                }
                return rv;
            }

        }

        public class VillagerStopMovement : BTNode
        {
            private Villager VillagerRef;
            public VillagerStopMovement(BaseBlackboard bb, Villager villager) : base(bb)
            {
                VillagerRef = villager;
            }

            public override BTStatus Execute()
            {
                VillagerRef.StopMovement();
                return BTStatus.SUCCESS;
            }
        }

        public class PickRandomLocationNearby : BTNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public PickRandomLocationNearby(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BTStatus Execute()
            {
                Vector3 A = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));

                vBB.MoveToLocation = villagerRef.transform.position + A;

                return BTStatus.SUCCESS;
            }
        }
    }
}