using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    float timer;
    public float spawnTime;

    Vector2 spawnPos;
    public GameObject enemy;

    public int maxEnemies = 20;
    public int numberOfEnemies;

    public float spawnDistFromPlayer;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((timer >= spawnTime) && (numberOfEnemies < maxEnemies))
        {
            SpawnEnemy();
            timer = 0;
            //GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = SpawnPoint();
        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
    }

    Vector2 SpawnPoint()
    {
        int spawnPointX = Random.Range(-15, 15);
        int spawnPointY = Random.Range(-15, 15);
        Vector2 spawnPos = new Vector2(spawnPointX,spawnPointY);
        Vector2 playerPos = new Vector2(player.transform.position.x,player.transform.position.y);
        if ((spawnPos.x - playerPos.x) < spawnDistFromPlayer && (spawnPos.y - playerPos.y) < spawnDistFromPlayer)
        {
            return SpawnPoint();
        }
        else return spawnPos;
    }
}
