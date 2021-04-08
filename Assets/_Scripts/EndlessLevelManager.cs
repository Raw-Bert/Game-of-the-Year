using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelManager : MonoBehaviour
{
    public GameObject wall;
    public float triggerPos = 13.5f;
    public GameObject target;
    bool wallAnimation = false;

    public GameObject player;
    private Vector3 velocity = Vector3.zero;
private void Awake()
{
    Time.timeScale = 1;
    player.GetComponent<Player>().shadowBarCurrent = 50;
}


    void Update() {
        if(!this.GetComponent<SpawnEnemies>().spawning)
        {
            if (player.transform.position.x > triggerPos)
            {
                //wall.transform.position = Vector2.Lerp(wall.transform.position, target.transform.position, 0.25f);
                //wall.SetActive(true);
                this.GetComponent<SpawnEnemies>().spawning = true;
            }
        }
        if(this.GetComponent<SpawnEnemies>().spawning && wall.transform.position != target.transform.position)
        {
            //wall.transform.position = Vector2.Lerp(wall.transform.position, target.transform.position, Time.deltaTime);
            wall.transform.position = Vector3.SmoothDamp(wall.transform.position, target.transform.position, ref velocity, 0.1f);
        }
        if(wallAnimation == false && wall.transform.position != target.transform.position)
        {
            //Play animation
            wallAnimation = true;
        }
    }

}
