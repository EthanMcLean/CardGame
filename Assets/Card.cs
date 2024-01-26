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

    public void CleanUpAfterMove()
    {
        Destroy(emptySpace);
        transform.SetParent(destinationTrans);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.DragStart(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.DragEnd(this);
    }
}
