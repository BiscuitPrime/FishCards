using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the buff cards, that will have their main effect to be buffs on the player, opponent, other cards, etc...
/// As such, they can NEVER attack
/// </summary>
//[CreateAssetMenu(fileName = "BuffCard", menuName = "Scriptable Objects/Cards/Buff Card")]
public abstract class BuffCardController : CardController
{
    [field:Header("Buff Values")]
    [field:SerializeField] public string BuffName { get; private set; }
    [field:SerializeField] public int TurnCounter { get; private set; }
    [field: SerializeField] public AudioClip Sound { get; private set; }

}
