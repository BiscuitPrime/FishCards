using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script
/// </summary>
[CreateAssetMenu(fileName = "Fishorama", menuName = "Scriptable Objects/Cards/Wild Cards/Fish-O-rama")]
public class FishoramaCardController : WildCardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        base.ActivateCardEffect(card);
        PlayController.Instance.GetPlayer().GetComponent<HandController>().DrawCards(2);
        Destroy(card);
    }
}
