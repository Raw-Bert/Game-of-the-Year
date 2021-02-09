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

    public GameObject normalGround;
    public GameObject shadowGround;

    public GameObject normalNavMesh;
    public GameObject shadowNavMesh;

    public Texture2D normalCursor;
    public Texture2D shadowCursor;

    void Start()
    {
        Cursor.SetCursor(normalCursor, new Vector2(normalCursor.width / 2, normalCursor.height / 2), CursorMode.Auto);

        shadowTileMap.SetActive(false);
        normalTileMap.SetActive(true);
        shadowNavMesh.SetActive(false);
        normalNavMesh.SetActive(true);
        shadowGround.SetActive(false);
        normalGround.SetActive(true);
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
                shadowNavMesh.SetActive(true);
                normalNavMesh.SetActive(false);
                shadowGround.SetActive(true);
                normalGround.SetActive(false);

                shadowForm = true;

                // cursor change
                Cursor.SetCursor(shadowCursor, new Vector2(shadowCursor.width/2, shadowCursor.height/2), CursorMode.Auto);
            }
            else if(shadowForm == true)
            {
                //Screen flash white
                flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, flashColor);

                //Change form from shadow to normal
                shadowTileMap.SetActive(false);
                normalTileMap.SetActive(true);
                shadowNavMesh.SetActive(false);
                normalNavMesh.SetActive(true);
                shadowGround.SetActive(false);
                normalGround.SetActive(true);

                shadowForm = false;

                // cursor change
                Cursor.SetCursor(normalCursor, new Vector2(normalCursor.width / 2, normalCursor.height / 2), CursorMode.Auto);
            }
        }
                
    }
}

