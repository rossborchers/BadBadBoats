﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateControl))]
public class Player : MonoBehaviour
{
	public List<TrailRenderer> TrailRenderers;

	public int playerID;

    public Material[] boatMainMats;
    public Material[] boatBitsMats;

    public Renderer[] mainRenderers;
    public Renderer[] bitsRenderers;

    public GameObject monster;

	public GameObject[] oars;

	private RotateControl _control;

	public BoatTrail BoatTrailParticle;

	public float BoatTrailParticleSpawnInterval = 0.25f;

	private float _lastBoatTrailSpawn;

	public bool isMonster;

	public float PlayerTrailBoost;
	public float PlayerMonsterBoost;

	public BrokenBoat BrokenBoatPrefab;

	private Player GhostTop;
	private Player GhostBottom;
	private Player GhostLeft;
	private Player GhostRight;

	private Player GhostTopLeft;
	private Player GhostTopRight;

	private Player GhostBottomLeft;
	private Player GhostBottomRight;

	private List<Player> Ghosts = new List<Player>();


	private bool Ghost
	{
		get;
		set;
	}

    public RotateControl Control
    {
        get
        {
            if(_control == null)
            {
                _control = GetComponent<RotateControl>();
            }
            return _control;
        }
    }

    public KeyCode KeyCode;

    private Point _point;

    public Color defaultColor;

    private void Awake()
    {
        monster.SetActive(false);
        SetMaterials(boatMainMats[playerID], boatBitsMats[playerID]);	
    }

	private void Start()
	{
		if(!Ghost)
		{
			BuildGhosts();
		}
		else
		{
			foreach(TrailRenderer trail in TrailRenderers)
			{
				trail.enabled = false;
			}
		}
	}

	private void BuildGhosts()
	{
		GhostTop = Instantiate(gameObject).GetComponent<Player>();
		GhostBottom = Instantiate(gameObject).GetComponent<Player>();
		GhostLeft = Instantiate(gameObject).GetComponent<Player>();
		GhostRight = Instantiate(gameObject).GetComponent<Player>();

		GhostTopLeft = Instantiate(gameObject).GetComponent<Player>();
		GhostTopRight = Instantiate(gameObject).GetComponent<Player>();

		GhostBottomLeft = Instantiate(gameObject).GetComponent<Player>();
		GhostBottomRight = Instantiate(gameObject).GetComponent<Player>();

		GhostTop.Ghost = true; 
		GhostBottom.Ghost = true;
		GhostLeft.Ghost = true;
		GhostRight.Ghost = true;

		GhostTopLeft.Ghost = true;
		GhostTopRight.Ghost = true;

		GhostBottomLeft.Ghost = true;
		GhostBottomRight.Ghost = true;

		Ghosts = new List<Player>() { GhostTop, GhostBottom, GhostLeft, GhostRight, GhostTopLeft, GhostTopRight, GhostBottomLeft, GhostBottomRight };
	}

	private void CopyGhosts()
	{
		float x = LevelBounds.Instance.RectSize.x * 2;
		float y = LevelBounds.Instance.RectSize.y * 2;

		GhostTop.transform.position = transform.position + new Vector3(0, 0, y);
		GhostBottom.transform.position = transform.position + new Vector3(0, 0, -y);
		GhostLeft.transform.position = transform.position + new Vector3(x, 0, 0);
		GhostRight.transform.position = transform.position + new Vector3(-x, 0, 0);

		GhostTopLeft.transform.position = transform.position + new Vector3(-x, 0, y);
		GhostTopRight.transform.position = transform.position + new Vector3(x, 0, y);

		GhostBottomLeft.transform.position = transform.position + new Vector3(-x, 0, -y);
		GhostBottomRight.transform.position = transform.position + new Vector3(x, 0, -y);


		GhostTop.transform.rotation = transform.rotation;
		GhostBottom.transform.rotation = transform.rotation;
		GhostLeft.transform.rotation = transform.rotation;
		GhostRight.transform.rotation = transform.rotation;

		GhostTopLeft.transform.rotation = transform.rotation;
		GhostTopRight.transform.rotation = transform.rotation;

		GhostBottomLeft.transform.rotation = transform.rotation;
		GhostBottomRight.transform.rotation = transform.rotation;
	}

