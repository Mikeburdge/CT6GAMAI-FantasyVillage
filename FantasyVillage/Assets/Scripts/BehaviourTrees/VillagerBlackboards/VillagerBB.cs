using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.BehaviourTrees.VillagerBlackboards
{
    public class VillagerBB : BaseBlackboard
    {
        public Vector3 MoveToLocation { get; internal set; }
        public List<Vector3> AStarPath { get; internal set; }
        public TreeScript CurrentNearestAvailableTree { get; internal set; }
    }
}