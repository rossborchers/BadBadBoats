using System.Collections;
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

    private void BecomeMonster()
    {
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
                    Control.ChangeDirection();
                }
                else
                {
                    GameManager.Instance.PlayerKilledOther(this);
                }
            }
            else
            {
                Control.ChangeDirection();
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

    public void Respawn()
    {
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
