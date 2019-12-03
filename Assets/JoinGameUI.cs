using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameUI : MonoBehaviour
{
	public GameObject ReadyText;
	public GameObject NotReadyText;

	public Light ReadyLight;

	public void Start()
	{
		NotReady();
	}

	public void Ready()
    {
		ReadyText.SetActive(true);
		NotReadyText.SetActive(false);
		ReadyLight.enabled = true;
	}

	public void NotReady()
    {
        ReadyText.SetActive(false);
		NotReadyText.SetActive(true);
		ReadyLight.enabled = false;
    }
}
