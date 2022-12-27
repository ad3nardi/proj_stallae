using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class php_mainMenu : MonoBehaviour
{
    public Button registerButton;
    public Button loginButton;
    public Button playButton;


    public TMP_Text playerDisplay;

    private void Start()
    {
        if (php_DBmanager.LoggedIn)
        {
            playerDisplay.text = "Player: " + php_DBmanager.username;
        }
        registerButton.interactable = !php_DBmanager.LoggedIn;
        loginButton.interactable = !php_DBmanager.LoggedIn;
        playButton.interactable = php_DBmanager.LoggedIn;
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(3);
    }
}
