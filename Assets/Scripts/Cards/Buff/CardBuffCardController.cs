using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the buff cards, that will have their main effect to be buffs on CARDS
/// As such, they can NEVER attack
/// </summary>
[CreateAssetMenu(fileName = "CardBuffCard", menuName = "Scriptable Objects/Cards/Buff Cards/Card Buff Card")]
public class CardBuffCardController : BuffCardController
{
    [field: SerializeField] public int ATK { get; private set; }
    [field: SerializeField] public int PIER { get; private set; }
    [field: SerializeField] public int TRACK { get; private set; }

    public override void ActivateCardEffect(GameObject cardPrefab)
    {
        Debug.Log("[CardBuffCardController] : Activating effect of card " + CardName);
        GameObject targetedCard = PlayController.Instance.ObtainNextAttackCardInPlay();
        if (targetedCard == null)
        {
            Debug.Log("[CardBuffCardController] : No card found, buff touches no cards");
        }
        else
        {
            Debug.Log("[CardBuffCardController] : Buffing card : "+targetedCard.GetComponent<CardPrefabController>().Card.CardName);
            BuffFactory.BuffAttackCard(targetedCard,BuffName,ATK,PIER,TRACK,TurnCounter);
        }
        Destroy(cardPrefab);
    }
}
