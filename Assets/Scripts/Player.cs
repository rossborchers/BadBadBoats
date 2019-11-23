using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateControl))]
public class Player : MonoBehaviour
{

    public int playerID;

    public Material[] boatMainMats;
    public Material[] boatBitsMats;

    public Renderer[] mainRenderers;
    public Renderer[] bitsRenderers;

    public GameObject monster;
   

    private RotateControl _control;

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
        monster.SetActive(true);
        if (_control != null) _control.Boost = 100;
    }

    private void BecomeBoat()
    {
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
        if(_point != null)
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
    }

    public void Respawn()
    {
        transform.position = GameManager.Instance.GetRespawnPoint();
        GameManager.Instance.IncreaseSpeed();
    }
}
