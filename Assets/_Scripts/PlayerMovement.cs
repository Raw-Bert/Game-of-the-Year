using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    public Camera cam;

    public GameObject weapon;
    public Rigidbody2D rbWeapon;
    
    public Vector2 weaponDir;

    Vector2 movement;
    Vector2 mousePosition;

    Animator playerAnimator;

    public Joystick joystick;

    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;


        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        rbWeapon.position = this.transform.position;

        if(movement.x !=0 || movement.y != 0)
        {
            playerAnimator.SetBool("isRun", true);
        }
        else
        {
            playerAnimator.SetBool("isRun", false);
        }
        //Debug.Log(mousePosition.x + " " + mousePosition.y);
    }

    void FixedUpdate()
    {
        //tempMove = (rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePosition - rb.position;
        weaponDir = lookDir;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rbWeapon.rotation = angle;
    }
}
