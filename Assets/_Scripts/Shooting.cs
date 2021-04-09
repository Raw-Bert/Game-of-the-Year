using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    private Animator animator;
    private Animator gunAnimator;

    public GameObject basicBullet;
    public GameObject muzzleFlash;
    public GameObject gun;
    Renderer gunRender;

    [EventRef] public string fire1SFX;

    public GameObject manager;

    float timeSinceLastShot = 10.0f;

    public float machineGunTime = 0.1f;
    public float plasmaRifleTime = 0.4f;
    public float shotGunTime = 1.2f;
    public float sniperTime = 1.2f;

    public enum Guns
    {
        plasmaRifle, //Default semi-auto gun
        remorse, //Machine gun named remorse? idk
        ravager, //Shot gun called the ravager
        deathsWhisper

    }

    public Guns equippedGun;

    Vector2 mousePos;

    public float bulletSpeed = 20f;

    public SpriteRenderer spriteR;
    public Sprite[] sprites;
    
    public Image weaponUI;
    public Sprite[] UIWeapons;

    int damage = 20;

    void Start()
    {
        animator = muzzleFlash.GetComponent<Animator>();
        gunAnimator = gun.GetComponent<Animator>();
        gunRender = gun.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        //States for each gun, changes the behaviour of the gun
        switch (equippedGun)
        {
            //Semi-Auto plasma rifle. Default gun. Good all around weapon. Medium bullet size. Medium fire rate
            case Guns.plasmaRifle:
                if (Input.GetMouseButton(0) && timeSinceLastShot > plasmaRifleTime)
                {
                    RuntimeManager.PlayOneShot(fire1SFX);
                    Shoot(1.5f, 0.1f, 1, 5.0f);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.remorse;
                    SwitchGun(1, 6);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    equippedGun = Guns.deathsWhisper;
                    SwitchGun(3, 60);
                }
                break;

                //Remorse: Fast firing, low damage-per-shot machine gun. Small bullet size
            case Guns.remorse:
                if (Input.GetMouseButton(0) && timeSinceLastShot > machineGunTime)
                {
                    RuntimeManager.PlayOneShot(fire1SFX);

                    Shoot(0.9f, 0.08f, 1, 3.0f);
                    Debug.Log("Remorsful Shot");
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    equippedGun = Guns.plasmaRifle;
                    SwitchGun(0, 20);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.ravager;
                    SwitchGun(2, 30);
                }
                break;

                //Ravager: slow shooting shotgun, shoots 5 medium bullets per shot, high damage, low bullet lifetime    
            case Guns.ravager:
                if (Input.GetMouseButton(0) && timeSinceLastShot > shotGunTime)
                {
                    RuntimeManager.PlayOneShot(fire1SFX);

                    Shoot(2.6f, 0.2f, 5, 0.5f);
                    Debug.Log("Ravaging Shot");
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.deathsWhisper;
                    SwitchGun(3, 60);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    equippedGun = Guns.remorse;
                    SwitchGun(1, 6);
                }
                break;

            case Guns.deathsWhisper:
                if (Input.GetMouseButton(0) && timeSinceLastShot > sniperTime)
                {
                    RuntimeManager.PlayOneShot(fire1SFX);

                    Shoot(2.0f, 0.3f, 1, 8.0f);
                    Debug.Log("Death Whisper Shot");
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    equippedGun = Guns.plasmaRifle;
                    SwitchGun(0, 20);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    equippedGun = Guns.ravager;
                    SwitchGun(2, 30);
                }
                break;

                //Gun names: 
                //Peacekeeper
                //Vanquisher
                //Early retirement
                //Amnesiac
                //Swan song
                //Cataclysm,
                //BFG
                //Sorrow's Whisper
        }

        if (animator.GetBool("hasShot") == false)
        {
            muzzleFlash.SetActive(false);
        }
    }

    void Shoot(float bulletScale, float recoilModifier, int bulletNumber, float lifeTime)
    {
        GameObject bullet = Instantiate(basicBullet, firePoint.position, firePoint.rotation);
        bullet.transform.localScale = new Vector3(bulletScale, bulletScale, 0.0f);

        bullet.GetComponent<Bullet>().damageAmount = damage;
        bullet.GetComponent<Bullet>().lifeTime = lifeTime;

        if (bulletNumber > 2)
        {
            for (int i = 0; i < ((bulletNumber - 1) / 2); i++)
            {
                GameObject bullet2 = Instantiate(basicBullet, firePoint.position, (firePoint.rotation) * Quaternion.Euler(0, 0, -45 / ((bulletNumber / 2)) / (i + 1)));
                bullet2.transform.localScale = new Vector3(bulletScale, bulletScale, 0.0f);

                bullet2.GetComponent<Bullet>().damageAmount = damage;
                bullet2.GetComponent<Bullet>().lifeTime = lifeTime;

                GameObject bullet3 = Instantiate(basicBullet, firePoint.position, (firePoint.rotation) * Quaternion.Euler(0, 0, 45 / ((bulletNumber / 2)) / (i + 1)));
                bullet3.transform.localScale = new Vector3(bulletScale, bulletScale, 0.0f);

                bullet3.GetComponent<Bullet>().damageAmount = damage;
                bullet3.GetComponent<Bullet>().lifeTime = lifeTime;
            }
        }

        gunAnimator.SetTrigger("Shoot");
        muzzleFlash.SetActive(true);
        animator.SetBool("hasShot", true);

        Camera.main.GetComponent<CameraOffset>().StartGunRecoil(0.05f, recoilModifier);

        timeSinceLastShot = 0.0f;
    }

    //WIP, changes gunsprite and any other attributes when gun state changed
    void SwitchGun(int spriteVersion, int damageAmount)
    {
        spriteR.sprite = sprites[spriteVersion];
        //gunRender.material.SetTexture("_Emissive", weaponEmissions[spriteVersion]);

        //spriteR.SecondarySpriteTexture = weaponEmissions[spriteVersion];// =  ("_EmissionMap", weaponEmissions[spriteVersion]);
        weaponUI.sprite = UIWeapons[spriteVersion];
        //spriteR.gameObject.material.SetTexture();
        //spriteR.SetEmissive(SetEmissive(renderer))

        damage = damageAmount;
    }
}