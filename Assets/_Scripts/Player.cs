using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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

    public bool death;

    public int healingAmount = 25;
    public TextMeshProUGUI healthText;

    //Shadow Bar variables
    public float shadowBarCurrent = 0;
    public int maxShadowBar = 100;
    public HealthBar shadowBar;
    float shadowBarThreshold = 0.2f;
    float shadowTimer = 0;
    //public GameObject mainCamera;

    public ParticleSystem shiftBarParticles;

    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        healthText.text = maxHealth + "/" + maxHealth;

        if (!shadowBar)return;
        shadowBar.SetMaxHealth(maxShadowBar);
        shadowBar.SetHealth(0);

        shiftBarParticles.Pause();
        death = false;

        playerAnimator = this.GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        shadowTimer += Time.deltaTime;

        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (timer < invincibilityTime)
        {
            invincible = true;
        }
        else invincible = false;

        healthText.text = currentHealth + " / " + maxHealth;

        if (shadowBarCurrent >= maxShadowBar * 0.4f)
        {
            this.GetComponent<ChangeForm>().canSwitch = true;
            
        }
        else
        {
            this.GetComponent<ChangeForm>().canSwitch = false;
            shiftBarParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (shadowTimer >= shadowBarThreshold && this.GetComponent<ChangeForm>().shadowForm == false)
        {
            shadowTimer = 0;
            shadowBarCurrent += 1.5f;
            shadowBarCurrent = Mathf.Min(shadowBarCurrent, maxShadowBar);
            if (shadowBar)
                shadowBar.SetHealth((int)shadowBarCurrent);
            if(shadowBarCurrent > maxShadowBar)
            {
                shadowBarCurrent = maxShadowBar;
            }
            shiftBarParticles.Play();

        }

        if (shadowTimer >= shadowBarThreshold && this.GetComponent<ChangeForm>().shadowForm == true)
        {
            shadowBarCurrent -= 1;
            shadowTimer = 0;
            shadowBarCurrent = Mathf.Max(shadowBarCurrent, 0);
            
            if (shadowBar)
                shadowBar.SetHealth((int)shadowBarCurrent);

        }

        if (shadowBarCurrent <= 0 && this.GetComponent<ChangeForm>().canSwitchNormal)
        {
            this.GetComponent<ChangeForm>().SwitchToNormal();
        }
        

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //If player collides with game objects of various tags, player takes X damage
        if (col.gameObject.tag == "Hurtful")
        {
            if (invincible == false)
            {
                this.TakeDamage(hurtfulDamage);

            }
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (invincible == false)
            {
                this.TakeDamage(enemyDamage);
                //timer = 0;
            }
        }
        else if (col.gameObject.tag == "EnemyProjectile")
        {
            if (invincible == false)
            {
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
        if (other.gameObject.tag == "HealthPickup")
        {
            this.Healing(healingAmount);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        this.GetComponent<HealthbarFlash>().StartHealthFlash(flashTime, flashMaxAlpha, flashColor);
        timer = 0;

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    public void Healing(int healAmount)
    {
        if (currentHealth <= maxHealth - healAmount)
        {
            currentHealth += healAmount;
            healthBar.SetHealth(currentHealth);
        }
        else if (currentHealth > maxHealth - healAmount)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }

    }

    void PlayerDeath()
    {
        playerAnimator.SetBool("isDead", true);
        manager.GetComponent<GameOver>().End(2f);
        death = true;
        
        //this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

}