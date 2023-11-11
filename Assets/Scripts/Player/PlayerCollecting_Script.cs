using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollecting_Script : MonoBehaviour
{
    public int collectible = 0;
    public int coinCount = 0;
    public int coinValue;

    private AudioSource aud;

    public AudioClip coinSound;

    
    public TMP_Text collectibleText;
    public GameObject panelVictory;

    private GameObject coin;
    private GameObject[] coinArray;

    private GameObject uiController;
    

    private void Awake()
    {
        
    }

    private void Start()
    {

        aud = GetComponent<AudioSource>();
        uiController = GameObject.FindGameObjectWithTag("uicontroller");


        coinArray = GameObject.FindGameObjectsWithTag("collectible");
        coinCount = coinArray.Length;
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("collectible") && collision.gameObject)
        {
            aud.PlayOneShot(coinSound, 0.5f);
            coin = collision.gameObject;

            Destroy(coin);
        }
    }

    public void IncrementCollectible()
    {
        collectible += coinValue;
        collectibleText.text = "Coins: " + collectible.ToString();
        if(collectible >= coinCount)
        {
            uiController.GetComponent<UIController_Script>().isVictory = true;
        }
    }

    
}
