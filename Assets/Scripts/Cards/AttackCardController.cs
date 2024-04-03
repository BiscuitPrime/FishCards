using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the attack cards, that will have their main effect to be attack against the opponent
/// </summary>
[CreateAssetMenu(fileName ="AttackCard",menuName ="Scriptable Objects/Cards/Attack Card")]
public class AttackCardController : CardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        Debug.Log("Activating effect of card "+CardName);
        Destroy(card);
    }
}
