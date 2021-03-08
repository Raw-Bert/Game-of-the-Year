using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=_Z1t7MNk0c4&ab_channel=Blackthornprod
public class EnemyProjectile : MonoBehaviour
{
    public float speed;

    Transform player;
    Vector2 target;

    public float lifeTime = 4f;
    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        Vector3 playerDir = player.position - this.transform.position;
        target = new Vector2(playerDir.x, playerDir.y);
        target.Normalize();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            this.transform.position += new Vector3(target.x * speed * Time.deltaTime, target.y * speed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tile" || collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
