using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerScript : MonoBehaviour
{
    public GameObject cam;
    public GameObject following;

    Vector3 cam_position;

    float prev_cam_x;
    float prev_cam_y;

    Vector3 prev_cam;
    Vector2 f_position;
   
    
    // Start is called before the first frame update
    void Start()
    {
        prev_cam = new Vector2(cam.transform.position.x, cam.transform.position.y);
        cam_position = new Vector3(following.transform.position.x, 0, -1);
        //prev_cam = cam.transform.position.x;
        prev_cam_y = cam.transform.position.y;
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        float f_position_x = following.transform.position.x;
        float f_position_y = following.transform.position.y;

        //f_position = new Vector2(f_position_x, f_position_y);
       
        if (f_position_y > prev_cam_y)
        {
            prev_cam_y += 0.3f;

        }
       // Vector3 cam_position = new Vector3(, , -1);
        cam.transform.position = cam_position;
        prev_cam = new Vector2(prev_cam.x, prev_cam.y);
    }
}
