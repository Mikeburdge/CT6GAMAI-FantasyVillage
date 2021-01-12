using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HouseScript : MonoBehaviour
{
    public GameObject door;

    public float MaxHouseHealth { get; } = 1000;
    public float HouseHealth { get; private set; } = 1000;

    public bool needsRepairing;

    private float DeteriorationRate;

    private void Start()
    {
        CalculateDeteriorationRate();
    }

    private void Update()
    {
        if (HouseHealth >= MaxHouseHealth)
        {
            HouseHealth = MaxHouseHealth - 1;
            CalculateDeteriorationRate();
        }
        else if (HouseHealth < MaxHouseHealth && !needsRepairing)
        {
            needsRepairing = true;
        }

        HouseHealth -= Time.deltaTime * DeteriorationRate;
    }

    private void CalculateDeteriorationRate()
    {
        DeteriorationRate = Random.Range(1, 2);
    }

}
