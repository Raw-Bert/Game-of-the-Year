using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SelectSymbol : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] GameObject symbol;

    public void OnPointerEnter(PointerEventData data)
    {
        symbol.SetActive(true);
        symbol.transform.SetParent(gameObject.transform);

        var trans = symbol.GetComponent<RectTransform>();

        trans.anchoredPosition = new Vector2(-25, 0);
    }
    public void OnPointerExit(PointerEventData data)
    {
        symbol.SetActive(false);
    }

}