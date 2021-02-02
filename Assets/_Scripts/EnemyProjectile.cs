using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=_Z1t7MNk0c4&ab_channel=Blackthornprod
public class EnemyProjectile : MonoBehaviour
{
    public float speed;

    Transform player;
    Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // destroy when arrive the target position
        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // destroy when hit player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
