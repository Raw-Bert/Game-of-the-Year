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

    //bool isTriggered = false;
    public List<GameObject> tutorialBoxes = new List<GameObject>();
    bool boxDrop = true;
    int initalBoxCount;


    void Awake()
    {
        initalBoxCount = tutorialBoxes.Count;
        Debug.Log(tutorialBoxes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialBoxes.Count < initalBoxCount && boxDrop == true)
        {
           
            for(int i = 0; i < tutorialBoxes.Count; i++)
            {
                
                tutorialBoxes[i].gameObject.GetComponent<Boxes>().tutorialItemDrop = false;
            }
            //boxDrop = false;
        }
        else
        {
            for(int i = 0; i < tutorialBoxes.Count; i++)
            {
                if(tutorialBoxes[i] == null)
                {
                    tutorialBoxes.Remove(tutorialBoxes[i]);
                }
            }
        }


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
