using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Registration : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField nicknameField;
    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private Button BtnSubmeter;
    [SerializeField]
    private Button BtnLogin;
    [SerializeField]
    private Text feedbackMsg;

    ArrayList dados;

    void Start()
    {
        BtnSubmeter.onClick.AddListener(escreverFicheiro);
        BtnLogin.onClick.AddListener(goToLoginScene);

        if (File.Exists(Application.dataPath + "/dados.csv"))
        {
            dados = new ArrayList(File.ReadAllLines(Application.dataPath + "/dados.csv"));
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/dados.csv", "");
        }

    }

    void goToLoginScene()
    {
        SceneManager.LoadScene(0);
    }


    void escreverFicheiro()
    {
        bool ifExist = false;

        dados = new ArrayList(File.ReadAllLines(Application.dataPath + "/dados.csv"));
        foreach (var i in dados)
        {
            if (i.ToString().Contains(nicknameField.text))
            {
                ifExist = true;
                break;
            }
        }

        if (ifExist)
        {
            FeedbackError($"Oops! O nickname '{nicknameField.text}' já existe!");
        }
        else
        {
            dados.Add(nicknameField.text + ";" + passwordField.text);
            File.WriteAllLines(Application.dataPath + "/dados.csv", (String[])dados.ToArray(typeof(string)));
            Debug.Log("YEY! Conta registada!");
            loadLoginScene();
        }
    }

    void loadLoginScene()
    {
        SceneManager.LoadScene(0);
    }

    void FeedbackError(string mensagem)
    {
        feedbackMsg.CrossFadeAlpha(100f, 1f, false);
        feedbackMsg.color = Color.red;
        feedbackMsg.text = mensagem;
        feedbackMsg.CrossFadeAlpha(0f, 2f, false);
        nicknameField.text = "";
        passwordField.text = "";
    }
}