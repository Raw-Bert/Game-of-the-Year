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

    public float flashTime;
    public float flashMaxAlpha;
    public Color flashColor;

    bool invincible = false;
    float timer;
    float invincibilityTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update(){
        timer += Time.deltaTime;
        if (timer < invincibilityTime)
        {
            invincible = true;
        }
        else invincible = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //If player collides with game objects of various tags, player takes X damage
        if (col.gameObject.tag == "Hurtful")
        {
            if (invincible == false){
                 this.TakeDamage(hurtfulDamage);
                 
            }
        }
        else if (col.gameObject.tag == "Enemy")
        {
             if (invincible == false){
                this.TakeDamage(enemyDamage);
                //timer = 0;
             }
        }
        else if (col.gameObject.tag == "EnemyProjectile")
        {
             if (invincible == false){
                this.TakeDamage(enemyProjectileDamage);
                //timer = 0;
             }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        this.GetComponent<HealthbarFlash>().StartHealthFlash(flashTime, flashMaxAlpha, flashColor);
        timer = 0;
    
        
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
