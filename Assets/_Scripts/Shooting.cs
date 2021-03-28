using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    private Animator animator;
    private Animator gunAnimator;

    public GameObject basicBullet;
    public GameObject muzzleFlash;
    public GameObject gun;

    public GameObject manager;

    float timeSinceLastShot = 10.0f;

    public float machineGunTime = 0.1f;
    public float plasmaRifleTime = 0.4f;

    public enum Guns
    {
        plasmaRifle, //Default semi-auto gun
        remorse //Machine gun named remorse? idk

    }

    public Guns equippedGun;

    Vector2 mousePos;

    public float bulletSpeed = 20f;

    public SpriteRenderer spriteR;
    public Sprite[] sprites;

    int damage = 20;
    
    void Start()
    {
        animator = muzzleFlash.GetComponent<Animator>();
        gunAnimator = gun.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        //States for each gun, changes the behaviour of the gun
        switch(equippedGun)
        {
            //Semi-Auto plasma rifle. Default gun. Good all around weapon. Medium bullet size. Medium fire rate
            case Guns.plasmaRifle:
                if(Input.GetButtonDown("Fire1") && timeSinceLastShot > plasmaRifleTime)
                {
                    Shoot(1.5f);
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.remorse;
                    SwitchGun(1, 8);
                }
                break;

            //Remorse: Fast firing, low damage-per-shot machine gun. Small bullet size
            case Guns.remorse:
                if(Input.GetMouseButton(0) && timeSinceLastShot > machineGunTime)
                {
                    Shoot(0.9f);
                    Debug.Log("Remorsful Shot");
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.plasmaRifle;
                    SwitchGun(0, 20);
                }
                break;
        }
        

        if (animator.GetBool("hasShot") == false)
        {
            muzzleFlash.SetActive(false);
        }
    }
 
    void Shoot(float bulletScale)
    {
        GameObject bullet = Instantiate(basicBullet, firePoint.position, firePoint.rotation);
        bullet.transform.localScale = new Vector3(bulletScale, bulletScale, 0.0f);

        bullet.GetComponent<Bullet>().damageAmount = damage;

        gunAnimator.SetTrigger("Shoot");
        muzzleFlash.SetActive(true);
        animator.SetBool("hasShot",true);

        Camera.main.GetComponent<CameraOffset>().StartGunRecoil(0.1f);

        timeSinceLastShot = 0.0f;
    }

    //WIP, changes gunsprite and any other attributes when gun state changed
    void SwitchGun(int spriteVersion, int damageAmount)
    {
        spriteR.sprite = sprites[spriteVersion];
        damage = damageAmount;
    }
}
