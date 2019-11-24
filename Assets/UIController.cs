using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public Animator UIControllerAnimator;

	public Animator VictoryAnimator;


	public JoinGameUI[] JoinGameUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public void LightningStrike()
	{
		if(Lightning.Instance != null)
		{
			Lightning.Instance.Strike();
		}
	}

	public void PlayerReady(int playerIndex)
	{
		JoinGameUI[playerIndex].Ready();
	}

	public void PlayerNotReady(int playerIndex)
	{
		JoinGameUI[playerIndex].NotReady();
	}

	public void AllPlayersReady()
	{
		UIControllerAnimator.SetTrigger("PlayersReady");
	}

	public void Won()
	{
		VictoryAnimator.SetTrigger("Win");
	}
}
