using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject PointPrefab;

    public Transform WinMessage;

    public GameObject PlayerScorePrefab;
    public Transform PlayerScoreRoot;

    private int _pointRespawnIndex;

    public Player[] Players;
    private List<Score> ScoreInstances;

    private int _respawnIndex;
    public Transform[] Respawns;

    public Transform[] PointRespawns;

    public float StartSpeed = 10;
    public float SpeedIncrease = 50f;
    public float SpeedCap = 500;

    private float Speed = 0f;

    public int MaxScore = 10;

	public float PointSpawnDelay = 5;
	private float LastSpawnTime = 0;
    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRespawnPoint()
    {
        if (_respawnIndex >= Respawns.Length)
        {
            _respawnIndex = 0;
        }
        Vector3 point = Respawns[_respawnIndex].position;
        _respawnIndex++;
        return point;
    }

    public void IncreaseSpeed()
    {
        Speed = Mathf.Min(Speed + SpeedIncrease, SpeedCap);
    }

    public void PlayerKilledOther(Player killer)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i] == killer)
            {
                ScoreInstances[i].IncrementScore();
            }
        }
    }

    IEnumerator Restart()
    {
        WinMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Start()
    {
        if(Players.Length < 1)
        {
            Debug.LogError("No Players!");
        }

        Speed = StartSpeed;

        SpawnNewPoint();

        ScoreInstances = new List<Score>();

        for(int i = 0; i < Players.Length; i++)
        {
            GameObject instance = Instantiate(PlayerScorePrefab);
            instance.transform.SetParent(PlayerScoreRoot);
            Score score = instance.GetComponent<Score>();
            ScoreInstances.Add(score);

            score.SetPlayer(i+1);
            score.SetScore(0);
        }
    }

    public void Update()
    {
        foreach(Player player in Players)
        {
            player.Control.Speed = Speed;
        }

        for(int i = 0; i < ScoreInstances.Count; i++)
        {
            if( ScoreInstances[i].CurrentScore >= MaxScore)
            {
                StartCoroutine(Restart());
            }
        }

        int points = 0;
        foreach(Point p in FindObjectsOfType<Point>())
        {
            if(p.gameObject.activeSelf)
            {
                points++;
            }
        }

        if(points < 1 && Time.time - LastSpawnTime > PointSpawnDelay)
        {
            SpawnNewPoint();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayerGotPoint(Player newPlayer)
    {
        for ( int i = 0; i < Players.Length; i++)
        {
            if (Players[i] != newPlayer)
            {
                Players[i].ClearPoint();
            }
            else
            {
                //ScoreInstances[i].IncrementScore();
            }
        }
    }

    public void SpawnNewPoint()
    {
        //Create new point
        GameObject point = Instantiate(PointPrefab);

        if (_pointRespawnIndex >= PointRespawns.Length)
        {
            _pointRespawnIndex = 0;
        }

        point.transform.position = PointRespawns[_pointRespawnIndex].position;
        _pointRespawnIndex++;

		LastSpawnTime = Time.time;
	}
}
