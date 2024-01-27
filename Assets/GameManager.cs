using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<GridSpace> currentGridSpaces = new List<GridSpace>();

    public List<Image> bigSpaces = new List<Image>();
    public List<Image> smallSpaces = new List<Image>();
    public List<Image> midSpaces = new List<Image>();

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < 5; i++)
        {   
            player.DrawCard();
        }
        for (int i = 0; i < 5; i++)
        {
            ai.DrawCard();
        }
    }

    public void DragStart(Card cardDragged)
    {
        if (cardDragged.owner == player)
        {
            foreach (Image img in smallSpaces)
            {
                img.raycastTarget = cardDragged.data.size == 1;
            }
            foreach (Image img in midSpaces)
            {
                img.raycastTarget = cardDragged.data.size == 2;
            }
            foreach (Image img in bigSpaces)
            {
                img.raycastTarget = cardDragged.data.size == 3;
            }

            cardDragged.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            GameObject cardSpace = Instantiate(GameManager.instance.cardSpacePrefab, cardDragged.transform.parent);
            cardSpace.GetComponent<RectTransform>().sizeDelta *= cardDragged.data.size;
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

            int spacesNeeded = 1;
         
            if (currentGridSpaces.Count >= spacesNeeded)
            {
                bool noSpace = currentGridSpaces[0].full;
                foreach (GridSpace space in currentGridSpaces[0].subGridSpace)
                {
                    if (space.full == true)
                    {
                        noSpace = true;
                    }
                }
                if (!noSpace)
                {
                    if (currentGridSpaces[0].subGridSpace.Count > 0)
                    {
                        foreach (GridSpace space in currentGridSpaces[0].subGridSpace)
                        {
                            space.full = true;
                        }
                    }
                    else
                    {
                        currentGridSpaces[0].full = true;
                    }
                    player.PlayCard(cardDragged, currentGridSpaces[0]);
                }
               
            }
            currentDraggedCard = null;
        }
    }
    

    public void UpdateCurrentGridSpace(GridSpace newSpace, bool add)
    {
        if (add)
        {
            currentGridSpaces.Add(newSpace);
        }
        else
        {
            currentGridSpaces.Remove(newSpace);
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
