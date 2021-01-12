using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees.VillagerBlackboards
{
    public class VillagerBB : BaseBlackboard
    {
        public List<Vector3> AStarPath { get; internal set; }
        public TreeScript CurrentNearestAvailableTree { get; internal set; }

        public TreeGenerator TreeGenerator;

        public List<HouseScript> AvailableHouses;

        public Transform HouseToRepair;
    }
}