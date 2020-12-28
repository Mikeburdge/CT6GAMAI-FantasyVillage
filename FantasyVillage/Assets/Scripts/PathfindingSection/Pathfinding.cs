﻿using System.Collections;
using Priority_Queue;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Villagers;


namespace PathfindingSection
{
    public abstract class Pathfinding
    {
        private static List<GraphNode> Nodes = new List<GraphNode>();

        public static bool GetPlayerPath(Humanoid playerToMove, Vector3 targetLocation, out List<Vector3> path)
        {
            path = FindPathAStar(new Vector3(22.5f, 1.5f, 5f), new Vector3(-6, 1.5f, -10));
            //path = FindPathAStar(playerToMove.transform.position, targetLocation);

            if (path != null) return true;


            Debug.LogError("Could not find a path");
            return false;


            //TODO: MAYBE MOVE THIS INTO A MOVING STATE TO GET MORE USE OUT OF THE FSM AND MAKE IT LOOK LIKE IT HAS AN ACTUAL IMPACT LMAO
            //todo: ahahahahaha sucks to be you, turns out you'll probably have to do this anyway looooooooool future future michael is fine but you're fucked cuuuunt

        }

        public static void LoadGraph()
        {
            var triangulatedNavMesh = NavMesh.CalculateTriangulation();

            var vertices = new List<Vector3>(triangulatedNavMesh.vertices); ;

            var triangles = new List<int>(triangulatedNavMesh.indices);

            SimplifyMeshTopology(vertices, triangles, 0.1f);

            var mesh = new Mesh()
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };


            for (var i = 0; i < mesh.vertices.Length; i++)
            {
                var vertex = mesh.vertices[i];

                Nodes.Add(new GraphNode(vertex));

                var aboveVertex = new Vector3(vertex.x, vertex.y + 6f, vertex.z);

                Debug.DrawLine(vertex, aboveVertex - new Vector3(0, .5f, 0), Color.blue, 10000000);

            }

            //debugging
            var CurrentColour = Color.black;
            var CurrentOffset = new Vector3();

            for (var i = 0; i < mesh.triangles.Length; i += 3)
            {
                var nodeOne = mesh.triangles[i];
                var nodeTwo = mesh.triangles[i + 1];
                var nodeThree = mesh.triangles[i + 2];

                #region Debugging

                //Debug.DrawLine(Nodes[nodeOne].Position + CurrentOffset, Nodes[nodeTwo].Position + CurrentOffset, CurrentColour, 10000000);
                //Debug.DrawLine(Nodes[nodeTwo].Position + CurrentOffset, Nodes[nodeThree].Position + CurrentOffset, CurrentColour, 10000000);
                //Debug.DrawLine(Nodes[nodeThree].Position + CurrentOffset, Nodes[nodeOne].Position + CurrentOffset, CurrentColour, 10000000);

                //CurrentOffset += new Vector3(0, 0f, 0);
                //CurrentColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                #endregion

                AddNodeToList(nodeOne, nodeTwo, i);
                AddNodeToList(nodeTwo, nodeOne, i);

                AddNodeToList(nodeTwo, nodeThree, i);
                AddNodeToList(nodeThree, nodeTwo, i);

                AddNodeToList(nodeThree, nodeOne, i);
                AddNodeToList(nodeOne, nodeThree, i);

                Nodes[nodeOne].Triangle = i;
                Nodes[nodeTwo].Triangle = i;
                Nodes[nodeThree].Triangle = i;


            }
        }

        static void AddNodeToList(int alpha, int beta, int triangle)
        {
            var graphEdge = new GraphEdge(alpha, beta, Vector3.Distance(Nodes[alpha].Position, Nodes[beta].Position), triangle);

            var bDoesContain = false;

            var nodeToUse = alpha;

            foreach (var edge in Nodes[alpha].AdjacencyList)
            {
                if (!(edge.To == graphEdge.To && edge.From == graphEdge.From)) continue;
                bDoesContain = true;
                break;
            }

            if (!bDoesContain)
            {
                Nodes[alpha].AdjacencyList.Add(graphEdge);
            }

        }


        //List<int> FindPathDFS(int SourcePos, int nearestToTargetNode)
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


        //        if (currentEdge.To == nearestToTargetNode)
        //        {
        //            int currentNode = nearestToTargetNode;

        //            List<int> path = new List<int> { nearestToTargetNode };

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

        //List<int> FindPathBFS(int SourcePos, int nearestToTargetNode)
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


        //        if (currentEdge.To == nearestToTargetNode)
        //        {
        //            int currentNode = nearestToTargetNode;

        //            List<int> path = new List<int> { nearestToTargetNode };

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

        //List<int> FindPathDijkstra(int SourcePos, int nearestToTargetNode)
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

        //    //bool to store whether we've found the targetLocation node yet
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


        //        if (currentEdge.To == nearestToTargetNode) bIsTargetNodeFound = true;

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
        //        int currentNode = nearestToTargetNode;

        //        List<int> path = new List<int> { nearestToTargetNode };

        //        while (currentNode != SourcePos)
        //        {
        //            currentNode = route[currentNode];
        //            path.Add(currentNode);
        //        }

