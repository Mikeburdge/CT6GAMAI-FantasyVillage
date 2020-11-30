using System.Linq;
using Priority_Queue;
using UnityEngine;
using Villagers;

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

        TreeScript[] treeChildren = GetComponentsInChildren<TreeScript>().ToArray();

        TreeScript[] priorityQueueArray = TreesPriorityQueue.ToArray();
        


        for (int i = 0; i < treeChildren.Length; i++)
        {
            TreesPriorityQueue.Enqueue(treeChildren[i], i);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

   public bool AreAvailableTreesInSanctuary()
    {
        TreeScript[] treeChildren = GetComponentsInChildren<TreeScript>().ToArray();

        if (!treeChildren[0])
        {
            Debug.Log("There are no trees in the sanctuary");
            return false;
        }

        bool rv = false;

        foreach (TreeScript tree in treeChildren)
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

        float distance;

        foreach (TreeScript tree in TreesPriorityQueue)
        {
            distance = (inVillager.transform.position - tree.transform.position).magnitude;
            TreesPriorityQueue.UpdatePriority(tree, distance);
        }

        TreeScript[] treesArray = TreesPriorityQueue.ToArray();

        for (int i = 0; i < treesArray.Length; i++)
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
