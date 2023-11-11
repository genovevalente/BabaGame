using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCollecting_Script : MonoBehaviour
{
    
    public bool taken = false;
    private GameObject player;
    private Animator anim;
    //public AudioSource audio;


    void Start()
    {
        //audio = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("player");
        anim = player.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        //audio.Play();
        player.gameObject.GetComponent<PlayerCollecting_Script>().IncrementCollectible();
    }
}
