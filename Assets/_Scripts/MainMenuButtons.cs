using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public string game;
    public void PlayButton()
    {
        SceneManager.LoadScene(game, LoadSceneMode.Single);
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
         Application.Quit();
    }
}
