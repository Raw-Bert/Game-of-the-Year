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

    public List<GameObject> spawnPoints = new List<GameObject>();

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

    // Spawns enemy and gives it an attack type.
    void SpawnEnemy()
    {
        Vector2 spawnPos = SpawnPoint();
        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().movementType = (EnemyAI.EnemyType)Random.Range(1, 4);
        newEnemy.GetComponent<Enemy>().player = player;
    }

    //Creates a spawn point away from player for the enemy
    Vector2 SpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Vector2 spawnPos = spawnPoints[randomIndex].transform.position;
        //int spawnPointX = Random.Range(-15, 15);
        //int spawnPointY = Random.Range(-15, 15);
        //Vector2 spawnPos = new Vector2(spawnPointX,spawnPointY);
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        if ((spawnPos.x - playerPos.x) < spawnDistFromPlayer && (spawnPos.y - playerPos.y) < spawnDistFromPlayer)
        {
            return SpawnPoint();
        }
        else return spawnPos;
    }
}