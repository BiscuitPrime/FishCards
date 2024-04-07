using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will be used to create Buff Script. Obj.
/// Basically, when a buff needs to be applied to something, this (or its children) will be created and granted to the BuffContainerController that will hold, for a gameobject (card or actor) all the buffs applied to it.
/// Also contains a turn counter that indicates the number of turn it remains (actualized with TURN_END) (so if counter =1 => at the next TURN_END, it is deleted).
/// The turn where the buff is created counts as 1 turn as such.
/// </summary>
public abstract class BuffObject : ScriptableObject
{
    [field:Header("Buff Values")]
    [field:SerializeField] public int TurnCounter { get; private set; }
    public void UpdateTurnCounter() { TurnCounter--; }
}
