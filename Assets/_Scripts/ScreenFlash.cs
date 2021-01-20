using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFlash : MonoBehaviour
{
    Image image = null;
    Coroutine currentFlashCo;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    
    //Accessed from the editor or called from other scripts
    //Starts the screen flash IEnumerator and if one is actively running stops it before starting a new one
    public void StartScreenFlash(float seconds, float maxAlpha, Color newColor)
    {
        image.color = newColor;

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if(currentFlashCo != null)
        {
            StopCoroutine(currentFlashCo);
        }
        currentFlashCo = StartCoroutine(Flash(seconds, maxAlpha));
    }

    //Screen flash magic happens here
    IEnumerator Flash(float seconds, float maxAlpha)
    {
        //duration is divided in half. Alpha is Lerped between 0 and the maxAlpha over the halved duration witrh the given colour
        float flashInDuraction = seconds / 2;
        for (float i = 0; i <= flashInDuraction; i += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, maxAlpha, i / flashInDuraction);
            image.color = tempColor;
            yield return null;
        }

        //Same thing as above but fades out, so starts at the max alpha and Lerps towards 0 over the halved duration witrh the given colour
        float flashOutDuration = seconds / 2;
        for (float i = 0; i <= flashOutDuration; i += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(maxAlpha, 0, i / flashOutDuration);
            image.color = tempColor;
            yield return null;
        }

    }
}
