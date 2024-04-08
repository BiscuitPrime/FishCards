using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script of the data script obj that will contain a list of cards.
/// Script. obj of this class can then be used as data containers of list of cards (for example, list of cards available to the player at the beginning of the game, opponents etc...)
/// </summary>
[CreateAssetMenu(fileName ="CardsListData",menuName ="Scriptable Objects/Cards/Cards List Data")]
public class CardsListData : ScriptableObject
{
    [field:Header("Cards")]
    [field:SerializeField] public List<CardController> Cards { get; private set; }
    public void SetCards(List<CardController> cards) { Cards = cards; }
}
