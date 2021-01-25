using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{

    public Transform player;
    public Vector2 mousePos;

    public Camera camera;

    public float factor = 3f;

    void LateUpdate() 
    {
        mousePos = CaptureMousePos();
        if(player != null)
        {

            camera.transform.position = new Vector3(player.transform.position.x + (mousePos.x / factor),
             player.transform.position.y + (mousePos.y / factor), this.transform.position.z);
        }
    }

    Vector2 CaptureMousePos()
    {
        Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pos *= 2;
        pos -= Vector2.one;
        Debug.Log("POS: " + pos);

        return pos;
    }
}
