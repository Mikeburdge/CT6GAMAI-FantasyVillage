using System.Collections.Generic;
using UnityEngine;

namespace PathfindingSection
{
    public class GraphNode
    {
        public bool bIsWalkable;
        public int Triangle { get; set; }
        public Vector3 Position { get; set; }



        public GraphNode(Vector3 position, bool bInIsWalkable = true)
        {
            Position = position;
            bIsWalkable = bInIsWalkable;

        }

        public List<GraphEdge> AdjacencyList = new List<GraphEdge>();

    }
}