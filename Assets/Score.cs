using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerText;
    public TMPro.TextMeshProUGUI scoreText;

    private int _score = 0;

    public void SetPlayer(int player)
    {
        playerText.text = "Player " + player;
    }

    public void SetScore(int score)
    {
        _score = score;
        scoreText.text = "" + score;
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
