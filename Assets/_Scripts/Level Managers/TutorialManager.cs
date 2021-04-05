using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject wall;
    public GameObject target;
    public GameObject levelEnd;
    //public GameObject tutorialOverUI;
    public float triggerPos = 13.5f;
    private Vector3 velocity = Vector3.zero;
    bool wallAnimation = false;
    public GameObject player;

    public float triggerEnemies = 17.5f;
    public GameObject enemyWall;
    bool enemiesUnleashed = false;

    bool isTriggered = false;



    // Update is called once per frame
    void Update()
    {
        if(!this.GetComponent<SpawnEnemies>().spawning)
        {
            if (player.transform.position.x > triggerPos)
            {
                this.GetComponent<SpawnEnemies>().spawning = true;
            }
        }

        if(this.GetComponent<SpawnEnemies>().spawning && wall.transform.position != target.transform.position)
        {
            wall.transform.position = Vector3.SmoothDamp(wall.transform.position, target.transform.position, ref velocity, 0.1f);
        }

        if(wallAnimation == false && wall.transform.position != target.transform.position)
        {
            //Play animation
            wallAnimation = true;
        }

        if(!enemiesUnleashed && player.transform.position.x > triggerEnemies)
        {
            Destroy(enemyWall);
            enemiesUnleashed = true;
        }
    }
    
    

}
