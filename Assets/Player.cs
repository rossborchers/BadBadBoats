using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateControl))]
public class Player : MonoBehaviour
{

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

    public SpriteRenderer spriteRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
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
            spriteRenderer.color = defaultColor;
            if (_control != null)  _control.Boost = 0;
        }
        else
        {
            spriteRenderer.color = _point.Color;
            if (_control != null) _control.Boost = 100;
        }
    }

    public void Respawn()
    {
        transform.position = GameManager.Instance.GetRespawnPoint();
        GameManager.Instance.IncreaseSpeed();
    }
}
