using UnityEngine;
using UnityEngine.Serialization;
using Villagers;

public class TreeScript : MonoBehaviour
{

    [FormerlySerializedAs("Health")] public float health = 100;

    [FormerlySerializedAs("WoodPerChop")] public int woodPerChop = 10;

    [FormerlySerializedAs("IsOccupied")] public bool isOccupied;

    private TreeGenerator treeSanctuary;

    [SerializeField]
    private float damageToTree = 20;

    private void Start()
    {
        treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();
    }

    public void Chop(Villager villager)
    {
        health -= damageToTree;

        if (health <= 0)
        {
            treeSanctuary.DestroyTree(this);
        }
    }
}
    