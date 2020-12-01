using JetBrains.Annotations;
using Priority_Queue;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;


namespace Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        private List<GraphNode> Nodes = new List<GraphNode>();

        public GameObject NodeStorage;
        public GameObject NodeVisualizer;


        private void Start()
        {
            LoadGraph();

            var path = FindPathAStar(3, 6);

            if (path == null) return;

            var pathway = "A*: ";
            foreach (var node in path)
            {
                pathway += node + ", ";
            }


            Debug.Log(pathway);

        }

        private void LoadGraph()
        {

            NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

            Mesh mesh = new Mesh();
            mesh.name = "NavMesh";
            mesh.vertices = triangulatedNavMesh.vertices;
            mesh.triangles = triangulatedNavMesh.indices;



            for (var i = 0; i < mesh.vertices.Length; i++)
            {
                var vertex = mesh.vertices[i];
                Nodes.Add(new GraphNode(vertex));

                Vector3 aboveVertex = new Vector3(vertex.x, vertex.y + 5, vertex.z);

                Debug.DrawLine(vertex, aboveVertex, Color.red, 10000000);

                var visualizerNumber = Instantiate(NodeVisualizer, aboveVertex, Quaternion.identity, NodeStorage.transform);

                foreach (var nodeTransform in NodeStorage.GetComponentsInChildren<Transform>())
                {
                    if (nodeTransform.name == visualizerNumber.name)
                        break;

                    if (nodeTransform.position == visualizerNumber.transform.position)
                    {
                        visualizerNumber.transform.position += new Vector3(0,1,0);
                    }
                }


                // TextMesh Pro Implementation
                var textMeshPro = visualizerNumber.transform.Find("Text Area").gameObject.AddComponent<TextMeshProUGUI>();

                textMeshPro.rectTransform.sizeDelta = new Vector2(3, 1);

                textMeshPro.color = Color.black;
                textMeshPro.enableAutoSizing = true;
                textMeshPro.fontSizeMin = 0;

                textMeshPro.alignment = TextAlignmentOptions.Center;

                textMeshPro.enableKerning = false;
                textMeshPro.text = i.ToString();
                visualizerNumber.name = i.ToString();



            }

            for (var i = 0; i < mesh.triangles.Length; i += 3)
            {
                var nodeOne = mesh.triangles[i];
                var nodeTwo = mesh.triangles[i + 1];
                var nodeThree = mesh.triangles[i + 2];


                Debug.DrawLine(Nodes[nodeOne].Position, Nodes[nodeTwo].Position, Color.green, 10000000);

                Debug.DrawLine(Nodes[nodeTwo].Position, Nodes[nodeThree].Position, Color.green, 10000000);

                Debug.DrawLine(Nodes[nodeThree].Position, Nodes[nodeOne].Position, Color.green, 10000000);

                Nodes[nodeOne].AdjacencyList.Add(new GraphEdge(nodeOne, nodeTwo, Vector3.Distance(Nodes[nodeOne].Position, Nodes[nodeTwo].Position)));
                Nodes[nodeTwo].AdjacencyList.Add(new GraphEdge(nodeTwo, nodeOne, Vector3.Distance(Nodes[nodeTwo].Position, Nodes[nodeOne].Position)));

                Nodes[nodeTwo].AdjacencyList.Add(new GraphEdge(nodeTwo, nodeThree, Vector3.Distance(Nodes[nodeTwo].Position, Nodes[nodeThree].Position)));
                Nodes[nodeThree].AdjacencyList.Add(new GraphEdge(nodeThree, nodeTwo, Vector3.Distance(Nodes[nodeThree].Position, Nodes[nodeTwo].Position)));

                Nodes[nodeThree].AdjacencyList.Add(new GraphEdge(nodeThree, nodeOne, Vector3.Distance(Nodes[nodeThree].Position, Nodes[nodeOne].Position)));
                Nodes[nodeOne].AdjacencyList.Add(new GraphEdge(nodeOne, nodeThree, Vector3.Distance(Nodes[nodeOne].Position, Nodes[nodeThree].Position)));
            }
        }


        //List<int> FindPathDFS(int SourcePos, int targetPos)
        //{
        //    bool[] IsVisited = new bool[Nodes.Count];

        //    List<int> route = new List<int>();

        //    foreach (var unused in Nodes)
        //    {
        //        route.Add(new int());
        //    }

        //    Stack<GraphEdge> Edges = new Stack<GraphEdge>();

        //    IsVisited[SourcePos] = true;



        //    foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
        //    {
        //        Edges.Push(adjacentNode);
        //    }

        //    while (Edges.Count > 0)
        //    {
        //        GraphEdge currentEdge = Edges.Pop();

        //        route[currentEdge.To] = currentEdge.From;
        //        IsVisited[currentEdge.To] = true;


        //        if (currentEdge.To == targetPos)
        //        {
        //            int currentNode = targetPos;

        //            List<int> path = new List<int> { targetPos };

        //            while (currentNode != SourcePos)
        //            {
        //                currentNode = route[currentNode];
        //                path.Add(currentNode);
        //            }

        //            return path;
        //        }

        //        foreach (GraphEdge adjacentEdge in Nodes[currentEdge.To].AdjacencyList)
        //        {
        //            if (!IsVisited[adjacentEdge.To])
        //            {
        //                Edges.Push(adjacentEdge);
        //            }
        //        }

        //    }

        //    return null;
        //}

        //List<int> FindPathBFS(int SourcePos, int targetPos)
        //{
        //    bool[] IsVisited = new bool[Nodes.Count];

        //    List<int> route = new List<int>();

        //    foreach (var unused in Nodes)
        //    {
        //        route.Add(new int());
        //    }

        //    Queue<GraphEdge> Edges = new Queue<GraphEdge>();

        //    IsVisited[SourcePos] = true;


        //    foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
        //    {
        //        Edges.Enqueue(adjacentNode);
        //    }

        //    while (Edges.Count > 0)
        //    {
        //        GraphEdge currentEdge = Edges.Dequeue();

        //        route[currentEdge.To] = currentEdge.From;
        //        IsVisited[currentEdge.To] = true;


        //        if (currentEdge.To == targetPos)
        //        {
        //            int currentNode = targetPos;

        //            List<int> path = new List<int> { targetPos };

        //            while (currentNode != SourcePos)
        //            {
        //                currentNode = route[currentNode];
        //                path.Add(currentNode);
        //            }

        //            return path;
        //        }

        //        foreach (GraphEdge adjacentEdge in Nodes[currentEdge.To].AdjacencyList)
        //        {
        //            if (!IsVisited[adjacentEdge.To])
        //            {
        //                Edges.Enqueue(adjacentEdge);
        //            }
        //        }

        //    }

        //    return null;
        //}

        //List<int> FindPathDijkstra(int SourcePos, int targetPos)
        //{
        //    //Create a route list
        //    List<int> route = new List<int>();
        //    //create a cost list
        //    List<float> cost = new List<float>();

        //    //initialize the variables to the maximum value
        //    for (int i = 0; i < Nodes.Count; i++)
        //    {
        //        route.Add(new int());
        //        cost.Add(float.MaxValue);
        //    }

        //    //set the cost of the starting node 0
        //    cost[SourcePos] = 0.0f;



        //    //create array of traversed edges
        //    List<GraphEdge> traversedEdges = new List<GraphEdge>();

        //    //create minimum priority queue
        //    SimplePriorityQueue<GraphEdge> GraphPriorityQueue = new SimplePriorityQueue<GraphEdge>();

        //    //bool to store whether we've found the target node yet
        //    bool bIsTargetNodeFound = false;

        //    foreach (GraphEdge adjacentNode in Nodes[SourcePos].AdjacencyList)
        //    {
        //        GraphPriorityQueue.Enqueue(adjacentNode, adjacentNode.GetCost());
        //    }

        //    while (GraphPriorityQueue.Count > 0)
        //    {
        //        GraphEdge currentEdge = GraphPriorityQueue.Dequeue();

        //        traversedEdges.Add(currentEdge);

        //        float potCost = cost[currentEdge.From] + currentEdge.GetCost();

        //        if (cost[currentEdge.To] > potCost)
        //        {
        //            route[currentEdge.To] = currentEdge.From;
        //            cost[currentEdge.To] = potCost;
        //        }


        //        if (currentEdge.To == targetPos)
        //            bIsTargetNodeFound = true;

        //        foreach (var adjacentNode in Nodes[currentEdge.To].AdjacencyList)
        //        {
        //            if (traversedEdges.Contains(adjacentNode) || GraphPriorityQueue.Contains(adjacentNode))
        //            {
        //                // DO NOT ADD IT TO THE PRIORITY QUEUE
        //            }
        //            else //ADD IT TO THE PRIORITY QUEUE
        //            {
        //                GraphPriorityQueue.Enqueue(adjacentNode, currentEdge.GetCost() + adjacentNode.GetCost());
        //            }
        //        }
        //    }

        //    if (bIsTargetNodeFound)
        //    {
        //        int currentNode = targetPos;

        //        List<int> path = new List<int> { targetPos };

        //        while (currentNode != SourcePos)
        //        {
        //            currentNode = route[currentNode];
        //            path.Add(currentNode);
        //        }

        //        return path;
        //    }


        //    return null;
        //}

        public List<Vector3> FindPathAStar(int sourcePos, int targetPos)
        {

            //var startNode = new GraphNode(sourcePos);

            //var targetNode = new GraphNode(targetPos);


            //Create a route list
            var route = new List<int>();
            //create a cost list
            var cost = new List<float>();

            //initialize the variables to the maximum value
            for (var i = 0; i < Nodes.Count; i++)
            {
                route.Add(new int());
                cost.Add(float.MaxValue);
            }

            //set the cost of the starting node 0
            cost[sourcePos] = 0.0f;


            //create array of traversed edges
            var traversedEdges = new List<GraphEdge>();

            //create minimum priority queue
            var graphPriorityQueue = new SimplePriorityQueue<GraphEdge>();
            
            foreach (GraphEdge adjacentNode in Nodes[sourcePos].AdjacencyList)
            {
                graphPriorityQueue.Enqueue(adjacentNode, adjacentNode.GetCost());
            }

            //While the queue is not empty
            while (graphPriorityQueue.Count > 0)
            {
                var currentEdge = graphPriorityQueue.Dequeue();

                traversedEdges.Add(currentEdge);

                var gCost = cost[currentEdge.From] + currentEdge.GetCost();

                var hCost = Abs(Nodes[currentEdge.To].Position - Nodes[targetPos].Position).magnitude; //this might be wrong i just guessed that you 

                var fCost = gCost + hCost;

                if (cost[currentEdge.To] > fCost)
                {
                    route[currentEdge.To] = currentEdge.From;
                    cost[currentEdge.To] = fCost;
                }

                if (currentEdge.To == targetPos)

                foreach (var adjacentNode in Nodes[currentEdge.To].AdjacencyList)
                {
                    if (traversedEdges.Contains(adjacentNode) || graphPriorityQueue.Contains(adjacentNode))
                    {
                        // DO NOT ADD IT TO THE PRIORITY QUEUE
                    }
                    else //ADD IT TO THE PRIORITY QUEUE
                    {
                        graphPriorityQueue.Enqueue(adjacentNode, currentEdge.GetCost() + adjacentNode.GetCost());
                    }
                }

                if (currentEdge.To == targetPos)
                {
                    int currentNode = targetPos;

                    List<Vector3> path = new List<Vector3> { Nodes[targetPos].Position };

                    while (currentNode != sourcePos)
                    {
                        currentNode = route[currentNode];
                        path.Add(Nodes[currentNode].Position);
                    }

                    return path;
                }
            }



            return null;
        }

        private static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

    }

}

