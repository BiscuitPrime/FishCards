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
        //we obtain all the values from the current buff container, that we will add up :
        int atk = ATK;
        int pier = PIER;
        int track = TRACK;
        var buffContainer = card.GetComponent<BuffContainer>();
        if(buffContainer != null)
        {
            List<BuffObject> buffs = buffContainer.ObtainListOfBuffs();
            foreach(BuffObject buff in buffs )
            {
                var b = buff as AttackCardBuffObject;
                if(b == null) { continue; }
                atk += b.ATK;
                pier += b.PIER;
                track += b.TRACK;
            }
        }
        Debug.Log("[ATTACK CARD CONTROLLER] : Attacking with values : ATK : " + atk + " PIER : " + pier + " TRACK : " + track);
        target.GetComponent<ActorValuesController>().AttemptToTakeDamage(atk,pier,track);
        Destroy(card);
    }
}
