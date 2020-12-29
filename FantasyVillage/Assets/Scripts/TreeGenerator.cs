using System;
using System.Linq;
using Priority_Queue;
using UnityEngine;
using Villagers;
using Random = UnityEngine.Random;

public class TreeGenerator : MonoBehaviour
{
    public int MaxTrees;
    public GameObject[] TreePrefabs;

    public SimplePriorityQueue<TreeScript> TreesPriorityQueue;

    private bool bShouldSpawnTrees = true;
    private Bounds colliderBounds;

    // Start is called before the first frame update
    private void Start()
    {
        colliderBounds = GetComponent<BoxCollider>().bounds;
        TreesPriorityQueue = new SimplePriorityQueue<TreeScript>();
    }

    private void Update()
    {
        if (TreesPriorityQueue.Count < MaxTrees && bShouldSpawnTrees)
        {
            SpawnTree();
        }
    }

    public void SpawnTree()
    {
        var randTreeNum = Random.Range(0, TreePrefabs.Length);


        var tree = Instantiate(TreePrefabs[randTreeNum], GetPosInTreeSanctuary(), Quaternion.identity);

        var treeScript = tree.AddComponent<TreeScript>();
        var treeCollider = tree.AddComponent<BoxCollider>();

        treeCollider.isTrigger = true;

        tree.transform.SetParent(transform, true);

        TreesPriorityQueue.Enqueue(treeScript, float.MaxValue);
        var bIsColliding = false;

        var colliders = Physics.OverlapBox(tree.transform.position, treeCollider.size / 2);

        foreach (var other in colliders)
        {
            if (other.gameObject == tree) continue;

            if (!other.gameObject.CompareTag("Tree")) continue;

            bIsColliding = true;
        }

        var ticker = 0;

        while (bIsColliding)
        {
            bIsColliding = false;

            tree.transform.position = GetPosInTreeSanctuary();

            colliders = Physics.OverlapBox(tree.transform.position, treeCollider.size / 2);

            foreach (var other in colliders)
            {
                if (other.gameObject == tree) continue;

                if (!other.gameObject.CompareTag("Tree")) continue;

                bIsColliding = true;
            }

            //stop the infinite loop
            if (ticker >= 5)
            {
                RemoveTargetTree(treeScript);
                bShouldSpawnTrees = false;
                return;
            }

            ticker++;
        }

    }

    private Vector3 GetPosInTreeSanctuary()
    {
        var randTreeX = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
        var randTreeZ = Random.Range(colliderBounds.min.z, colliderBounds.max.z);

        return new Vector3(randTreeX, 0, randTreeZ);
    }


    public bool AreAvailableTreesInSanctuary()
    {
        var treeChildren = GetComponentsInChildren<TreeScript>().ToArray();

        if (treeChildren.Length <= 0)
        {
            Debug.Log("There are no trees in the sanctuary");
            return false;
        }

        var rv = false;

        foreach (var tree in treeChildren)
        {
            if (!tree.isOccupied)
            {
                rv = true;
            }
        }

        return rv;

    }

    public bool CalculateAvailableNearestTree(Villager inVillager, out TreeScript nearestTree)
    {
        if (!AreAvailableTreesInSanctuary())
        {
            nearestTree = null;
            return false;
        }

        foreach (var tree in TreesPriorityQueue)
        {
            var distance = (inVillager.transform.position - tree.transform.position).magnitude;
            TreesPriorityQueue.UpdatePriority(tree, distance);
        }

        var treesArray = TreesPriorityQueue.ToArray();

        for (var i = 0; i < treesArray.Length; i++)
        {
            if (!treesArray[i].isOccupied)
            {
                nearestTree = treesArray[i];
                return true;
            }
        }
        inVillager.UpdateAIText("No Available Trees");

        nearestTree = null;
        return false;
    }

    public bool RemoveTargetTree(TreeScript tree)
    {
        if (!AreAvailableTreesInSanctuary()) { return false; }

        if (!TreesPriorityQueue.Contains(tree))
        {
            Debug.Log($"{tree.gameObject}Tree is not in the list of trees");
            return false;
        }

        TreesPriorityQueue.Remove(tree);
        return true;
    }

    public void DestroyTree(TreeScript tree)
    {
        RemoveTargetTree(tree);

        Destroy(tree.gameObject);

        if (TreesPriorityQueue.Count <= 1)
        {
            bShouldSpawnTrees = true;
        }
    }

}
