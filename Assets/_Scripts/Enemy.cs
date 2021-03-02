using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public bool onCam { get; set; } = false;
    GameObject manager;
    public GameObject player { get; set; }

    public HealthBar healthBar;
    void OnDestroy()
    {
        if(!player)return;
        if (onCam)
            switch (gameObject.GetComponent<EnemyAI>().movementType)
            {
                case EnemyAI.EnemyType.Ranged:
                    --player.GetComponent<AudioSwapper>().amountOfEnemyType[0];
                    break;

                case EnemyAI.EnemyType.Charge:
                    --player.GetComponent<AudioSwapper>().amountOfEnemyType[1];
                    break;
            }

        var amountEnemy = player.GetComponent<AudioSwapper>().amountOfEnemyType;
        for (int a = 0; a < amountEnemy.Count; ++a)
            if (amountEnemy[a] < 0)
                amountEnemy[a] = 0;

    }

    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.Find("LevelController");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        manager.GetComponent<SpawnEnemies>().numberOfEnemies += 1;
    }

    //void Update(){
    //if (Input.GetKeyDown(KeyCode.Space))
    //{
    //    TakeDamage(20);
    //}
    //}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player Bullet")
        {
            TakeDamage(15);

            Destroy(col.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {

            Die();
        }
    }

    void Die()
    {
        //Play Enemy death animation here
        manager.GetComponent<GameScore>().countScore(10);
        manager.GetComponent<SpawnEnemies>().numberOfEnemies -= 1;
        Destroy(this.gameObject);
    }
}