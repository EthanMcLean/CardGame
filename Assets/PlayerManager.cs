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


    public Transform deckData;

    public AudioClip[] summonClip;
    public AudioClip[] drawClip;
    public AudioClip[] attackClip;
    public AudioClip[] negativeReaction;

    public void Awake()
    {
        cardsInDeck = new List<CardData>(deckData.GetComponentsInChildren<CardData>());
    }

    public void DrawCard(bool initialCard)
    {
        if (cardsInDeck.Count > 0)
        {
            CardData nextCard = cardsInDeck[0];
            cardsInDeck.RemoveAt(0);
            CardAnimationHandler.instance.AddMoveCardAnimation(deckTrans, handTrans, this, "Hand", nextCard, false, initialCard);
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
        CardAnimationHandler.instance.AddMoveCardAnimation(handTrans, slot, this, "Field", cardPlayed.data, true, false);
        cardsInHand.Remove(cardPlayed.data);
    }

    public void EndTurn()
    {
        GameManager.instance.EndTurn();
    }
}
