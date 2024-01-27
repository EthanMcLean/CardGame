using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public Transform handTrans;
    public Transform deckTrans;

    public List<CardData> cardsInDeck = new List<CardData>();

    public List<CardData> cardsInHand = new List<CardData>();


    public void DrawCard()
    {
        if (cardsInDeck.Count > 0)
        {
            CardData nextCard = cardsInDeck[0];
            cardsInDeck.RemoveAt(0);
            CardAnimationHandler.instance.AddMoveCardAnimation(deckTrans, handTrans, this, "Hand", nextCard);
            cardsInHand.Add(nextCard);
        }
    }

    public void PlayCard(Card cardPlayed, GridSpace spacePlayedOn)
    {
        Destroy(cardPlayed.gameObject);
        Transform slot = spacePlayedOn.transform;
        if (spacePlayedOn.altPos != null)
        {
            slot = spacePlayedOn.altPos;
        }
        CardAnimationHandler.instance.AddMoveCardAnimation(handTrans, slot, this, "Field", cardPlayed.data);
        cardsInHand.Remove(cardPlayed.data);
    }

    public void EndTurn()
    {
        GameManager.instance.EndTurn();
    }
}
