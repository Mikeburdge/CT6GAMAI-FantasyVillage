using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class GraphNode
    {
        public bool bIsWalkable;
        public Vector3 Position { get; set; }



        public GraphNode(Vector3 position, bool bIsWalkable = true)
        {
            Position = position;
            this.bIsWalkable = bIsWalkable;
        }

        public List<GraphEdge> AdjacencyList = new List<GraphEdge>();

    }
}