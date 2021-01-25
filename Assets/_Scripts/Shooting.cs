using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject basicBullet;

    public GameObject manager;

    Vector2 mousePos;

    public float bulletSpeed = 20f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
 
    void Shoot()
    {
        
        GameObject bullet = Instantiate(basicBullet, firePoint.position, firePoint.rotation);

        //bullet.transform.Translate(mousePos * bulletSpeed * Time.deltaTime);

        //Debug.Log("POS: " + mousePos);
        //Rigidbody2D rb = basicBullet.GetComponent<Rigidbody2D>();
        //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        //replace with transforms
    }
}
