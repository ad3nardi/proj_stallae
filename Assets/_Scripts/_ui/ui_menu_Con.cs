using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ui_menu_Con : MonoBehaviour
{
    [SerializeField] private int _menuMain;

    


    public void GoToMenuMain()
    {
        SceneManager.LoadScene(_menuMain);
    }

    public void Options()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
