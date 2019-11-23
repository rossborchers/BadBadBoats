using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watermovement : MonoBehaviour
{
	Renderer renderer;

    public float speed;
	private float offset;

    // Start is called before the first frame update
    void Start()
    {
		renderer = GetComponentInChildren<Renderer>();
	}

    // Update is called once per frame
    void Update()
    {
		offset += speed * Time.deltaTime;
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(Mathf.Sin(offset) * 0.1f, 0));
	}
}
