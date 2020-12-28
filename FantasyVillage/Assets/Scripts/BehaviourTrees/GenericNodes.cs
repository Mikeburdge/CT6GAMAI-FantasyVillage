using System.Linq;
using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using LocationThings;
using PathfindingSection;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class GenericNodes
    {


        public class GetMoveToLocation : BtNode
        {
            public Villager villagerRef { get; }
            private VillagerBB vBB;

            private LocationNames _targetLocation;


            public GetMoveToLocation(BaseBlackboard bb, LocationNames inLocations, Villager inVillagerRef) : base(bb)
            {
                villagerRef = inVillagerRef;
                vBB = (VillagerBB)bb;
                _targetLocation = inLocations;
            }

            public override BtStatus Execute()
            {
                var targetPosition = LocationPositions.GetPositionFromLocation(_targetLocation);

                if (targetPosition == Vector3.zero)
                {
                    return BtStatus.Failure;
                }

                vBB.MoveToLocation = targetPosition;

                Pathfinding.GetPlayerPath(villagerRef, targetPosition, out var path);

                if (path == null) return BtStatus.Failure;

                vBB.AStarPath = path;

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
                //Update Floating text
                villagerRef.UpdateAIText($"Moving To {vBB.MoveToLocation}");
                
                //if the @see VillagerMoveAlongPath returns true then return successful, if false then return running
                return villagerRef.VillagerMoveAlongPath() ? BtStatus.Success : BtStatus.Running;
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
                var rv = BtStatus.Running;

                if (!((villagerRef.transform.position - vBB.MoveToLocation).magnitude <= 1.0f)) return rv;

                villagerRef.UpdateAIText("Reached Destination");
                    
                //villagerRef.navMesh.isStopped = true;
                rv = BtStatus.Success;
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
                
                var offset = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));

                vBB.MoveToLocation = villagerRef.transform.position + offset;

                villagerRef.UpdateAIText($"Picked {vBB.MoveToLocation} as random location");

                return BtStatus.Success;
            }
        }
    }
}