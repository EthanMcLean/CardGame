using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public Transform handTrans;
    public Transform deckTrans;

    public void DrawCard()
    { 
        CardAnimationHandler.instance.AddMoveCardAnimation(deckTrans,handTrans, this);
    }
}
