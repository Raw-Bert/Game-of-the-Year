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
    public float speed = 2, chargeTimer = 0, cooldownTimer = 0;
    bool hasCharged = false;
    bool isCollidingWithPlayer = false;

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
                RangedMove();
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
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= 5)
            {
                hasCharged = false;
                cooldownTimer = 0;
                chargeTimer = 0;
                chargeLoc = player;
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
    void SlowMove() {}
    void RangedMove() {}

}