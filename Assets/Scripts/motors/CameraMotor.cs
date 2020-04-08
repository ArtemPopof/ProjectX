using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // Our player // object we're looking at
    public Vector3 offset;
    public Vector3 reversedOffset = new Vector3(0, 1, 6);

    public Vector3 rotation = new Vector3(18, 0, 0);
    public bool isReversed;
    public int downAngle;

    public bool IsMoving {set; get;}

    // private void Start()
    // {
    //     transform.Rotate(isReversed ? 0 : downAngle, isReversed ? 180 : 0, 0);
    // }

    private void Update() {
        Vector3 realOffset = isReversed ? reversedOffset : offset;

        if (!IsMoving)
            return;

        Vector3 desiredPosition = lookAt.position + realOffset;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);

        if (isReversed)
        {
            transform.position = desiredPosition;
        }
        else
        {
            // smooth camera 
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);

            // instant following
            //transform.position = desiredPosition;
        }
    }

    public void ZoomPlayer()
    {
        // zoom a little bit after start
        //offset.z -= offset.z / 2;
    }
}
