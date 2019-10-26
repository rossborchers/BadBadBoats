using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour
{
    public Transform rotate1;
    public Transform rotate2;

    public KeyCode KeyCode;

    public float speed= 10;

    private bool rotate;

    float direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate)
        {
            transform.RotateAround(rotate1.position, Vector3.forward, direction * speed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(rotate2.position, Vector3.forward, direction  * - speed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode))
        {
            rotate = !rotate;
        }
    }
}
