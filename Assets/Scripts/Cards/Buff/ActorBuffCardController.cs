using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTOR_BUFF_TYPE
{
    PLAYER,
    OPPONENT
}
/// <summary>
/// Script that will handle the buff cards, that will have their main effect to be buffs on ACTORS
/// As such, they can NEVER attack
/// </summary>
[CreateAssetMenu(fileName = "ActorBuffCard", menuName = "Scriptable Objects/Cards/Buff Cards/Actor Buff Card")]
public class ActorBuffCardController : BuffCardController
{
    [field: SerializeField] public int HP { get; private set; }
    [field: SerializeField] public int DEF { get; private set; }
    [field: SerializeField] public int AGI { get; private set; }
    [field:SerializeField] public ACTOR_BUFF_TYPE Type { get; private set; }

    public override void ActivateCardEffect(GameObject cardPrefab)
    {
        Debug.Log("[ActorBuffCardController] : Activating effect of card " + CardName);
        if(Type==ACTOR_BUFF_TYPE.OPPONENT)
        {
            if (PlayController.Instance.GetOpponent() == null)
            {
                Debug.LogError("[ActorBuffCardController] : Buff failed : no opponent detected");
                return;
            }
            BuffFactory.BuffActor(PlayController.Instance.GetOpponent(), BuffName, HP, DEF, AGI, TurnCounter);
            TextVFXSpawner.Instance.RequestSpawnVFX(PlayController.Instance.GetOpponent().GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.BUFF);
        }
        else
        {
            if(PlayController.Instance.GetPlayer() == null)
            {
                Debug.LogError("[ActorBuffCardController] : Buff failed : no player detected");
                return;
            }
            BuffFactory.BuffActor(PlayController.Instance.GetPlayer(), BuffName, HP, DEF, AGI, TurnCounter);
            TextVFXSpawner.Instance.RequestSpawnVFX(PlayController.Instance.GetPlayer().GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.BUFF);
        }
        Destroy(cardPrefab);
    }
}
