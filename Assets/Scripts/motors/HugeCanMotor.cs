using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeCanMotor : MonoBehaviour
{
    public Transform victim; // Our player
    public Vector3 offset;

    private void LateUpdate()
    {
        if (GameManager.Instance.IsDead)
        {
            // running ahead of victim to smash him!
            transform.position += new Vector3(0, 0, Time.deltaTime * 20);
            transform.Rotate(Vector3.down, Time.deltaTime * 180);
            return;
        }

        Vector3 desiredPosition = victim.position + offset;
        desiredPosition.x = 0;
        desiredPosition.y = offset.y;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        transform.Rotate(Vector3.down, Time.deltaTime * 180);
    }
}
