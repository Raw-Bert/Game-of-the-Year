using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Camera camera;

   // public float maxPositionX = 30.0f;
    //public float minPositionX = -30.0f;
    //public float maxPositionY = 30.0f;
    //public float minPositionY = -30.0f;

    
    void Update() {
        Debug.Log("run dammit");
        
    }
    void FixedUpdate() 
    {
        Debug.Log("its running?");
    }
    void LateUpdate()
    {
        
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        //camera.transform.position.x = Math.Clamp(camera.transform.position.x, minPositionX, maxPositionX);
        //camera.transform.position.y = Math.Clamp(camera.transform.position.y, minPositionY, maxPositionY);
        
        Debug.Log("LateUpdate");
    }
}
