using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Target;

    public Vector3 offset;
    public bool shouldFollow;

    // Update is called once per frame
    void Update()
    {
        if (shouldFollow)
        {
            transform.position = Target.transform.position + offset;
        }
    }
}
