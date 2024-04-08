using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// This script will handle the deck of the player and its opponent, containing ALL the possible cards they have during an encounter.
/// Each turn, the player/opponent will draw cards from the deck to the hand.
/// The cards should follow, within the drawn section, a FILO logic.
/// However, considering that the game might have cards that have special effects linked to specific other cards, this script should be as mosular as possible.
/// </summary>
public class DeckController : MonoBehaviour
{
    private Stack<CardController> _deckCards; //the cards within the deck
    private static System.Random rng = new System.Random();

    private void Awake()
    {
        _deckCards = new Stack<CardController>();
    }

    private void Start()
    {
        TurnEventsHandler.Instance.EncounterEvent.AddListener(OnEncounterEvent);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEvent);
    }

    /// <summary>
    /// Function called when the encounter event is received.
    /// In case of encounter START, we shuffle the deck. 
    /// We assume, by that point, that the deck has been constructed.
    /// </summary>
    /// <param name="arg"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnEncounterEvent(EncounterEventArg arg)
    {
        if(arg.State==ENCOUNTER_EVENT_STATE.ENCOUNTER_START)
        {
            ShuffleDeck();
        }
    }

    /// <summary>
    /// Builds the deck with the inputted cards.
    /// </summary>
    /// <param name="cards"></param>
    public void ConstructDeck(List<CardController> cards)
    {
        _deckCards.Clear();
        foreach (CardController card in cards)
        {
            _deckCards.Push(card);
        }
        //PrintCurrentDeck();
    }

    #region HANDLING CARDS
    /// <summary>
    /// Adds a new card to the TOP of the deck (will be taken out first).
    /// </summary>
    /// <param name="card">Inputted new card</param>
    public void AddCardToDeck(CardController card)
    {
        _deckCards.Push(card);
    }

    /// <summary>
    /// Adds a new card to the BOTTOM of the deck (will be taken last)
    /// </summary>
    /// <param name="card">Inputted new card</param>
    public void AddCardToBottomOfDeck(CardController card)
    {
        List<CardController> newDeck =  new List<CardController>();
        newDeck.Add(card);
        CardController[] tmp = _deckCards.ToArray();
        for(int i=1;i<tmp.Length+1; i++)
        {
            newDeck.Add(tmp[tmp.Length - i]);
        }
        ConstructDeck(newDeck);
    }

    /// <summary>
    /// Function that will draw out the required number of cards, in order, with 0 being the first one drawn, and number-1 the last
    /// </summary>
    /// <param name="number">Number of cards drawn</param>
    /// <returns>Array of drawn cards</returns>
    public CardController[] DrawCards(int number)
    {
        CardController[] cards = new CardController[number];
        if (_deckCards.Count < 1)
        {
            Debug.LogError("[DECK CONTROLLER] :NO CARDS LEFT TO DRAW");
            return null;
        }
        for(int i = 0; i < number; i++)
        {
            cards[i] = _deckCards.Pop();
        }
        return cards;
    }
    #endregion

    /// <summary>
    /// Will randomize the cards order.
    /// </summary>
    public void ShuffleDeck()
    {
        Debug.Log("[DECK CONTROLLER] : Shuffling cards of the deck");
        List<CardController> tmp = new List<CardController>();
        foreach(var card in _deckCards)
        {
            tmp.Add(card);
        }
        _deckCards.Clear();
        List<CardController> shuffledcards = tmp.OrderBy(_ => rng.Next()).ToList();
        foreach(var card in shuffledcards)
        {
            _deckCards.Push(card);
        }
}


    /// <summary>
    /// Prints the content of the deck, in order
    /// </summary>
    public void PrintCurrentDeck()
    {
        Debug.Log("[DECK CONTROLLER] : PRINTING DECK --------------------------------------------------");
        foreach(var card in _deckCards)
        {
            Debug.Log("[DECK CONTROLLER] : Card in deck : " + card.CardName);
        }
        Debug.Log("[DECK CONTROLLER] : Card on top of the deck : " + _deckCards.Peek().CardName);
    }
}
