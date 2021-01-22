using UnityEngine;

namespace PathfindingSection
{

    public class LoadNavMesh : MonoBehaviour
    {
        private void Awake()
        {
            Pathfinding.LoadGraph();
        }
    }
}