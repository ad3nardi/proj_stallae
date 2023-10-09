using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ui_menu_Con : MonoBehaviour
{
    [SerializeField] private int _menuMain;
    [SerializeField] private int _demoScene;

    


    public void GoToMenuMain()
    {
        SceneManager.LoadScene(_menuMain);
    }

    public void LoadGameDemoScene()
    {
        SceneManager.LoadScene(_demoScene);
    }

    public void Options()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
