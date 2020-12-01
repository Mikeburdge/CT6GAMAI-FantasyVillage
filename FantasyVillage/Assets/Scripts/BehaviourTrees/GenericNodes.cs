using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using LocationThings;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class GenericNodes
    {


        public class GetMoveToLocation : BtNode
        {
            private VillagerBB vBB;

            private LocationNames _targetLocation;


            public GetMoveToLocation(BaseBlackboard bb, LocationNames inLocations) : base(bb)
            {
                vBB = (VillagerBB)bb;
                _targetLocation = inLocations;
            }

            public override BtStatus Execute()
            {
                Vector3 targetPosition = LocationPositions.GetPositionFromLocation(_targetLocation);

                if (targetPosition == Vector3.zero)
                {
                    return BtStatus.Failure;
                }

                vBB.MoveToLocation = targetPosition;


                return BtStatus.Success;
            }
        }
        
        public class SetMoveToHome : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public SetMoveToHome(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                villagerRef.UpdateAIText("Set Move To Home");
                

                if (!villagerRef.Home)
                {
                    return BtStatus.Failure;
                }

                vBB.MoveToLocation = villagerRef.Home.transform.position;


                return BtStatus.Success;
            }
        }

        public class VillagerMoveTo : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public VillagerMoveTo(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                villagerRef.UpdateAIText($"Moving To {vBB.MoveToLocation}");
                
                villagerRef.VillagerMoveTo(vBB.MoveToLocation);
                return BtStatus.Success;
            }
        }

        public class VillagerWaitTillAtLocation : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public VillagerWaitTillAtLocation(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                BtStatus rv = BtStatus.Running;
                if ((villagerRef.transform.position - vBB.MoveToLocation).magnitude <= 1.0f)
                {
                    villagerRef.UpdateAIText("Reached Destination");
                    
                    //villagerRef.navMesh.isStopped = true;
                    rv = BtStatus.Success;
                }
                return rv;
            }

        }

        public class VillagerStopMovement : BtNode
        {
            private Villager villagerRef;
            public VillagerStopMovement(BaseBlackboard bb, Villager villager) : base(bb)
            {
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                villagerRef.StopMovement();
                return BtStatus.Success;
            }
        }

        public class PickRandomLocationNearby : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public PickRandomLocationNearby(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                
                Vector3 offset = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));

                vBB.MoveToLocation = villagerRef.transform.position + offset;

                villagerRef.UpdateAIText($"Picked {vBB.MoveToLocation} as random location");

                return BtStatus.Success;
            }
        }
    }
}