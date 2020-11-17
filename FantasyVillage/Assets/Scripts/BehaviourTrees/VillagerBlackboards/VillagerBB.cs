using UnityEngine;

namespace Assets.BehaviourTrees.VillagerBlackboards
{
    public class VillagerBB : BaseBlackboard
    {
        public Vector3 MoveToLocation { get; internal set; }

        public TreeScript CurrentNearestAvailableTree { get; internal set; }
    }
}