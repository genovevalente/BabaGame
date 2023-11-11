using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet_Script : MonoBehaviour
{
    PlayerMovement_Script player_move_script = null;
    private void Awake()
    {
        player_move_script = GetComponentInParent<PlayerMovement_Script>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
        {
            player_move_script.grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
        {
            player_move_script.grounded = false;
        }
    }
}
