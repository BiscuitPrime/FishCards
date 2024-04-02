using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the HAND of the player/opponent
/// The hand is the ensemble of cards a player has (in-hand) during a turn, and that they can play during their play.
/// The hand will be drawn from the Deck, in the order where they are following a FILO logic
/// </summary>
[RequireComponent(typeof(DeckController))]
public class HandController : MonoBehaviour
{
    [Header("Deck")]
    [SerializeField] private DeckController _deck;

    [Header("Cards elements")]
    [SerializeField] private int _handCardNumbers;
    private List<CardController> _handCards; //cards within the hand : can be null

    private void Awake()
    {
        _deck = GetComponent<DeckController>();
        _handCards = new List<CardController>();
    }

    /// <summary>
    /// Will attempt to draw cards from the deck
    /// </summary>
    public void DrawCards()
    {
        int cardsToDraw = _handCardNumbers - _handCards.Count;
        CardController[] drawnCards = _deck.DrawCards(_handCardNumbers);
        for(int i=0; i < drawnCards.Length; i++)
        {
            Debug.Log("Adding card : " + drawnCards[i].name + " to hand");
            _handCards.Add(drawnCards[i]);
        }
    }

    /// <summary>
    /// Function that will play the inputted card from within the hand. Will throw an error if the card is not within the hand.
    /// </summary>
    /// <param name="card"></param>
    public void PlayCard(CardController card)
    {
        if (!_handCards.Contains(card))
        {
            Debug.LogError("Card " + card.name + " not present in the hand");
            return;
        }
        RemoveCardFromHand(card);
    }

    private void RemoveCardFromHand(CardController card)
    {
        foreach(CardController cardController in _handCards)
        {
            if(cardController == card)
            {
                _handCards.Remove(card);
                return;
            }
        }
    }
}
