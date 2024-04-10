using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used by the Blabberfish card that will display the opponents cards
/// </summary>
[CreateAssetMenu(fileName = "Blabberfish", menuName = "Scriptable Objects/Cards/Wild Cards/Blabberfish")]
public class BlabberfishCardController : WildCardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        base.ActivateCardEffect(card);
        PlayController.Instance.GetOpponent().GetComponent<HandController>().ShowFrontCards();
        Destroy(card);
    }
}
