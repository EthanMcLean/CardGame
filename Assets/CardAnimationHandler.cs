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

    public AudioSource voice;

    public void Awake()
    {
        instance = this; 
    }
    
    public void AddAttackCardAnimation(Card cardAttacking, Card cardDefending)
    {
        CardAttackAnimation newAnim = new CardAttackAnimation();

        newAnim.attacker = cardAttacking;
        newAnim.defender = cardDefending;

        currentAnimations.Add(newAnim);
        TryToTriggerNextAnimation();
    }


    public void AddMoveCardAnimation(Transform fromTrans, Transform toTrans, PlayerManager owner, string newZone, CardData data, bool PlayCard)
    {
        CardMovementAnimation newAnim = new CardMovementAnimation();
        GameObject cardSpace = Instantiate(GameManager.instance.cardSpacePrefab, toTrans);
        cardSpace.GetComponent<RectTransform>().sizeDelta *= data.size;
        newAnim.cardSpace = cardSpace;
        newAnim.cardToMove = Instantiate(GameManager.instance.cardPrefab, fromTrans);
        newAnim.cardToMove.GetComponent<RectTransform>().sizeDelta *= data.size;
        newAnim.cardToMove.SetActive(false);
        newAnim.playCard = PlayCard;

        Card newCard = newAnim.cardToMove.GetComponent<Card>();
        if (owner == GameManager.instance.ai && newZone == "Hand")
        {
            newCard.cardDisplay.sprite = newCard.cardBack;
            newCard.cardArtDisplay.gameObject.SetActive(false);
            newCard.attack.transform.parent.gameObject.SetActive(false);
            newCard.defence.transform.parent.gameObject.SetActive(false);
        }
        else
        {
        
            newCard.cardArtDisplay.sprite = data.cardSprite;
            newCard.cardArtDisplay.gameObject.SetActive(true);
            newCard.attack.transform.parent.gameObject.SetActive(true);
            newCard.defence.transform.parent.gameObject.SetActive(true);
        }

        newCard.zoneIn = newZone;
        newCard.data = data;
        newAnim.owner = owner;
        newCard.UpdateFromData();
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
    public bool playCard;
    public override IEnumerator AnimationCoroutine()
    {
        card = cardToMove.GetComponent<Card>();
        if (playCard)
        {
            CardAnimationHandler.instance.voice.clip = owner.summonClip;
            CardAnimationHandler.instance.voice.Play();
        }
        else
        {
            CardAnimationHandler.instance.voice.clip = owner.drawClip;
            //CardAnimationHandler.instance.voice.Play();
        }
        while (CardAnimationHandler.instance.voice.isPlaying)
        {
            yield return null;
        }
        if (playCard)
        {
            if(owner == GameManager.instance.player)
            {
                CardAnimationHandler.instance.voice.clip = card.data.playerNameClip;
            }
            else
            {
                CardAnimationHandler.instance.voice.clip = card.data.aiNameClip;
            }
           
            CardAnimationHandler.instance.voice.Play();
        }
      

        
        card.emptySpace = cardSpace;
        card.destinationTrans = cardSpace.transform.parent;
        card.owner = owner;
        cardToMove.SetActive(true);
        Vector3 startPoint = cardToMove.transform.position;
        while (Vector3.Distance(cardToMove.transform.position, cardSpace.transform.position) > 0.1f)
        {
            lerp += Time.deltaTime * speed;
            cardToMove.transform.position = Vector3.Lerp(
                startPoint,
                cardSpace.transform.position,
                lerp) ;
            yield return null;
        }
        card.CleanUpAfterMove();
        while (CardAnimationHandler.instance.voice.isPlaying)
        {
            yield return null;
        }
        CardAnimationHandler.instance.FinishAnimation();
        CardAnimationHandler.instance.TryToTriggerNextAnimation();
    }
}

[System.Serializable]
public class CardAttackAnimation : GameAnimation
{
    
    public Card attacker;
    public Card defender;

    public override IEnumerator AnimationCoroutine()
    {

        CardAnimationHandler.instance.voice.clip = attacker.owner.attackClip;
        CardAnimationHandler.instance.voice.Play();
    
        while (CardAnimationHandler.instance.voice.isPlaying)
        {
            yield return null;
        }

        if (attacker.owner == GameManager.instance.player)
        {
            CardAnimationHandler.instance.voice.clip = attacker.data.playerNameClip;
        }
        else
        {
            CardAnimationHandler.instance.voice.clip = attacker.data.aiNameClip;
        }
        CardAnimationHandler.instance.voice.Play();
     

        float movementtoHitTimer = 0;
        Vector3 startPoint = attacker.transform.position;
        while (Vector3.Distance(attacker.transform.position,defender.transform.position) > 100)
        {
            attacker.transform.position = Vector3.Slerp(startPoint, defender.transform.position, movementtoHitTimer);
            movementtoHitTimer += Time.deltaTime*10;
            yield return null;
        }
        movementtoHitTimer = 0;

        while (Vector3.Distance(attacker.transform.position, startPoint) > 1)
        {
            attacker.transform.position = Vector3.Slerp(defender.transform.position, startPoint, movementtoHitTimer);
            movementtoHitTimer += Time.deltaTime * 10;
            yield return null;
        }


        defender.hit.SetActive(true);
        defender.SetDamage(attacker.data.attack);
        yield return new WaitForSeconds(0.2f);
        defender.hit.SetActive(false);



        attacker.hit.SetActive(true);
        attacker.SetDamage(defender.data.attack);
        yield return new WaitForSeconds(0.2f);
        attacker.hit.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        while (CardAnimationHandler.instance.voice.isPlaying)
        {
            yield return null;
        }

        if (defender.currentDamage >= defender.data.health)
        {
            CardAnimationHandler.instance.voice.clip = defender.owner.negativeReaction;
            CardAnimationHandler.instance.voice.Play();

            while (CardAnimationHandler.instance.voice.isPlaying)
            {
                yield return null;
            }

            GameManager.instance.DestoryCardInCombat(defender);


        }
        if (attacker.currentDamage >= attacker.data.health)
        {
            GameManager.instance.DestoryCardInCombat(attacker);
        }
        CardAnimationHandler.instance.FinishAnimation();
        CardAnimationHandler.instance.TryToTriggerNextAnimation();
    }
}

