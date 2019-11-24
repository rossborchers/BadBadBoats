using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public static Lightning Instance;

	public float Intensity;

	public Light light;

	public float DecreaseSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
		light = GetComponent<Light>();
		Instance = this;
	}

	public void Strike()
	{
		light.intensity = Intensity;
	}

    void Update()
    {
		light.intensity = Mathf.Max(0, light.intensity - Time.deltaTime * DecreaseSpeed);
	}
}
