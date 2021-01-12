using Assets.BehaviourTrees;
using BehaviourTrees.VillagerBlackboards;
using LocationThings;
using PathfindingSection;
using System.Linq;
using UnityEngine;
using Villagers;

namespace BehaviourTrees
{
    public class GenericNodes
    {


        public class GetMovePath : BtNode
        {
            public Villager villagerRef { get; }
            private VillagerBB vBB;

            private Vector3 targetPosition;


            public GetMovePath(BaseBlackboard bb, Vector3 _targetLocation, Villager inVillagerRef) : base(bb)
            {
                villagerRef = inVillagerRef;
                vBB = (VillagerBB)bb;
                targetPosition = _targetLocation;
            }

            public override BtStatus Execute()
            {
                //basically a null check
                if (targetPosition == Vector3.zero)
                {
                    return BtStatus.Failure;
                }

                Pathfinding.GetPlayerPath(villagerRef, targetPosition, out var path);

                if (path == null) return BtStatus.Failure;

                vBB.AStarPath = path;

                return BtStatus.Success;
            }
        }

        public class VillagerMoveTo : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;
            private float distanceToTarget;

            public VillagerMoveTo(BaseBlackboard bb, Villager villager, float distanceTo = 0.3f) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
                distanceToTarget = distanceTo;
            }

            public override BtStatus Execute()
            {
                if (vBB.AStarPath.Count <= 0) return BtStatus.Success;
                if (villagerRef.bIsMoving) return BtStatus.Running;

                villagerRef.MinDistanceToMovePos = distanceToTarget;
                villagerRef.bIsMoving = true;

                //sets the MoveToLocations Y to be that of the villager and the rest to the next node in the queue
                //as currently the nodes are on the ground and not adjusted properly for terrain (possible TODO) should be pretty simple tbh but not needed rn
                villagerRef.MoveToLocation = new Vector3(vBB.AStarPath.Last().x, villagerRef.transform.position.y, vBB.AStarPath.Last().z);


                //check if its reached the final node in the path, return success if it has and running if not   
                return vBB.AStarPath.Count <= 0 ? BtStatus.Success : BtStatus.Running;
            }
        }


        public class GetPathToRandomNearbyLocation : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;
            public GetPathToRandomNearbyLocation(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {

                var targetPosition = Object.FindObjectOfType<LocationPositions>().GetRandomIdleLocation().position;

                Pathfinding.GetPlayerPath(villagerRef, targetPosition, out var path);

                if (path == null) return BtStatus.Failure;

                vBB.AStarPath = path;

                return BtStatus.Success;
            }
        }
    }
}