    private void BecomeMonster()
    {
		foreach (Player ghost in Ghosts)
		{
			ghost.BecomeMonster();
		}

		isMonster = true;
		foreach(GameObject oar in oars)
		{
			oar.SetActive(false);
		}
        monster.SetActive(true);
        if (_control != null) _control.Boost = 100;
    }

    private void BecomeBoat()
    {
		foreach(Player ghost in Ghosts)
		{
			ghost.BecomeBoat();
		}

		isMonster = false;
		foreach (GameObject oar in oars)
		{
			oar.SetActive(true);
		}
		monster.SetActive(false);
        SetMaterials(boatMainMats[playerID], boatBitsMats[playerID]);
        if (_control != null) _control.Boost = 0;
    }

    private void SetMaterials(Material mainMaterial, Material bitsMaterial)
    {
        //temp hack
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in mainRenderers)
        {
            renderer.sharedMaterial = mainMaterial;
        }

        foreach (Renderer renderer in bitsRenderers)
        {           
            renderer.sharedMaterial = bitsMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Point point = collision.gameObject.GetComponent<Point>();     
        if (point != null)
        {
            _point = point;

			Lightning.Instance.Strike();

			if(isMonster)
			{
				Control.GotPointBoost = 200;
			}
			_point.Hide();
            GameManager.Instance.PlayerGotPoint(this);
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player._point != null)
                {
                    Respawn();
                }

                if (_point == null)
                {
                    //Control.ChangeDirection();
                }
                else
                {
                    GameManager.Instance.PlayerKilledOther(this);
                }
            }
            else
            {
               // Control.ChangeDirection();
            }    
        }
    }
	private void OnTriggerEnter(Collider other)
	{
		WhirlPool pool = other.GetComponent<WhirlPool>();
		if (pool != null)
		{
			transform.position = pool.GetLinkPosition();
			ResetTrail();
		}

		BoatTrail boatTrail = other.GetComponent<BoatTrail>();
		if(boatTrail != null && boatTrail.PlayerId != playerID)
		{
			if(isMonster)
			{
				_control.TrailBoost = PlayerMonsterBoost;
			}
			else
			{
				_control.TrailBoost = PlayerTrailBoost;
			}
			
		}
	}

	public void ClearPoint()
    {
        if(_point != null)
        {
            _point.Destroy();
            _point = null;
        }
    }

    private void OnDestroy()
    {
		if (_point != null)
        {
            _point.Destroy();
        }   
    }

    void Update()
    {
		if(Ghost)
		{
			return;
		}

        if (Input.GetKeyDown(KeyCode))
        {
            Control.Rotate = !Control.Rotate;
        }

        if(_point == null)
        {
            BecomeBoat();  
			
        }
        else
        {
            BecomeMonster();
        }

		//Try wrap
		Vector3 wrappedPos = transform.position;
		if (LevelBounds.Hit(ref wrappedPos))
		{
			Lightning.Instance.Strike();
			transform.position = wrappedPos;
			ResetTrail();
		}

		if(Time.time - _lastBoatTrailSpawn > BoatTrailParticleSpawnInterval)
		{
			GameObject instance = Instantiate(BoatTrailParticle.gameObject);
			instance.transform.position = transform.position;
			instance.GetComponent<BoatTrail>().PlayerId = playerID;
			_lastBoatTrailSpawn = Time.time;
		}

		if (isMonster)
		{
			Control.TrailBoost = Mathf.Min(0, Control.TrailBoost + (Time.deltaTime * Control.TrailBoostDecrese));
		}
		else
		{
			Control.TrailBoost = Mathf.Max(0, Control.TrailBoost - Time.deltaTime * Control.TrailBoostDecrese);
		}
	}

	private void LateUpdate()
	{
		if(!Ghost)CopyGhosts();
	}

	public void Respawn()
    {
		GameObject instnace = Instantiate(BrokenBoatPrefab.gameObject);
		instnace.GetComponent<BrokenBoat>().PlayerId = playerID;
		instnace.transform.position = transform.position;

		Lightning.Instance.Strike();
		transform.position = GameManager.Instance.GetRespawnPoint();
		ResetTrail();
		
		GameManager.Instance.IncreaseSpeed();
    }

	private void ResetTrail()
	{
		foreach (TrailRenderer renderer in TrailRenderers)
		{
			renderer.Clear();
		}
	}
}
