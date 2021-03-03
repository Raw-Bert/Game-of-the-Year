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
                switch (enemy.GetComponent<EnemyAI>().movementType)
                {
                    case EnemyAI.EnemyType.Ranged:
                        ++player.GetComponent<AudioSwapper>().amountOfEnemyType[0];
                        print("Enemy On Screen P1: " + player.GetComponent<AudioSwapper>().amountOfEnemyType[0]);
                        break;

                    case EnemyAI.EnemyType.Charge:
                        ++player.GetComponent<AudioSwapper>().amountOfEnemyType[1];
                        print("Enemy On Screen P2: " + player.GetComponent<AudioSwapper>().amountOfEnemyType[1]);
                        break;
                }

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
                switch (enemy.GetComponent<EnemyAI>().movementType)
                {
                    case EnemyAI.EnemyType.Ranged:
                        --player.GetComponent<AudioSwapper>().amountOfEnemyType[0];
                        print("Enemy Off Screen P1: " + player.GetComponent<AudioSwapper>().amountOfEnemyType[0]);
                        break;

                    case EnemyAI.EnemyType.Charge:
                        --player.GetComponent<AudioSwapper>().amountOfEnemyType[1];
                        print("Enemy Off Screen P2: " + player.GetComponent<AudioSwapper>().amountOfEnemyType[2]);
                        break;
                }

                var amountEnemy = player.GetComponent<AudioSwapper>().amountOfEnemyType;
                for (int a = 0; a < amountEnemy.Count; ++a)
                    if (amountEnemy[a] < 0)
                        amountEnemy[a] = 0;
            }
    }
}