using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not the transformation returned from hit happens over center of this game object. All triggers should be equally spaced from this point
public class LevelBounds : MonoBehaviour
{
    public Vector2 RectSize;

    public static LevelBounds Instance;

	public float Offset = 0.01f;

    public static bool Hit(ref Vector3 position)
    {
        if(Instance == null)
        {
			//yolo
			Instance = FindObjectOfType<LevelBounds>();
        }

        Vector3 localPos = position - Instance.transform.position;

        bool hit = false;
        if (localPos.x > Instance.RectSize.x)
        {
            hit = true;
            localPos = new Vector3(localPos.x * -1 + Instance.Offset, localPos.y, localPos.z);
        }
        else if (localPos.x < -Instance.RectSize.x)
        {
            hit = true;
            localPos = new Vector3(localPos.x * -1 - Instance.Offset, localPos.y, localPos.z);
        }
        else if(localPos.z > Instance.RectSize.y)
        {
            hit = true;
            localPos = new Vector3(localPos.x, localPos.y, localPos.z * -1 + Instance.Offset);
        }
        else if(localPos.z < -Instance.RectSize.y)
        {
            hit = true;
            localPos = new Vector3(localPos.x , localPos.y, localPos.z * -1 - Instance.Offset);
        }

        position = Instance.transform.position + localPos;
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
