using System.Linq;
using Assets.BehaviourTrees;
using Assets.BehaviourTrees.VillagerBlackboards;
using LocationThings;
using PathfindingSection;
using UnityEngine;
using UnityEngine.AI;
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


        public class CheckAStarPath : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;

            public CheckAStarPath(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                //set the previous position
                var previousPosition = villagerRef.transform.position;

                //Scan through each location in the path and check if its theres a valid path
                for (var i = vBB.AStarPath.Count; i > 0; i--)
                {
                    //bit of a cheat using unity's calcualte path to check each individual path created by my a star pathfinding to check if its a valid path.
                    if (!NavMesh.CalculatePath(previousPosition, vBB.AStarPath[i], NavMesh.AllAreas, new NavMeshPath()))
                    {
                        Debug.LogError($"{nameof(CheckAStarPath)} cannot reach destination", villagerRef);
                        return BtStatus.Failure;
                    }

                    previousPosition = vBB.AStarPath[i];
                }

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
                villagerRef.UpdateAIText($"Moving To {vBB.AStarPath.Last()}");

                //if the @see VillagerMoveAlongPath returns true then return successful, if false then return running
                return villagerRef.VillagerMoveAlongPath() ? BtStatus.Success : BtStatus.Running;
            }
        }


        public class PickRandomLocationNearby : BtNode
        {
            private VillagerBB vBB;
            private Villager villagerRef;
            private Vector3 targetPosition;
            public PickRandomLocationNearby(BaseBlackboard bb, Villager villager) : base(bb)
            {
                vBB = (VillagerBB)bb;
                villagerRef = villager;
            }

            public override BtStatus Execute()
            {
                var offset = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));

                targetPosition = villagerRef.transform.position + offset;

                villagerRef.UpdateAIText($"Picked {targetPosition} as random location");

                Pathfinding.GetPlayerPath(villagerRef, targetPosition, out var path);

                if (path == null) return BtStatus.Failure;

                vBB.AStarPath = path;

                return BtStatus.Success;
            }
        }
    }
}