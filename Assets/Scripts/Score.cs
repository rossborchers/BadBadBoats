using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	public Animator ScoreAnimator;

    public TMPro.TextMeshProUGUI playerText;
    public TMPro.TextMeshProUGUI scoreText;

	public CanvasGroup TablesTurnedCanvasGroup;

    private int _score = 0;

    public void SetScore(int score)
    {
        _score = score;
        scoreText.text = "" + score;
    }

	public void Show(bool tablesTurned)
	{
		if(tablesTurned)
		{
			TablesTurnedCanvasGroup.alpha = 1;
		}
		else
		{
			TablesTurnedCanvasGroup.alpha = 0;
		}

		ScoreAnimator.SetTrigger("Show");
	}

    public int CurrentScore
    {
        get
        {
            return _score;
        }
    }

    public void IncrementScore()
    {
        _score++;
        scoreText.text = "" + _score;
    }
}
