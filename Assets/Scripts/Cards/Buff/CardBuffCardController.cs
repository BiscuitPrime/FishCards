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

        Destroy(cardPrefab);
    }
}
