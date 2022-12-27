using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class php_login : MonoBehaviour
{
    public TMP_Text playerDisplay;

    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    
    public Button submitButton;
    public void Start()
    {
        if (php_DBmanager.LoggedIn)
        {
            playerDisplay.text = "Player: " + php_DBmanager.username;
        }
    }

    public void CallLogin()
    {
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        //UnityWebRequest
        yield return www;

        if(www.text[0] == '0')
        {
            php_DBmanager.username = nameField.text;
            php_DBmanager.score = int.Parse(www.text.Split('\t')[1]);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("User Login Failed. Error #" + www.text);
            
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

}
