using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBoat : MonoBehaviour
{
	public GameObject[] Boats;

	public float WaitTime = 1f;
	public float DestroyTime = 2f;

	public Material[] MainTextures;
	public Material[] SideTextures;

	public int PlayerId = 0;


	IEnumerator Start()
    {
		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers)
		{
			Material[] mats = new Material[renderer.materials.Length];

			mats[0] = MainTextures[PlayerId];

			if(mats.Length > 1)
			{
				mats[1] = SideTextures[PlayerId];
			}

			renderer.materials = mats;
		}

		while (WaitTime > 0)
		{
			WaitTime -= Time.deltaTime;
			yield return null;
		}
    }

	private void Update()
	{
		if(WaitTime > 0)
		{
			return;
		}

		foreach (GameObject boat in Boats)
		{
			boat.transform.localScale = new Vector3(boat.transform.localScale.x + (0 - boat.transform.localScale.x) * Time.deltaTime, boat.transform.localScale.y + (0 - boat.transform.localScale.y) * Time.deltaTime, boat.transform.localScale.z + (0 - boat.transform.localScale.z) * Time.deltaTime);
		}

		DestroyTime -= Time.deltaTime;

		if(DestroyTime <= 0)
		{
			Destroy(gameObject);
		}

	}
}
