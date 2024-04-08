using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used by the Kraken's Purse card that will draw 1 card from the top of the opponent's deck immediately
/// </summary>
[CreateAssetMenu(fileName = "KrakensPurse", menuName = "Scriptable Objects/Cards/Wild Cards/Kraken's Purse")]
public class KrakensPurseCardController : WildCardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        base.ActivateCardEffect(card);
        PlayController.Instance.GetPlayer().GetComponent<HandController>().AddNewCard(PlayController.Instance.GetOpponent().GetComponent<DeckController>().DrawCards(1)[0]);
        Destroy(card);
    }
}
