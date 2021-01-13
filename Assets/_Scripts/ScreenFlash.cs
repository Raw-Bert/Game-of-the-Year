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

    IEnumerator Flash(float seconds, float maxAlpha)
    {
        float flashInDuraction = seconds / 2;
        for (float i = 0; i <= flashInDuraction; i += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, maxAlpha, i / flashInDuraction);
            image.color = tempColor;
            yield return null;
        }

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
