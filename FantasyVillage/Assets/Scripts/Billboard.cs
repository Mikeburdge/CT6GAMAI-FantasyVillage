using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform cam;

	private void Start()
	{
		if (!(Camera.main is null)) cam = Camera.main.transform;
	}

	void LateUpdate()
    {
		transform.LookAt(transform.position + cam.forward);
    }
}
