using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing_Script : MonoBehaviour
{
    public Transform target;

    public float speed = 0.125f;
    public float offset_x;
    public float offset_y;

    private void FixedUpdate()
    {
        Vector3 desired_position = new Vector3(target.position.x, target.position.y, -10);
        Vector3 smoothed_position = Vector3.Lerp(transform.position, desired_position, speed);

        transform.position = smoothed_position;

    }
}
