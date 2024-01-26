using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CardAnimationHandler : MonoBehaviour
{
    public List<GameAnimation> currentAnimations = new List<GameAnimation>();

    public bool animating;

    public static CardAnimationHandler instance;
    public void Awake()
    {
        instance = this; 
    }

    public void AddMoveCardAnimation(Transform fromTrans, Transform toTrans, PlayerManager owner)
    {
        CardMovementAnimation newAnim = new CardMovementAnimation();
        GameObject cardSpace = Instantiate(GameManager.instance.cardSpacePrefab, toTrans);
        newAnim.cardSpace = cardSpace;
        newAnim.cardToMove = Instantiate(GameManager.instance.cardPrefab, fromTrans);
        newAnim.cardToMove.SetActive(false);
        newAnim.owner = owner;
        currentAnimations.Add(newAnim);
        TryToTriggerNextAnimation();

    }
    public void TryToTriggerNextAnimation()
    {
        if (!animating)
        {
            if (currentAnimations.Count > 0)
            {
                animating = true;
                StartCoroutine(currentAnimations[0].AnimationCoroutine());
            }
        }
    }
    public void FinishAnimation()
    {
        animating = false;
        currentAnimations.RemoveAt(0);
    }
}

[System.Serializable]
public class GameAnimation
{ 
    public virtual IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        CardAnimationHandler.instance.FinishAnimation();
        CardAnimationHandler.instance.TryToTriggerNextAnimation();
    }
}
[System.Serializable]
public class CardMovementAnimation: GameAnimation
{
    public PlayerManager owner;
    public Card card;
    public GameObject cardToMove;
    public GameObject cardSpace;
    public float speed = 10f;
    public float lerp;
    public override IEnumerator AnimationCoroutine()
    {
        card = cardToMove.GetComponent<Card>();
        card.emptySpace = cardSpace;
        card.destinationTrans = cardSpace.transform.parent;
        card.owner = owner;
        cardToMove.SetActive(true);
        Vector3 startPoint = cardToMove.transform.position;
        while (cardToMove.transform.position != cardSpace.transform.position)
        {
            lerp += Time.deltaTime * speed;
            cardToMove.transform.position = Vector3.Lerp(
                startPoint,
                cardSpace.transform.position,
                lerp) ;
            yield return null;
        }
        card.CleanUpAfterMove();
        CardAnimationHandler.instance.FinishAnimation();
        CardAnimationHandler.instance.TryToTriggerNextAnimation();
    }
}

