using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used by the Go Fish card that ends the play immediately
/// </summary>
[CreateAssetMenu(fileName = "GoFish", menuName = "Scriptable Objects/Cards/Wild Cards/Go Fish")]
public class GoFishCardController : WildCardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        //base.ActivateCardEffect(card);
        PlayController.Instance.RequestEndOfPlay();
        Destroy(card);
    }
}
