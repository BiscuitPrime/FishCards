using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the attack cards, that will have their main effect to be buffs on the player, opponent, other cards, etc...
/// As such, they can NEVER attack
/// </summary>
[CreateAssetMenu(fileName = "BuffCard", menuName = "Scriptable Objects/Cards/Buff Card")]
public class BuffCardController : CardController
{
    public override void ActivateCardEffect(GameObject card)
    {
        throw new System.NotImplementedException();
    }
}
