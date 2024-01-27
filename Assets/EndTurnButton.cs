using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EndTurnButton : MonoBehaviour, IPointerClickHandler
{
    public PlayerManager player;
    public void OnPointerClick(PointerEventData data)
    {
        player.EndTurn();
    }
}
