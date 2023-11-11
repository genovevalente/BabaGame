using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController_Script : MonoBehaviour
{
    private Transform startPos;
    private Transform pos1, pos2;
    private Transform currentPos;

    public bool isHorizontal;
    
    public Vector2 position1, position2;
    public float distance;

    private Vector2 p;
    public bool forwards;

    public float speed;

    void Start()
    {
        p = transform.position;
        position1 = new Vector2(transform.position.x, transform.position.y);
        position2 = new Vector2(transform.position.x+distance, transform.position.y);

    }


    void Update()
    {
        if (Mathf.Approximately(transform.position.x, position2.x))
        {
            forwards = false;
        }else if(Mathf.Approximately(transform.position.x, position1.x))
        {
            forwards = true;
        }



        if (forwards)
        {
            transform.position = Vector2.MoveTowards(transform.position, position2, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, position1, speed * Time.deltaTime);
        }

    }

    public void OnDrawGismos()
    {

    }
}
