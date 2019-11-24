using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Wake : MonoBehaviour
{
	private float _wakeDieTime = 5f;

	private float _currentTimeToDeath;

	public Vector3 Offset;

	private void Awake()
	{
		_currentTimeToDeath = Mathf.Infinity; //Before refresh has been called
	}

	private void Start()
	{
		_wakeDieTime = GetComponent<TrailRenderer>().time;
	}

	public void SetLocalOffset(Transform parent)
	{
		Offset = parent.transform.InverseTransformPoint(transform.position);
		transform.SetParent(null, true);
	}
	public void Refresh(Transform parent)
    {
		_currentTimeToDeath = _wakeDieTime;
		Vector3 newPoint = parent.TransformPoint(Offset);
		transform.position = new Vector3(newPoint.x, transform.position.y, newPoint.z);
	}

    void Update()
    {
		_currentTimeToDeath -= Time.deltaTime;

		if(_currentTimeToDeath <= 0)
		{
			Destroy(gameObject);
		}
	}
}
