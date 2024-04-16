using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ActorValues",menuName ="Scriptable Objects/Actor/Actor Values")]
public class ActorValuesData : ScriptableObject
{
    [field:Header("Values")]
    [field:SerializeField] public int HP { get; private set; }
    [field: SerializeField] public int DEF { get; private set; }
    [field: SerializeField] public int AGI { get; private set; }

    [field:Header("Audio Values")]
    [field:SerializeField] public AudioClip HPDmgTakenSound { get; private set; }
    [field: SerializeField] public AudioClip DEFDmgTakenSound { get; private set; }
    [field:SerializeField] public AudioClip MissSound { get; private set; }


}
