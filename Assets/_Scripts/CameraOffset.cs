//Shoutout to wBeaty at https://github.com/wbeaty/NuclearThronesCamera/tree/master/NuclearThroneCaseStudy for this 
//camera solution

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{

    public Transform player;
    

    public Camera camera;

   // public float factor = 3f;
    public float cameraDistance = 3.5f;
    public float max = 0.9f;
    float smoothTime = 0.2f, zStart;

    Vector3 mousePos, target, refVel;

    Coroutine currentRecoil;
    Vector2 tempNewPos;

    void Start()
    {
        target = player.position;
        zStart = transform.position.z;
    }

    void Update() 
    {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPos();
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

    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDistance;
        Vector3 ret = player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }

    void UpdateCameraPos()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(new Vector3(transform.position.x + tempNewPos.x, transform.position.y + tempNewPos.y, this.transform.position.z),
         target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public void StartGunRecoil(float seconds)
    {
        if (currentRecoil != null)
        {
            StopCoroutine(currentRecoil);
        }
        currentRecoil = StartCoroutine(GunRecoil(seconds));
    }

    IEnumerator GunRecoil(float seconds)
    {
        float backDuration = seconds / 2;
        for (float i = 0; i <= backDuration; i += Time.deltaTime)
        {
            Vector2 tempPos = mousePos;
            Vector2 targetDir = new Vector2(player.transform.position.x, player.transform.position.y);
           
            tempNewPos = Vector3.Lerp(mousePos, targetDir, i / backDuration);
            target += new Vector3(tempNewPos.x, tempNewPos.y,0);

            yield return null;
        }

        float forwardDuration = seconds / 2;
        for (float i = 0; i <= backDuration; i += Time.deltaTime)
        {
            Vector2 tempPos = mousePos;
            Vector2 targetDir = new Vector2(player.transform.position.x, player.transform.position.y);
           
            tempNewPos = Vector3.Lerp(targetDir, mousePos, i / backDuration);
            target += new Vector3(tempNewPos.x, tempNewPos.y,0);

            yield return null;
        }
    }
}