        //        return path;
        //    }


        //    return null;
        //}


        //bool GetClosestNavMeshEdge(Vector3 inPosition, out int node, out NavMeshHit hit, int area = NavMesh.AllAreas)
        //{   
        //    if (NavMesh.FindClosestEdge(inPosition, out hit, NavMesh.AllAreas))
        //    {
        //        for (var i = 0; i < Nodes.Count; i++)
        //        {
        //            foreach (var t in Nodes[i].AdjacencyList)
        //            {
        //                if (hit.position != Nodes[t.From].Position) continue;
        //                node = i;
        //                Debug.DrawRay(hit.position, transform.TransformDirection(Vector3.up) * 10,Color.red);
        //                return true;
        //            }
        //        }
        //    }

        //    node = 0;
        //    return false;

        //}


        /*
         * I have at least 2 options here:
         *
         *      A: Get the 3 vertices that make a triangle around the start and end positions
         *          and add them to the adjacency list of the starting node we made. 
         *
         *      B: Get the closest node to the source position and move straight to that
         */
        static float CalculateStartEndFCost(Vector3 StartPosition, Vector3 NodePosition, Vector3 EndPosition) //TODO: I THINK THERES A PROBLEM WITH THE HEURISTIC COST OR AT LEAST WITH HOW IM DOING IT HERE PLEASE GET J TO HELP
        {
            //calculate the g cost by getting the current cost and adding it to the cost of moving to the current edgeAlpha
            var dCost = Vector3.Distance(StartPosition, NodePosition);

            //calculate the heuristic cost by getting the absolute value of a vector 
            var hCost = Vector3.Distance(NodePosition, EndPosition);

            // adding the g cost and heuristic cost to calculate the cost we'll use to compare
            var fCost = dCost + hCost;
            return fCost;
        }

        public static List<Vector3> FindPathAStar(Vector3 sourcePos, Vector3 targetPos)
        {
            //USES A HEURISTIC COST TO CHECK WHICH OF THE 3 NODES IN THE SURROUNDING TRIANGLE ARE BEST FOR TRAVELING TO THE TARGET POSITION

            //Create the priority queues note to self and anyone watching im really not liking
            //doing it this way as believe there is probably a better way of doing it for example creating
            //an operator to find the closer of two positions and run through them
            var closestStartNodeQueue = new SimplePriorityQueue<int>();
            var closestTargetNodeQueue = new SimplePriorityQueue<int>();

            for (var i = 0; i < Nodes.Count; i++)
            {
                closestStartNodeQueue.Enqueue(i, CalculateStartEndFCost(sourcePos, Nodes[i].Position, targetPos));
                closestTargetNodeQueue.Enqueue(i, CalculateStartEndFCost(targetPos, Nodes[i].Position, sourcePos));
            }
            var nodesInStartingTriangle = new SimplePriorityQueue<int>();
            var nodesInTargetTriangle = new SimplePriorityQueue<int>();

            for (var i = 0; i < 3; i++)
            {
                var closestDequeue = closestStartNodeQueue.Dequeue();
                nodesInStartingTriangle.Enqueue(closestDequeue, CalculateStartEndFCost(sourcePos, Nodes[closestDequeue].Position, targetPos));

                closestDequeue = closestTargetNodeQueue.Dequeue();
                nodesInTargetTriangle.Enqueue(closestDequeue, CalculateStartEndFCost(targetPos, Nodes[closestDequeue].Position, sourcePos));
            }

            var nearestToStartNode = nodesInStartingTriangle.First;
            var nearestToTargetNode = closestTargetNodeQueue.First;

            closestStartNodeQueue.Clear();
            closestTargetNodeQueue.Clear();


            #region Debugging

            var startEndOffset = new Vector3(0, 1, 0);

            Debug.DrawLine(sourcePos, Nodes[nearestToStartNode].Position + startEndOffset, Color.magenta, 10000000);
            Debug.DrawLine(targetPos, Nodes[nearestToTargetNode].Position + startEndOffset, Color.magenta, 10000000);

            #endregion


            //if (!GetClosestNavMeshEdge(sourcePos, out var nearestToStartNode, out var sourceMeshHit)) Debug.Log("couldn't get closest node to source pos");
            //Debug.Log($"Nearest node to {nameof(sourcePos)} is {nearestToStartNode}");

            //if (!GetClosestNavMeshEdge(targetPos, out var nearestToTargetNode, out var targetMeshHit)) Debug.Log("couldn't get closest node to targetLocation pos");
            //nearestToTargetNode = 1;
            //Debug.Log($"Nearest node to {nameof(targetPos)} is {nearestToTargetNode}");




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
            cost[nearestToStartNode] = 0.0f;


            //create array of traversed edges
            var traversedEdges = new List<GraphEdge>();

            //create minimum priority queue
            var graphPriorityQueue = new SimplePriorityQueue<GraphEdge>();

            foreach (var adjacentNode in Nodes[nearestToStartNode].AdjacencyList)
            {
                graphPriorityQueue.Enqueue(adjacentNode, adjacentNode.GetCost());
            }

            var bHasReachedTarget = false;

            //While the queue is not empty
            while (graphPriorityQueue.Count > 0)
            {
                //Keep track of the current edgeAlpha while removing it form the queue
                var currentEdge = graphPriorityQueue.Dequeue();

                //Add the current edgeAlpha to the list of edges we've already traversed
                traversedEdges.Add(currentEdge);

                //calculate the g cost by getting the current cost and adding it to the cost of moving to the current edgeAlpha
                var gCost = cost[currentEdge.From] + currentEdge.GetCost();

                //calculate the heuristic cost by getting the absolute value of a vector 
                var hCost = Vector3.Distance(Nodes[currentEdge.To].Position, Nodes[nearestToTargetNode].Position);

                // adding the g cost and heuristic cost to calculate the cost we'll use to compare
                var fCost = gCost + hCost;

                //if the cost of the current edgeAlpha is more than the cost of moving to the next edgeAlpha then set the current edges .to index to that of the current edges .from and set its cost
                if (cost[currentEdge.To] > fCost)
                {
                    route[currentEdge.To] = currentEdge.From;
                    cost[currentEdge.To] = fCost;
                }

                if (currentEdge.To == nearestToTargetNode) bHasReachedTarget = true;


                // cycle through each adjacent node of the current edgeAlpha
                foreach (var adjacentNode in Nodes[currentEdge.To].AdjacencyList.Where(adjacentNode =>
                    !CheckEdges(adjacentNode, traversedEdges) && !CheckEdges(adjacentNode, graphPriorityQueue)))
                {
                    // add the current adjacent node to the priority queue 
                    graphPriorityQueue.Enqueue(adjacentNode, currentEdge.GetCost() + adjacentNode.GetCost());
                }
            }
            // if the current edgeAlpha is the targetLocation node then calculate the best path from the targetLocation node back to the starting node
            if (bHasReachedTarget)
            {
                var currentNode = nearestToTargetNode;

                var path = new List<Vector3> { Nodes[nearestToTargetNode].Position };

                var previousNode = nearestToTargetNode;

                while (currentNode != nearestToStartNode)
                {
                    currentNode = route[currentNode];

                    path.Add(Nodes[currentNode].Position);

                    #region Debugging

                    var offset = new Vector3(0, 1, 0);

                    Debug.DrawLine(Nodes[previousNode].Position + offset, Nodes[currentNode].Position + offset, Color.magenta, 10000000);

                    previousNode = currentNode;

                    #endregion

                }

                return path;
            }
            return null;
        }

