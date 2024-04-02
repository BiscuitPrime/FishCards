using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _deckCards = new Stack<CardController>();
    }

    /// <summary>
    /// Builds the deck with the inputted cards.
    /// </summary>
    /// <param name="cards"></param>
    public void ConstructDeck(List<CardController> cards)
    {
        foreach (CardController card in cards)
        {
            _deckCards.Push(card);
        }
    }

    /// <summary>
    /// Will randomize the cards order
    /// </summary>
    public void ShuffleDeck()
    {

    }

    /// <summary>
    /// Function that will draw out the required number of cards, in order, with 0 being the first one drawn, and number-1 the last
    /// </summary>
    /// <param name="number">Number of cards drawn</param>
    /// <returns>Array of drawn cards</returns>
    public CardController[] DrawCards(int number)
    {
        CardController[] cards = new CardController[number];
        for(int i = 0; i < number; i++)
        {
            cards[i] = _deckCards.Pop();
        }
        return cards;
    }
}
