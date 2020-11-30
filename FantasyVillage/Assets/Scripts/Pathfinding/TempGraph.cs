using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;


namespace Pathfinding
{
    public class TempGraph : MonoBehaviour
    {
        private List<GraphNode> Nodes;


        void Start()
        {
            LoadGraph();

            List<int> Path = FindPathDFS(0, 8);


            string pathway = "DFS: ";
            foreach (var node in Path)
            {
                pathway += node + ", ";
            }

            Debug.Log(pathway);

            Path = FindPathBFS(0, 8);
            pathway = "BFS: ";
            foreach (var node in Path)
            {
                pathway += node + ", ";
            }

            Debug.Log(pathway);

            Path = FindPathDijkstra(0, 8);
            pathway = "Dijkstra: ";
            foreach (var node in Path)
            {
                pathway += node + ", ";
            }

            Debug.Log(pathway);

            Path = FindPathAStar(0, 8);
            pathway = "A*: ";
            foreach (var node in Path)
            {
                pathway += node + ", ";
            }


            Debug.Log(pathway);
        }

        public void LoadGraph()
        {
            //You would use this function to "Load" in a graph dynamically. We're just going to create a basic graph like so:
            // 6-7-8
            // | | |
            // 3-4-5
            // | | |
            // 0-1-2

            // I do not recommend creating your graphs this way in the real world. You should look to dynamically create them instead.
            List<GraphNode> nodesArr = new List<GraphNode>();

            for (int i = 0; i < 9; i++)
            {
                nodesArr.Add(new GraphNode());
                nodesArr[i].NodeIndex = i;
            }

            //Node 0
            nodesArr[0].AdjacencyList.Add(new QuickGraphEdge(0, 1));
            nodesArr[0].AdjacencyList.Add(new GraphEdge(0, 3));

            //Node 1
            nodesArr[1].AdjacencyList.Add(new GraphEdge(1, 0));
            nodesArr[1].AdjacencyList.Add(new QuickGraphEdge(1, 2));
            nodesArr[1].AdjacencyList.Add(new GraphEdge(1, 4));

            //Node 2
            nodesArr[2].AdjacencyList.Add(new GraphEdge(2, 1));
            nodesArr[2].AdjacencyList.Add(new QuickGraphEdge(2, 5));

            nodesArr[3].AdjacencyList.Add(new GraphEdge(3, 0));
            nodesArr[3].AdjacencyList.Add(new GraphEdge(3, 4));
            nodesArr[3].AdjacencyList.Add(new GraphEdge(3, 6));

            nodesArr[4].AdjacencyList.Add(new GraphEdge(4, 1));
            nodesArr[4].AdjacencyList.Add(new GraphEdge(4, 3));
            nodesArr[4].AdjacencyList.Add(new GraphEdge(4, 5));
            nodesArr[4].AdjacencyList.Add(new QuickGraphEdge(4, 7));

            nodesArr[5].AdjacencyList.Add(new GraphEdge(5, 2));
            nodesArr[5].AdjacencyList.Add(new QuickGraphEdge(5, 4));
            nodesArr[5].AdjacencyList.Add(new GraphEdge(5, 8));

            nodesArr[6].AdjacencyList.Add(new GraphEdge(6, 3));
            nodesArr[6].AdjacencyList.Add(new GraphEdge(6, 7));

            nodesArr[7].AdjacencyList.Add(new GraphEdge(7, 6));
            nodesArr[7].AdjacencyList.Add(new GraphEdge(7, 4));
            nodesArr[7].AdjacencyList.Add(new QuickGraphEdge(7, 8));

            nodesArr[8].AdjacencyList.Add(new GraphEdge(8, 7));
            nodesArr[8].AdjacencyList.Add(new GraphEdge(8, 5));


            Nodes = new List<GraphNode>();

            for (int i = 0; i < 9; i++)
                Nodes.Add(nodesArr[i]);


            int node = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Nodes[node].X = x;
                    Nodes[node].Y = y;
                    node++;
                }
            }

        }

        List<int> FindPathDFS(int SourcePos, int TargetPos)
        {
            bool[] IsVisited = new bool[Nodes.Count];

            List<int> route = new List<int>();

            foreach (var unused in Nodes)
            {
                route.Add(new int());
            }

            Stack<GraphEdge> Edges = new Stack<GraphEdge>();

            IsVisited[SourcePos] = true;



            foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
            {
                Edges.Push(adjacentNode);
            }

            while (Edges.Count > 0)
            {
                GraphEdge currentEdge = Edges.Pop();

                route[currentEdge.To] = currentEdge.From;
                IsVisited[currentEdge.To] = true;


                if (currentEdge.To == TargetPos)
                {
                    int currentNode = TargetPos;

                    List<int> path = new List<int> { TargetPos };

                    while (currentNode != SourcePos)
                    {
                        currentNode = route[currentNode];
                        path.Add(currentNode);
                    }

                    return path;
                }

                foreach (GraphEdge adjacentEdge in Nodes[currentEdge.To].AdjacencyList)
                {
                    if (!IsVisited[adjacentEdge.To])
                    {
                        Edges.Push(adjacentEdge);
                    }
                }

            }

            return null;
        }

        List<int> FindPathBFS(int SourcePos, int TargetPos)
        {
            bool[] IsVisited = new bool[Nodes.Count];

            List<int> route = new List<int>();

            foreach (var unused in Nodes)
            {
                route.Add(new int());
            }

            Queue<GraphEdge> Edges = new Queue<GraphEdge>();

            IsVisited[SourcePos] = true;


            foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
            {
                Edges.Enqueue(adjacentNode);
            }

            while (Edges.Count > 0)
            {
                GraphEdge currentEdge = Edges.Dequeue();

                route[currentEdge.To] = currentEdge.From;
                IsVisited[currentEdge.To] = true;


                if (currentEdge.To == TargetPos)
                {
                    int currentNode = TargetPos;

                    List<int> path = new List<int> { TargetPos };

                    while (currentNode != SourcePos)
                    {
                        currentNode = route[currentNode];
                        path.Add(currentNode);
                    }

                    return path;
                }

                foreach (GraphEdge adjacentEdge in Nodes[currentEdge.To].AdjacencyList)
                {
                    if (!IsVisited[adjacentEdge.To])
                    {
                        Edges.Enqueue(adjacentEdge);
                    }
                }

            }

            return null;
        }

        List<int> FindPathDijkstra(int SourcePos, int TargetPos)
        {
            //Create a route list
            List<int> route = new List<int>();
            //create a cost list
            List<float> cost = new List<float>();

            //initialize the variables to the maximum value
            for (int i = 0; i < Nodes.Count; i++)
            {
                route.Add(new int());
                cost.Add(float.MaxValue);
            }

            //set the cost of the starting node 0
            cost[SourcePos] = 0.0f;



            //create array of traversed edges
            List<GraphEdge> traversedEdges = new List<GraphEdge>();

            //create minimum priority queue
            SimplePriorityQueue<GraphEdge> GraphPriorityQueue = new SimplePriorityQueue<GraphEdge>();

            //bool to store whether we've found the target node yet
            bool bIsTargetNodeFound = false;

            foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
            {
                GraphPriorityQueue.Enqueue(adjacentNode, adjacentNode.GetCost());
            }

            while (GraphPriorityQueue.Count > 0)
            {
                GraphEdge currentEdge = GraphPriorityQueue.Dequeue();

                traversedEdges.Add(currentEdge);

                float potCost = cost[currentEdge.From] + currentEdge.GetCost();

                if (cost[currentEdge.To] > potCost)
                {
                    route[currentEdge.To] = currentEdge.From;
                    cost[currentEdge.To] = potCost;
                }


                if (currentEdge.To == TargetPos)
                    bIsTargetNodeFound = true;

                foreach (var adjacentNode in Nodes[currentEdge.To].AdjacencyList)
                {
                    if (traversedEdges.Contains(adjacentNode) || GraphPriorityQueue.Contains(adjacentNode))
                    {
                        // DO NOT ADD IT TO THE PRIORITY QUEUE
                    }
                    else //ADD IT TO THE PRIORITY QUEUE
                    {
                        GraphPriorityQueue.Enqueue(adjacentNode, currentEdge.GetCost() + adjacentNode.GetCost());
                    }
                }
            }

            if (bIsTargetNodeFound)
            {
                int currentNode = TargetPos;

                List<int> path = new List<int> { TargetPos };

                while (currentNode != SourcePos)
                {
                    currentNode = route[currentNode];
                    path.Add(currentNode);
                }

                return path;
            }


            return null;
        }


        List<int> FindPathAStar(int SourcePos, int TargetPos)
        {
            //Create a route list
            List<int> route = new List<int>();
            //create a cost list
            List<float> cost = new List<float>();

            //initialize the variables to the maximum value
            for (int i = 0; i < Nodes.Count; i++)
            {
                route.Add(new int());
                cost.Add(float.MaxValue);
            }

            //set the cost of the starting node 0
            cost[SourcePos] = 0.0f;



            //create array of traversed edges
            List<GraphEdge> traversedEdges = new List<GraphEdge>();

            //create minimum priority queue
            SimplePriorityQueue<GraphEdge> GraphPriorityQueue = new SimplePriorityQueue<GraphEdge>();

            //bool to store whether we've found the target node yet
            bool bIsTargetNodeFound = false;

            foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
            {
                GraphPriorityQueue.Enqueue(adjacentNode, adjacentNode.GetCost());
            }

            while (GraphPriorityQueue.Count > 0)
            {
                GraphEdge currentEdge = GraphPriorityQueue.Dequeue();

                traversedEdges.Add(currentEdge);

                float gCost = cost[currentEdge.From] + currentEdge.GetCost();

                float hCost = Mathf.Abs(Nodes[currentEdge.To].X - Nodes[currentEdge.From].X) +
                              Mathf.Abs(Nodes[currentEdge.To].Y - Nodes[currentEdge.From].Y);

                float fCost = gCost + hCost;

                if (cost[currentEdge.To] > fCost)
                {
                    route[currentEdge.To] = currentEdge.From;
                    cost[currentEdge.To] = fCost;
                }


                if (currentEdge.To == TargetPos)
                    bIsTargetNodeFound = true;

                foreach (var adjacentNode in Nodes[currentEdge.To].AdjacencyList)
                {
                    if (traversedEdges.Contains(adjacentNode) || GraphPriorityQueue.Contains(adjacentNode))
                    {
                        // DO NOT ADD IT TO THE PRIORITY QUEUE
                    }
                    else //ADD IT TO THE PRIORITY QUEUE
                    {
                        GraphPriorityQueue.Enqueue(adjacentNode, currentEdge.GetCost() + adjacentNode.GetCost());
                    }
                }
            }

            if (bIsTargetNodeFound)
            {
                int currentNode = TargetPos;

                List<int> path = new List<int> { TargetPos };

                while (currentNode != SourcePos)
                {
                    currentNode = route[currentNode];
                    path.Add(currentNode);
                }

                return path;
            }


            return null;
        }

    }
}