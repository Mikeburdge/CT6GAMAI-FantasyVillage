using System.Collections.Generic;

namespace Pathfinding
{
    public class GraphNode
    {
        public int NodeIndex;

        public int X { get; set; }
        public int Y { get; set; }

        public List<GraphEdge> AdjacencyList = new List<GraphEdge>();
        
    }
}