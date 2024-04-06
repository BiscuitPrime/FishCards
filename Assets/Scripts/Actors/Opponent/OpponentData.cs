using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script of script. obj that will contain all the necessary data for Game manager to create opponents.
/// Opponents have fixed lists of cards they can use, but the opponent itself will be chosen at semi-random based on its difficulty rating value (defined here)
/// Basically, for 1st encounter, rating must be =1, for 2nd must be 2 etc... (and at some point, only rating>x will be taken)
/// The selection process will be done by subsidiaries of Game Manager
/// </summary>
[CreateAssetMenu(fileName ="OpponentData",menuName ="Scriptable Objects/Actor/Opponent Data")]
public class OpponentData : ScriptableObject
{
    [field:Header("Opponent Art Data")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field:SerializeField] public AudioClip OpponentSound { get; private set; }

    [field:Header("Opponent Card Data")]
    [field: SerializeField] public CardsListData InitCardDeck { get; private set; } //the starting deck of the opponent : unlike players who get to pick new cards during play, these will not change in runtime.
    [field:SerializeField] public int HandSize { get; private set; } //the size of the opponent's hand
    
    [field:Header("Opponent System Data")]
    [field:SerializeField] public int DifficultyRating { get; private set; }
    [field:SerializeField] public ActorValuesData Values { get; private set; }
}
