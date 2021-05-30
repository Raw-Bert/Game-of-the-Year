using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    private Animator animator;
    private Animator gunAnimator;

    public GameObject basicBullet;
    public GameObject muzzleFlash;
    public GameObject gun;
    Renderer gunRender;

    [EventRef] public List<string> fireSFX = new List<string>();
    EventInstance lazerSFX;

    public GameObject manager;

    float timeSinceLastShot = 10.0f;

    public float machineGunTime = 0.1f;
    public float plasmaRifleTime = 0.4f;
    public float shotGunTime = 1.2f;
    public float sniperTime = 1.8f;

    bool gunImageShowing = false;
    float gunImageTimer = 0;

    public enum Guns
    {
        plasmaRifle, //Default semi-auto gun
        remorse, //Machine gun named remorse? idk
        ravager, //Shot gun called the ravager
        deathsWhisper, //Sniper rifle
        soulburn //Laser gun

    }

    public Guns equippedGun;

    public float bulletSpeed = 20f;

    public SpriteRenderer spriteR;
    public Sprite[] sprites;
    
    public Image weaponUI;
    public Sprite[] UIWeapons;

    int damage = 20;

    bool remorsePickUp = false;
    bool ravagerPickUp = false;
    bool deathsWhisperPickUp = false;
    bool soulBurnPickUp = false;
    GameObject gunDrop;

    public Image pickUpImage;


    /// - Laser Specifc Variables - ///
    public LineRenderer lineRenderer;
    float laserDamageTimer = 0;
    public float laserDamageThreshold = 0.15f;
    public ParticleSystem laserParticles;


    void Start()
    {
        lazerSFX = FMODUnity.RuntimeManager.CreateInstance(fireSFX[2]);

        animator = muzzleFlash.GetComponent<Animator>();
        gunAnimator = gun.GetComponent<Animator>();
        gunRender = gun.GetComponent<Renderer>();

        pickUpImage.gameObject.SetActive(false);
        laserParticles.Pause();
        StopLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<Player>().death)
        {
            if (gunImageShowing == true)
            {
                gunImageTimer += Time.deltaTime;
                if (gunImageTimer >= 3.0f)
                {
                    gunImageShowing = false;
                    pickUpImage.gameObject.SetActive(false);
                    gunImageTimer = 0;
                }
            }
            if (equippedGun != Guns.soulburn)
            {
                StopLaser();
            }
            timeSinceLastShot += Time.deltaTime;

            //States for each gun, changes the behaviour of the gun
            switch (equippedGun)
            {
                //Semi-Auto plasma rifle. Default gun. Good all around weapon. Medium bullet size. Medium fire rate
                case Guns.plasmaRifle:
                    if (Input.GetMouseButton(0) && timeSinceLastShot > plasmaRifleTime)
                    {
                        RuntimeManager.PlayOneShot(fireSFX[0]);
                        Shoot(1.5f, 0.1f, 1, 5.0f);
                    }
                    //if (Input.GetKeyDown(KeyCode.E))
                    //{
                    //    equippedGun = Guns.remorse;
                    //    SwitchGun(1, 6);
                    //}
                    //if (Input.GetKeyDown(KeyCode.Q))
                    //{
                    //    equippedGun = Guns.deathsWhisper;
                    //    SwitchGun(3, 60);
                    //}
                    break;

                    //Remorse: Fast firing, low damage-per-shot machine gun. Small bullet size
                case Guns.remorse:
                    if (Input.GetMouseButton(0) && timeSinceLastShot > machineGunTime)
                    {
                        RuntimeManager.PlayOneShot(fireSFX[1]);

                        Shoot(0.9f, 0.08f, 1, 3.0f);
                        //Debug.Log("Remorsful Shot");
                    }
                    //if (Input.GetKeyDown(KeyCode.Q))
                    //{
                    //    equippedGun = Guns.plasmaRifle;
                    //    SwitchGun(0, 20);
                    //}
                    //if (Input.GetKeyDown(KeyCode.E))
                    //{
                    //    equippedGun = Guns.ravager;
                    //    SwitchGun(2, 30);
                    //}
                    break;

                    //Ravager: slow shooting shotgun, shoots 5 medium bullets per shot, high damage, low bullet lifetime    
                case Guns.ravager:
                    if (Input.GetMouseButton(0) && timeSinceLastShot > shotGunTime)
                    {
                        RuntimeManager.PlayOneShot(fireSFX[0]);

                        Shoot(2.6f, 0.5f, 5, 0.5f);
                        //Debug.Log("Ravaging Shot");
                    }
                    //if (Input.GetKeyDown(KeyCode.E))
                    //{
                    //    equippedGun = Guns.deathsWhisper;
                    //    SwitchGun(3, 60);
                    //}
                    //if (Input.GetKeyDown(KeyCode.Q))
                    //{
                    //    equippedGun = Guns.remorse;
                    //    SwitchGun(1, 6);
                    //}
                    break;

                case Guns.deathsWhisper:
                    if (Input.GetMouseButton(0) && timeSinceLastShot > sniperTime)
                    {
                        RuntimeManager.PlayOneShot(fireSFX[0]);

                        Shoot(2.0f, 0.4f, 1, 8.0f);
                        Debug.Log("Death Whisper Shot");
                    }
                    //if (Input.GetKeyDown(KeyCode.E))
                    //{
                    //    equippedGun = Guns.plasmaRifle;
                    //    SwitchGun(0, 20);
                    //}
                    //if (Input.GetKeyDown(KeyCode.Q))
                    //{
                    //    equippedGun = Guns.ravager;
                    //    SwitchGun(2, 30);
                    //}
                    break;

                case Guns.soulburn:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        ShootLaser();
                    }

                    if (Input.GetButton("Fire1"))
                    {
                        UpdateLaser();
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        StopLaser();
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
            }

            if (animator.GetBool("hasShot") == false)
            {
                muzzleFlash.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (remorsePickUp == true && equippedGun != Guns.remorse)
                {
                    equippedGun = Guns.remorse;
                    SwitchGun(1, 6, "Remorse");
                    Destroy(gunDrop);
                }
                if (ravagerPickUp == true && equippedGun != Guns.ravager)
                {
                    equippedGun = Guns.ravager;
                    SwitchGun(2, 30, "The Ravager");
                    Destroy(gunDrop);
                }
                if (deathsWhisperPickUp == true && equippedGun != Guns.deathsWhisper)
                {
                    equippedGun = Guns.deathsWhisper;
                    SwitchGun(3, 60, "Deaths Whisper");
                    Destroy(gunDrop);
                }
                if (soulBurnPickUp == true && equippedGun != Guns.soulburn)
                {
                    equippedGun = Guns.soulburn;
                    SwitchGun(4, 10, "Soul Burn");
                    Destroy(gunDrop);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {      
        if(other.gameObject.tag == "RemorseDrop")
        {
            gunDrop = other.gameObject;
            remorsePickUp = true;
        }
        if(other.gameObject.tag == "RavagerDrop")
        {
            gunDrop = other.gameObject;
            ravagerPickUp = true;
        }
        if(other.gameObject.tag == "WhisperDrop")
        {
            deathsWhisperPickUp = true;
            gunDrop = other.gameObject;
        }
        if(other.gameObject.tag == "SoulburnDrop")
        {
            soulBurnPickUp = true;
            gunDrop = other.gameObject;
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {        
        if(other.gameObject.tag == "RemorseDrop")
        {
            remorsePickUp = false;
        }
        if(other.gameObject.tag == "RavagerDrop")
        {
            ravagerPickUp = false;
        }
        if(other.gameObject.tag == "WhisperDrop")
        {
            deathsWhisperPickUp = false;
        }
        if(other.gameObject.tag == "SoulburnDrop")
        {
            soulBurnPickUp = false;
        }
        
    }

    void ShootLaser()
    {
        PLAYBACK_STATE sTATE;
        lazerSFX.getPlaybackState(out sTATE);
        if (sTATE == PLAYBACK_STATE.STOPPED)
            lazerSFX.start();

        lineRenderer.enabled = true;
        Camera.main.GetComponent<CameraOffset>().laserShooting = true;
        laserParticles.Play();
    }

    void UpdateLaser()
    {
        laserDamageTimer += Time.deltaTime;
        var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 endPos;
        Vector2 tempPos = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 dir = mousePos - tempPos;
        lineRenderer.SetPosition(0, tempPos);
        //float dist = Mathf.Clamp(Vector2.Distance(tempPos, mousePos), 0.1f, 8);
        float dist = 8;
        endPos = tempPos + (dir.normalized * dist);
        lineRenderer.SetPosition(1, endPos);

        
        
        RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.transform.position, dir.normalized, 8);
        if(hit)
        {
            lineRenderer.SetPosition (1, hit.point);
            if(hit.transform.gameObject.tag == "Enemy" && laserDamageTimer >= laserDamageThreshold)
            {
                hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                laserDamageTimer = 0;
            }
            
            else if( hit.transform.gameObject.tag == "Boss" && laserDamageTimer >= laserDamageThreshold)
            {
                hit.transform.gameObject.GetComponent<BossSlime>().TakeDamage(damage);
                laserDamageTimer = 0;
            }
            else if(hit.transform.gameObject.tag == "Box" && laserDamageTimer >= laserDamageThreshold)
            {
                hit.transform.gameObject.GetComponent<Boxes>().DamageBox(damage);
            }
        }
    }

    void StopLaser()
    {
        PLAYBACK_STATE sTATE;
        lazerSFX.getPlaybackState(out sTATE);
        if (sTATE == PLAYBACK_STATE.PLAYING)
            lazerSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        lineRenderer.enabled = false;
        Camera.main.GetComponent<CameraOffset>().laserShooting = false;
        laserParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
    void SwitchGun(int spriteVersion, int damageAmount, string weaponName)
    {
        spriteR.sprite = sprites[spriteVersion];

        weaponUI.sprite = UIWeapons[spriteVersion];

        gunImageShowing = true;
        pickUpImage.gameObject.SetActive(true);

        GameObject textObj = pickUpImage.transform.GetChild(0).gameObject;
        textObj.GetComponent<TextMeshProUGUI>().text = "\\" + weaponName + "\\ Equipped";

        GameObject gunImage = pickUpImage.transform.GetChild(1).gameObject;
        gunImage.GetComponent<Image>().sprite = UIWeapons[spriteVersion];

        


        damage = damageAmount;
    }

    private void OnDestroy()
    {
        lazerSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lazerSFX.release();
        lazerSFX.clearHandle();

    }
}