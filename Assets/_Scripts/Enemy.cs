using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    GameObject manager;

    public HealthBar healthBar;
    

    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.Find("LevelController");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //void Update(){
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(20);
       //}
    //}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player Bullet")
        {
            TakeDamage(15);

            Destroy(col.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            
            Die();
        }
    }

    void Die()
    {
        //Play Enemy death animation here
        manager.GetComponent<GameScore>().countScore(10);
        Destroy(this.gameObject);
    } 
}
