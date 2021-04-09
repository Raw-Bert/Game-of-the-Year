using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public string story;
    public string endless;
    public void StoryButton()
    {
        SceneManager.LoadScene(story, LoadSceneMode.Single);
    }
 public void EndlessButton()
    {
        SceneManager.LoadScene(endless, LoadSceneMode.Single);
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
         Application.Quit();
    }
}
