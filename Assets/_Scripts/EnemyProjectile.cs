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


        float rot_z = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

        //Quaternion.LookRotation(new Vector3(0,0, target.x));
        this.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
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

   //private void OnCollisionEnter2D(Collision2D collision)

   //{

   //    if (collision.gameObject.tag == "Tile" || collision.gameObject.tag == "Player")
   //    {
   //        Destroy(gameObject);
   //    }
   //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.gameObject.tag == "Tile" || other.gameObject.tag == "Player")
            {
                Destroy(this.gameObject);
            }
        }
    }

}
