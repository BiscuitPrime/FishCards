using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the attack cards, that will have their main effect to be attack against the opponent
/// </summary>
[CreateAssetMenu(fileName ="AttackCard",menuName ="Scriptable Objects/Cards/Attack Card")]
public class AttackCardController : CardController
{
    [field:Header("Attack Cards Values")]
    [field:SerializeField] public int ATK { get; private set; }
    [field:SerializeField] public int PIER { get; private set; }
    [field:SerializeField] public int TRACK { get; private set; }

    public override void ActivateCardEffect(GameObject card)
    {
        Debug.Log("[AttackCardController] : Activating effect of card " + CardName);
        if (PlayController.Instance.GetOpponent() == null)
        {
            Debug.LogError("[AttackCardController] : Attack failed : no opponent detected");
            return;
        }

        GameObject target = PlayController.Instance.GetOpponent();
        if(target.GetComponent<ActorValuesController>() == null)
        {
            Debug.LogError("[AttackCardController] : Attack failed : opponent does not possess appropriate ActorValuesController component");
            return;
        }

        target.GetComponent<ActorValuesController>().AttemptToTakeDamage(ATK,PIER,TRACK);
        Destroy(card);
    }
}
