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
    bool isCollidingWithPlayer = false;

    Vector3 chargeLoc;
    public float chargeSpeed = 5;

    public List<GameObject> spawnPoints;
    public GameObject slimes;
    public int numberOfSpawns = 3;

    public bool aggro = false;

    public GameObject collideEffect;

    public enum BossStates
    {
        idle,   //idle
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
    // Update is called once per frame
    void Update()
    {   
        if(health < maxHealth / 2 && enrage == false)
        {
            enrage = true;
            timeBetweenMoves = 5.0f;
            numberOfSpawns += 2;
        }

        

        switch (states)
        {
            case BossStates.idle:
                //TODO: Make slime do idle animation
                if(aggro){
                    timeSinceLastMove += Time.deltaTime;

                    if(timeSinceLastMove >= timeBetweenMoves)
                    {
                        int selectMove = Random.Range(1,4);
                        Debug.Log("SELECTED MOVE: " + selectMove);
                        if(selectMove == 1 || selectMove == 2)
                        {
                            states = BossStates.chargeUp;
                        }
                        else
                        {
                            states = BossStates.spawning;
                            spawnSlimes = true;
                        }
                    }
                }
                break;

            case BossStates.chargeUp:
                //TODO: Make slime do charge up animation

                chargeTimer += Time.deltaTime;
                if(chargeTimer >= chargeThreshold)
                {
                    chargeTimer = 0.0f;
                    chargeLoc = player.transform.position;
                    states = BossStates.dashing;
                }
                break;

            case BossStates.dashing:
                //TODO: Make slime do dash animation

                if (dashAttackTime >= 2 || isCollidingWithPlayer || transform.position == chargeLoc)
                {
                    timeSinceLastMove = 0;
                    states = BossStates.idle;
                }
                else
                {
                    transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * chargeSpeed;
                    chargeTimer += Time.deltaTime;
                }

                break;

            case BossStates.spawning:
                if(spawnSlimes == true)
                {
                    SpawnEnemy();
                    spawnSlimes = false;

                }
                //Maybe play an animation in here somewhere, and play a spawning animation for the slimes?
                
                
                timeSinceLastMove = 0;
                //spawnSlimes = false;
                states = BossStates.idle;

                break;
        }
    }

    void SpawnEnemy()
    {
        
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        for(int i = 0; i < numberOfSpawns; i++){
            int breakLoop = 4;
            while(breakLoop > 0)
            {
                GameObject spawnPoint = SpawnPoint();

                if(Mathf.Abs(spawnPoint.transform.position.x - playerPos.x) > 3 && Mathf.Abs(spawnPoint.transform.position.y - playerPos.y) > 3)
                {
                    GameObject newEnemy = Instantiate(slimes, spawnPoint.transform.position, Quaternion.identity);
                    Debug.Log("Spawn a slime");
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


    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("SLIIIIME");
        if(other.gameObject.tag == "Player")
        {
            isCollidingWithPlayer = true;
            player.GetComponent<Player>().TakeDamage(30);
            //TODO: Deal damage to player here
        }
        if(other.gameObject.tag == "Player Bullet")
        {
            Instantiate(collideEffect, other.transform.position, other.transform.rotation);
            TakeDamage(other.gameObject.GetComponent<Bullet>().damageAmount);
            if(health < 0)
            {
                health = 0;
            }
            
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isCollidingWithPlayer = false;
        }
    }   

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if(health <= 0)
        {
            BossDeath();
        }
    }

    void BossDeath()
    {
        //Have a cool animation and stuff here!
        Destroy(this.gameObject);
    }
}


//   void ChargeMove(Vector3 playerPos)
//   {
//       if (hasCharged)
//       {
//           cooldownTimer += Time.deltaTime;
//           if (cooldownTimer >= 5 && (Vector3.Distance(transform.position, playerPos) < chargeDistance))
//           {
//               enemyAnimator.SetBool("isMoving", false);
//               enemyAnimator.SetBool("isJump", true);
//               enemyAnimator.SetBool("isAngry", false);
//
//               patrolPointSet = false;
//               agent.isStopped = true;
//               agent.ResetPath();
//
//               Vector2 direct = new Vector2(playerPos.x, playerPos.y) - new Vector2(transform.position.x, transform.position.y);
//               RaycastHit2D hit = Physics2D.Raycast(transform.position, direct.normalized);
//
//               if (hit.collider != null)
//               {
//                   if (hit.collider.gameObject.tag == "Player")
//                   {
//                       hasCharged = false;
//                       cooldownTimer = 0;
//                       chargeTimer = 0;
//                       chargeLoc = playerPos;
//                   }
//               }
//           }
//           else if (Vector3.Distance(transform.position, playerPos) > detectDistance)
//           {
//               enemyAnimator.SetBool("isMoving", true);
//               enemyAnimator.SetBool("isJump", false);
//               enemyAnimator.SetBool("isAngry", false);
//
//               if (!patrolPointSet)
//               {
//                   int random = Random.Range(0, patrollingPoints.Count);
//                   patrollingPoint = patrollingPoints[random];
//                   patrolPointSet = true;
//                   agent.isStopped = false;
//               }
//               else
//               {
//                   agent.SetDestination(patrollingPoint);
//               }
//
//               Vector3 distance = this.transform.position - patrollingPoint;
//
//               if (distance.magnitude < 1f)
//                   patrolPointSet = false;
//           }
//           else
//           {
//               patrolPointSet = false;
//               agent.isStopped = true;
//               agent.ResetPath();
//               enemyAnimator.SetBool("isMoving", false);
//               enemyAnimator.SetBool("isJump", false);
//               enemyAnimator.SetBool("isAngry", true);
//           }
//       }
//
//       if (!hasCharged)
//       {
//           enemyAnimator.SetBool("isMoving", false);
//           enemyAnimator.SetBool("isJump", false);
//           enemyAnimator.SetBool("isAngry", true);
//
//           if (chargeTimer >= 2 || isCollidingWithPlayer)
//               hasCharged = true;
//           else
//               transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * speed;
//           chargeTimer += Time.deltaTime;
//       }
//   }
//
