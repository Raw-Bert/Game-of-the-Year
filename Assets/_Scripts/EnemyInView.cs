using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInView : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject enemy = other.gameObject;
        if (enemy)
            if (enemy.tag.ToLower().Contains("enemy"))
            {
                if (enemy.tag.ToLower().Contains("projectile"))return;
                if (enemy.tag.ToLower().Contains("bullet"))return;

                enemy.GetComponent<Enemy>().onCam = true;
                increaseAmoutOfEnemyType(enemy);
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        GameObject enemy = other.gameObject;
        if (enemy)
            if (enemy.tag.ToLower().Contains("enemy"))
            {
                if (enemy.tag.ToLower().Contains("projectile"))return;
                if (enemy.tag.ToLower().Contains("bullet"))return;

                enemy.GetComponent<Enemy>().onCam = false;
                decreaseAmountOfEnemyType(enemy);
            }
    }

    public void increaseAmoutOfEnemyType(GameObject enemy)
    {
      //  Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), true);

        switch (enemy.GetComponent<EnemyAI>().movementType)
        {
            case EnemyAI.EnemyType.Slowmoving:
            case EnemyAI.EnemyType.Ranged:
                ++player.GetComponent<AudioSwapper>().amountOfEnemyType[0];
                print("Enemy type 1:" + player.GetComponent<AudioSwapper>().amountOfEnemyType[0]);
                break;

            case EnemyAI.EnemyType.Charge:
                ++player.GetComponent<AudioSwapper>().amountOfEnemyType[1];
                print("Enemy type 2:" + player.GetComponent<AudioSwapper>().amountOfEnemyType[1]);
                break;
        }
    }
    public void decreaseAmountOfEnemyType(GameObject enemy)
    {
     //   Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), true);

        switch (enemy.GetComponent<EnemyAI>().movementType)
        {
            case EnemyAI.EnemyType.Slowmoving:
            case EnemyAI.EnemyType.Ranged:
                --player.GetComponent<AudioSwapper>().amountOfEnemyType[0];
                print("Enemy type 1:" + player.GetComponent<AudioSwapper>().amountOfEnemyType[0]);
                break;

            case EnemyAI.EnemyType.Charge:
                --player.GetComponent<AudioSwapper>().amountOfEnemyType[1];
                print("Enemy type 2:" + player.GetComponent<AudioSwapper>().amountOfEnemyType[1]);
                break;
        }

        var amountEnemy = player.GetComponent<AudioSwapper>().amountOfEnemyType;
        for (int a = 0; a < amountEnemy.Count; ++a)
            if (amountEnemy[a] < 0)
                amountEnemy[a] = 0;
    }
}