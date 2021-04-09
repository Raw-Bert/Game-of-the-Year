using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public void resumeScene()
    {
        Time.timeScale = 1;
        transform.parent.gameObject.SetActive(false);
    }

    public void restartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void toMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}