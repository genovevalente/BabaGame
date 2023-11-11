using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text;

using TMPro;


public class UIController_Script : MonoBehaviour
{
    public bool isVictory;
    public bool isDead;
    public bool isPaused;
    public bool sent;

    private GameObject victoryUI;
    private GameObject deathUI;
    private GameObject pauseUI;
    private GameObject player;
    private GameObject victoryUIInfo;

    private int coinNum;
    private int pontosAtual;
    private string utilizador = "";

    private int valorVale;


    ArrayList currentUser;
    ArrayList BDpontos;
    ArrayList utilizadorArr;

    private void Awake()
    {

    }

    void Start()
    {
        Time.timeScale = 1;
        coinNum = 0;
        isVictory = false;
        isDead = false;
        isPaused = false;
        sent = false;

        //string path = Application.dataPath + "/pontos.txt";

        pauseUI = GameObject.FindGameObjectWithTag("pausemenu");
        victoryUI = GameObject.FindGameObjectWithTag("victory_screen");
        deathUI = GameObject.FindGameObjectWithTag("gameover_screen");
        player = GameObject.FindGameObjectWithTag("player");

        victoryUIInfo = GameObject.FindGameObjectWithTag("infotext");

        pauseUI.SetActive(false);
        victoryUI.SetActive(false);
        deathUI.SetActive(false);

        utilizadorArr = new ArrayList(File.ReadAllLines(Application.dataPath + "/currentUser.csv"));
        utilizador = utilizadorArr[utilizadorArr.Count - 1].ToString();

        //PromoBtn.onClick.AddListener(promoCode);

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {

            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (isVictory && !isDead)
        {
            StartVictory();
        }

        if (isDead)
        {
            StartDeath();
        }

    }

    public void ResumeGame()
    {
        pauseUI = GameObject.FindGameObjectWithTag("pausemenu");
        pauseUI.SetActive(false);
        ResumeTime();
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        StopTime();
        isPaused = true;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1.0f;
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void StartVictory()
    {
        StopTime();
        victoryUI.SetActive(true);
        if (!sent)
        {
            ShowCoins();
        }
    }

    public void StopVictory()
    {
        ResumeTime();
        victoryUI.SetActive(false);
    }

    public void StartDeath()
    {
        StopTime();
        deathUI.SetActive(true);
    }

    public void CloseDeathUI()
    {
        deathUI.SetActive(false);
    }

    public void RestartLevel()
    {
        isDead = false;
        ResumeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowCoins()
    {
        coinNum = player.GetComponent<PlayerCollecting_Script>().collectible;
        string pathPontos = Application.dataPath + "/BDpontos.txt";
        bool ifExist1 = false;
        BDpontos = new ArrayList(File.ReadAllLines(Application.dataPath + "/BDpontos.csv"));
        foreach (var i in BDpontos) //este foreach é para verificar se o utilizador existe, and nothing else
        {
            if (i.ToString().Contains(utilizador))
            {
                ifExist1 = true;
                break;
            }
        }

        if (ifExist1)
        {
            //código para subtituir a pontuação na linha seguinte pela pontuação atualizada
            int posUser = BDpontos.IndexOf(utilizador);
            int pontosPrevUser = Convert.ToInt32(BDpontos[posUser + 1]);
            pontosAtual = pontosPrevUser + coinNum;
            BDpontos[posUser + 1] = pontosAtual.ToString(); //valor no array está atualizado, mandar array para ficheiro:
            File.WriteAllLines(Application.dataPath + "/BDpontos.csv", (String[])BDpontos.ToArray(typeof(string)));
        }
        else
        {
            //código caso esta seja a primeira run deste utilizador, ou seja, adicionar o user à tabela e, na linha seguinte, a pontuação
            BDpontos.Add(utilizador);
            BDpontos.Add(coinNum.ToString());
            File.WriteAllLines(Application.dataPath + "/BDpontos.csv", (String[])BDpontos.ToArray(typeof(string)));
            pontosAtual = coinNum;
        }

        Debug.Log(coinNum);
        sent = true;

        promoCode();
        SendVictoryText();
    }

    void promoCode()
    {
        //temos que ir buscar os pontos do utilizador. por cada 500 pontos, redime 10 euros em talão
        //depois de ter o valor atribuido, tem de mostar uma textbox a mostrar o valor acumulado em pontos e vales

        float valorVf = pontosAtual / 100;
        valorVale = (int)valorVf; //não pode arredondar, para não atribuir vales antes de atingir a pontuação necessária
    }

    public void SendVictoryText()
    {

        string strInfo = "Tens " + pontosAtual.ToString() + " pontos! Tens direito a " + valorVale.ToString() + " vales de desconto.";

        victoryUIInfo.GetComponent<TMP_Text>().text = strInfo;
    }

}