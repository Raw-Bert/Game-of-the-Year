using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForm : MonoBehaviour
{
    public GameObject flashImage;
    public bool shadowForm = false;
    public KeyCode shadowModeKey;

    public float flashTime;
    public float flashMaxAlpha;
    public Color flashColor = Color.white;
    public Color shadowFlashColor = Color.black;

    public GameObject normalTileMap;
    public GameObject shadowTileMap;

    void Start()
    {
        shadowTileMap.SetActive(false);
        normalTileMap.SetActive(true);
    }
    // Update is called once per frame
    //If designated key pressed, switch form, switch dimension, and make the screen flash
    void Update()
    {
        if (Input.GetKeyDown(shadowModeKey))
        {
            if (shadowForm == false)
            {
                //screen flash black
                flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, shadowFlashColor);

                //Change form from normal to shadow
                shadowTileMap.SetActive(true);
                normalTileMap.SetActive(false);

                shadowForm = true;
            }
            else if(shadowForm == true)
            {
                //Screen flash white
                flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, flashColor);

                //Change form from shadow to normal
                shadowTileMap.SetActive(false);
                normalTileMap.SetActive(true);
                
                shadowForm = false;
            }
        }
                
    }
}

