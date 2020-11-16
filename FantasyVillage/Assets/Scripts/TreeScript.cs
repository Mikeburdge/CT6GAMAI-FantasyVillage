using Assets.Scripts.Villagers;
using UnityEngine;

public class TreeScript : MonoBehaviour
{

    public float Health = 100;

    public int WoodPerChop = 10;

    public bool IsOccupied;

    [SerializeField]
    private float damageToTree = 20;

    public void Chop(Villager villager)
    {
        Health -= damageToTree;

        if (Health <= 0)
        {
            TreeGenerator treeSanctuary = GameObject.Find("Tree Sanctuary").GetComponent<TreeGenerator>();

            treeSanctuary.DestroyTree(this);
        }
    }

}
    