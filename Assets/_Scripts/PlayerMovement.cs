﻿using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    public Camera cam;

    public GameObject weapon;
    public Rigidbody2D rbWeapon;

    public Vector2 weaponDir;
    [EventRef] public string moveSound;
    Vector2 movement;
    Vector2 mousePosition;
    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
    }

    float walkCount = 0;

    void Update()
    {
        if (!this.GetComponent<Player>().death)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            rbWeapon.position = this.transform.position;

            if (movement.x != 0 || movement.y != 0)
            {
                playerAnimator.SetBool("isMoving", true);
                walkCount -= Time.deltaTime;
            }
            else
            {
                playerAnimator.SetBool("isMoving", false);
                walkCount = 0;
            }

            if (walkCount <= 0 && playerAnimator.GetBool("isMoving"))
            {
                walkCount = .27f;
                FMODUnity.RuntimeManager.PlayOneShot(moveSound);
            }
        }
    }

    void FixedUpdate()
    {
        if (!this.GetComponent<Player>().death)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePosition - rb.position;
            weaponDir = lookDir;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rbWeapon.rotation = angle;
        }
    }
}