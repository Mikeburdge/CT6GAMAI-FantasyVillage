using Assets.Scripts.Villagers;
using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{


    public SimplePriorityQueue<TreeScript> TreesPriorityQueue;


    // Start is called before the first frame update
    void Start()
    {
        TreesPriorityQueue = new SimplePriorityQueue<TreeScript>();

        if (!AreAvailableTreesInSanctuary())
        {
            return;
        }

        TreeScript[] TreeChildren = GetComponentsInChildren<TreeScript>().ToArray();

        TreeScript[] PriorityQueueArray = TreesPriorityQueue.ToArray();
        


        for (int i = 0; i < TreeChildren.Length; i++)
        {
            TreesPriorityQueue.Enqueue(TreeChildren[i], i);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

   public bool AreAvailableTreesInSanctuary()
    {
        TreeScript[] TreeChildren = GetComponentsInChildren<TreeScript>().ToArray();

        if (!TreeChildren[0])
        {
            Debug.Log("There are no trees in the sanctuary");
            return false;
        }

        bool rv = false;

        foreach (TreeScript tree in TreeChildren)
        {
            if (!tree.IsOccupied)
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

        float distance;

        foreach (TreeScript tree in TreesPriorityQueue)
        {
            distance = (inVillager.transform.position - tree.transform.position).magnitude;
            TreesPriorityQueue.UpdatePriority(tree, distance);
        }

        TreeScript[] treesArray = TreesPriorityQueue.ToArray();

        for (int i = 0; i < treesArray.Length; i++)
        {
            if (!treesArray[i].IsOccupied)
            {
                nearestTree = treesArray[i];
                return true;
            }
        }

        Debug.Log("No Available Trees", inVillager);

        nearestTree = null;
        return false;
    }

    public bool RemoveTargetTree(TreeScript tree)
    {
        if (!AreAvailableTreesInSanctuary()) { return false; }
        
        if (!TreesPriorityQueue.Contains(tree))
        {
            Debug.Log("Tree is not in the list of trees");
            return false;
        }

        TreesPriorityQueue.Remove(tree);
        return true;
    }

    public void DestroyTree(TreeScript tree)
    {
        RemoveTargetTree(tree);

        Destroy(tree.gameObject);

    }

}
