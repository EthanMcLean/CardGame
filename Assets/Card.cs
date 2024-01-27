using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform destinationTrans;
    public GameObject emptySpace;
    public PlayerManager owner;
    public string zoneIn;
    public CardData data;
    public Image cardDisplay;
    public Sprite cardBack;

    public void CleanUpAfterMove()
    {
        GetComponent<Image>().raycastTarget = true;
        Destroy(emptySpace);
        transform.SetParent(destinationTrans);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (zoneIn == "Hand" && !CardAnimationHandler.instance.animating)
        {
            GameManager.instance.DragStart(this);
        }
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.DragEnd(this);
    }
}
