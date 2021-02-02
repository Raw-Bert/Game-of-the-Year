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
    public float speed = 2, chargeTimer = 0, cooldownTimer = 0;
    bool hasCharged = false;
    bool isCollidingWithPlayer = false;

    public float stopDis;
    public float backDis;
    public float timeBtwAttack;
    float timeBtwAttackUpdate;
    public GameObject projectile;

    NavMeshAgent agent;
    bool isAgentEnable = true;

    private void Start()
    {
        timeBtwAttackUpdate = timeBtwAttack;

        // nav mesh reset for 2D
        if (GameObject.Find("NavMesh") != null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        else
        {
            isAgentEnable = false;
            Debug.Log("Nave Mesh is not exist!");
        }

    }

    // Update is called once per frame
    void Update()
    {
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
        print("hit player");
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.ToLower() == "player")
            isCollidingWithPlayer = false;
        print("exit player");

    }

    Vector3 chargeLoc;
    void ChargeMove(Vector3 playerPos)
    {
        if (hasCharged)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= 5)
            {
                hasCharged = false;
                cooldownTimer = 0;
                chargeTimer = 0;
                chargeLoc = playerPos;
            }
        }

        if (!hasCharged)
        {
            if (!isCollidingWithPlayer)
                transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * speed;

            if (chargeTimer >= 2)
                hasCharged = true;
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
        // Movement
        if (Vector2.Distance(transform.position, playerPos) > stopDis)
        {
            if (isAgentEnable)
            {
                agent.SetDestination(playerPos);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            }
        }
        else if (Vector2.Distance(transform.position, playerPos) < stopDis && Vector2.Distance(transform.position, playerPos) > backDis)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, playerPos) < backDis)
        {
            if (isAgentEnable)
            {
                agent.SetDestination(-playerPos);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos, -speed * Time.deltaTime);
            }
        }

        // Attack
        if (timeBtwAttackUpdate <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwAttackUpdate = timeBtwAttack;
        }
        else
        {
            timeBtwAttackUpdate -= Time.deltaTime;
        }
    }

}