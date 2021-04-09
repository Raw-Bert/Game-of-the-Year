using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForm : MonoBehaviour
{
    public GameObject flashImage;
    public bool shadowForm { get; private set; } = false;
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

    public GameObject normalEmblem;
    public GameObject shiftEmblem;

    public GameObject wallColour;
    public Color newWall;

    public bool canSwitch = false;
    public bool canSwitchNormal { get; private set; } = false;

    float timer = 0;

    public Sprite shadowPlayer;
    public Sprite lightPlayer;

    public RuntimeAnimatorController shadowPlayerController;
    public RuntimeAnimatorController lightPlayerController;

    [SerializeField][FMODUnity.EventRef] string shiftSFX;

    void Start()
    {
        Cursor.SetCursor(normalCursor, new Vector2(normalCursor.width / 2, normalCursor.height / 2), CursorMode.Auto);

        shadowTileMap.SetActive(false);
        normalTileMap.SetActive(true);
        shadowNavMesh.SetActive(false);
        normalNavMesh.SetActive(true);
        shadowGround.SetActive(false);
        normalGround.SetActive(true);
        normalEmblem.SetActive(true);
    }
    // Update is called once per frame
    //If designated key pressed, switch form, switch dimension, and make the screen flash
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(shadowModeKey))
        {
            if (canSwitch)
            {
                if (!shadowForm)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(shiftSFX);

                    //Change player oputlook
                    this.GetComponent<SpriteRenderer>().sprite = shadowPlayer;
                    this.GetComponent<Animator>().runtimeAnimatorController = shadowPlayerController;

                    //screen flash black
                    flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, shadowFlashColor);

                    //Change form from normal to shadow
                    shadowTileMap.SetActive(true);
                    normalTileMap.SetActive(false);
                    shadowNavMesh.SetActive(true);
                    normalNavMesh.SetActive(false);
                    shadowGround.SetActive(true);
                    normalGround.SetActive(false);

                    this.GetComponent<Player>().shadowBarCurrent -= (int)(this.GetComponent<Player>().maxShadowBar * 0.2f);
                    shadowForm = true;

                    // cursor change
                    Cursor.SetCursor(shadowCursor, new Vector2(shadowCursor.width / 2, shadowCursor.height / 2), CursorMode.Auto);

                    //change emblem
                    normalEmblem.SetActive(false);
                    shiftEmblem.SetActive(true);

                    //this.GetComponent<Player>().shadowBarCurrent = 0;

                    wallColour.GetComponent<Renderer>().material.SetColor("_Color", newWall);
                    canSwitchNormal = true;
                    timer = 0.0f;
                    //canSwitch = false;
                }

            }
            if (shadowForm == true && canSwitchNormal == true && timer > 0.2f)
            {
                SwitchToNormal();
            }

        }
    }

    public void SwitchToNormal()
    {
        FMODUnity.RuntimeManager.PlayOneShot(shiftSFX);

        //Change player oputlook
        this.GetComponent<SpriteRenderer>().sprite = lightPlayer;
        this.GetComponent<Animator>().runtimeAnimatorController = lightPlayerController;

        //Screen flash white
        flashImage.GetComponent<ScreenFlash>().StartScreenFlash(flashTime, flashMaxAlpha, flashColor);

        //Change form from shadow to normal
        shadowTileMap.SetActive(false);
        normalTileMap.SetActive(true);
        shadowNavMesh.SetActive(false);
        normalNavMesh.SetActive(true);
        shadowGround.SetActive(false);
        normalGround.SetActive(true);

        //shadowForm = false;

        // cursor change
        Cursor.SetCursor(normalCursor, new Vector2(normalCursor.width / 2, normalCursor.height / 2), CursorMode.Auto);

        shadowForm = false;
        canSwitchNormal = false;

        // cursor change
        Cursor.SetCursor(normalCursor, new Vector2(normalCursor.width / 2, normalCursor.height / 2), CursorMode.Auto);

        //change emblem
        normalEmblem.SetActive(true);
        shiftEmblem.SetActive(false);

        wallColour.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
    }
}