        private static bool CheckEdges(GraphEdge edgeAlpha, IEnumerable<GraphEdge> inList)
        {
            foreach (var edge in inList)
            {
                if (edge.To == edgeAlpha.To && edge.From == edgeAlpha.From)
                {
                    return true;
                }

                if (edge.From == edgeAlpha.To && edge.To == edgeAlpha.From)
                {
                    return true;
                }
            }

            return false;
        }


        public static void SimplifyMeshTopology(List<Vector3> vertices, List<int> indices, float weldThreshold)
        {
            var startTime = Time.realtimeSinceStartup;

            var startingVerts = vertices.Count;

            // Put vertex indices into buckets based on their position
            var vertexBuckets = new Dictionary<Vector3Int, List<int>>(vertices.Count);
            var shiftedIndices = new Dictionary<int, int>(indices.Count);
            var uniqueVertices = new List<Vector3>();
            var weldThresholdMultiplier = Mathf.RoundToInt(1 / weldThreshold);

            // Heuristic for skipping indices that relies on the fact that the first time a vertex index appears on the indices array, it will always be the highest-numbered
            // index up to that point (e.g. if there's a 5 in the indices array, all the indices to the left of it are in the range [0, 4])
            var minRepeatedIndex = 0;

            for (var i = 0; i < vertices.Count; ++i)
            {
                var currentVertex = Vector3Int.RoundToInt(vertices[i] * weldThresholdMultiplier);
                if (vertexBuckets.TryGetValue(currentVertex, out var indexRefs))
                {
                    indexRefs.Add(i);
                    shiftedIndices[i] = shiftedIndices[indexRefs[0]];
                    if (minRepeatedIndex == 0)
                    {
                        minRepeatedIndex = i;
                    }
                }
                else
                {
                    indexRefs = new List<int> { i };
                    vertexBuckets.Add(currentVertex, indexRefs);
                    shiftedIndices[i] = uniqueVertices.Count;
                    uniqueVertices.Add(vertices[i]);
                }
            }

            // Walk indices array and replace any repeated vertex indices with their corresponding unique one
            for (var i = 0; i < indices.Count; ++i)
            {
                var currentIndex = indices[i];
                if (currentIndex < minRepeatedIndex)
                {
                    // Can't be a repeated index, skip.
                    continue;
                }

                indices[i] = shiftedIndices[currentIndex];
            }

            vertices.Clear();
            vertices.AddRange(uniqueVertices);

            Debug.Log($"Finished simplifying mesh topology. Time: {Time.realtimeSinceStartup - startTime}. initVerts: {startingVerts}, endVerts: {vertices.Count}");
        }




    }
}

