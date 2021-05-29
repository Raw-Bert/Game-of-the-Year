using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossLevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    private Vector3 velocity = Vector3.zero;
    public GameObject arenaWallFront;
    public GameObject frontWallTrigger;
    public GameObject arenaWallBack;
    public GameObject backWallTrigger;

    public bool wallsDrop = false;

    void Awake()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (arenaWallFront != null)
        {
            if (player.transform.position.x > frontWallTrigger.transform.position.x + 0.5f && arenaWallFront.transform.position != frontWallTrigger.transform.position && wallsDrop == false)
            {

                boss.GetComponent<BossSlime>().aggro = true;
                arenaWallFront.transform.position = Vector3.SmoothDamp(arenaWallFront.transform.position, frontWallTrigger.transform.position, ref velocity, 0.1f);
                arenaWallBack.transform.position = Vector3.SmoothDamp(arenaWallBack.transform.position, backWallTrigger.transform.position, ref velocity, 0.1f);
                if (arenaWallFront.transform.position.y <= frontWallTrigger.transform.position.y + 0.01f)
                {
                    wallsDrop = true;
                }
            }

            if (!boss)
            {
                if (arenaWallFront)
                    Destroy(arenaWallFront);
                    
                if (arenaWallBack)
                    Destroy(arenaWallBack);
            }
        }

    }
}