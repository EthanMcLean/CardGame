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

    public List<Image> AIbigSpaces = new List<Image>();
    public List<Image> AIsmallSpaces = new List<Image>();
    public List<Image> AImidSpaces = new List<Image>();

    public string currentPhase = "Play 1 Card";
    public TMPro.TextMeshProUGUI phaseDisplay;
    public GameObject endTurnButton;
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
        if (cardDragged.owner == player && currentPhase == "Play 1 Card" && currentPlayer == cardDragged.owner)
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
        if (currentDraggedCard == cardDragged && currentPhase == "Play 1 Card" && currentPlayer == cardDragged.owner)
        {
            currentDraggedCard.CleanUpAfterMove();
            if (currentGridSpaces.Count > 0)
            {
                TryToPlayCard(cardDragged, currentGridSpaces[0],player);
            }
            currentDraggedCard = null;
        }
    }

    public bool TryToPlayCard(Card cardDragged, GridSpace targetSpace, PlayerManager playerOfCard)
    {
        int fullSpaces = 0;
        if (targetSpace.subGridSpace.Count > 0)
        {
            foreach (GridSpace space in targetSpace.subGridSpace)
            {
                if (space.full == true)
                {
                    fullSpaces += 1;
                }
            }
        }
        else if (targetSpace.full)
        {
            fullSpaces += 1;
        }
        bool canPlay = false;
        if (cardDragged.data.size == 1)
        {
            canPlay = fullSpaces == 0;
        }
        else if (cardDragged.data.size == 2)
        {
            canPlay = fullSpaces >= 1;
        }
        else if (cardDragged.data.size == 3)
        {
            canPlay = fullSpaces >= 2;
        }

        if (canPlay)
        {
            if (targetSpace.subGridSpace.Count > 0)
            {
                foreach (GridSpace space in targetSpace.subGridSpace)
                {
                    Card[] sacCosts = space.transform.GetComponentsInChildren<Card>();
                    foreach (Card sac in sacCosts)
                    {
                        Destroy(sac.gameObject);
                    }
                    space.full = true;
                    if (space.parentGridSpace.Count > 0)
                    {
                        foreach (GridSpace space2 in space.parentGridSpace)
                        {
                            Card[] sacCosts2 = space2.transform.GetComponentsInChildren<Card>();
                            foreach (Card sac in sacCosts2)
                            {
                                Destroy(sac.gameObject);
                            }
                        }
                    }
                }
            }
            else
            {
                Card[] sacCosts = targetSpace.transform.GetComponentsInChildren<Card>();
                foreach (Card sac in sacCosts)
                {
                    Destroy(sac.gameObject);
                }
                targetSpace.full = true;
            }
            
            currentPhase = "Attack with Cards";
            phaseDisplay.text = currentPhase;
            playerOfCard.PlayCard(cardDragged, targetSpace);
            return true;
        }
        return false;
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

    public void EndTurn()
    {
        if (currentPlayer == player)
        {
            currentPlayer = ai;
        }
        else
        {
            currentPlayer = player;
        }
        currentPhase = "Play 1 Card";
        if (currentPlayer == player)
        {
            endTurnButton.SetActive(true);
            phaseDisplay.text = currentPhase;
        }
        else
        {
            endTurnButton.SetActive(false);
            phaseDisplay.text = "Enemy Turn";
            StartCoroutine(AITurn());
        }
        currentPlayer.DrawCard();
    }

    IEnumerator AITurn()
    {
        yield return new WaitForSeconds(0.5f);
        Card[] cardsInHand = ai.handTrans.GetComponentsInChildren<Card>();
        bool playedCard = false;
        foreach (Card card in cardsInHand)
        {
            if (card.data.size == 1)
            {
                foreach (Image space in AIsmallSpaces)
                {
                    GridSpace actualSpace = space.GetComponent<GridSpace>();
                    if (!actualSpace.full)
                    {
                        if (TryToPlayCard(card, actualSpace, ai))
                        {
                            playedCard = true;
                            break;
                        }
                    }
                }
               
            }
            else if (card.data.size == 2)
            {
                foreach (Image space in AImidSpaces)
                {
                    GridSpace actualSpace = space.GetComponent<GridSpace>();
                    if (!actualSpace.full)
                    {
                        if (TryToPlayCard(card, actualSpace,ai))
                        {
                            playedCard = true;
                            break;
                        }
                    }
                }

            }
            else if (card.data.size == 3)
            {
                foreach (Image space in AIbigSpaces)
                {
                    GridSpace actualSpace = space.GetComponent<GridSpace>();
                    if (!actualSpace.full)
                    {
                        if (TryToPlayCard(card, actualSpace,ai))
                        {
                            playedCard = true;
                            break;
                        }
                    }
                }

            }
            if (playedCard)
            {
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }
}
