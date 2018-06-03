using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WireType
{
    Purple,
    Orange,
    Green,
}

public class WireBehaviour : MonoBehaviour {

    Vector3 vel;
    Vector3 targetPos;
    [SerializeField] float dampFactor;
    bool inPlane = false;

    public WireType wType;
    // Use this for initialization
    void Awake () {
        targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition.z > 0.001f)
        {
            MoveToZeroPlane();
        }
        else if (!inPlane)
        {
            inPlane = true;
            transform.localPosition = targetPos;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void MoveToZeroPlane()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref vel, dampFactor);
    }
}
