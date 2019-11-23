using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour
{
    public Transform rotate1;
    public Transform rotate2;

	public Animator[] LeftOars;
	public Animator[] RightOars;

	public float TrailBoostDecrese = 0.1f;

	public float GotPointDecrease = 50f;

	public float Speed
    {
        get;
        set;
    }

	private bool _rotate;

	private float lastFinalDir;

    public bool Rotate
    {
        get
		{
			return _rotate;
		}
        set
		{
			_rotate = value;
		}
    }

    public float Boost
    {
        get;
        set;
    }
	public float TrailBoost
	{
		get;
		set;
	}

	public float GotPointBoost
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
            transform.RotateAround(rotate1.position, Vector3.up, direction * (Speed + Boost + TrailBoost + GotPointBoost) * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(rotate2.position, Vector3.up, direction  * -(Speed + Boost + TrailBoost + GotPointBoost) * Time.deltaTime);
        }

		 float finalDir = ((!Rotate) ? -1 * direction : direction);
		if (finalDir > 0)
		{
			if (lastFinalDir != finalDir)
			{
				foreach(Animator left in LeftOars) left.SetTrigger("Off");
				foreach (Animator right in RightOars) right.SetTrigger("On");

				lastFinalDir = finalDir;
			}
		}
		else
		{
			if (lastFinalDir != finalDir)
			{
				foreach (Animator left in LeftOars) left.SetTrigger("On");
				foreach (Animator right in RightOars) right.SetTrigger("Off");
				lastFinalDir = finalDir;
			}
		}

		GotPointBoost = Mathf.Max(0, GotPointBoost - Time.deltaTime * GotPointDecrease);
		if(GotPointBoost > 0)
		{
			Debug.Log(GotPointBoost);
		}
	}
}
