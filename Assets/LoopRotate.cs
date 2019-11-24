using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRotate : MonoBehaviour
{
	public Vector3 Range;

	public Transform Target;

	private Quaternion StartRotation;

	public float Speed;

    void Start()
    {
		StartRotation = Target.rotation;
    }

    void Update()
    {
		float offset = Mathf.Sin(Time.time * Speed);
		Target.rotation = StartRotation * Quaternion.Euler(offset * Range.x, offset * Range.y, offset * Range.z);
    }
}
