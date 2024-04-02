using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the attack cards, that will have their main effect to be diverse and ACTIVATED IMMEDIATELY WHEN PLAYED
/// </summary>
[CreateAssetMenu(fileName = "WildCard", menuName = "Scriptable Objects/Cards/Wild Card")]
public class WildCardController : CardController
{
    public override void ActivateCardEffect()
    {
        throw new System.NotImplementedException();
    }
}
