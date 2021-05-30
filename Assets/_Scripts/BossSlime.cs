using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlime : MonoBehaviour
{
    public GameObject player;

    [Header("Boss Settings")]
    //bool hasCharged = false;
    bool spawnSlimes = false;

    public HealthBar healthBar;

    [Tooltip("Time it takes to charge up the dashing attack")]
    public float chargeThreshold = 2.0f;

    float timeSinceLastMove = 0.0f;
    public float timeBetweenMoves = 9.0f;

    float chargeTimer = 0.0f;
    //float spawnTimer = 15.0f;

    float dashAttackTime;

    bool shadowState;

    public int health;
    public int maxHealth = 300;

    bool enrage = false;
    bool isCollidingWithThing = false;

    Vector3 chargeLoc;
    public float chargeSpeed = 5;

    public List<GameObject> spawnPoints;
    public GameObject slimes;
    public int numberOfSpawns = 3;

    public bool aggro = false;
    private float damageTimer = 0;
    public bool isDead = false;
    public float dieTime = 5f;
    float waitTime = 0;

    public GameObject collideEffect;
    [SerializeField] private bool isCollidingWithPlayer = false;

    Animator slimeBoseAnimation;
    Renderer render;
    Shader origin;
    Shader rgb;
    private List<GameObject> testList = new List<GameObject>();

    public enum BossStates
    {
        idle, //idle
        chargeUp, //Charging up to dash
        dashing, //after charging up, moves at player quickly
        spawning //spawning enemies
    }

    public BossStates states;
    void Awake()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Start()
    {
        slimeBoseAnimation = this.GetComponent<Animator>();
        render = this.GetComponent<SpriteRenderer>();
        origin = Shader.Find("Sprites/Default");
        rgb = Shader.Find("Shader Graphs/RgbShift");
        render.material.shader = origin;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (health < maxHealth / 2 && enrage == false)
            {
                enrage = true;
                timeBetweenMoves = 5.0f;
                numberOfSpawns += 2;
            }

            if(damageTimer >= 1)
            {
                isCollidingWithPlayer = false;
            }
            else
            {
                damageTimer += Time.deltaTime;
            }

            switch (states)
            {
                case BossStates.idle:
                    if (aggro)
                    {
                        timeSinceLastMove += Time.deltaTime;

                        if (timeSinceLastMove >= 2)
                        {
                            if (slimeBoseAnimation.GetBool("isSpawn") == true)
                            {
                                slimeBoseAnimation.SetBool("isSpawn", false);
                                render.material.shader = origin;
                            }
                        }

                        if (timeSinceLastMove >= timeBetweenMoves)
                        {
                            int selectMove = Random.Range(1, 4);
                            Debug.Log("SELECTED MOVE: " + selectMove);
                            if (selectMove == 1 || selectMove == 2)
                            {
                                states = BossStates.chargeUp;
                                slimeBoseAnimation.SetBool("isChargeUp", true);
                                render.material.shader = rgb;
                            }
                            else
                            {
                                states = BossStates.spawning;
                                spawnSlimes = true;
                                slimeBoseAnimation.SetBool("isSpawn", true);
                                render.material.shader = rgb;
                            }
                        }
                    }
                    break;

                case BossStates.chargeUp:
                    chargeTimer += Time.deltaTime;
                    if (chargeTimer >= chargeThreshold)
                    {
                        chargeTimer = 0.0f;
                        chargeLoc = player.transform.position;
                        states = BossStates.dashing;
                        slimeBoseAnimation.SetBool("isChargeUp", false);
                        slimeBoseAnimation.SetBool("isDash", true);
                        dashAttackTime = 0;
                    }
                    break;

                case BossStates.dashing:
                    dashAttackTime += Time.deltaTime;
                    if (dashAttackTime >= 0.5f && (isCollidingWithThing || Vector3.Distance(transform.position, chargeLoc) < 0.1f))
                    {
                        states = BossStates.idle;
                        print("heeeeeeee");
                        timeSinceLastMove = 0;

                        slimeBoseAnimation.SetBool("isDash", false);
                        render.material.shader = origin;
                    }
                    else
                    {
                        transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * chargeSpeed;
                        chargeTimer += Time.deltaTime;
                    }

                    break;

                case BossStates.spawning:
                    if (spawnSlimes == true)
                    {
                        SpawnEnemy();
                        spawnSlimes = false;
                    }
                    timeSinceLastMove = 0;
                    states = BossStates.idle;

                    break;
            }
        }
        else
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
    }

    void SpawnEnemy()
    {

        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        for (int i = 0; i < numberOfSpawns; i++)
        {
            int breakLoop = 4;
            while (breakLoop > 0)
            {
                GameObject spawnPoint = SpawnPoint();

                if (Mathf.Abs(spawnPoint.transform.position.x - playerPos.x) > 3 && Mathf.Abs(spawnPoint.transform.position.y - playerPos.y) > 3)
                {
                    GameObject newEnemy = Instantiate(slimes, spawnPoint.transform.position, Quaternion.identity);
                    break;
                }
                else
                {
                    breakLoop--;
                }
            }
        }
    }

    GameObject SpawnPoint()
    {
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        return spawnPoint;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Tile")
        {
            isCollidingWithThing = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.tag.ToLower().Contains("enemy"))
        {
            print("clide????");
            isCollidingWithThing = true;           
        }

        if (other.gameObject.tag == "Player")
        {
            if(isCollidingWithPlayer == false && !testList.Contains(other.gameObject))
            {
                isCollidingWithPlayer = true;
                player.GetComponent<Player>().TakeDamage(30);
                Debug.Log("Give The Damage");
                testList.Add(other.gameObject);
                damageTimer = 0;
            }
            isCollidingWithThing = true;
        }

        if(other.gameObject.tag == "Player Bullet")
        {
            Instantiate(collideEffect, other.transform.position, other.transform.rotation);
            TakeDamage(other.gameObject.GetComponent<Bullet>().damageAmount);
            if (health < 0)
            {
                health = 0;
            }

            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            isCollidingWithThing = false;
        }
        if(other.gameObject.tag == "Player")
        {
            //isCollidingWithPlayer = false;
            testList.Remove(other.gameObject);
        }
    }   

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            BossDeath();
        }
    }

    void BossDeath()
    {
        slimeBoseAnimation.SetBool("isDead", true);
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        healthBar.DestroyBar();
        waitTime = dieTime;
        isDead = true;
        //Destroy(this.gameObject);
    }
}
