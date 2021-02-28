using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    private SpriteRenderer gunRenderer;
    Vector2 mousePosition;
    public Camera cam;
    //public int sortingOrder = 3;
    public GameObject player;
    public Vector3 factor = new Vector3(0.5f,0.5f,0.0f);
    public Vector3 weaponOffset = new Vector3(0.0f, -0.04f, 0.0f);
    public float max = 0.9f;
    
    // Start is called before the first frame update
    void Awake()
    {
        gunRenderer = GetComponent<SpriteRenderer>();
        //this.transform.rotation = (0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        //Set gun position to player position plus offset. This is to prevent the gun object from lagging behind the parent of the player
        
        mousePosition = CaptureMousePos();
        this.transform.position = player.transform.position + weaponOffset + (new Vector3(factor.x * mousePosition.x, factor.y * mousePosition.y, 0.0f));
        //Debug.Log(mousePosition);
        //Debug.Log(new Vector3(factor.x * mousePosition.x, factor.y * mousePosition.y, 0.0f));

        //Flip sprite if mouse past certain axis in relation to the player.
        if(mousePosition.x < 0.0f)
        {
            gunRenderer.flipY = true;
            player.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(mousePosition.x >= 0.0f){
            gunRenderer.flipY = false;
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        //Put gun in layer behind the player depending on mouse position in relation to player
        if (mousePosition.y > 0.0f)
        {
            gunRenderer.sortingOrder = 1;
        }
        else gunRenderer.sortingOrder = 3;

        //this.gameObject.transform.position.x = this.gameObject.transform.position.x + (mousePosition.x * factor);
        //this.transform.position.y = this.transform.position.y + (mousePosition.y * factor);
    }

    public Vector2 CaptureMousePos()
    {
        Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pos *= 2;
        pos -= Vector2.one;

        if (Mathf.Abs(pos.x) > max || Mathf.Abs(pos.y) > max)
        {
            pos = pos.normalized;
        }
        //Debug.Log("POS: " + pos);

        return pos;
    }
}
