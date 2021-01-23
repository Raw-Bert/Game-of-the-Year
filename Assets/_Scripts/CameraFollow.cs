using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    //public Transform targetTransform;
    
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
        Debug.Log("LateUpdate");
    }
}
