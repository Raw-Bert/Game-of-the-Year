using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    GameObject manager;
    

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        manager = GameObject.Find("LevelController");
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player Bullet")
        {
            TakeDamage(25);

            Destroy(col.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            manager.GetComponent<GameScore>().countScore(10);
            Die();
        }
    }

    void Die()
    {
        //Play Enemy death animation here
        Destroy(this.gameObject);
    } 
}
