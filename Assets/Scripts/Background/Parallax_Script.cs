using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_Script : MonoBehaviour
{
    private float length, startPos, startHeight;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startHeight = transform.position.y;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        
        float camPositionX = cam.transform.position.x;
        float temp = (camPositionX * (1 - parallaxEffect));
        float dist = camPositionX * parallaxEffect;

        transform.position = new Vector3(startPos + dist, startHeight + (cam.transform.position.y * 0.5f), transform.position.z); ;

        if(temp > startPos + length)
        {
            startPos += length;
        }
        else if(temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
