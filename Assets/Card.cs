using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Transform destinationTrans;
    public GameObject emptySpace;
    public PlayerManager owner;
    public string zoneIn;
    public CardData data;
    public Image cardDisplay;
    public Image cardArtDisplay;
    public Sprite cardBack;
    public bool attackedThisRound;
    public GameObject hit;

    public TMPro.TextMeshProUGUI attack;
    public TMPro.TextMeshProUGUI defence;
    public TMPro.TextMeshProUGUI damage;

    public int currentDamage;

    public bool isFace;
    public GameObject onePower;
    public GameObject twoPower;

    public void CleanUpAfterMove()
    {
        GetComponent<Image>().raycastTarget = true;
        Destroy(emptySpace);
        transform.SetParent(destinationTrans);
    }

    public void UpdateFromData()
    {
        attack.text = data.attack.ToString();
        defence.text = data.health.ToString();
        if (data.size == 2)
        {
            onePower.SetActive(true);
        }
        else if (data.size == 3)
        {
            twoPower.SetActive(true);
        }
    }
    public void SetDamage(int newDam)
    {
        currentDamage += newDam;
        if (isFace)
        {
            damage.text = (data.health - currentDamage).ToString();
        }
        else
        {
            damage.text = currentDamage.ToString();
        }
        if (currentDamage > 0)
        {
            damage.transform.parent.gameObject.SetActive(true);
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.TryToSelectCardForAttack(this);
        GameManager.instance.TryToAttack(this);
    }
}
