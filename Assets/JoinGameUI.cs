using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameUI : MonoBehaviour
{
	public GameObject ReadyText;
	public GameObject NotReadyText;

	public void Start()
	{
		NotReady();
	}

	public void Ready()
    {
		ReadyText.SetActive(true);
		NotReadyText.SetActive(false);
	}

	public void NotReady()
    {
        ReadyText.SetActive(false);
		NotReadyText.SetActive(true);
    }
}
