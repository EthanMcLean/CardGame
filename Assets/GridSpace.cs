using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string owner;
    public bool full;
    public List<GridSpace> subGridSpace = new List<GridSpace>();
    public Transform altPos;
    public List<GridSpace> parentGridSpace = new List<GridSpace>();
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (owner == "Player")
        {
            GameManager.instance.UpdateCurrentGridSpace(this, true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.UpdateCurrentGridSpace(this, false);
    }
}
