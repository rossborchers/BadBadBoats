using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not the transformation returned from hit happens over center of this game object. All triggers should be equally spaced from this point
public class LevelBounds : MonoBehaviour
{
    public Vector2 RectSize;

    private static LevelBounds _instance;

    public static bool Hit(ref Vector3 position)
    {
        if(_instance == null)
        {
            //yolo
            _instance = FindObjectOfType<LevelBounds>();
        }

        Vector3 localPos = position - _instance.transform.position;

        bool hit = false;
        if (localPos.x > _instance.RectSize.x)
        {
            hit = true;
            localPos = new Vector3(localPos.x * -1, localPos.y, localPos.z);
        }
        else if (localPos.x < -_instance.RectSize.x)
        {
            hit = true;
            localPos = new Vector3(localPos.x * -1, localPos.y, localPos.z);
        }
        else if(localPos.z > _instance.RectSize.y)
        {
            hit = true;
            localPos = new Vector3(localPos.x, localPos.y, localPos.z * -1);
        }
        else if(localPos.z < -_instance.RectSize.y)
        {
            hit = true;
            localPos = new Vector3(localPos.x , localPos.y, localPos.z * -1);
        }

        position = _instance.transform.position + localPos;
        return hit;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + new Vector3(RectSize.x, 0, RectSize.y), transform.position + new Vector3(-RectSize.x, 0, RectSize.y));
        Gizmos.DrawLine(transform.position + new Vector3(RectSize.x, 0, -RectSize.y), transform.position + new Vector3(-RectSize.x, 0, -RectSize.y));

        Gizmos.DrawLine(transform.position + new Vector3(RectSize.x, 0, RectSize.y), transform.position + new Vector3(RectSize.x, 0, -RectSize.y));
        Gizmos.DrawLine(transform.position + new Vector3(-RectSize.x, 0, RectSize.y), transform.position + new Vector3(-RectSize.x, 0, -RectSize.y));
        Gizmos.color = Color.white;
    }
}
