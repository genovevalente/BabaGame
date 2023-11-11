using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath_Script : MonoBehaviour
{
    private GameObject canvasGameOver;
    private GameObject player;
    private AudioSource audioSource;

    private GameObject uiController;

    public AudioClip spikeDeathSound;

    public bool isDead = false;
    public bool playDeathMusic = false;

    void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("uicontroller");
        canvasGameOver = GameObject.FindGameObjectWithTag("gameover_screen");
        player = GameObject.FindGameObjectWithTag("player");
       // canvasGameOver.SetActive(false);
        audioSource = player.GetComponent<AudioSource>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("killplane") || collision.gameObject.CompareTag("deadlytouchy"))
        {
            audioSource.PlayOneShot(spikeDeathSound);
            GameOver();
        }
    }

    private void GameOver()
    {
        //canvasGameOver.SetActive(true);
        //player.GetComponent<PlayerMovement_Script>().isDead = true;

        uiController.GetComponent<UIController_Script>().isDead = true;
    }
}
