using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class php_game : MonoBehaviour
{
    public TMP_Text playerDisplay;
    public TMP_Text scoreDisplay;

    private void Awake()
    {
        if(php_DBmanager.username == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        playerDisplay.text = "Player: " + php_DBmanager.username;
        scoreDisplay.text = "Score: " + php_DBmanager.score;
    }

    public void CallSaveData()
    {
        StartCoroutine(SavePlayerData());
    }

    IEnumerator SavePlayerData()
    {
        
        WWWForm form = new WWWForm();
        form.AddField("name", php_DBmanager.username);
        form.AddField("score", php_DBmanager.score);

        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Game Saved.");
        }
        else
        {
            Debug.Log("Save failed. Error #" + www.text);
        }

        php_DBmanager.LogOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        
    }

    public void IncreaseScore()
    {
        php_DBmanager.score++;
        scoreDisplay.text = "Score: " + php_DBmanager.score;

    }
}
