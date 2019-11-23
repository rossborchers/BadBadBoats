using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTrail : MonoBehaviour
{
	public int PlayerId;

	public float LifeTime;

    void Start()
    {
		
	}

    void Update()
    {
		LifeTime -= Time.deltaTime;

		if(LifeTime < 0)
		{
			Destroy(gameObject);
		}
	}
}
