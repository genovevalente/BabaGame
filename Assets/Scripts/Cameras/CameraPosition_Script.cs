using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition_Script : MonoBehaviour
{
    public GameObject cam;
    public GameObject following;

    Vector3 cam_position;
    float cam_position_x;
    float cam_position_y;

    Vector3 prev_cam;
    float prev_cam_x;
    float prev_cam_y;

    Vector2 follow_p;
    float f_x;
    float f_y;


    // Start is called before the first frame update
    void Start()
    {
        cam_position_x = following.transform.position.x;
        cam_position_y = following.transform.position.y;

        prev_cam_x = following.transform.position.x;
        prev_cam_y = following.transform.position.y;

        f_x = following.transform.position.x;
        f_y = following.transform.position.y;

    }


    // Update is called once per frame
    void Update()
    {
        f_x = following.transform.position.x;
        f_y = following.transform.position.y;

        if (cam_position_y > f_y)
        {
            cam_position_y -= f_y;
        }

        Vector3 cam_position = new Vector3(cam_position_x, cam_position_y, -1);
        cam.transform.position = cam_position;

        prev_cam = new Vector2(prev_cam.x, prev_cam.y);
    }
}
