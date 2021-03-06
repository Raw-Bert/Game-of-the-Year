﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //If player collides with game objects of various tags, player takes X damage
        Debug.Log("OnCollisionEnter2D");
        if (col.gameObject.tag == "Hurtful")
        {
            this.TakeDamage(20);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            this.TakeDamage(10);
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
        Destroy(this.gameObject);
    }

}
