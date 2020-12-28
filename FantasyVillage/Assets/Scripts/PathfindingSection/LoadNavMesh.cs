using System;
using System.Collections;
using System.Collections.Generic;
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