using UnityEngine;
using UnityEngine.Serialization;
using Villagers;

public class TreeScript : MonoBehaviour
{

    [FormerlySerializedAs("Health")] public float health = 100;

    [FormerlySerializedAs("WoodPerChop")] public int woodPerChop = 10;

    [FormerlySerializedAs("IsOccupied")] public bool isOccupied;

    [SerializeField]
    private float damageToTree = 20;

    public void Chop(Villager villager)
    {
        health -= damageToTree;

        if (health <= 0)
        {
            TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

            treeSanctuary.DestroyTree(this);
        }
    }

}
    