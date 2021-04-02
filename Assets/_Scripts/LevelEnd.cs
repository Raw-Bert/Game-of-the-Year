using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public GameObject levelEndUI;

    void Awake()
    {
        levelEndUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Time.timeScale = 0.0f;
            levelEndUI.SetActive(true);
        }
    }
}
