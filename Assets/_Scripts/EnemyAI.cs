using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float speed = 2, chargeTimmer = 0, cooldownTimmer = 0;
    bool hasCharged = false;
    bool isCollidingWithPlayer = false;
    
    public float stopDis;
    public float backDis;
    public float timeBtwAttack;
    float timeBtwAttackUpdate;
    public GameObject projectile;

    private void Start()
    {
        timeBtwAttackUpdate = timeBtwAttack;
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
                SlowMove();
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
    void ChargeMove(Vector3 player)
    {
        if (hasCharged)
        {
            cooldownTimmer += Time.deltaTime;
            if (cooldownTimmer >= 5)
            {
                hasCharged = false;
                cooldownTimmer = 0;
                chargeTimmer = 0;
                chargeLoc = player;
            }
        }

        if (!hasCharged)
        {
            if (!isCollidingWithPlayer)
                transform.position += (chargeLoc - transform.position).normalized * Time.deltaTime * speed;

            if (chargeTimmer >= 2)
                hasCharged = true;
            chargeTimmer += Time.deltaTime;
        }
    }

    void SlowMove() {}

    void RangedMove(Vector3 player) {
        // Movement
        if(Vector2.Distance(transform.position, player) > stopDis)
        {
            transform.position = Vector2.MoveTowards(transform.position, player, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player) < stopDis && Vector2.Distance(transform.position, player) > backDis)
        {
            transform.position = this.transform.position;
        }
        else if(Vector2.Distance(transform.position, player) < backDis)
        {
            transform.position = Vector2.MoveTowards(transform.position, player, -speed * Time.deltaTime);
        }

        // Attack
        if(timeBtwAttackUpdate <= 0)
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