using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Buff Object made for actor-based buffs
/// </summary>
[CreateAssetMenu(fileName ="ActorBuffObject",menuName = "Scriptable Objects/Buff/Actor Buff Object")]
public class ActorBuffObject : BuffObject
{
    [field: Header("Values")]
    [field: SerializeField] public int HP { get; private set; }
    [field: SerializeField] public int DEF { get; private set; }
    [field: SerializeField] public int AGI { get; private set; }
    public void DefineValues(string name, int hp, int def, int agi, int turnCounter=1)
    {
        this.name = name;
        this.HP = hp;
        this.DEF = def;
        this.AGI = agi;
        this.TurnCounter = turnCounter;
    }
}
