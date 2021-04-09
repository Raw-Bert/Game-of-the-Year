using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseCanvas.activeInHierarchy)
            {
                disablePause();
            }
            else
            {
                enablePause();
            }
        }
    }

    public void disablePause()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void enablePause()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}