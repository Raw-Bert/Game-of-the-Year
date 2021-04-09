using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public GameObject selectSymbol;
    public GameObject upLocation;
    public GameObject downLocation;

    Vector2 mousePosition;

    private void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        if (mousePos.y > 0)
        {
            selectSymbol.transform.position = upLocation.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Level 3 Scene");
            }
        }
        else
        {
            selectSymbol.transform.position = downLocation.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 3 Scene");
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}
