using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoTo : MonoBehaviour, IPointerDownHandler
{
    public LocationManager manager;

    public int destination;

    public void OnPointerDown(PointerEventData eventData)
    {
        manager.GoToLocation(destination);
    }
}