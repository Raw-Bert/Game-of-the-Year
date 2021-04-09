using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyType
    {
        Static,
        Charge,
        Slowmoving,
        Ranged
    }

    public EnemyType movementType;
    public float speed = 2, chargeTimer = 0, cooldownTimer = 5, chargeDistance = 5, detectDistance = 8;
    bool hasCharged = true;
    bool isCollidingWithPlayer = false;
    List<Vector3> patrollingPoints;
    Vector3 patrollingPoint;
    bool patrolPointSet = false;

    public float stopDis;
    public float backDis;
    public float timeBtwAttack;
    float timeBtwAttackUpdate;
    public GameObject projectile;

    NavMeshAgent agent;
    bool isAgentEnable = true;

    Animator enemyAnimator;

    public GameObject rangeEnemyArm;
    public GameObject firePoint;
    public GameObject armLeftPos;
    public GameObject armRightPos;

    //public Animator muzzleAnimator;
    //public GameObject muzzleFlash;
    
    

    private void Start()
    {
        enemyAnimator = this.GetComponent<Animator>();

        timeBtwAttackUpdate = timeBtwAttack;

        // nav mesh reset for 2D
        if (GameObject.FindGameObjectWithTag("NavMesh") != null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        else
        {
            isAgentEnable = false;
        }

        // assign patrolling points
        patrollingPoints = new List<Vector3>();
        GameObject[] pp = GameObject.FindGameObjectsWithTag("spawnPoint");
        for(int i = 0; i < pp.Length; i++)
        {
            patrollingPoints.Add(pp[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if (!agent.isOnNavMesh)
        //{
        //    isAgentEnable = false;
        //    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //}
        //else
        //{
        //    this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //    isAgentEnable = true;
        //    print(isAgentEnable);
        //}

        Vector3 playerPos = Vector3.zero;
        if (Transform.FindObjectOfType<PlayerMovement>())
            playerPos = Transform.FindObjectOfType<PlayerMovement>().gameObject.transform.position;
        switch (movementType)
        {
            case EnemyType.Charge:
                ChargeMove(playerPos);
                break;
            case EnemyType.Slowmoving:
                SlowMove(playerPos);
                break;
            case EnemyType.Ranged:
                RangedMove(playerPos);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.ToLower() == "player")
            isCollidingWithPlayer = true;
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.ToLower() == "player")
            isCollidingWithPlayer = false;
  
    }

    Vector3 chargeLoc;
    void ChargeMove(Vector3 playerPos)
    {
        if (hasCharged)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= 5 && (Vector3.Distance(transform.position, playerPos) < chargeDistance))
            {
                enemyAnimator.SetBool("isMoving", false);
                enemyAnimator.SetBool("isJump", true);
                enemyAnimator.SetBool("isAngry", false);

                patrolPointSet = false;
                agent.isStopped = true;
                agent.ResetPath();

                Vector2 direct = new Vector2(playerPos.x, playerPos.y) - new Vector2(transform.position.x, transform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direct.normalized);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        hasCharged = false;
                        cooldownTimer = 0;
                        chargeTimer = 0;
                        chargeLoc = playerPos;
                    }
                }
            }
            else if (Vector3.Distance(transform.position, playerPos) > detectDistance)
            {
                enemyAnimator.SetBool("isMoving", true);
                enemyAnimator.SetBool("isJump", false);
                enemyAnimator.SetBool("isAngry", false);

                if (!patrolPointSet)
                {
                    int random = Random.Range(0, patrollingPoints.Count);
                    patrollingPoint = patrollingPoints[random];
                    patrolPointSet = true;
                    agent.isStopped = false;
                }
                else
                {
                    agent.SetDestination(patrollingPoint);
                }

                Vector3 distance = this.transform.position - patrollingPoint;

                if (distance.magnitude < 1f)
                    patrolPointSet = false;
            }
            else
            {
                patrolPointSet = false;
                agent.isStopped = true;
                agent.ResetPath();
                enemyAnimator.SetBool("isMoving", false);
                enemyAnimator.SetBool("isJump", false);
                enemyAnimator.SetBool("isAngry", true);
            }
        }

        if (!hasCharged)
        {
            enemyAnimator.SetBool("isMoving", false);
            enemyAnimator.SetBool("isJump", false);
            enemyAnimator.SetBool("isAngry", true);

            if (chargeTimer >= 2 || isCollidingWithPlayer)
                hasCharged = true;
            else
                transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * speed;
            chargeTimer += Time.deltaTime;
        }
    }

    void SlowMove(Vector3 playerPos)
    {
        if (isAgentEnable)
        {
            agent.SetDestination(playerPos);
        }
        else
        {
            transform.position += (playerPos - transform.position).normalized * Time.deltaTime * speed / 4;
        }
    }

    void RangedMove(Vector3 playerPos)
    {
        // Arm Movement
        Vector3 dir = playerPos - rangeEnemyArm.transform.position;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(ang + 90, Vector3.forward);
        rangeEnemyArm.transform.rotation = Quaternion.Slerp(rangeEnemyArm.transform.rotation, q, Time.deltaTime * 10);

        // Movement
        if (Vector2.Distance(transform.position, playerPos) > stopDis)
        {
            enemyAnimator.enabled = true;
            if (isAgentEnable)
            {
                agent.isStopped = false;
                agent.SetDestination(playerPos);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            }
        }
        else if (Vector2.Distance(transform.position, playerPos) < backDis)
        {
            enemyAnimator.enabled = true;
            if (isAgentEnable)
            {
                agent.isStopped = false;
                agent.SetDestination(this.transform.position - (playerPos - this.transform.position));
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos, -speed * Time.deltaTime);
            }
        }
        else if (Vector2.Distance(transform.position, playerPos) <= stopDis && Vector2.Distance(transform.position, playerPos) >= backDis)
        {
            enemyAnimator.enabled = false;
            //transform.position = this.transform.position;
            if (isAgentEnable)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            else
            {
                transform.position = this.transform.position;
            }
        }

        // Attack
        if (timeBtwAttackUpdate <= 0)
        {
            //muzzleFlash.SetActive(true);
            //muzzleAnimator.SetBool("hasShot", true);
            Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
            timeBtwAttackUpdate = timeBtwAttack;
        }
        else
        {
            timeBtwAttackUpdate -= Time.deltaTime;
        }

        //if (muzzleAnimator.GetBool("hasShot") == false)
        //{
            //muzzleFlash.SetActive(false);
        //}

        // Sprite direction and arm position
        if(dir.x < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            rangeEnemyArm.transform.position = armRightPos.transform.position;
        }
        else if(dir.x >= 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            rangeEnemyArm.transform.position = armLeftPos.transform.position;
        }
    }

}