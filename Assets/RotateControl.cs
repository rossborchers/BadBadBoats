using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour
{
    public Transform rotate1;
    public Transform rotate2;

    public Rigidbody2D rb;

    public float Speed
    {
        get;
        set;
    }

    public bool Rotate
    {
        get;
        set;
    }

    public float Boost
    {
        get;
        set;
    }


    private float direction = 1;

    public void ChangeDirection()
    {
        direction *= -1;
    }

    void Update()
    {
        if(Rotate)
        {
            transform.RotateAround(rotate1.position, Vector3.forward, direction * (Speed + Boost) * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(rotate2.position, Vector3.forward, direction  * -(Speed + Boost) * Time.deltaTime);
        }
    }
}
