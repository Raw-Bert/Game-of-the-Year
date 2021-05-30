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
    public float flashTime;
    public float flashMaxAlpha;
    public Color flashColor = Color.red;
        public List<GameObject> weaponDrops;

    public int probabilityOfHPDrop = 6; //4%
    public GameObject HPDrop;

    bool isDied = false;
    public float dieTime = 5f;
    float waitTime = 0;

    void OnDestroy()
    {
        if (!player)return;
        Camera.main.GetComponent<EnemyInView>().decreaseAmountOfEnemyType(this.gameObject);

    }

    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.Find("LevelController");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        manager.GetComponent<SpawnEnemies>().numberOfEnemies += 1;
    }

    void Update()
    {
        if (isDied)
        {
            if (waitTime > 0)
            {
                Color tmp = this.GetComponent<SpriteRenderer>().color;
                tmp.a = waitTime / dieTime;
                this.GetComponent<SpriteRenderer>().color = tmp;
                waitTime -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(20);
        //}
        //}

        // void OnCollisionEnter2D(Collision2D col)
        // {
        //     if(col.gameObject.tag == "Player Bullet")
        //     {
        //         TakeDamage(15);

        //         Destroy(col.gameObject);
        //     }
    }

    public void TakeDamage(int damage)
    {
        this.gameObject.GetComponent<Flash>().StartScreenFlash(flashTime, flashMaxAlpha, flashColor);
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
        if (this.GetComponent<EnemyAI>().movementType == EnemyAI.EnemyType.Ranged)
        {
            this.GetComponent<Animator>().enabled = true;
            this.GetComponent<EnemyAI>().rangeEnemyArm.SetActive(false);
        }
        this.GetComponent<Animator>().SetBool("isDie", true);

        this.GetComponent<CapsuleCollider2D>().enabled = false;
        healthBar.gameObject.SetActive(false);
        this.GetComponent<EnemyAI>().agent.isStopped = true;
        this.GetComponent<EnemyAI>().agent.ResetPath();
        this.GetComponent<EnemyAI>().movementType = EnemyAI.EnemyType.Static;
        waitTime = dieTime;

        if (HealthDropChance(probabilityOfHPDrop))
        {
            Instantiate(HPDrop, this.transform.position, Quaternion.identity);
        }

        //gun drop chance
        if (HealthDropChance(7))
        {
            int gunDrop = Random.Range(0, weaponDrops.Count);
            Debug.Log("Count: " + weaponDrops.Count);
            Instantiate(weaponDrops[gunDrop], this.transform.position, Quaternion.identity);
        }

        manager.GetComponent<GameScore>().countScore(10);
        manager.GetComponent<SpawnEnemies>().numberOfEnemies -= 1;
        isDied = true;
        //Destroy(this.gameObject);
    }

    bool HealthDropChance(int probability)
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < probability)
        {
            return true;
        }
        return false;
    }
}