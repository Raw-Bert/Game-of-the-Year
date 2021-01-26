using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public GameObject gameOverMenu;
    Coroutine end;
    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu.SetActive(false);
    }

    public void End(float seconds)
    {
        end = StartCoroutine(EndGame(seconds));
    }

    IEnumerator EndGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;

    }
}
