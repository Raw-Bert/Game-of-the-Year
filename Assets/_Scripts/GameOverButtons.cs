using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{


    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("_GameScene");
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}
