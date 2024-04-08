using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Buff Object made for Attack Card-based buffs
/// </summary>
[CreateAssetMenu(fileName = "AtkCardBuffObject", menuName = "Scriptable Objects/Buff/Attack Card Buff Object")]
public class AttackCardBuffObject : BuffObject
{
    [field:Header("Attack Card Buff Values")]
    [field:SerializeField] public int ATK { get; private set; }
    [field: SerializeField] public int PIER { get; private set; }
    [field: SerializeField] public int TRACK { get; private set; }
    public void DefineValues(string name, int atk, int pier, int track, int turnCounter = 1)
    {
        this.name = name;
        this.ATK = atk;
        this.PIER = pier;
        this.TRACK = track;
        this.TurnCounter = turnCounter;
    }

}
