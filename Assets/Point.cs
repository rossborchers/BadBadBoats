using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Color Color
    {
        get
        {
            return GetComponent<SpriteRenderer>().color;
        }
    }

    public Type pointType;

    public enum Type
    {
        Villan,
        Goodie
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
