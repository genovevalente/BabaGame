using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Script : MonoBehaviour
{
    public bool paused = false;
    public bool gameOverMenu = false;

    private GameObject menuPanel;


    private void Awake()
    {
        menuPanel = GameObject.FindGameObjectWithTag("pausemenu");
        ResumeGame();
    }

    private void Update()
    {
        Debug.Log("Updote");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pressed Escape");

            if (!paused)
            { 
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }
    public void PauseGame()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }



}
