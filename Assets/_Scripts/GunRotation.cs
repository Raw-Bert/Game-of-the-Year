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
    public Vector2 factor = new Vector3(0.01f,0.01f);
    
    // Start is called before the first frame update
    void Awake()
    {
        gunRenderer = GetComponent<SpriteRenderer>();
        //this.transform.rotation = (0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        //var mousePos = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mousePosition);
        //Debug.Log("FLIP U LITTLE SHIT");
        if(mousePosition.x < player.transform.position.x)
        {
            //Debug.Log("FLIP U LITTLE SHIT");
            gunRenderer.flipY = true;
        }
        else if(mousePosition.x >= player.transform.position.x){
            gunRenderer.flipY = false;
        }
        
        
        if (mousePosition.y > player.transform.position.y)
        {
            gunRenderer.sortingOrder = 1;
        }
        else gunRenderer.sortingOrder = 3;

        //this.gameObject.transform.position.x = this.gameObject.transform.position.x + (mousePosition.x * factor);
        //this.transform.position.y = this.transform.position.y + (mousePosition.y * factor);
    }
}
