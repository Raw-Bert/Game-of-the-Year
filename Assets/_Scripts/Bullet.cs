using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [EventRef]
    public string enemyHitSFX;
    GameObject manager;
    Vector2 shootDir;
    public GameObject mainCamera;
    public GameObject collideEffect;
    public float lifeTime = 5f;
    float timer;

    public float speed = 5.0f;
    public int damageAmount = 20;

    EventInstance fireSFXEvent;
    void Awake()
    {
        //manager = GameObject.Find("LevelController");
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mainCamera = GameObject.FindWithTag("MainCamera");
        //shootDir = mousePos;
        shootDir = this.transform.right;
        //shootDir = mainCamera.GetComponent<CameraOffset>().CaptureMousePos();
        shootDir.Normalize();

        fireSFXEvent = RuntimeManager.CreateInstance(enemyHitSFX);
        fireSFXEvent.start();
        fireSFXEvent.setPaused(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
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
        if (col.gameObject.tag == "Tile" || col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().TakeDamage(damageAmount);
                fireSFXEvent.setPaused(false);
                fireSFXEvent.release();
            }

            Instantiate(collideEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

}