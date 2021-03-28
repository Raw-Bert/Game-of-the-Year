using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    GameObject manager;
    Vector2 shootDir;
    public GameObject mainCamera;
    public GameObject collideEffect;
    public float lifeTime = 4f;
    float timer;

    public float speed = 5.0f;
    public int damageAmount = 20;
    // Start is called before the first frame update

    void Awake()
    {
        //manager = GameObject.Find("LevelController");
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width/2;
        mousePos.y -= Screen.height/2;
        mainCamera = GameObject.FindWithTag("MainCamera");
        shootDir = mousePos;
        //shootDir = mainCamera.GetComponent<CameraOffset>().CaptureMousePos();
        shootDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            Destroy(this.gameObject);
        }
        else
        {            
            this.transform.position += new Vector3(shootDir.x * speed * Time.deltaTime, shootDir.y * speed * Time.deltaTime, this.transform.position.z);
        }
    }

    void OnCollisionEnter2D(Collision2D col) 
    {
        if(col.gameObject.tag == "Tile" || col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().TakeDamage(damageAmount);
            }
            //Debug.Log("Tile Collision");
            Instantiate(collideEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);

           
        }
    }

}
