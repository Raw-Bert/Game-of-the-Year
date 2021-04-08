using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEnd : MonoBehaviour
{
    public GameObject levelEndUI;
    public string nextScene;

    void Awake()
    {
        levelEndUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0.0f;
            SceneManager.LoadScene(nextScene);
        }
    }
}