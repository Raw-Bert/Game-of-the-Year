using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject manager;
    Vector2 shootDir;
    public GameObject mainCamera;

    public float speed = 5.0f;
    // Start is called before the first frame update

    void Awake()
    {
        //manager = GameObject.Find("LevelController");
        mainCamera = GameObject.FindWithTag("MainCamera");
        shootDir = mainCamera.GetComponent<CameraOffset>().CaptureMousePos();
        shootDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(shootDir.x * speed * Time.deltaTime, shootDir.y * speed * Time.deltaTime, this.transform.position.z);
       
    }
}
