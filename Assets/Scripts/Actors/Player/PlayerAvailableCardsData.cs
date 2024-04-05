using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script of the data script obj that will contain ALL the available cards to the player during the game.
/// This will be used by the GameManager to pick new cards amongst this pile.
/// </summary>
[CreateAssetMenu(fileName ="PlayerAvailableCardsData",menuName ="Scriptable Objects/Cards/Player Available Cards")]
public class PlayerAvailableCardsData : ScriptableObject
{
    [field:Header("Available Cards")]
    [field:SerializeField] public List<CardController> Cards { get; private set; }
}
