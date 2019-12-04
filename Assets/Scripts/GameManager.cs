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

	public Animator GrabTheLootAnimator;

    private int _pointRespawnIndex;

    public Player[] Players;
	private Player activeMonster = null;

    private int _respawnIndex;
    public Transform[] Respawns;

    public Transform[] PointRespawns;

    public float StartSpeed = 10;
    public float SpeedIncrease = 50f;
    public float SpeedCap = 500;

    private float Speed = 0f;

	public int ScoreToWin = 4;
	public Score Score;

	public float PointSpawnDelay = 5;
	private float LastSpawnTime = 0;

	public AudioSource boatBreakSound;

	public UIController UIController;

	HashSet<int> ReadyPlayers = new HashSet<int>();

	public GameObject LevelAssetRoot;


	public float TimeoutTime;
	private float _currentTimeoutTime;
	private bool restarting;


	public float TimeToReset;
	private float _resetTime;

	public enum GameState
	{
		Attractor,
		Game,
		Winner
	}

	private GameState _state;
	public GameState State
	{
		get
		{ return _state; }
		set
		{
			if(value == GameState.Game)
			{
				OnGameStart();
			}

			_state = value;
		}
	}

    private void Awake()
    {
        Instance = this;
		State = GameState.Attractor;

		foreach (Player player in Players)
		{
			player.gameObject.SetActive(false);
		}

		LevelAssetRoot.SetActive(false);
		Score.SetScore(ScoreToWin);
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
		boatBreakSound.Play();

		Score.SetScore(Score.CurrentScore-1);
		if(Score.CurrentScore > 0)
		{
			Score.Show(false);
		}
	}

    IEnumerator Restart(bool showWin = true)
    {
		if (!restarting)
		{
			restarting = true;
			if(showWin)
			{
				UIController.Won();
				yield return new WaitForSeconds(6);

			}

			Scene scene = SceneManager.GetActiveScene();
			restarting = false;
			SceneManager.LoadScene(scene.name);
		}
		
    }

	 public void OnGameStart()
	 {
		LevelAssetRoot.SetActive(true);

		foreach (Player player in Players)
		{
			player.gameObject.SetActive(true);
		}

		if (Players.Length < 1)
		{
			Debug.LogError("No Players!");
		}

		Speed = StartSpeed;

		SpawnNewPoint();


		GrabTheLootAnimator.SetTrigger("On");
	}

    public void Update()
    {
		if(State == GameState.Attractor)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].CheckPlayerInput())
				{
					if(ReadyPlayers.Contains(i))
					{
						ReadyPlayers.Remove(i);
						UIController.PlayerNotReady(i);
					}
					else
					{
						ReadyPlayers.Add(i);
						UIController.PlayerReady(i);
					}
				}
			}

			if (ReadyPlayers.Count >= Players.Length)
			{
				State = GameState.Game;
				UIController.AllPlayersReady();
			}
		}

		if(State == GameState.Attractor)
		{
			return;
		}

		if (!Input.anyKey)
		{
			_currentTimeoutTime+=Time.deltaTime;
		}
		if(_currentTimeoutTime > TimeoutTime)
		{
			StartCoroutine(Restart(false));
		}

		int down = 0;
		foreach(Player p in Players)
		{
			if(Input.GetKey(p.KeyCode))
			{
				down++;
			}
		}

		if(down == Players.Length)
		{
			_resetTime += Time.deltaTime;
			if(_resetTime > TimeToReset)
			{
				StartCoroutine(Restart(false));
			}
		}
		else
		{
			_resetTime = 0;
		}

		foreach (Player player in Players)
        {
            player.Control.Speed = Speed;
        }

        if(Score.CurrentScore == 0)
        {
            StartCoroutine(Restart());
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
		if(activeMonster != newPlayer)
		{
			Score.SetScore(ScoreToWin);
			Score.Show(true);
		}

		activeMonster = newPlayer;

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
