﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HouseScript : MonoBehaviour
{
    public GameObject door;

    public float MaxHouseHealth { get; } = 1000;
    public float HouseHealth { get; private set; } = 800;

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
        DeteriorationRate = Random.Range(2f, 4f);
    }

    public void RepairHouseBy(float inNum)
    {
        HouseHealth += inNum;
    }

}
