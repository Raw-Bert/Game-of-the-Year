using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public GameObject manager;

    public int hurtfulDamage = 20;
    public int enemyDamage = 10;
    public int enemyProjectileDamage = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //If player collides with game objects of various tags, player takes X damage
        //Debug.Log("OnCollisionEnter2D");
        if (col.gameObject.tag == "Hurtful")
        {
            this.TakeDamage(hurtfulDamage);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            this.TakeDamage(enemyDamage);
        }
        else if (col.gameObject.tag == "EnemyProjectile")
        {
            this.TakeDamage(enemyProjectileDamage);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    
        
        if(currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        //Play player death animation here
        manager.GetComponent<GameOver>().End(2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        
    }

}
