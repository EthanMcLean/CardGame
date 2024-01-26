using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerManager player;
    public PlayerManager ai;

    public static GameManager instance;

    public GameObject cardPrefab;
    public GameObject cardSpacePrefab;

    public PlayerManager currentPlayer;

    public Card currentDraggedCard;

    public Transform defaultCanvas;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            
            player.DrawCard();
        }
        for (int i = 0; i < 10; i++)
        {
            ai.DrawCard();
        }
    }

    public void DragStart(Card cardDragged)
    {
        if (cardDragged.owner == player)
        {
            GameObject cardSpace = Instantiate(GameManager.instance.cardSpacePrefab, cardDragged.transform.parent);
            cardSpace.transform.SetSiblingIndex(cardDragged.transform.GetSiblingIndex());
            cardDragged.emptySpace = cardSpace;
            cardDragged.destinationTrans = cardSpace.transform.parent;
            cardDragged.transform.SetParent(defaultCanvas) ;
            currentDraggedCard = cardDragged;
        }
    }
    public void DragEnd(Card cardDragged)
    {
        if (currentDraggedCard == cardDragged)
        {
            currentDraggedCard.CleanUpAfterMove();
            currentDraggedCard = null;
        }
    }

    public void Update()
    {
        if (currentDraggedCard != null)
        {
            currentDraggedCard.transform.position = Input.mousePosition;
        }
    }
}
