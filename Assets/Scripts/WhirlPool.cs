using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    public Spout Link;

    public Vector3 GetLinkPosition()
    {
        return Link.transform.position;
    }
}
