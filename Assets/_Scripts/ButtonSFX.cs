using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField][EventRef] string hover;

    public void OnPointerEnter(PointerEventData data)
    {

        if (GetComponent<Button>().interactable)
            FMODUnity.RuntimeManager.PlayOneShot(hover);
    }

}