using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemies : MonoBehaviour
{
    float timer;
    public float spawnTime;

    Vector2 spawnPos;
    public List<GameObject> enemies;

    public int maxEnemies = 20;
    public int numberOfEnemies;

    public float spawnDistFromPlayer;

    public GameObject player;

    public List<GameObject> spawnPoints = new List<GameObject>();

    bool spawning = false;

    public GameObject wall;
    public float triggerPos = 13.5f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((timer >= spawnTime) && (numberOfEnemies < maxEnemies))
        {
            timer = 0;
            if(spawning == true) 
            {
                SpawnEnemy();
            }
            
        }
        if(!spawning)
        {
            if (player.transform.position.x > triggerPos)
            {
                wall.SetActive(true);
                spawning = true;
            }
        }
        
    }

    // Spawns enemy and gives it an attack type if chosen spawn point not near player
    void SpawnEnemy()
    {
        int breakLoop = 4;
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        while(breakLoop > 0)
        {
            GameObject spawnPoint = SpawnPoint();
            if(Mathf.Abs(spawnPoint.transform.position.x - playerPos.x) > spawnDistFromPlayer && Mathf.Abs(spawnPoint.transform.position.y - playerPos.y) > spawnDistFromPlayer)
            {
                GameObject newEnemy = Instantiate(enemies[Random.Range(0, 3)], spawnPoint.transform.position, Quaternion.identity);
                //newEnemy.GetComponent<EnemyAI>().movementType = (EnemyAI.EnemyType)Random.Range(1,4);
                //Debug.Log("spawn: " + spawnPoint.name);
                break;
            }
            else
            {
                breakLoop--;
                //Debug.Log("Not spawning...: " + spawnPoint.name);
            }
            
        }  
         //Debug.Log("Did not spawn... ");
    }

    //Gets a spawn point from the list
    GameObject SpawnPoint()
    {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            return spawnPoint;
    }

}
