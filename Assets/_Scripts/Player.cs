using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.UI;
using TMPro;

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

    public int healingAmount = 25;
    public TextMeshProUGUI healthText;

    //Shadow Bar variables
    public int shadowBarCurrent = 0;
    public int maxShadowBar = 100;
    public HealthBar shadowBar;
    float shadowTimer = 0;
    float shadowBarThreshold = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        healthText.text = maxHealth + "/" + maxHealth;

        shadowBar.SetMaxHealth(maxShadowBar);
        shadowBar.SetHealth(0);
    }

    void Update(){
        timer += Time.deltaTime;
        shadowTimer += Time.deltaTime;

        if (timer < invincibilityTime)
        {
            invincible = true;
        }
        else invincible = false;

        healthText.text = currentHealth + "/" + maxHealth;

        if(shadowBarCurrent == maxShadowBar)
        {
            this.GetComponent<ChangeForm>().canSwitch = true;
        }
        else
        {
            if(shadowTimer >= shadowBarThreshold && this.GetComponent<ChangeForm>().shadowForm == false)
            {
                shadowBarCurrent += 1;
                shadowBar.SetHealth(shadowBarCurrent);
                shadowTimer = 0;
            }
        }

        if(shadowTimer >= shadowBarThreshold && this.GetComponent<ChangeForm>().shadowForm == true)
        {
            shadowBarCurrent -= 3;
            shadowBar.SetHealth(shadowBarCurrent);
            shadowTimer = 0;
            
        }

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

    void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "EnemyProjectile")
            {
                if (invincible == false)
                {
                    this.TakeDamage(enemyProjectileDamage);
                }
            }
            if(other.gameObject.tag == "HealthPickup")
            {
                this.Healing(healingAmount);
                Destroy(other.gameObject);
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

    public void Healing(int healAmount)
    {
        if(currentHealth <= maxHealth - healAmount)
        {
            currentHealth += healAmount;
            healthBar.SetHealth(currentHealth);
        }
        else if(currentHealth > maxHealth - healAmount)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
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
