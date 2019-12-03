using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public static Lightning Instance;

	public float Intensity;

	public Light light;

	public float DecreaseSpeed = 50;

	public AudioSource[] Thunders;

	// Start is called before the first frame update
	void Start()
	{
		light = GetComponent<Light>();
		Instance = this;
	}

	public void Strike()
	{
		light.intensity = Intensity;
		StartCoroutine(ThunderStrike());
	}

	IEnumerator ThunderStrike()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0, 1f));

		bool thunderPlaying = false;
		foreach(AudioSource s in Thunders)
		{
			if(s.isPlaying)
			{
				thunderPlaying = true;
			}
		}

		if(!thunderPlaying)
		{
			Thunders[UnityEngine.Random.Range(0, Thunders.Length)].Play();
		}
	}

    void Update()
    {
		light.intensity = Mathf.Max(0, light.intensity - Time.deltaTime * DecreaseSpeed);
	}
}
