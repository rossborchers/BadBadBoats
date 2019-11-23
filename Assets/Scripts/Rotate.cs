using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        transform.Rotate(Vector3.forward, Speed * Time.deltaTime);
    }
}
