using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;
//using System.Diagnostics;
//using static System.Net.Mime.MediaTypeNames;

public class LoginController : MonoBehaviour
{

    [SerializeField]
    private InputField nicknameField;
    [SerializeField]
    private InputField passwordField;
    [SerializeField]
    private UnityEngine.UI.Text feedbackMsg;
    [SerializeField]
    private Button LoginBtn;
    [SerializeField]
    private Button RegistoBtn;
    [SerializeField]
    private Toggle rememberData;

    ArrayList dados;
    ArrayList currentUser;

    void Start()
    {
        if (PlayerPrefs.HasKey("remember") && PlayerPrefs.GetInt("remember") == 1)
        {
            nicknameField.text = PlayerPrefs.GetString("rememberUser");
            passwordField.text = PlayerPrefs.GetString("rememberPass");
        }

        LoginBtn.onClick.AddListener(fazerLogin);
        RegistoBtn.onClick.AddListener(Registar);

        if (File.Exists(Application.dataPath + "/dados.csv"))
        {
            dados = new ArrayList(File.ReadAllLines(Application.dataPath + "/dados.csv"));
        }
        else
        {
            Debug.Log("Ainda não tem utilizadores registados!");
        }

    }

    void fazerLogin()
    {
        bool isExists = false;

        dados = new ArrayList(File.ReadAllLines(Application.dataPath + "/dados.csv"));

        if (rememberData.isOn)
        {
            PlayerPrefs.SetInt("remember", 1);
            PlayerPrefs.SetString("rememberUser", nicknameField.text);
            PlayerPrefs.SetString("rememberPass", passwordField.text);
        }

        foreach (var i in dados)
        {
            if (i.ToString().Substring(0, i.ToString().IndexOf(";")).Equals(nicknameField.text) &&
                i.ToString().Substring(i.ToString().IndexOf(";") + 1).Equals(passwordField.text))
            {
                isExists = true;
                break;
            }
        }

        if (isExists)
        {
            Debug.Log($"YEY! Login realizado com sucesso '{nicknameField.text}'");

            currentUser = new ArrayList(File.ReadAllLines(Application.dataPath + "/currentUser.csv"));
            currentUser.Add(nicknameField.text);
            File.WriteAllLines(Application.dataPath + "/currentUser.csv", (String[])currentUser.ToArray(typeof(string)));

            loadMainMenu();
        }
        else
        {
            FeedbackError("Oops! Nickname ou password incorreta!");
        }
    }

    void Registar()
    {
        SceneManager.LoadScene(1);
    }

    void loadMainMenu()
    {
        SceneManager.LoadScene(2);
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
