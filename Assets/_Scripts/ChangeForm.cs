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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shadowModeKey))
        {
            if (shadowForm == false)
            {
                //screen flash black
                flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, shadowFlashColor);
                //Change sprite to shadow player
                shadowForm = true;
            }
            else if(shadowForm == true)
            {
                flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, flashColor);
                //Change sprite to normal player
                shadowForm = false;
            }
        }
                
    }
}

